using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using KnowledgeDB;
using GUIHelpFunctions;


namespace KnowledgeDbGUI.GenericThicknessTable
{
    public abstract class GenericThicknessTableRow<T> where T:IRange
    {
        public T ThicknessObject { get; set; }
        protected TextBox RowNumberTextBox;
        protected TextBox KG3xxNameTextBox;
        protected TextBox LayerTypeTextBox;
        protected TextBox MinThicknessTextBox;
        protected TextBox AvgThicknessTextBox;
        protected TextBox MaxThicknessTextBox;
        protected double minThickness;
        protected double avgThickness;
        protected double maxThickness;

        public GenericThicknessTableRow (T thicknessObject, KnowledgeContext context)
        {
            this.ThicknessObject = thicknessObject;
            RowNumberTextBox = FormsHelper.CreateTextBox("", 30);
            KG3xxNameTextBox = FormsHelper.CreateTextBox(context.KG3xxNames.Where(name => name.Id == thicknessObject.KG3xxNameId).First().Name, 60);
            LayerTypeTextBox = FormsHelper.CreateTextBox(context.LayerTypeNames.Where(name => name.Id == thicknessObject.LayerTypeNameId).First().Name, 60);

            MinThicknessTextBox = FormsHelper.CreateTextBox(thicknessObject.getMin().ToString(), 200);
            AvgThicknessTextBox = FormsHelper.CreateTextBox(thicknessObject.getAvg().ToString(), 200);
            MaxThicknessTextBox = FormsHelper.CreateTextBox(thicknessObject.getMax().ToString(), 200);

            MinThicknessTextBox.ReadOnly = false;
            AvgThicknessTextBox.ReadOnly = false;
            MaxThicknessTextBox.ReadOnly = false;
        }

        public virtual void AddToTable(TableLayoutPanel table, int row)
        {
            RowNumberTextBox.Text = row.ToString();
            table.Controls.Add(RowNumberTextBox, 0, row);
            table.RowCount++;
            table.Controls.Add(KG3xxNameTextBox, 1, row);
            table.Controls.Add(LayerTypeTextBox, 2, row);
            table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, FormsHelper.GetDefaultHeight()));
        }

        public virtual void DeleteFromTable(TableLayoutPanel table)
        {
            table.Controls.Remove(RowNumberTextBox);
            table.Controls.Remove(KG3xxNameTextBox);
            table.Controls.Remove(LayerTypeTextBox);
            table.Controls.Remove(MinThicknessTextBox);
            table.Controls.Remove(AvgThicknessTextBox);
            table.Controls.Remove(MaxThicknessTextBox);
            table.RowCount--;
        }

        public virtual void PrepareChanges()
        {
            //Look for wrong decimal separators
            foreach(var text in new List<string> { MinThicknessTextBox.Text, AvgThicknessTextBox.Text, MaxThicknessTextBox.Text })
                {
                    if (text.Contains('.') && text.Contains(','))
                    {
                        throw new Exception("Unclear decimal separation: Usage of . and , in the same textbox");
                    }
                    if ((text.Contains('.') || text.Contains(',')) && !text.Contains(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))
                    {
                    throw new Exception("Wrong decimal separation: Please use your system's decimal separator ('"
                        + CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "')");
                    }
                }
            try
            {
                minThickness = Double.Parse(MinThicknessTextBox.Text);
                avgThickness = Double.Parse(AvgThicknessTextBox.Text);
                maxThickness = Double.Parse(MaxThicknessTextBox.Text);
            }
            catch
            {
                throw new Exception("Can't convert thicknesses into number");
            }
            if (minThickness <= avgThickness && avgThickness <= maxThickness)
            {
                return;
            }
            else
            {
                throw new Exception("Thicknessvalues not in order Min<=Avg<=Max");
            }
        }

        public virtual void WriteChanges()
        {
            ThicknessObject.setMin(minThickness);
            ThicknessObject.setAvg(avgThickness);
            ThicknessObject.setMax(maxThickness);
        }
    }
}
