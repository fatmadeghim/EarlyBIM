using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConverter
{
    public class MultipleHandler
    {
        /***
            Finds all multiples in the given PositionList (which refers to the positions in List<SingleModEntry>) and returns them as Key Value pair (key = first index, values = multiple indices)
            :param completePositionList: List of int with all positions that shall be inspected from all entries
            :param UUIDpos: position of UUID in GeneralInformation of a SingleModEntry
            :param entries: List of all SingleModEntries (needs to belong to completePositionList)
            :return: List of KeyValuePairs with multiples
        ***/
        public static List<KeyValuePair<int, List<int>>> FindMultiples(List<int> completePositionList, int UUIDpos, List<SingleModEntry> entries)
        {
            var result = new List<KeyValuePair<int, List<int>>>();
            
            //go through each entry and if uuid is same as next then add as multiple
            for (int i = 0; i < completePositionList.Count() - 1; i++)
            {
                var current = entries[completePositionList[i]];
                var next = entries[completePositionList[i+1]];
                if (current.GeneralInformation[UUIDpos] == next.GeneralInformation[UUIDpos])
                {
                    var key = completePositionList[i];
                    var value = new List<int>();
                    while(current.GeneralInformation[UUIDpos] == next.GeneralInformation[UUIDpos] && i < completePositionList.Count() - 2)
                    {
                        value.Add(completePositionList[i + 1]);
                        i++;
                        current = entries[completePositionList[i]];
                        next = entries[completePositionList[i + 1]];
                    }
                    if(current.GeneralInformation[UUIDpos] == next.GeneralInformation[UUIDpos] && i == completePositionList.Count() - 2)
                    {
                        value.Add(completePositionList[i+1]);
                    }
                    var multiple = new KeyValuePair<int, List<int>>(key, value);
                    result.Add(multiple);
                }
            }
            return result;
        }//FindMultiples
    }//Multiples
}//DataConverter
