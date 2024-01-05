using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataConverter
{

    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            //filepaths
            /******************************************************************************/
            //path to Oekobaudat with komma as decimal separator
            string PathOekobaudat = Path.GetFullPath("../../../../../CSV files/OBD_2020_II.csv");
            
            //Path to DB
            var filepathDB = Path.GetFullPath("../../../../../BuildingElementsKnowledge.db");
            
            //path to Baustoffkonfiguration (eLCA)
            string pathBaustoffkonfiguration = Path.GetFullPath("../../../../../CSV files/Baustoffkonfigurationen_OBD_2020_II_modified.csv");
            
            //path to xml files
            string pathXmlFolder = Path.GetFullPath("../../../../../OBD_2020_II/ILCD/processes/");
            
            //path to MappingKategorien
            string pathMappingCategories = Path.GetFullPath("../../../../../CSV files/MappingKategorien-LayerType.csv");
            
            //path Mapping UUIDs
            string pathMappingUUIDs = Path.GetFullPath("../../../../../CSV files/ManualMappingUUID.csv");
            
            //path Manual Completion
            string pathManualDataCompletion = Path.GetFullPath("../../../../../CSV files/ManualDataCompletion.csv");
            string pathManualThicknessCompletion = Path.GetFullPath("../../../../../CSV files/ManualThicknessCompletion.csv");
            
            //path Thermal Conductivity
            string pathThermalConductivity = Path.GetFullPath("../../../../../CSV files/ThermalConductivity.csv");
            
            //path Manual Service Life 0 and 50
            string pathManualServiceLife0 = Path.GetFullPath("../../../../../CSV files/ManualServiceLife0.csv");
            string pathManualServiceLife50 = Path.GetFullPath("../../../../../CSV files/ManualServiceLife50.csv");
            /******************************************************************************/

            //Call CSVParser for structure and entries
            StructureOekobaudat structure = OekobaudatCSVParser.ParsingStructure(PathOekobaudat);
            List<SingleModEntry> entries = OekobaudatCSVParser.ParsingEntries(PathOekobaudat, structure.ModulePos);
            
            //Filter empty entries
            FilterHandler.FilterEmptyEntries(ref entries);

            //Sort entries into KG300
            var positionKG300 = SortingHandler.SortKG300(entries, structure.ModulePos, structure.NamePos, structure.CategoryPos,structure);

            //copy all positions with end of life
            var positionsWithEndOfLife = new List<int>(positionKG300);
            //remove end of life from positionsKG300
            SortingHandler.RemoveEndOfLife(entries, positionKG300, structure.CategoryPos);

            //Read CategoryMapping and UUIDMaps
            List<CategoryMap> categoryMaps = CsvImportHandler.ReadCategories(pathMappingCategories);
            List<UUIDMap> uuidMaps = CsvImportHandler.ReadUUIDMaps(pathMappingUUIDs);
            //delete entries that are unapplicable in early stages
            FilterHandler.FilterUnapplicableEntries(categoryMaps, uuidMaps, ref positionKG300, entries, structure.CategoryPos, structure.UUIDPos, structure);

            var aluminiumprofilId = entries.FindIndex(x => x.GeneralInformation[2].Contains("Aluminiumprofil anodisiert"));

            //Data Completion (Automatedly correct entries so that only correct units are present)
            DataCompletion.AutomatedDataCompletion(positionKG300, entries, pathXmlFolder, pathBaustoffkonfiguration,
                structure.CategoryPos, structure.UUIDPos, structure.ReferenceUnitPos, structure.ReferenceFluxNamePos, structure.ReferenceValuePos, structure.ConversionToKgPos, structure.DensityPos, structure.ThicknessPos, structure.AreaWeightPos, structure.BulkDensityPos, structure.LengthMassPos);
            //manual data completion from external file
            DataCompletion.ManualDataCompletion(positionKG300, entries, pathManualDataCompletion,
                structure.UUIDPos, structure.CategoryPos, structure.ReferenceUnitPos, structure.ReferenceValuePos, structure.DensityPos, structure.BulkDensityPos,structure.AreaWeightPos,structure.ThicknessPos,structure.ConversionToKgPos,structure.LengthMassPos);
            DataCompletion.ManualThicknessCompletion(positionKG300, entries, pathManualThicknessCompletion, structure, structure.UUIDPos, structure.ReferenceUnitPos, structure.ThicknessPos);
            
            //Multiples
            var multiplesKV = MultipleHandler.FindMultiples(positionKG300, structure.UUIDPos, entries);

            //Generate OekobaudatEntries from SingleModEntries 
            var oekobaudatEntries = ConversionHandler.ConvertSingleModToOekobaudatEntries(entries, multiplesKV, positionKG300, structure.ModulePos, structure.ReferenceUnitPos, structure.ReferenceValuePos, structure.UUIDPos, structure.CategoryPos, structure);

            var aluminiumprofilOeId = oekobaudatEntries.FindIndex(x => x.GeneralInformation[2].Contains("Aluminiumprofil anodisiert"));

            //Mapping OekobaudatEntry to KG3xx and LayerType
            MappingHandler.MapOekobaudatEntryToCategoryMap(ref oekobaudatEntries, categoryMaps, structure.CategoryPos);
            MappingHandler.ManualMapOekobaudatEntry(ref oekobaudatEntries, uuidMaps, structure.UUIDPos, structure);

            //Adding Thermal Conductivity
            MappingHandler.MapThermalConductivity(ref oekobaudatEntries, pathThermalConductivity, structure.CategoryPos, structure.ReferenceUnitPos, structure.ThicknessPos, structure);

            //Completing entries through Conversion, Information from eLCA and manual Information
            CompletingHandlerOekobaudatEntry.CompleteGeneralInformation(oekobaudatEntries, structure.AreaWeightPos, structure.DensityPos, structure.ThicknessPos);
            CompletingHandlerOekobaudatEntry.CompleteOekobaudatEntries(pathBaustoffkonfiguration, oekobaudatEntries, structure.UUIDPos, entries, positionsWithEndOfLife, structure.ReferenceUnitPos, structure.FindIndex("Rohdichte (kg/m3)"), structure.FindIndex("Schichtdicke (m)"), structure.FindIndex("Flaechengewicht (kg/m2)"));
            CompletingHandlerOekobaudatEntry.UpdateServiceLife(oekobaudatEntries, structure, structure.UUIDPos, pathManualServiceLife0, pathManualServiceLife50);

            //Read Database
            var kG3xxNames = DBReadingHandler.ReadKG3xxName(filepathDB);
            var standardLayerTypes = DBReadingHandler.ReadStandardLayerTypes(filepathDB);

            //Add air entries
            oekobaudatEntries.AddRange(AirHandler.GenerateAir(structure, kG3xxNames));

            //Export for overview
            CsvExportHandler.ExportOekobaudatEntriesWithLayerTypesAndKG("Entries", oekobaudatEntries, structure);
            OverviewKGLayerTypeMapping.ExportLayers(standardLayerTypes, oekobaudatEntries);
            OverviewKGLayerTypeMapping.ExportKGs(kG3xxNames, oekobaudatEntries);

            //Fill Database
            DBWritingHandler.WritingTables(structure, oekobaudatEntries, kG3xxNames, standardLayerTypes, filepathDB);
        }
    }
}
