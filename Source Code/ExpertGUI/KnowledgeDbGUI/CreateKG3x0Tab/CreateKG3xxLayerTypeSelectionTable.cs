using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Forms;

using KnowledgeDB;
using SharedDBLibrary;
using GUIHelpFunctions;

namespace KnowledgeDbGUI.CreateKG3x0Tab
{
    class CreateKG3xxLayerTypeSelectionTable
    {
        private class CreateKG3xxLayerTypeSelectionTableRow
        {
            public TextBox LayerTypeTextbox { get; set; }
            public Button AddButton { get; set; }
            public CreateKG3xxLayerTypeSelectionTableRow(string layertypename, CreateKG3xxOrderTable orderTable)
            {
                //LayerTypeTextbox = FormsHelper.CreateTextBox(layertypename, 100, 20);
                LayerTypeTextbox = FormsHelper.CreateTextBox(layertypename);
                LayerTypeTextbox.Multiline = false;
                //LayerTypeTextbox.MaximumSize = new System.Drawing.Size(100000000, 20); //Allows the textbox to grow infinitely in length, but not in height

                AddButton = FormsHelper.CreateButton("Add");
                AddButton.Click += new EventHandler((object sender, EventArgs e) => { orderTable.AddLayerType(layertypename);
                                                                                      orderTable.UpdateLayerTypeNumber(); });
            }
        }

        public TableLayoutPanel Table { get; set; }
        private List<CreateKG3xxLayerTypeSelectionTableRow> rows;

        public CreateKG3xxLayerTypeSelectionTable()
        {
            Table = FormsHelper.CreateTable(2, 1);
            Table.AutoScroll = true;
            Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0F));
            Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0F));
            rows = new List<CreateKG3xxLayerTypeSelectionTableRow>();
        }

        public void ShowLayerTypeNames(List<string> layertypenames, CreateKG3xxOrderTable orderTable)
        {
            var rowcount = 0;
            foreach (var layertypename in layertypenames)
            {
                var row = new CreateKG3xxLayerTypeSelectionTableRow(layertypename, orderTable);
                Table.Controls.Add(row.LayerTypeTextbox, 0, rowcount);
                Table.Controls.Add(row.AddButton, 1, rowcount);
                Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
                rowcount++;
                rows.Add(row);
            }
        }
    }
}
