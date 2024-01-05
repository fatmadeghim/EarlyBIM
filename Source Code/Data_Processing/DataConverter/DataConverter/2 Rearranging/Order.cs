using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConverter
{
    public class Order
    {
        /***
            This function eliminates the duplicates in the list of positionKG300 (which points at the singleModEntries)
            :param singleModEntries: list with all singleModEntries in Oekobaudat
            :param positionKG300: list with indices to singleModEntries that belong to KG300
            :param UUIDPos: position of uuid in general position of SingleModEntry
            :return: list with filtered indices
        ***/
        public static List<int> EliminateDuplicates(List<SingleModEntry> singleModEntries, List<int> positionKG300, int UUIDPos)
        {
            //check if positionKG300 has entries (= is valid)
            if(positionKG300.Count <= 0)
            {
                throw new InvalidOperationException("No entries in list");
            }

            //add first entry if (resp. "as") positionsKG300 not empty
            var filteredPositions = new List<int> 
            {
                positionKG300[0]
            };

            //iterate through all other positions in positionKG300
            for (int i = 1; i < positionKG300.Count; i++)
            {
                var currentPos = positionKG300[i];
                
                //add entries only if not already existent (check if UUID of last entry in filteredPositions is the same as the UUID of the current entry)
                if (singleModEntries[currentPos].GeneralInformation[UUIDPos] != singleModEntries[filteredPositions.Last()].GeneralInformation[UUIDPos])
                {
                    filteredPositions.Add(currentPos);
                }
            }
            return filteredPositions;
        }

        /***
            This function returns only positions with correct unit
            :param allEntries: list with all singleModEntries in Oekobaudat
            :param positions: list with indices to singleModEntries that belong to KG300
            :param unitPosition: position of unit in GeneralInformation
            :param valuePosition: position of value in GeneralInformation
            :param categoryPosition: position of category in GeneralInformation
            :return: list with filtered indices
        ***/
        public static List<int> GetEntriesCorrectUnit(List<SingleModEntry> allEntries, List<int> positions, int unitPosition, int valuePosition, int categoryPosition)
        {
            List<int> positionsCorrectUnit = new List<int>();
            foreach (var p in positions)
            {
                SingleModEntry entry = allEntries[p];
                bool correctUnit = CorrectUnitHandler.CheckCorrectUnit(entry, valuePosition, unitPosition, categoryPosition);
                if (correctUnit)
                {
                    positionsCorrectUnit.Add(p);
                }
            }
            return positionsCorrectUnit;
        }
        /***
            This function returns only positions with wrong unit
            :param singleModEntries: list with all singleModEntries in Oekobaudat
            :param positionKG300: list with indices to singleModEntries that belong to KG300
            :param unitPosition: position of unit in GeneralInformation
            :param valuePosition: position of value in GeneralInformation
            :param categoryPosition: position of category in GeneralInformation
            :return: list with filtered indices
        ***/
        public static List<int> GetEntriesWrongUnit(List<SingleModEntry> allEntries, List<int> positions, int unitPosition, int valuePosition, int categoryPosition)
        {
            List<int> positionsCorrectUnit = GetEntriesCorrectUnit(allEntries, positions, unitPosition, valuePosition, categoryPosition);
            var positionsWrongUnit = positions.Except(positionsCorrectUnit).ToList();
            return positionsWrongUnit;
        }
    }//Order
}//Namespace DataConverter
