using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;

using KnowledgeDB;
using GUIHelpFunctions;

namespace KnowledgeDbGUI.CostRangesTab
{
    public class CostRangeTable : GenericThicknessTable.GenericThicknessTable<CostRangeTableRow, CostRange>
    {

        public CostRangeTable(TableLayoutPanel table) : base(table) { }
        public override void BuildHeadline()
        {
            Table.Controls.Add(FormsHelper.CreateTextBox("Row", 40, 20), 0, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("KG3xx", 100, 20), 1, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("LayerType", 100, 20), 2, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Layer", 100, 20), 3, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Unit", 80, 20), 4, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Exposure Quality", 100, 20), 5, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Min Cost [€ / Unit]", 200, 20), 6, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Std Cost [€ / Unit]", 200, 20), 7, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Max Cost [€ / Unit]", 200, 20), 8, 0);
        }

        public override void BuildTable(KnowledgeDB.KnowledgeContext context)
        {
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
            var ranges = (from range in context.CostRanges
                          orderby range.KG3xxName
                          select range).ToList();
            foreach (var range in ranges)
            {
                rows.Add(new CostRangeTableRow(range, context, changedRows));
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
