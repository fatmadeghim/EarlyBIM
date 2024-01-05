using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConverter
{
    public class SortingHandler
    {
        /***
            Sorts correct entries into KG300 (possibility to export also KG400 and B6)
            :param allEntries: List of all SingleModEntries
            :param modulePosition: position of module in general information of SinglModeEntry
            :param namePosition: position of name in general information of SinglModeEntry
            :param categoryPosition: position of category in general information of SinglModeEntry
            :param structure: StructureOekobaudat for export
            :return: List of int with all positions of KG300 in list of all singleModEntries
        ***/
        public static List<int> SortKG300(List<SingleModEntry> allEntries, int modulePosition, int namePosition, int categoryPosition, StructureOekobaudat structure)
        {
            List<int> PositionKG300 = new List<int>();
            List<int> PositionKG400 = new List<int>();
            List<int> PositionB6 = new List<int>();
            List<int> PositionB7 = new List<int>();

            //assign entries to different CSVs
            for (int i = 0; i < allEntries.Count(); i++)
            {
                var currentEntry = allEntries[i];
                var moduleCurrentEntry = currentEntry.GeneralInformation[modulePosition];
                var categoryCurrentEntry = currentEntry.GeneralInformation[categoryPosition];
                var nameCurrentEntry = currentEntry.GeneralInformation[namePosition];

                //if module "B6" or category "Sonstige / Energieträger - Bereitstellung frei Verbraucher / Strom"
                bool conditionsB6 = moduleCurrentEntry.Equals("B6") || categoryCurrentEntry.Equals("Sonstige / Energieträger - Bereitstellung frei Verbraucher / Strom");
                //if module "B7"
                bool conditionsB7 = moduleCurrentEntry.Equals("B7");
                //if category contains "rohre", "Gebäudetechnik", "Gussteile", "Schmiedeteile"
                bool conditionsKG400 = categoryCurrentEntry.ToLower().Contains("rohr") ||
                    nameCurrentEntry.ToLower().Contains("rohr") ||
                    categoryCurrentEntry.ToLower().Contains("gebäudetechnik") ||
                    categoryCurrentEntry.ToLower().Contains("gussteile") ||
                    categoryCurrentEntry.ToLower().Contains("schmiedeteile");
                //if module "A4", "A5", something from B or C1 or C2
                bool conditionsWrongModules = moduleCurrentEntry.Equals("A4") || moduleCurrentEntry.Equals("A5") ||
                        moduleCurrentEntry.Contains("B") ||
                        moduleCurrentEntry.Equals("C1") || moduleCurrentEntry.Equals("C2");

                //if all conditions wrong, then it is in KG300
                if (!conditionsB6 && !conditionsB7 && !conditionsKG400 && !conditionsWrongModules)
                {
                    PositionKG300.Add(i);
                }
                else if (conditionsKG400)
                {
                    PositionKG400.Add(i);
                }
                else if (conditionsB6)
                {
                    PositionB6.Add(i);
                }
                else if (conditionsB7)
                {
                    PositionB7.Add(i);
                }
            }
            CsvExportHandler.ExportPositionsMult("EntriesAfterSortingKG400", PositionKG400, allEntries, structure);
            CsvExportHandler.ExportPositionsMult("EntriesAfterSortingKG300", PositionKG300, allEntries, structure);
            CsvExportHandler.ExportPositionsMult("EntriesAfterSortingB6", PositionB6, allEntries, structure);
            CsvExportHandler.ExportPositionsMult("EntriesAfterSortingB7", PositionB7, allEntries, structure);

            return PositionKG300;
        }

        /***
            Removes the end of life entries from kG300Positions
            :param allEntries: List of all singleModEntries
            :param kG300Positions: List of indices to KG300 Entries in allEntries
            :param categoryPosition: index of category in GeneralInformation of a SingleModEntry
         ***/
        public static void RemoveEndOfLife(List<SingleModEntry> allEntries, List<int> kG300Positions, int categoryPosition)
        {
            for(int i = 0; i < kG300Positions.Count; i++)
            {
                var current = allEntries[kG300Positions[i]];
                if(current.GeneralInformation[categoryPosition].ToLower().Contains("end of life"))
                {
                    kG300Positions.RemoveAt(i);
                    i--;
                }
            }
        }
    }//SortEntries
}//DataConverter
