using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Forms;

using KnowledgeDB;
using SharedDBLibrary;
using GUIHelpFunctions;

namespace KnowledgeDbGUI.ViewTab
{
    public class ViewTableKG3x0TableEntry
    {
        public KG3x0Option KG3x0 { get; set; }
        public TextBox CTypeTextBox { get; set; }
        public List<List<TextBox>> KG3xxTextBoxes { get; set; } = new List<List<TextBox>>();
        public Button DeleteButton { get; set; }
        public Button ImportToCreateButton { get; set; }
        public ViewTableKG3x0TableEntry(KG3x0Option kG3x0, KnowledgeContext context, EventHandler resetEventHandler, Action<KG3x0Option> importAction)
        {
            this.KG3x0 = kG3x0;

            //Textbox specifying construction Type
            CTypeTextBox = FormsHelper.CreateTextBox(KG3x0.ConstructionTypeName.Name);
            CTypeTextBox.MaximumSize = new System.Drawing.Size(1000, FormsHelper.GetDefaultHeight()); //Allows the textbox to grow infinitely in length, but not in height

            //Deletebutton
            DeleteButton = new Button();
            DeleteButton.Size = new System.Drawing.Size(100, FormsHelper.GetDefaultHeight());
            DeleteButton.Text = "Delete";
            DeleteButton.Click += new System.EventHandler((object sender, EventArgs e) => {
                context.KG3x0Options.Remove(kG3x0);
                context.SaveChanges();
            });
            DeleteButton.Click += resetEventHandler;

            //Import to Create Button
            ImportToCreateButton = new Button();
            ImportToCreateButton.Size = new System.Drawing.Size(100, FormsHelper.GetDefaultHeight());
            ImportToCreateButton.Text = "Import";
            ImportToCreateButton.Click += new System.EventHandler((object sender, EventArgs e) =>
            {
                importAction(KG3x0);
            });

            foreach (var kg3xx in context.FindKg3xxWhereKg3x0Is(KG3x0))
            {
                BuildKg3xxBoxes(kg3xx, context, context.VariationParams.Where(vp => vp.KG3x0OptionId == kG3x0.Id).ToList());
            }
        }

        private void BuildKg3xxBoxes(KG3xxOption kg3xx, KnowledgeContext context, List<VariationParam> variationParams)
        {
            //Find KG3xxOption_LayerTypeName Entries for variationParams, only considers variationParams in this kg3xxOption
            List<KG3xxOption_LayerTypeName> varKG3xxOption_LayerTypeName = new List<KG3xxOption_LayerTypeName>(); 
            foreach (var variationParam in variationParams)
            {
                try
                {
                    varKG3xxOption_LayerTypeName.Add(context.KG3xx_LayerTypeNames.Where(kg_ltn => kg_ltn.Id == variationParam.KG3xxOption_LayerTypeNameId
                                                                                                  && kg_ltn.Id1 == kg3xx.Id).First());
                }
                catch
                {
                    continue;
                }
            }

            var textBoxes = new List<TextBox>();
            var layerTypeNames = context.FindLayerTypeNamesWhereKG3xxIs(kg3xx);

            //Insert a textbox saying "None" for empty KG3xxs
            if (!layerTypeNames.Any())
            {
                var textBox = FormsHelper.CreateTextBox("None", 100);
                textBox.MaximumSize = new System.Drawing.Size(100000000, FormsHelper.GetDefaultHeight()); //Allows the textbox to grow infinitely in length, but not in height
                textBoxes.Add(textBox);
            }

            foreach (var layerTypeName in layerTypeNames)
            {
                var textbox = FormsHelper.CreateTextBox(layerTypeName.Name, 40);
                textbox.MaximumSize = new System.Drawing.Size(100000000, FormsHelper.GetDefaultHeight()); //Allows the textbox to grow infinitely in length, but not in height
                textBoxes.Add(textbox);

                if (varKG3xxOption_LayerTypeName.Any()) //Only do the following if there are any variation parameters in this kg3xx
                {
                    var currentLayerType_VariationParam = varKG3xxOption_LayerTypeName.Where(varkg => varkg.Id2 == layerTypeName.Id 
                                                                                             && varkg.Position == textBoxes.Count).ToList();
                    if (currentLayerType_VariationParam.Any()) //This is true when current layertype is a variationParam
                    {
                         //This code is written with only 2 kinds of variation Parameters in mind (thermal 
                         //and structural). Adjustments for more variationparams can be made, but requires
                         //either expanding it in this way, or some sort of automatic colour assingment
                        foreach(var lt_vp in currentLayerType_VariationParam)
                        {
                            var vartargets = (from vt in context.VariationTargets
                                             join vp in context.VariationParams on vt.Id equals vp.NameId
                                             where vp.KG3xxOption_LayerTypeNameId == lt_vp.Id &&
                                                   vp.KG3x0OptionId == KG3x0.Id
                                             select vt.Name).ToList();
                            if (vartargets.Contains("Thermal") && !vartargets.Contains("Structural")){ textbox.BackColor = System.Drawing.Color.FromArgb(
                                                                     ((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192))))); } //light red
                            else if (vartargets.Contains("Structural") && !vartargets.Contains("Thermal")){ textbox.BackColor = 
                                    System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192))))); }  //light green
                            else if (vartargets.Contains("Thermal") && vartargets.Contains("Structural")) { textbox.BackColor =
                                    System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))); } //light blue
                        }
                    }
                }
            }
            KG3xxTextBoxes.Add(textBoxes);
        }

        public List<int> InsertIntoTable(int rownumber, TableLayoutPanel table, List<int> cutOffColumns, int index)
        {
            //Update cutOffColumns if needed
            if (KG3xxTextBoxes[index].Count > cutOffColumns[index+1]-cutOffColumns[index])
            {
                cutOffColumns[index + 1] = cutOffColumns[index] + KG3xxTextBoxes[index].Count;
                if (table.ColumnCount < cutOffColumns[index + 1] + 1)
                {
                    table.ColumnCount = cutOffColumns[index+1] + 2;
                }
            }

            //Add textboxes to table
            for (int localColumn = 0; localColumn < KG3xxTextBoxes[index].Count; localColumn++)
            {
                table.Controls.Add(KG3xxTextBoxes[index][localColumn], cutOffColumns[index]+localColumn, rownumber);
            }
            return cutOffColumns;
        }
    }
}
