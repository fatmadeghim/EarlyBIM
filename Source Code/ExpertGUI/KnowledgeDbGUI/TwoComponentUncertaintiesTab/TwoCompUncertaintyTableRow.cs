using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using KnowledgeDB;
using GUIHelpFunctions;

namespace KnowledgeDbGUI.TwoComponentUncertaintiesTab
{
    public class TwoCompUncertaintyTableRow : GenericThicknessTable.GenericThicknessTableRow<TwoComponentUncertainty>
    {
        private TextBox Comp1TextBox;
        private TextBox Comp2TextBox;
        private TextBox MinPercentageTextBox;
        private TextBox AvgPercentageTextBox;
        private TextBox MaxPercentageTextBox;
        private double minPercentage;
        private double avgPercentage;
        private double maxPercentage;

        public TwoCompUncertaintyTableRow(TwoComponentUncertainty thicknessObject, KnowledgeContext context, List<TwoCompUncertaintyTableRow> changedRows)
            :base(thicknessObject, context)
        {
            var comp1Name = (from ltn in context.LayerTypeNames
                             join slt in context.StandardLayerTypes on ltn.Id equals slt.NameId
                             join tclt in context.TwoComponentLayerTypes on slt.Id equals tclt.Component1Id
                             where tclt.NameId == thicknessObject.LayerTypeNameId
                             select ltn.Name).FirstOrDefault();
            var comp2Name = (from ltn in context.LayerTypeNames
                             join slt in context.StandardLayerTypes on ltn.Id equals slt.NameId
                             join tclt in context.TwoComponentLayerTypes on slt.Id equals tclt.Component2Id
                             where tclt.NameId == thicknessObject.LayerTypeNameId
                             select ltn.Name).FirstOrDefault();

            Comp1TextBox = FormsHelper.CreateTextBox(comp1Name, 100);
            Comp2TextBox = FormsHelper.CreateTextBox(comp2Name, 100);

            MinPercentageTextBox = FormsHelper.CreateTextBox(thicknessObject.Component2PercentageMin.ToString(), 40);
            AvgPercentageTextBox = FormsHelper.CreateTextBox(thicknessObject.Component2PercentageAverage.ToString(), 40);
            MaxPercentageTextBox = FormsHelper.CreateTextBox(thicknessObject.Component2PercentageMax.ToString(), 40);

            MinPercentageTextBox.ReadOnly = false;
            AvgPercentageTextBox.ReadOnly = false;
            MaxPercentageTextBox.ReadOnly = false;

            var rowChangeHandler = new EventHandler((object obj, EventArgs e) => {
                if (!changedRows.Contains(this))
                { changedRows.Add(this); };
            });

            MinThicknessTextBox.TextChanged += rowChangeHandler;
            AvgThicknessTextBox.TextChanged += rowChangeHandler;
            MaxThicknessTextBox.TextChanged += rowChangeHandler;
            MinPercentageTextBox.TextChanged += rowChangeHandler;
            AvgPercentageTextBox.TextChanged += rowChangeHandler;
            MaxPercentageTextBox.TextChanged += rowChangeHandler;
        }

        public override void AddToTable(TableLayoutPanel table, int row)
        {
            base.AddToTable(table, row);
            table.Controls.Add(Comp1TextBox, 3, row);
            table.Controls.Add(Comp2TextBox, 4, row);
            table.Controls.Add(MinPercentageTextBox, 5, row);
            table.Controls.Add(AvgPercentageTextBox, 6, row);
            table.Controls.Add(MaxPercentageTextBox, 7, row);
            table.Controls.Add(MinThicknessTextBox, 8, row);
            table.Controls.Add(AvgThicknessTextBox, 9, row);
            table.Controls.Add(MaxThicknessTextBox, 10, row);
        }

        public override void DeleteFromTable(TableLayoutPanel table)
        {
            base.DeleteFromTable(table);
            table.Controls.Remove(MinPercentageTextBox);
            table.Controls.Remove(AvgPercentageTextBox);
            table.Controls.Remove(MaxPercentageTextBox);
            table.Controls.Remove(Comp1TextBox);
            table.Controls.Remove(Comp2TextBox);
        }

        public override void PrepareChanges()
        {
            base.PrepareChanges();
            try
            {
                minPercentage = Double.Parse(MinPercentageTextBox.Text);
                avgPercentage = Double.Parse(AvgPercentageTextBox.Text);
                maxPercentage = Double.Parse(MaxPercentageTextBox.Text);
            }
            catch
            {
                throw new Exception("Can't convert Percentages into number");
            }
            if (minPercentage <= avgPercentage && avgPercentage <= maxPercentage)
            {
                return;
            }
            else
            {
                throw new Exception("Percentagevalues not in order Min<=Avg<=Max");
            }
        }

        public override void WriteChanges()
        {
            base.WriteChanges();
            ThicknessObject.Component2PercentageMin = minPercentage;
            ThicknessObject.Component2PercentageAverage = avgPercentage;
            ThicknessObject.Component2PercentageMax = maxPercentage;
        }
    }
}
