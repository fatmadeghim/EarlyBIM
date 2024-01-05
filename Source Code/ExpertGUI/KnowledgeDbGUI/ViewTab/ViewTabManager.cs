using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;

using KnowledgeDB;
using SharedDBLibrary;
using GUIHelpFunctions;

namespace KnowledgeDbGUI.ViewTab
{
    public class ViewTabManager
    {
        public ViewTable Table { get; set; }
        public KnowledgeContext Context { get; set; }
        public CheckedListBox KG3x0FilterCheckedList { get; set; }
        public CheckedListBox CTypeFilterCheckedList { get; set; }
        public ViewTabManager(TableLayoutPanel table, KnowledgeContext context, CheckedListBox kg3x0CheckedListBox, CheckedListBox cTypeCheckedListBox,
                              Action<KG3x0Option> importAction)
        {
            this.Table = new ViewTable(table, importAction);
            this.Context = context;
            this.KG3x0FilterCheckedList = kg3x0CheckedListBox;
            this.CTypeFilterCheckedList = cTypeCheckedListBox;

            //Reload the table when filters are changed
            KG3x0FilterCheckedList.SelectedIndexChanged += new System.EventHandler((object sender, EventArgs e) => ReLoadTable());
            CTypeFilterCheckedList.SelectedIndexChanged += new System.EventHandler((object sender, EventArgs e) => ReLoadTable());
        }
        public void LoadFilters()
        {
            var kgCounter = 0;
            foreach (var kg3x0 in Context.KG3x0Names)
            {
                KG3x0FilterCheckedList.Items.Add(kg3x0.getName());
                KG3x0FilterCheckedList.SetItemChecked(kgCounter, false);
                kgCounter++;
            }

            var cTypeCounter = 0;
            foreach (var constructionType in Context.ConstructionTypeNames)
            {
                CTypeFilterCheckedList.Items.Add(constructionType.getName());
                CTypeFilterCheckedList.SetItemChecked(cTypeCounter, false);
                cTypeCounter++;
            }
        }

        public void ReLoadTable()
        {
            Table.Clear();
            LoadTable();
            Table.Show();
        }

        public void LoadTable()
        {            
            Table.BuildTable(Context, KG3x0FilterCheckedList.CheckedItems.GetStringsFromItems(), CTypeFilterCheckedList.CheckedItems.GetStringsFromItems(),
                                new System.EventHandler((object sender, EventArgs e) => ReLoadTable()));
        }
    }
}
