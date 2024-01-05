using System;
using System.Collections.Generic;
using System.Text;

namespace DataConverter
{
    public class FilterHandler
    {

        /***
            Filter all empty entries (no entry != 0)
            :param allEntries: reference to List of all SingleModEntries
        ***/
        public static void FilterEmptyEntries(ref List<SingleModEntry> allEntries)
        {
            double tolerance = 1e-12;
            for (int i = 0; i < allEntries.Count; i++)
            {
                var current = allEntries[i];
                //check if all indicators of current are zero, then delete it allEntries
                bool indicatorsCompletelyZero = true;
                for (int j = 0; j < current.Indicators.Count; j++)
                {
                    if (Math.Abs(current.Indicators[j]) > tolerance)
                    {
                        indicatorsCompletelyZero = false;
                        break;
                    }
                }

                if (indicatorsCompletelyZero)
                {
                    allEntries.RemoveAt(i);
                    i = -1;
                }
            }
        }//FilterEmptyEntries

        /***
            This function filters (removes) unapplicable entries from the positionKG300 list
            :param categoryMaps: list of all Category Maps
            :param uuidMaps: List of uuid maps
            :param positionKG300: positions of all KG300 entries in the complete list of SingleModEntries
            :param entries: List of all SingleModEntry
            :param catPosition: position of category in general information of SingleModEntry
            :param uuidPosition: position of uuid in general information of SingleModEntry
            :param structure: structure of Oekobaudat (for exporting/testing only)
        ***/
        public static void FilterUnapplicableEntries(List<CategoryMap> categoryMaps, List<UUIDMap> uuidMaps, ref List<int> positionKG300, List<SingleModEntry> entries, int catPosition, int uuidPosition, StructureOekobaudat structure)
        {
            //find unapplicable category maps
            var unapplicableCategoryMaps = categoryMaps.FindAll(n => n.UsefulInEarlyDesignPhases == false);
            //Remove irrelevant Entries (not applicable in early design phase)
            //for testing: add unapplicable indices to new list
            var unapplicable = new List<int>();
            for (int i = 0; i < positionKG300.Count; i++)
            {
                //try to find current category in unapplicableMaps
                string currentCategory = entries[positionKG300[i]].GeneralInformation[catPosition];
                CategoryMap map = unapplicableCategoryMaps.Find(n => n.Category.Equals(currentCategory));
                //if found, remove index from positions

                if (map != null)
                {
                    unapplicable.Add(positionKG300[i]);
                    positionKG300.RemoveAt(i);
                    i--;
                }
            }
            //find unapplicable uuid maps
            var unapplicableUUIDMaps = uuidMaps.FindAll(n => n.UsefulInEarlyDesignPhases == false);
            //Remove irrelevant Entries (not applicable in early design phase)
            for (int i = 0; i < positionKG300.Count; i++)
            {
                //try to find current category in unapplicableMaps
                string currentUUID = entries[positionKG300[i]].GeneralInformation[uuidPosition];
                UUIDMap map = unapplicableUUIDMaps.Find(n => n.UUID.Equals(currentUUID));
                //if found, remove index from positions
                if (map != null)
                {
                    unapplicable.Add(positionKG300[i]);
                    positionKG300.RemoveAt(i);
                    i--;
                }
            }
            //export for testing
            //ExportEntries.ExportPosition("UnapplicableEntries", unapplicable, allEntries, structure);
        }
    }
}
