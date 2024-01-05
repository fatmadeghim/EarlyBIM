using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using System.Diagnostics;

using KnowledgeDB;
using SharedDBLibrary;
using GUIHelpFunctions;

namespace KnowledgeDbGUI.CreateKG3x0Tab
{
    public class CreateKG3xxTableColumn
    {
        public KG3xxName Name { get; set; }
        private TextBox textbox;
        private ComboBox replacementOrderSelection;
        private CreateKG3xxLayerTypeSelectionTable createKG3XxLayerTypeSelectionTable;
        private CreateKG3xxOrderTable createKG3XxOrderTable;

        public CreateKG3xxTableColumn(KG3xxName name, List<string> replacementOrders, string preSelectedReplacementOrder, List<string> layerTypeNames, List<string> variationTargets, CreateDataManager createDataManager)
        {
            this.Name = name;
            this.textbox = FormsHelper.CreateTextBox(name.Name, 100, 20); //Contains the name
            this.replacementOrderSelection = FormsHelper.CreateComboBox(replacementOrders); //Drop down menu that shows options for replacement order
            this.replacementOrderSelection.SelectedIndex = replacementOrders.IndexOf(preSelectedReplacementOrder); //pre-select preferred order for this kg3xx
            this.replacementOrderSelection.MaximumSize = new System.Drawing.Size(500, 29); //Adjust combobox size, doesn't work...
            this.replacementOrderSelection.SelectedIndexChanged += new EventHandler((object sender, EventArgs e) =>
            {
                if (replacementOrderSelection.SelectedItem.ToString().Equals("Custom"))
                {
                    createKG3XxOrderTable.ShowAccessibility();
                }
                else
                {
                    createKG3XxOrderTable.HideAccessibility();
                }
            }
            );
            this.createKG3XxLayerTypeSelectionTable = new CreateKG3xxLayerTypeSelectionTable();
            this.createKG3XxOrderTable = new CreateKG3xxOrderTable(variationTargets, createDataManager, Name.getName());
            createKG3XxLayerTypeSelectionTable.ShowLayerTypeNames(layerTypeNames, createKG3XxOrderTable);
        }

        public void Show(TableLayoutPanel table, int column)
        {
            table.Controls.Add(textbox, column, 0);
            table.Controls.Add(replacementOrderSelection, column, 1);
            table.Controls.Add(createKG3XxLayerTypeSelectionTable.Table, column, 2);
            table.Controls.Add(createKG3XxOrderTable.Table, column, 3);
        }

        public void ImportKG3xx(KG3xxOption kg3xx, List<VariationParam> varParams, KnowledgeContext context)
        {
            //Adjust ReplacementOrder
            replacementOrderSelection.SelectedIndex = kg3xx.ReplacementOrderId - 1;

            //Determine whether or not replacement Order needs to be adjusted in the Ordertable (check if replacementorder is "Custom"
            bool adjustaccessOrder = replacementOrderSelection.SelectedItem.ToString().Equals("Custom");

            createKG3XxOrderTable.ImportKG3xx(kg3xx, adjustaccessOrder, varParams, context);
        }

        public KG3xxOption BuildKG3xx(KnowledgeContext context)
        {
            var layernames = createKG3XxOrderTable.GetLayerTypeNames();
            var layerpositions = createKG3XxOrderTable.GetPositions();
            var accessibilities = createKG3XxOrderTable.GetAccessibilities(replacementOrderSelection.SelectedItem.ToString());
            var hasExposureQualities = createKG3XxOrderTable.GetExposureQualitites();

            //Check for layertypes with no layers for this KG3xxName
            foreach (var layername in layernames)
            {
                if (!context.LayersInLayerType(layername, textbox.Text))
                {
                    //Show a messagebox asking the user if they want to proceed building this KG3xx or if they want to cancel the process
                    var result = MessageBox.Show("No Layers for \"" + layername + "\" in KG" + textbox.Text + ". Continue writing this KG3x0Option to the DB?",
                                                 "No Layer conflict",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Warning);
                    if (result == DialogResult.No)
                    {
                        throw new Exception("Writing process cancelled by user");
                    }
                }
            }
            var kg3xx =  KG3xxOption.BuildKG3xx(context, textbox.Text, layernames, layerpositions, accessibilities, hasExposureQualities,
                                   context.ReplacementOrders.Where(ro => ro.Order == replacementOrderSelection.SelectedItem.ToString()).FirstOrDefault());
            return kg3xx;
        }
    }
}
