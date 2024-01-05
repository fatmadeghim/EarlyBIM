using System;
using KnowledgeDB;
using SharedDBLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

using GUIHelpFunctions;

namespace KnowledgeDbGUI.CreateKG3x0Tab
{
    public class CreateDataManager
    {
        public KnowledgeContext Context { get; set; }
        public ComboBox KG3x0ComboBox { get; set; }
        public ComboBox ConstructionTypeComboBox { get; set; }
        public BuildKG3xxTable KG3xxTable { get; set; }
        private List<CreateKG3xxOrderTable.CreateKG3xxOrderTableRow> currentVariationParams;
        public CreateDataManager(KnowledgeContext context, ComboBox kg3x0ComboBox, ComboBox constructionTypeComboBox,
                                 TableLayoutPanel kg3xxTable)
        {
            this.Context = context;
            this.KG3x0ComboBox = kg3x0ComboBox;
            this.ConstructionTypeComboBox = constructionTypeComboBox;
            this.KG3xxTable = new BuildKG3xxTable(kg3xxTable);
            this.currentVariationParams = new List<CreateKG3xxOrderTable.CreateKG3xxOrderTableRow>();
            foreach (var count in context.VariationTargets.ToList())
            {
                currentVariationParams.Add(new CreateKG3xxOrderTable.CreateKG3xxOrderTableRow());
            }
        }

        public void LoadKG3x0s()
        {
            foreach (var kg3x0 in Context.KG3x0Names)
            {
                KG3x0ComboBox.Items.Add(kg3x0.getName());
            }
        }

        public void LoadConstructionTypes()
        {
            foreach (var constructionType in Context.ConstructionTypeNames)
            {
                ConstructionTypeComboBox.Items.Add(constructionType.getName());
            }

        }

        public void UpdateCurrentVariationParam(int index, CreateKG3xxOrderTable.CreateKG3xxOrderTableRow newVariationParam)
        {
            if (currentVariationParams[index] == newVariationParam) //happens when you uncheck the current variationparam 
            {
                currentVariationParams[index] = new CreateKG3xxOrderTable.CreateKG3xxOrderTableRow();
            }
            else //happens when you checked a new variationparam
            {
                //Uncheck old variationParam
                if (currentVariationParams[index].TextBox != null) //Check if currently selected is a dummy element
                {
                    currentVariationParams[index].CheckBoxes[index].Checked = false;
                }
                //Overwrite currentvariationparams
                currentVariationParams[index] = newVariationParam;
            }      
        }

        public void ImportKG3x0(KG3x0Option kg3x0)
        {
            KG3x0ComboBox.SelectedIndex = kg3x0.NameId-1;
            ConstructionTypeComboBox.SelectedIndex = kg3x0.ConstructionTypeNameId-1;
            KG3xxTable.Reset();
            KG3xxTable.Load(KG3x0ComboBox.SelectedItem.ToString(), Context, this);
            KG3xxTable.ImportKG3x0Option(kg3x0, Context);
        }

