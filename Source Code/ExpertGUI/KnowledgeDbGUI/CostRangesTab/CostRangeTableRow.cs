using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using KnowledgeDB;
using GUIHelpFunctions;

namespace KnowledgeDbGUI.CostRangesTab
{
    public class CostRangeTableRow : GenericThicknessTable.GenericThicknessTableRow<CostRange>
    {
        private TextBox LayerTextBox;
        private TextBox UnitTextBox;
        private TextBox ExposureQualityTextBox;

        public CostRangeTableRow(CostRange thicknessObject, KnowledgeContext context, List<CostRangeTableRow> changedRows)
            : base(thicknessObject, context)
        {
            LayerTextBox = FormsHelper.CreateTextBox(context.Layers.Where(name => name.Id == thicknessObject.LayerId).FirstOrDefault().Name, 60, 20);
            UnitTextBox = FormsHelper.CreateTextBox(context.FindUnitOfLayer(context.Layers.Where(name => name.Id == thicknessObject.LayerId)
                          .FirstOrDefault()).ReferenceUnit, 80, 20);
            ExposureQualityTextBox = FormsHelper.CreateTextBox(ThicknessObject.HasExposureQuality.ToString(), 90, 20);

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
            table.Controls.Add(LayerTextBox, 3, row);
            table.Controls.Add(UnitTextBox, 4, row);
            table.Controls.Add(ExposureQualityTextBox, 5, row);
            table.Controls.Add(MinThicknessTextBox, 6, row);
            table.Controls.Add(AvgThicknessTextBox, 7, row);
            table.Controls.Add(MaxThicknessTextBox, 8, row);
        }

        public override void DeleteFromTable(TableLayoutPanel table)
        {
            base.DeleteFromTable(table);
            table.Controls.Remove(LayerTextBox);
            table.Controls.Remove(UnitTextBox);
            table.Controls.Remove(ExposureQualityTextBox);
        }
    }
}

