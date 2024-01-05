using System;
using System.Collections.Generic;
using System.Text;

namespace DataConverter
{
    class DataCompletion
    {
        /***
        Corrects the entries of given input indices with respect to their unit (looks for unit in Referenzfluss-Name) and converts entries to m³ (or other correct unit) if possible
        Remark: entries need to be filled beforehand
        :param indicesKG: list of indices for relevant entries in list of singleModEntries
        :param allEntries: list of all singleModEntries
        :param pathXmlFolder: string with path to folder with xml files
        :param path_eLCA: string with path to eLCA file
        :param categoryPosition: position of category in general information of SingleModEntry
        :param UUIDPosition: position of UUID in general information of SingleModEntry
        :param referenceUnitPosition: position of reference unit in general information of SingleModEntry
        :param referenceFluxNamePosition: position of reference flux name in general information of SingleModEntry
        :param referenceValuePosition: position of reference value in general information of SingleModEntry
        :param conversionToKgPosition: position of conversion to kg in general information of SingleModEntry
        :param densityPos: position of density in general information of SingleModEntry
        :param thicknessPos: position of thickness in general information of SingleModEntry
        :param areaWeightPos: position of area weight in general information of SingleModEntry
        :param bulkDensityPos: position of bulk density in general information of SingleModEntry
        :param lengthMassPos: position of length mass in general information of SingleModEntry
         ***/
        public static void AutomatedDataCompletion(
            List<int> indicesKG,
            List<SingleModEntry> allEntries,
            string pathXmlFolder,
            string path_eLCA,
            int categoryPosition,
            int UUIDPosition,
            int unitPos,
            int referenceFluxNamePos,
            int valuePos,
            int conversionToKgPos,
            int densityPos,
            int thicknessPos,
            int areaWeightPos,
            int bulkDensityPos,
            int lengthMassPos)
        {
            CompletingHandlerSMEntry.AddInformationFromXML(indicesKG, allEntries, UUIDPosition, referenceFluxNamePos, unitPos, pathXmlFolder);
            CompletingHandlerSMEntry.AddMissingUnits(indicesKG, allEntries, unitPos, valuePos, referenceFluxNamePos, conversionToKgPos, thicknessPos);
            CompletingHandlerSMEntry.AddMissingConversionsFrom_eLCA(path_eLCA, indicesKG, allEntries, unitPos, UUIDPosition, densityPos, areaWeightPos);
            CorrectionHandler.CorrectReferenceValue(indicesKG, allEntries, unitPos, valuePos, conversionToKgPos);
            CorrectionHandler.CorrectUnitThroughConversion(indicesKG, allEntries, categoryPosition, unitPos, valuePos, conversionToKgPos, densityPos, thicknessPos, areaWeightPos, bulkDensityPos, lengthMassPos);

            //for all entries still left (as prone to flaws not for all)
            CompletingHandlerSMEntry.AddMissingConversions(indicesKG, allEntries, unitPos, referenceFluxNamePos, densityPos, areaWeightPos, thicknessPos);
            CorrectionHandler.CorrectUnitThroughConversion(indicesKG, allEntries, categoryPosition, unitPos, valuePos, conversionToKgPos, densityPos, thicknessPos, areaWeightPos, bulkDensityPos, lengthMassPos);
        }

