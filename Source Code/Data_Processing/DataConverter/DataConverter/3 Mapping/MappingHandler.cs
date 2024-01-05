using System;
using System.Collections.Generic;
using System.Text;

namespace DataConverter
{
    public class MappingHandler
    {
        /***
        Maps the correct Layers and KGs to each oekobaudatEntries based on the category (from categoryMaps)
        :param oekobaudatEntries: List of all oekobaudatEntries (passed as ref to make clear that it gets changed inside, but wouldn't be necessary as passed as pointer anyways)
        :param categoryMaps: List of CategoryMaps for correct assignment of Layers and KGs
        :param catPos: int with index of CategoryPosition of General Information
        ***/
        public static void MapOekobaudatEntryToCategoryMap(ref List<OekobaudatEntry> oekobaudatEntries, List<CategoryMap> categoryMaps, int catPos)
        {
            for (int i = 0; i < oekobaudatEntries.Count; i++) 
            {
                OekobaudatEntry entry = oekobaudatEntries[i];
                //try to find current category in categoryMaps
                string currentCategory = entry.GeneralInformation[catPos];
                CategoryMap map = categoryMaps.Find(n => n.Category.Equals(currentCategory));

                //if not found, throw error
                if (map == null)
                {
                    throw new KeyNotFoundException("Couldn't find the respective category");
                }

                //shouldn't happen, but check anyway
                if(map.UsefulInEarlyDesignPhases == false)
                {
                    //wrong error type but couldn't find any more suitable
                    throw new MissingMemberException("This should never happen, but still happened that a entry is in OekobaudatEntries that is not applicable in EarlyPhases");
                }

                //if not manualMapping, add Layers and KGs
                if (!map.ManualMapping)
                {
                    entry.LayerTypes = map.Layers;
                    entry.KGs = map.KGs;
                }
            }
        }

        /***
        Maps the correct Layers and KGs to each oekobaudatEntries based on the UUID from manual Mapping
        :param oekobaudatEntries: List of all oekobaudatEntries (passed as ref to make clear that it gets changed inside, but wouldn't be necessary as passed as pointer anyways)
        :param UUIDMaps: List of UUIDMaps for correct assignment of Layers and KGs
        :param UUIDPos: int with index of UUIDPosition of General Information
        ***/
        public static void ManualMapOekobaudatEntry(ref List<OekobaudatEntry> oekobaudatEntries, List<UUIDMap> uuidMaps, int UUIDPos, StructureOekobaudat structure)
        {
            var entriesManualMapping = new List<OekobaudatEntry>();
            for (int i = 0; i < oekobaudatEntries.Count; i++)
            {
                OekobaudatEntry entry = oekobaudatEntries[i];
                if(entry.KGs == null || entry.LayerTypes == null)
                {
                    //try to find current UUID in uuidMaps
                    string currentUUID = entry.GeneralInformation[UUIDPos];
                    var map = uuidMaps.Find(n => n.UUID.Equals(currentUUID));
                    if (map != null && map.UsefulInEarlyDesignPhases == true)
                    {
                        entry.LayerTypes = map.Layers;
                        entry.KGs = map.KGs;
                    }
                    else if (map != null && map.UsefulInEarlyDesignPhases == false)
                    {
                        oekobaudatEntries.Remove(entry);
                    }
                    else
                    {
                        entriesManualMapping.Add(entry);
                    }
                }
            }
            CsvExportHandler.ExportOekobaudatEntries("ManualMappingUUIDToLayerType", entriesManualMapping, structure);
        }

        /***
        Mapping of thermal conductivity according to Category
        :param entries: reference to List of all oekobaudatEntries
        :param pathThermalConductivity: path to thermal conductivity file (csv)
        :param catPos: position of category in general information of oekobaudat entries
        :param unitPos: position of unit in general information of oekobaudat entries
        :param thicknessPos: position of thickness in general information of oekobaudat entries
        :param structure: StructureOekobaudat for export
        ***/
        public static void MapThermalConductivity(ref List<OekobaudatEntry> entries, string pathThermalConductivity, int catPos, int unitPos, int thicknessPos, StructureOekobaudat structure)
        {
            //get thermal conductivity maps
            var thermalConductivityMaps = CsvImportHandler.ReadThermalConductivityMaps(pathThermalConductivity);

            //save all entries that need more information here
            var thicknessNeeded = new List<OekobaudatEntry>();

            //go through all entries
            foreach(var entry in entries)
            {
                //try to find current category in categoryMaps
                string currentCategory = entry.GeneralInformation[catPos];
                var map = thermalConductivityMaps.Find(n => n.Category.Equals(currentCategory));

                //if not found, throw error
                if (map == null)
                {
                    throw new KeyNotFoundException("Couldn't find this category in " + pathThermalConductivity + ": " + currentCategory);
                }

                //always assign value (is 0.0 if thermal conductivity not relevant)
                entry.ThermalConductivity = map.ThermalConductivity;

                if (entry.GeneralInformation[unitPos] == "qm" && map.RelevanceOfThermalConductivity == true)
                {
                    if(entry.GeneralInformation[thicknessPos] != string.Empty)
                    {
                        double thickness = ConversionHandler.ConvertStringToDouble(entry.GeneralInformation[thicknessPos]);
                        entry.ReverseOfR = entry.ThermalConductivity / thickness;
                    }
                    else if(entry.EstimatedThickness != null && entry.EstimatedThickness != string.Empty)
                    {
                        double thickness = ConversionHandler.ConvertStringToDouble(entry.EstimatedThickness);
                        entry.ReverseOfR = entry.ThermalConductivity / thickness;
                    }
                    else
                    {
                        thicknessNeeded.Add(entry);
                    }
                }
                else if(entry.GeneralInformation[unitPos] == "qm" && map.RelevanceOfThermalConductivity == false)
                {
                    entry.ReverseOfR = 0.0;
                }
            }
            CsvExportHandler.ExportOekobaudatEntries("EntriesNeedThicknessForUValue", thicknessNeeded, structure);
        }
    }
}