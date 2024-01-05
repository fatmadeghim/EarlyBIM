using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Forms;

using GUIHelpFunctions;
using KnowledgeDB;

namespace KnowledgeDbGUI.TwoComponentUncertaintiesTab
{
    public class TwoCompUncertaintyTable : GenericThicknessTable.GenericThicknessTable<TwoCompUncertaintyTableRow, TwoComponentUncertainty>
    {
        public TwoCompUncertaintyTable(TableLayoutPanel table) : base(table) { }
        public override void BuildHeadline()
        {
            Table.Controls.Add(FormsHelper.CreateTextBox("Row", 40), 0, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("KG3xx", 100), 1, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("LayerType", 100), 2, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Comp1", 100), 3, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Comp2", 100), 4, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Comp2 Min %", 100), 5, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Comp2 Avg %", 100), 6, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Comp2 Max %", 100), 7, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Min Thickness [m]", 100), 8, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Std Thickness [m]", 100), 9, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Max Thickness [m]", 100), 10, 0);
        }

        public override void BuildTable(KnowledgeContext context)
        {
            /*
            var rangesZero = (from range in context.TwoComponentUncertainties
                              where range.ThicknessMin == 0
                              orderby range.KG3xxName
                              select range
              ).ToList();
            foreach (var range in rangesZero)
            {
                rows.Add(new TwoCompUncertaintyTableRow(range, context, changedRows));
            }
            var rangesNonZero = (from range in context.TwoComponentUncertainties
                                 where range.ThicknessMin != 0
                                 orderby range.KG3xxName
                                 select range
                          ).ToList();
            foreach (var range in rangesNonZero)
            {
                rows.Add(new TwoCompUncertaintyTableRow(range, context, changedRows));
            }
            */
            var ranges = (from range in context.TwoComponentUncertainties
                          orderby range.KG3xxName
                          select range).ToList();
            foreach (var range in ranges)
            {
                rows.Add(new TwoCompUncertaintyTableRow(range, context, changedRows));
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