        /***
        Finds all entries with wrong unit and tries to correct them through information from an external file
        :param indices: all indices for considered entries in allEntries
        :param allEntries: List of all SingleModEntries
        :param pathManualDataCompletion: path to csv file with manual data completion
        :param generalUUIDPos: position of UUID in general Information of SingleModEntries
        :param generalCategoryPosition: position of category in general information of SingleModEntry
        :param generalUnitPosition: position of reference Unit in general Information of SingleModEntries
        :param generalValuePosition: position of reference Value in general Information of SingleModEntries
        :param generalDensityPos: position of density in general Information of SingleModEntries
        :param generalBulkDensityPos: position of Bulk Density in general Information of SingleModEntries
        :param generalAreaWeightPos: position of Area weight in general Information of SingleModEntries
        :param generalThicknessPos: position of thickness in general Information of SingleModEntries
        :param generalConversionToKgPos: position of conversion to kg in general Information of SingleModEntries
        :param generalLenghtMassPos: position of length mass in general Information of SingleModEntries
        ***/
        public static void ManualDataCompletion(
            List<int> indices,
            List<SingleModEntry> allEntries,
            string pathManualDataCompletion,
            int generalUUIDPos,
            int generalCategoryPosition,
            int generalUnitPos,
            int generalValuePos,
            int generalDensityPos,
            int generalBulkDensityPos,
            int generalAreaWeightPos,
            int generalThicknessPos,
            int generalConversionToKgPos,
            int generalLengthMassPos)
        {
            //find indices with wrong unit
            var indicesWrongUnit = Order.GetEntriesWrongUnit(allEntries, indices, generalUnitPos, generalValuePos, generalCategoryPosition);

            //Read information from external file
            var header = CsvImportHandler.ReadHeader(pathManualDataCompletion);
            var entries = CsvImportHandler.ReadFromCSV(pathManualDataCompletion, false, Encoding.UTF8);

            //relevant positions in header resp. entries from external file
            int manUUIDPos = header.FindIndex(n => n == "UUID");
            int manReferenceValuePos = header.FindIndex(n => n == "Bezugsgroesse");
            int manReferenceUnitPos = header.FindIndex(n => n == "Bezugseinheit");
            int manBulkDensityPos = header.FindIndex(n => n == "Schuettdichte (kg/m3)");
            int manAreaWeightPos = header.FindIndex(n => n == "Flaechengewicht (kg/m2)");
            int manDensityPos = header.FindIndex(n => n == "Rohdichte (kg/m3)");
            int manThicknessPos = header.FindIndex(n => n == "Schichtdicke (m)");
            int manConversionToKgPos = header.FindIndex(n => n == "Umrechungsfaktor auf 1kg");
            int manLengthMassPos = header.FindIndex(n => n == "Laengengewicht (kg/m)");

            //go through entries with wrong unit and try to correct them
            for (int j = 0; j < indicesWrongUnit.Count; j++)
            {
                //find complementing entry
                var current = allEntries[indicesWrongUnit[j]];
                
                var entry = entries.Find(n => n[manUUIDPos] == current.GeneralInformation[generalUUIDPos]);
                //check if successfully found complementing entry, else continue with next entry
                if (entry == null)
                { continue; }

                
                current.GeneralInformation[generalUnitPos] = entry[manReferenceUnitPos];
                current.GeneralInformation[generalValuePos] = entry[manReferenceValuePos];
                current.GeneralInformation[generalBulkDensityPos] = entry[manBulkDensityPos];
                current.GeneralInformation[generalAreaWeightPos] = entry[manAreaWeightPos];
                current.GeneralInformation[generalDensityPos] = entry[manDensityPos];
                current.GeneralInformation[generalThicknessPos] = entry[manThicknessPos];
                current.GeneralInformation[generalConversionToKgPos] = entry[manConversionToKgPos];
                current.GeneralInformation[generalLengthMassPos] = entry[manLengthMassPos];
                current.ChangesToEntry += "Manually tried to update conversions, ";
            }
            //correct units with the now updated (only for entries with wrong unit)
            CorrectionHandler.CorrectReferenceValue(indicesWrongUnit, allEntries, generalUnitPos, generalValuePos, generalConversionToKgPos);
            CorrectionHandler.CorrectUnitThroughConversion(indicesWrongUnit, allEntries, generalCategoryPosition, generalUnitPos, generalValuePos, generalConversionToKgPos, generalDensityPos, generalThicknessPos, generalAreaWeightPos, generalBulkDensityPos, generalLengthMassPos);
        }

        /***
        Adds thickness to all entries with qm as unit
        :param indices: all indices for considered entries in allEntries
        :param allEntries: List of all SingleModEntries
        :param pathManualThicknessCompletion: path to csv file with manual thickness completion
        :param structure: Oekobaudat structure for export
        :param generalUUIDPos: position of UUID in general Information of SingleModEntries
        :param generalUnitPosition: position of reference Unit in general Information of SingleModEntries
        :param generalThicknessPos: position of thickness in general Information of SingleModEntries
        ***/
        public static void ManualThicknessCompletion(
            List<int> indices,
            List<SingleModEntry> allEntries,
            string pathManualThicknessCompletion,
            StructureOekobaudat structure,
            int generalUUIDPos,
            int generalUnitPos,
            int generalThicknessPos)
        {
            //Read information from external file
            var header = CsvImportHandler.ReadHeader(pathManualThicknessCompletion);
            var entries = CsvImportHandler.ReadFromCSV(pathManualThicknessCompletion, false, Encoding.UTF8);

            //relevant positions in header resp. entries from external file
            int manUUIDPos = header.FindIndex(n => n == "UUID");
            int manThicknessPos = header.FindIndex(n => n == "Schichtdicke (m)");
            int manEstimatedThicknessPos = header.FindIndex(n => n.Contains("EstimatedThickness"));

            //save all entries that need thickness
            var indicesEntriesNeedThickness = new List<int>();

            //go through all entries and add estimated or real thickness if no thickness present
            for (int j = 0; j < indices.Count; j++)
            {
                //find complementing entry
                var current = allEntries[indices[j]];
                if (current.GeneralInformation[generalUnitPos] == "qm" && current.GeneralInformation[generalThicknessPos] == "")
                {
                    var entry = entries.Find(n => n[manUUIDPos] == current.GeneralInformation[generalUUIDPos]);
                    //check if successfully found complementing entry, else continue with next entry
                    if (entry == null)
                    {
                        indicesEntriesNeedThickness.Add(indices[j]);
                        continue; 
                    }

                    //add thickness
                    if (entry[manThicknessPos] != "")
                    {
                        current.GeneralInformation[generalThicknessPos] = entry[manThicknessPos];
                        current.ChangesToEntry += "Manually added thickness (exact value), ";
                    }
                    else if (entry[manEstimatedThicknessPos] != "")
                    {
                        current.EstimatedThickness = entry[manEstimatedThicknessPos];
                        current.ChangesToEntry += "Manually added estimated thickness (for thermal calculations only), ";
                    }
                    else
                    {
                        indicesEntriesNeedThickness.Add(indices[j]);
                    }
                }
            }
            CsvExportHandler.ExportPositionsMult("NeedThickness", indicesEntriesNeedThickness, allEntries, structure);
        }
    }
}
