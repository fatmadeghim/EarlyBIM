using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using KnowledgeDB;

namespace KnowledgeDbGUI.DefaultThicknessTab
{
    public class DefaultThicknessTableRow : GenericThicknessTable.GenericThicknessTableRow<DefaultThicknessRange>
    {
        protected KnowledgeContext context;

        public DefaultThicknessTableRow(DefaultThicknessRange thicknessObject, KnowledgeContext context, List<DefaultThicknessTableRow> changedRows) 
            : base(thicknessObject, context) 
        {
            this.context = context;
            var rowChangeHandler = new EventHandler((object obj, EventArgs e) => {
                if (!changedRows.Contains(this))
                { changedRows.Add(this); };
            });

            MinThicknessTextBox.TextChanged += rowChangeHandler;
            AvgThicknessTextBox.TextChanged += rowChangeHandler;
            MaxThicknessTextBox.TextChanged += rowChangeHandler;
        }

        public override void AddToTable(TableLayoutPanel table, int row)
        {
            base.AddToTable(table, row);
            table.Controls.Add(MinThicknessTextBox, 3, row);
            table.Controls.Add(AvgThicknessTextBox, 4, row);
            table.Controls.Add(MaxThicknessTextBox, 5, row);
        }

        public override void WriteChanges()
        {
            //Enter changes to DB
            base.WriteChanges();

            //Change ThicknessRangeObjects that are set to their DefaultValue
            foreach (var thicknessRange in context.ThicknessRanges.Where(tr => tr.KG3xxNameId == ThicknessObject.KG3xxNameId &&
                                                                               tr.LayerTypeNameId == ThicknessObject.LayerTypeNameId &&
                                                                               tr.IsDefault == true).ToList())
            {
                thicknessRange.ThicknessMin = minThickness;
                thicknessRange.ThicknessAverage = avgThickness;
                thicknessRange.ThicknessMax = maxThickness;
            }

        }
    }
}
