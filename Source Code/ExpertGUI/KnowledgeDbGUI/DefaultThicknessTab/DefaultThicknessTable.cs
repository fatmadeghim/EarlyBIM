using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Forms;

using KnowledgeDB;
using GUIHelpFunctions;

namespace KnowledgeDbGUI.DefaultThicknessTab
{
    public class DefaultThicknessTable : GenericThicknessTable.GenericThicknessTable<DefaultThicknessTableRow, DefaultThicknessRange>
    {
        public DefaultThicknessTable(TableLayoutPanel table) : base(table) { }

        public override void BuildHeadline()
        {
            Table.Controls.Add(FormsHelper.CreateTextBox("Row", 40), 0, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("KG3xx", 100), 1, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("LayerType", 100), 2, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Min Thickness [m]", 200), 3, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Std Thickness [m]", 200), 4, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Max Thickness [m]", 200), 5, 0);
        }

        public override void BuildTable(KnowledgeDB.KnowledgeContext context)
        {
            /*
            //Following Code puts 0m RangeEntries on top
            var rangesZero = (from range in context.DefaultThicknessRanges
                              where range.ThicknessMin == 0
                              orderby range.KG3xxName
                              select range
                          ).ToList();
            foreach (var range in rangesZero)
            {
                rows.Add(new DefaultThicknessTableRow(range, context, changedRows));
            }
            var rangesNonZero = (from range in context.DefaultThicknessRanges
                                 where range.ThicknessMin != 0
                                 orderby range.KG3xxName
                                 select range
                          ).ToList();
            foreach (var range in rangesNonZero)
            {
                rows.Add(new DefaultThicknessTableRow(range, context, changedRows));
            }
            */
            var ranges = (from range in context.DefaultThicknessRanges
                          orderby range.KG3xxName
                          select range).ToList();
            foreach (var range in ranges)
            {
                rows.Add(new DefaultThicknessTableRow(range, context, changedRows));
            }

            var rowcounter = 1;
            foreach (var row in rows)
            {
                row.AddToTable(Table, rowcounter);
                rowcounter++;
            }
        }
    }
}
