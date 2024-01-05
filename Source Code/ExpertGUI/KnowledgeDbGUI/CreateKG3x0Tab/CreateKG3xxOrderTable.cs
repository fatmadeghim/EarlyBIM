using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using KnowledgeDB;
using GUIHelpFunctions;

namespace KnowledgeDbGUI.CreateKG3x0Tab
{
    public class CreateKG3xxOrderTable
    {
        public class CreateKG3xxOrderTableRow
        {
            public TextBox TextBox { get; set; }
            public CheckBox HasExposureCheckbox { get; set; }
            public ComboBox PositionComboBox { get; set; }
            public ComboBox AccessibilityComboBox { get; set; }
            public Button RemoveButton { get; set; }
            public List<CheckBox> CheckBoxes { get; set; }
            public string KG3xxName { get; set; }
            public CreateKG3xxOrderTableRow(string name, int number, string KG3xxName, List<string> variationTargets, CreateDataManager createDataManager)
            {
                this.KG3xxName = KG3xxName;

                //TextBox - Name of the LayerType
                //Following 2 lines are commented out during some layout tests
                //TextBox = FormsHelper.CreateTextBox(name, 100, 20);
                //TextBox.MaximumSize = new System.Drawing.Size(100000000, 30); //Allows the textbox to grow infinitely in length, but not in height
                TextBox = FormsHelper.CreateTextBox(name);

                //Checkbox - Check if layertype has to have exposure quality
                HasExposureCheckbox = FormsHelper.CreateCheckBox("", false, false, (object sender, EventArgs e) => { });
                HasExposureCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top))));

                //PositionCombobox - Selection of order
                PositionComboBox = FormsHelper.CreateComboBox(new List<string>());
                PositionComboBox.AutoSize = true;

                for ( int i= 1; i<=number; i++)
                {
                    PositionComboBox.Items.Add(i);
                }

                //AccessibilityComboBox - Selection of ReplacementOrder - only shown for custom orders
                AccessibilityComboBox = FormsHelper.CreateComboBox(new List<string>());
                AccessibilityComboBox.AutoSize = true;

                for (int i = 1; i <= number; i++)
                {
                    AccessibilityComboBox.Items.Add(i);
                }
                PositionComboBox.SelectedItem = number;

                //Removebutton
                RemoveButton = FormsHelper.CreateButton("Remove");
                RemoveButton.Size = new System.Drawing.Size(150, 30);

                //Checkboxes - VariationParameterSettings
                CheckBoxes = new List<CheckBox>();
                var indexcounter = 0;
                foreach( var target in variationTargets)
                {
                    //var checkbox = FormsHelper.CreateCheckBox(target, false, false, 100, 20, (object sender, EventArgs e) => { });
                    var checkbox = FormsHelper.CreateCheckBox(target, false, false, (object sender, EventArgs e) => { });
                    checkbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top))));
                    checkbox.CheckedChanged += new EventHandler((object sender, EventArgs e) =>
                    {
                        createDataManager.UpdateCurrentVariationParam(variationTargets.IndexOf(target), this);
                    });
                    CheckBoxes.Add(checkbox);
                    indexcounter++;
                }
            }

            public CreateKG3xxOrderTableRow() {} //dummy row, used in CreateDataManager as Placeholders for the current variationParams.
            public void UpdateLayerTypeNumber(int number)
            {
                if (number > PositionComboBox.Items.Count)
                {
                    PositionComboBox.Items.Add(number);
                    AccessibilityComboBox.Items.Add(number);
                }
                else
                {
                    while (number < PositionComboBox.Items.Count)
                    {
                        PositionComboBox.Items.RemoveAt(PositionComboBox.Items.Count - 1);
                        AccessibilityComboBox.Items.RemoveAt(AccessibilityComboBox.Items.Count - 1);
                    }
                }
            }

            public void RemoveFromTable(TableLayoutPanel table)
            {
                table.Controls.Remove(TextBox);
                table.Controls.Remove(HasExposureCheckbox);
                table.Controls.Remove(PositionComboBox);
                table.Controls.Remove(RemoveButton);
                table.Controls.Remove(AccessibilityComboBox);
                foreach (var checkbox in CheckBoxes)
                {
                    table.Controls.Remove(checkbox);
                }
            }

            public void AddToTable(TableLayoutPanel table, int row, bool ShowAccessibility)
            {
                table.Controls.Add(TextBox, 0, row);
                table.Controls.Add(HasExposureCheckbox, 1, row);
                table.Controls.Add(PositionComboBox, 2, row);
                if (ShowAccessibility)
                {
                    table.Controls.Add(AccessibilityComboBox, 3, row);
                }
                table.Controls.Add(RemoveButton, 4, row);
                var columncount = 0;
                foreach (var checkBox in CheckBoxes) //Variationparam checkboxes
                {
                    table.Controls.Add(CheckBoxes[columncount], 5 + columncount, row);
                    columncount++;
                }
            }

            public void ShowAccessibility(TableLayoutPanel table, int row)
            {
                table.Controls.Add(AccessibilityComboBox, 3, row);
            }

            public void HideAccessibility(TableLayoutPanel table)
            {
                table.Controls.Remove(AccessibilityComboBox);
            }
        } //End of CreateKG3xxOrderTableRow




        public TableLayoutPanel Table { get; set; }
        private List<CreateKG3xxOrderTableRow> createKG3xxOrderTableRows { get; set; }
        private List<string> variationTargets;
        private CreateDataManager createDataManager;
        private string kg3xxName;
        private bool showAccessibility;
        private TextBox accessibilityTextBox;
        public CreateKG3xxOrderTable(List<string> variationTargets, CreateDataManager createDataManager, string kg3xxName)
        {
            createKG3xxOrderTableRows = new List<CreateKG3xxOrderTableRow>();
            this.variationTargets = variationTargets;
            this.createDataManager = createDataManager;
            this.kg3xxName = kg3xxName;
            this.showAccessibility = false;

            Table = FormsHelper.CreateTable(5+variationTargets.Count, 0);

            //Headline
            Table.Controls.Add(FormsHelper.CreateTextBox("LayerType", 150, 20), 0, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Exposure Quality", 150, 20), 1, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Position", 150, 20), 2, 0);
            Table.Controls.Add(FormsHelper.CreateTextBox("Remove", 150, 20), 4, 0);
            accessibilityTextBox = FormsHelper.CreateTextBox("Access Order", 150, 20);
            var variationTargetTextBox = FormsHelper.CreateTextBox("Variation Parameter Settings", 200, 20);
            Table.Controls.Add(variationTargetTextBox, 5, 0);
            Table.SetColumnSpan(variationTargetTextBox, variationTargets.Count);

            //Table Layout
            Table.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            Table.AutoScroll = true;
            Table.AutoSize = false;
            Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            for (var i = 0; i < variationTargets.Count; i++)
            {
                Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
                Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
                Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            }
            Table.RowCount = 1;
        }

        public void AddLayerType(string name)
        {
            var row = new CreateKG3xxOrderTableRow(name, createKG3xxOrderTableRows.Count + 1, kg3xxName, variationTargets, createDataManager);
            createKG3xxOrderTableRows.Add(row);
            Table.RowCount++;
            row.RemoveButton.Click += new EventHandler((object sender, EventArgs e) =>
            {
                RemoveLayerType(row);
                UpdateLayerTypeNumber();
            });

            //Add elements to Table
            row.AddToTable(Table, Table.RowCount - 1, showAccessibility);

            //Add new RowStyle so every line has the same size
            Table.RowStyles.Add((new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F)));
        }

        private void RemoveLayerType(CreateKG3xxOrderTableRow toBeDeleted)
        {
            toBeDeleted.RemoveFromTable(Table);

            //Lower selected Positions for the Layertypes that selected a higher index than the deleted row
            foreach (var row in createKG3xxOrderTableRows.Where(row => row.PositionComboBox.SelectedIndex > toBeDeleted.PositionComboBox.SelectedIndex))
            {
                row.PositionComboBox.SelectedIndex--;
            }
            
            //Move all rows below toBeDeleted one row up
            for(var index = createKG3xxOrderTableRows.IndexOf(toBeDeleted)+1; index < createKG3xxOrderTableRows.Count; index++)
            {
                createKG3xxOrderTableRows[index].RemoveFromTable(Table);
                createKG3xxOrderTableRows[index].AddToTable(Table, index, showAccessibility);
            }

            Table.RowCount--;
            createKG3xxOrderTableRows.Remove(toBeDeleted);
        }
        
        public void UpdateLayerTypeNumber()
        {
            foreach(var row in createKG3xxOrderTableRows)
            {
                row.UpdateLayerTypeNumber(createKG3xxOrderTableRows.Count);
            }
        }

        public void ShowAccessibility()
        {
            //Do nothing if Accessibility is already shown
            if (showAccessibility == true) 
            {
                return;
            }
            //Show otherwise
            showAccessibility = true;
            Table.Controls.Add(accessibilityTextBox, 3, 0);
            Table.ColumnStyles[3] = new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F);
            foreach (var row in createKG3xxOrderTableRows)
            {
                row.ShowAccessibility(Table, createKG3xxOrderTableRows.IndexOf(row)+1);
            }
        }

        public void HideAccessibility()
        {
            //Do nothing if Accessibility is already hidden
            if (showAccessibility == false)
            {
                return;
            }
            //Hide otherwise
            showAccessibility = false;
            Table.Controls.Remove(accessibilityTextBox);
            Table.ColumnStyles[3] = new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 0F);
            foreach (var row in createKG3xxOrderTableRows)
            {
                row.HideAccessibility(Table);
            }
        }

        public List<string> GetLayerTypeNames()
        {
            var layerTypeNames = new List<string>();
            foreach (var row in createKG3xxOrderTableRows)
            {
                layerTypeNames.Add(row.TextBox.Text);
            }
            return layerTypeNames;
        }

        public List<int> GetPositions()
        {
            var positions = new List<int>();
            foreach(var row in createKG3xxOrderTableRows)
            {
                if (positions.Contains(int.Parse(row.PositionComboBox.SelectedItem.ToString())))
                {
                    throw new Exception("Duplicate Positions in KG" + kg3xxName);
                }
                positions.Add(int.Parse(row.PositionComboBox.SelectedItem.ToString()));
            }
            return positions;
        }

        public List<int> GetAccessibilities(string replacementOrder)
        {
            var accessibilities = new List<int>();
            if (showAccessibility) //Custom Replacement Order => return selected Number
            {
                foreach(var row in createKG3xxOrderTableRows)
                {
                    accessibilities.Add(row.AccessibilityComboBox.SelectedIndex + 1);
                }
            }
            else if (replacementOrder.Equals("Position 1 most accessible")) //Accessibility = Position)
            {
                foreach(var row in createKG3xxOrderTableRows)
                {
                    accessibilities.Add(row.PositionComboBox.SelectedIndex + 1);
                }
            }
            else if (replacementOrder.Equals("Position 1 least accessible")) //Accessibility is reversed Position
            {
                foreach(var row in createKG3xxOrderTableRows)
                {
                    accessibilities.Add(createKG3xxOrderTableRows.Count - row.PositionComboBox.SelectedIndex);
                }
            }
            else if (replacementOrder.Equals("No Replacement")) //We don't care about replacement -> put 0 everywhere
            {
                foreach(var row in createKG3xxOrderTableRows)
                {
                    accessibilities.Add(0);
                }
            }

            return accessibilities;
        }

        public List<bool> GetExposureQualitites()
        {
            var exposureQualities = new List<bool>();
            foreach(var row in createKG3xxOrderTableRows)
            {
                exposureQualities.Add(row.HasExposureCheckbox.Checked);
            }
            return exposureQualities;
        }

        public void ImportKG3xx(KG3xxOption kg3xx, bool adjustAccessOrder, List<VariationParam> varParams, KnowledgeContext context)
        {
            var layerTypeNamesWithAccessibilities = (from kg3xx_ltn in context.KG3xx_LayerTypeNames
                                                     join ltn in context.LayerTypeNames on kg3xx_ltn.Id2 equals ltn.Id
                                                     where kg3xx_ltn.Id1 == kg3xx.Id
                                                     orderby kg3xx_ltn.Position
                                                     select new { ltn.Name, kg3xx_ltn.AccessOrder, kg3xx_ltn.Id }).ToList();

            foreach(var ltn_a in layerTypeNamesWithAccessibilities)
            {
                AddLayerType(ltn_a.Name);
                //Handle Variation Parameters
                foreach (var vp in varParams)
                {
                    //Check if current ltn_a is a varParam
                    if (vp.KG3xxOption_LayerTypeNameId == ltn_a.Id)
                    {
                        createKG3xxOrderTableRows.Last().CheckBoxes[vp.NameId - 1].Checked = true;
                    }
                }
            }

            //Set AccessOrder
            //This needs to be done in a separate step in order to create the necessary indices in the AccessOrder DropdownMenu
            if (adjustAccessOrder)
            {
                for (var i = 0; i < layerTypeNamesWithAccessibilities.Count; i++)
                {
                    createKG3xxOrderTableRows[i].AccessibilityComboBox.SelectedIndex = layerTypeNamesWithAccessibilities[i].AccessOrder - 1;
                }
            }
        }
    }
}