        public void CreateKG3xx()
        {
            try
            {
                //Look for common errors to give better error feedback and prevent false data to be written into the DB
                //Check if KG3x0 is selected
                if (KG3x0ComboBox.SelectedItem == null) { throw new Exception("No KG3x0 selected"); }
                if (ConstructionTypeComboBox.SelectedItem == null) { throw new Exception("No Construction Type selected"); }

                //KG3xxOptions
                var kg3xxs = KG3xxTable.BuildKG3xxes(Context);

                //Check if KG3x0 already exists
                var exKg3x0s = (from kg in Context.KG3x0Options
                                join ctn in Context.ConstructionTypeNames on kg.ConstructionTypeNameId equals ctn.Id
                                join kg3x0N in Context.KG3x0Names on kg.NameId equals kg3x0N.Id
                                where ctn.Name.Equals(ConstructionTypeComboBox.SelectedItem.ToString()) &&
                                      kg3x0N.Name.Equals(KG3x0ComboBox.SelectedItem.ToString())
                                select kg).ToList();

                foreach(var exKG3x0 in exKg3x0s)
                {
                    //Get KG3xxs of exKG3x0
                    var exKG3xxs = (from kg3xx in Context.KG3xxOptions
                                    join kg3x0_kg3xx in Context.KG3x0_KG3xxs on kg3xx.Id equals kg3x0_kg3xx.Id2
                                    where kg3x0_kg3xx.Id1 == exKG3x0.Id
                                    orderby kg3x0_kg3xx.Position
                                    select kg3xx).ToList();
                    if (exKG3xxs.ContainsSameIds(kg3xxs))
                    {
                        throw new Exception("The KG3x0 Option you're trying to create already exists!");
                    }
                }


                //Following code is only executed if KG3x0 did not already exist
                //KG3x0Option
                var kg3x0 = new KG3x0Option()
                    {
                        ConstructionTypeName = Context.ConstructionTypeNames.
                                                        Where(ctN => ctN.Name.Equals(ConstructionTypeComboBox.SelectedItem.ToString())).First(),
                        Name = Context.KG3x0Names.
                                            Where(kg3xN => kg3xN.Name.Equals(KG3x0ComboBox.SelectedItem.ToString())).First()

                    };

                Context.KG3x0Options.Add(kg3x0);

                var positions = (from kg3x0N_kg3xxN in Context.KG3x0Name_KG3xxNames
                                 join kg3x0N in Context.KG3x0Names on kg3x0N_kg3xxN.Id1 equals kg3x0N.Id
                                 where kg3x0N.Id == kg3x0.Name.Id
                                 orderby kg3x0N_kg3xxN.Position
                                 select kg3x0N_kg3xxN.Position).ToList();
                Context.KG3x0_KG3xxs.LinkT1ToManyT2sWithPositions(kg3x0, kg3xxs, positions);


                Context.SaveChanges(); //Following codes needs some changes to be written to the DB, so we need to save here

                //Create neccessary new ThicknessRanges and CostRanges
                foreach (var kg3xx in kg3xxs)
                    {
                        var layertypenames = (from ltname in Context.LayerTypeNames
                                              join kg_ltn in Context.KG3xx_LayerTypeNames on ltname.Id equals kg_ltn.Id2
                                              where kg_ltn.Id1 == kg3xx.Id
                                              where ltname.Is2Component == false //Only include single component layers, 2Component layertypes are handled separately
                                              orderby kg_ltn.Position
                                              select ltname).ToList();

                        //Create new Defaultentries if needed
                        DefaultThicknessRange.BuildDefaultThicknessRanges(
                                                   Context.KG3xxNames.Where(kgName => kgName.Id == kg3xx.NameId).First(),
                                                   layertypenames,
                                                   Context);

                        Context.SaveChanges(); //We need to save here so the new ThicknessRanges can refer to their Defaults 

                        ThicknessRange.BuildThicknessRanges(
                                            Context.KG3xxNames.Where(kgName => kgName.Id == kg3xx.NameId).First(),
                                            layertypenames,
                                            Context);

                        var twoCompLayertypeNames = (from ltname in Context.LayerTypeNames
                                              join kg_ltn in Context.KG3xx_LayerTypeNames on ltname.Id equals kg_ltn.Id2
                                              where kg_ltn.Id1 == kg3xx.Id
                                              where ltname.Is2Component == true //Only include 2component layers, 2Component layertypes are handled separately
                                              select ltname).ToList();

                        TwoComponentUncertainty.BuildTwoComponentUncertainties(
                                                     Context.KG3xxNames.Where(kgName => kgName.Id == kg3xx.NameId).First(),
                                                     twoCompLayertypeNames,
                                                     Context);

                        var hasExposureQualities = (from kg_ltn in Context.KG3xx_LayerTypeNames
                                                    where kg_ltn.Id1 == kg3xx.Id
                                                    orderby kg_ltn.Position
                                                    select kg_ltn.HasExposureQuality).ToList();

                        //CostRanges
                        CostRange.BuildCostRanges(
                                             Context.KG3xxNames.Where(kgName => kgName.Id == kg3xx.NameId).FirstOrDefault(),
                                             layertypenames,
                                             hasExposureQualities,
                                             Context);
                        CostRange.BuildCostRanges(
                                             Context.KG3xxNames.Where(kgName => kgName.Id == kg3xx.NameId).FirstOrDefault(),
                                             twoCompLayertypeNames,
                                             hasExposureQualities,
                                             Context);
                }

                Context.SaveChanges(); //Save changes here to prevent "Sequence contains no elements error" from creating thickness Ranges

                //VariationParams
                int index = 0;
                foreach (var variationParamRow in currentVariationParams)
                {
                    if (variationParamRow.TextBox != null)
                    {
                        var kg3xx = kg3xxs.Where(kg => kg.getName().Equals(variationParamRow.KG3xxName)).First();
                        var ltn = Context.LayerTypeNames.Where(ltn => ltn.Name.Equals(variationParamRow.TextBox.Text)).First();
                        var variationParam = new VariationParam()
                        {
                            Name = Context.VariationTargets.ToList()[index],
                            KG3X0Option = kg3x0,
                            KG3xxOption_LayerTypeName = Context.KG3xx_LayerTypeNames.Where(kg_ltn => (kg_ltn.Id1 == kg3xx.Id)
                                                                                                      && kg_ltn.Id2 == ltn.Id).First()
                        };
                        Context.VariationParams.Add(variationParam);
                    }
                    index++;
                }
            Context.SaveChanges();

            }
            catch (Exception e)
            {
                FormsHelper.ShowErrorMessage(e.Message, "Error creating KG3x0Option");
            }  
            
        }
    }
}
