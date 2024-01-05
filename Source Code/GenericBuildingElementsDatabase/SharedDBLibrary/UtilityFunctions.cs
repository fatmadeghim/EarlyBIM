using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SharedDBLibrary
{
    public static class UtilityFunctions
    {
        //returns a copy of unorderedList, ordered by positions
        public static List<T> OrderListByPositionList<T>(List<T> unorderedList, List<int> positions)
        {
            if (unorderedList.Count != positions.Count)
            {
                throw new Exception("Number of Items unequal to number of positions");
            }
            var orderedList = new List<T>();
            for (int i = 0; i < positions.Count; i++)
            {
                orderedList.Add(unorderedList[positions.IndexOf(i + 1)]);
            }
            return orderedList;
        }

        public static bool ContainsSameIds<T>(this List<T> listA, List<T> listB) where T: IHasID
        {
            //Get Ids of ListB
            var idListB = listB.Select(a => a.Id).ToList();

            foreach(var ele in listA)
            {
                if (!idListB.Contains(ele.Id))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
