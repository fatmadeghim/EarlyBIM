using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUIHelpFunctions
{
    public static class UtilityFunctions
    {
        public static List<string> GetStringsFromItems (this CheckedListBox.CheckedItemCollection checkedItems)
        {
            var stringlist = new List<string>();
            foreach (var item in checkedItems)
            {
                stringlist.Add(item.ToString());
            }
            return stringlist;
        }
    }
}
