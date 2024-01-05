using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using KnowledgeDB;

namespace KnowledgeDbGUI.ViewTab
{
    public class ViewTable
    {
        public TableLayoutPanel Table { get; set; }
        public List<ViewTableKG3x0Table> KG3x0Tables { get; set; }
        private Action<KG3x0Option> importAction;

        public ViewTable(TableLayoutPanel table, Action<KG3x0Option> importAction)
        {
            this.Table = table;
            this.importAction = importAction;
            KG3x0Tables = new List<ViewTableKG3x0Table>();
        }

        public void BuildTable(KnowledgeContext context, List<string> kg3x0Filter, List<string> cTypeFilter, EventHandler resetEventHandler)
        {
            var kg3x0names = context.KG3x0Names.Where(kgname => kg3x0Filter.Contains(kgname.Name)).ToList();
            foreach(var name in kg3x0names)
            {
                var subTable = new ViewTableKG3x0Table(name.Name);
                subTable.BuildTable(context, cTypeFilter, resetEventHandler, importAction);
                KG3x0Tables.Add(subTable);
            }
        }

        public void Show()
        {
            var rowcounter = 0;
            foreach (var subtable in KG3x0Tables)
            {
                subtable.Show(Table, rowcounter);
                rowcounter++;
            }
        }

        public void Clear()
        {
            Table.Controls.Clear();
            KG3x0Tables.Clear();
        }

    }
}
