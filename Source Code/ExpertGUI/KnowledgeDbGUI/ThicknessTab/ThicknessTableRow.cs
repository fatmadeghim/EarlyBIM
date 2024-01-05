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
    public class ThicknessTableRow : GenericThicknessTable.GenericThicknessTableRow<ThicknessRange>
    {
        private TextBox LayerTextBox;
        private TextBox referenceTextBox;

        public ThicknessTableRow(ThicknessRange thicknessObject, KnowledgeContext context, List<ThicknessTableRow> changedRows)
            :base(thicknessObject, context)
        {
            LayerTextBox = FormsHelper.CreateTextBox(context.Layers.Where(name => name.Id == thicknessObject.LayerId).FirstOrDefault().Name, 60, 20);
            referenceTextBox = FormsHelper.CreateTextBox(context.FindUnitOfLayer(context.Layers.Where(
                                                                          name => name.Id == thicknessObject.LayerId).FirstOrDefault()).ReferenceUnit, 90, 20);

            var rowChangeHandler = new EventHandler((object obj, EventArgs e) => { if (!changedRows.Contains(this))
                                                                                            { changedRows.Add(this); }; });

            MinThicknessTextBox.TextChanged += rowChangeHandler;
            AvgThicknessTextBox.TextChanged += rowChangeHandler;
            MaxThicknessTextBox.TextChanged += rowChangeHandler;
        }

        public override void AddToTable(TableLayoutPanel table, int row)
        {
            base.AddToTable(table, row);
            table.Controls.Add(LayerTextBox, 3, row);
            table.Controls.Add(referenceTextBox, 4, row);
            table.Controls.Add(MinThicknessTextBox, 5, row);
            table.Controls.Add(AvgThicknessTextBox, 6, row);
            table.Controls.Add(MaxThicknessTextBox, 7, row);
        }

        public override void DeleteFromTable(TableLayoutPanel table)
        {
            base.DeleteFromTable(table);
            table.Controls.Remove(LayerTextBox);
            table.Controls.Remove(referenceTextBox);
        }

        public override void WriteChanges()
        {
            base.WriteChanges();
            ThicknessObject.IsDefault = false; //Disconnects this object from its default
        }
    }
}
