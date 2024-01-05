using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;

using KnowledgeDB;
using GUIHelpFunctions;

namespace KnowledgeDbGUI.ThicknessTab
{
    public class ThicknessTable : GenericThicknessTable.GenericThicknessTable<ThicknessTableRow, ThicknessRange>
    {

        public ThicknessTable(TableLayoutPanel table) : base(table) { }
        public override void BuildHeadline()
        {
            Table.Controls.Add(FormsHelper.CreateTextBox("Row", 40, 20), 0, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("KG3xx", 100, 20), 1,0);
            Table.Controls.Add(FormsHelper.CreateTextBox("LayerType", 100, 20), 2, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Layer", 100, 20), 3, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Ref. Unit", 100, 20), 4, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Min Thickness [m]", 200, 20), 5, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Std Thickness [m]", 200, 20), 6, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Max Thickness [m]", 200, 20), 7, 0);
        }

        public override void BuildTable(KnowledgeDB.KnowledgeContext context)
        {
            //following code would put ranges containing zeros at the top, commented out for now
            /*
            var rangesZero = (from range in context.ThicknessRanges
                              where range.ThicknessMin == 0
                              orderby range.KG3xxName
                              select range
                          ).ToList();
            foreach (var range in rangesZero)
            {
                rows.Add(new ThicknessTableRow(range, context, changedRows));
            }
            var rangesNonZero = (from range in context.ThicknessRanges
                              where range.ThicknessMin != 0
                              orderby range.KG3xxName
                              select range
                          ).ToList();
            foreach (var range in rangesNonZero)
            {
                rows.Add(new ThicknessTableRow(range, context, changedRows));
            }
            */
            var ranges = (from range in context.ThicknessRanges
                          orderby range.KG3xxName
                          select range).ToList();
            foreach (var range in ranges)
            {
                rows.Add(new ThicknessTableRow(range, context, changedRows));
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
