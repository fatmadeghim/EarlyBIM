using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using KnowledgeDB;
using GUIHelpFunctions;

namespace KnowledgeDbGUI
{
    public partial class KnowledgeDbNavigator : Form
    {
        public KnowledgeContext Context { get; set; }
        public CreateKG3x0Tab.CreateDataManager CreateDataManager { get; set; }
        public DefaultThicknessTab.DefaultThicknessTable DefaultThicknessTable { get; set; }
        public ThicknessTab.ThicknessTable ThicknessTable { get; set; }
        public TwoComponentUncertaintiesTab.TwoCompUncertaintyTable TwoCompUncertaintyTable { get; set; }
        public CostRangesTab.CostRangeTable CostRangeTable { get; set; }
        public ViewTab.ViewTabManager ViewTabManager { get; set; }

        public KnowledgeDbNavigator()
        {
            //Create elements made in Designer
            InitializeComponent();

            //Following events should be in the auto-generated code, but visual studio likes to overwrite changes to the auto-generated-code, so it's here
            //Reset Tabs when you enter them
            this.DefaultThicknessRangesTabPage.Enter += new System.EventHandler((object sender, EventArgs e) => DefaultThicknessTable.ResetTable(Context));
            this.ThicknessRangesTab.Enter += new System.EventHandler((object sender, EventArgs e) => ThicknessTable.ResetTable(Context));
            this.TwoCompUncertaintiesTab.Enter += new System.EventHandler((object sender, EventArgs e) => TwoCompUncertaintyTable.ResetTable(Context));
            this.CostRangeTabPage.Enter += new System.EventHandler((object sender, EventArgs e) => CostRangeTable.ResetTable(Context));
            this.ViewDataTabPage.Enter += new System.EventHandler((object sender, EventArgs e) => ViewTabManager.ReLoadTable());

            //Changing the default height in the utilitytab should call the respective function in Formshelper
            this.UtilityDefaultHeightSetter.ValueChanged += new System.EventHandler((object sender, EventArgs e) =>
                                            FormsHelper.ChangeDefaultHeight((int)this.UtilityDefaultHeightSetter.Value));

            SelectDbDialog.ShowDialog(); //Shows a filedialog where the user can select the DB
            Context = new KnowledgeContext(SelectDbDialog.FileName);
            CreateDataManager = new CreateKG3x0Tab.CreateDataManager(Context, CreateKG3x0ComboBox, CreateConstructionTypeComboBox,
                                                        CreateKG3xxTablePanel);
            CreateDataManager.LoadKG3x0s();
            CreateDataManager.LoadConstructionTypes();

            DefaultThicknessTable = new DefaultThicknessTab.DefaultThicknessTable(DefaultThicknessRangesTable);

            ThicknessTable = new ThicknessTab.ThicknessTable(ThicknessRangeTable);

            TwoCompUncertaintyTable = new TwoComponentUncertaintiesTab.TwoCompUncertaintyTable(TwoCompUncertaintyTableLayoutPanel);

            CostRangeTable = new CostRangesTab.CostRangeTable(CostRangesTableLayoutPanel);

            ViewTabManager = new ViewTab.ViewTabManager(ViewTable, Context, ViewKg3x0Filter, ViewCTypeFilter,
                                                            (KG3x0Option kg3x0) => ImportKG3x0ToCreateTab(kg3x0));
            ViewTabManager.LoadFilters();
            ViewTabManager.LoadTable();
            ViewTabManager.Table.Show();
        }

        private void CreateKG3x0ComboBox_KG3x0Selected(object sender, EventArgs e)
        {
            CreateDataManager.KG3xxTable.Reset();
            CreateDataManager.KG3xxTable.Load(CreateKG3x0ComboBox.SelectedItem.ToString(), Context, CreateDataManager);
        }

        private void CreateConstructionTypeComboBox_CTypeSelected(object sender, EventArgs e)
        {

        }

        private void CreateResetButton_Click(object sender, EventArgs e)
        {
            CreateDataManager.KG3xxTable.Reset();
            CreateDataManager.KG3xxTable.Load(CreateKG3x0ComboBox.SelectedItem.ToString(), Context, CreateDataManager);
        }

        private void CreateWriteToDBButton_Click(object sender, EventArgs e)
        {
            CreateDataManager.CreateKG3xx();
        }

        private void ThicknessWriteButton_Click(object sender, EventArgs e)
        {
            ThicknessTable.WriteChanges(Context);
        }

        private void DefaultThicknessRangeWriteButton_Click(object sender, EventArgs e)
        {
            DefaultThicknessTable.WriteChanges(Context);
        }

        private void TwoComponentWriteButton_Click(object sender, EventArgs e)
        {
            TwoCompUncertaintyTable.WriteChanges(Context);
        }
        private void CostRangeWriteButton_Click(object sender, EventArgs e)
        {
            CostRangeTable.WriteChanges(Context);
        }

        private void ImportKG3x0ToCreateTab(KG3x0Option kg3x0)
        {
            CreateDataManager.ImportKG3x0(kg3x0);
            //Jump to Create Tab
            this.NavigatorTabControl.SelectedIndex = 0;
        }

        private void ExportThicknessrangesButton_Click(object sender, EventArgs e)
        {
            //Ask for folder location
            if (this.UtilitySelectExportFolderDialog.ShowDialog() == DialogResult.OK) //Only execute if dialog was ended with a click on ok.
            {
                Context.ExportThicknessesToCSV(this.UtilitySelectExportFolderDialog.SelectedPath);
            }
        }

        private void ImportThicknessrangesButton_Click(object sender, EventArgs e)
        {
            //Ask for folder location
            if (this.UtilitySelectExportFolderDialog.ShowDialog() == DialogResult.OK) //Only execute if dialog was ended with a click on ok.
            {
                //Make sure there is exactly 1 file for each kind of thicknessRanges
                var csvFiles = Directory.GetFiles(this.UtilitySelectExportFolderDialog.SelectedPath, "*.csv");

                if (csvFiles.Where(csvfile => csvfile.EndsWith("DefaultThicknessRanges.csv")).Count() != 1)
                {
                    FormsHelper.ShowErrorMessage("Make sure there is exactly 1 csv called \"DefaultThicknessRanges.csv\" in the selected folder.",
                        "Can't import csvs");
                    return;
                }
                if (csvFiles.Where(csvfile => csvfile.EndsWith("ThicknessRanges.csv") && !csvfile.EndsWith("DefaultThicknessRanges.csv")).Count() != 1)
                {
                    FormsHelper.ShowErrorMessage("Make sure there is exactly 1 csv called \"ThicknessRanges.csv\" in the selected folder.",
                        "Can't import csvs");
                    return;
                }
                if (csvFiles.Where(csvfile => csvfile.EndsWith("TwoComponentUncertainties.csv")).Count() != 1)
                {
                    FormsHelper.ShowErrorMessage("Make sure there is exactly 1 csv called \"TwoComponentUncertainties.csv\" in the selected folder.",
                        "Can't import csvs");
                    return;
                }

                //Import files if no error has been thrown
                Context.ImportThicknessesCSV(this.UtilitySelectExportFolderDialog.SelectedPath);
            }
        }

        private void ExportCostRangesButtonClick(object sender, EventArgs e)
        {
            //Ask for folder location
            if (this.UtilitySelectExportFolderDialog.ShowDialog() == DialogResult.OK) //Only execute if dialog was ended with a click on ok.
            {
                Context.ExportCostRangesToCSV(this.UtilitySelectExportFolderDialog.SelectedPath);
            }
        }

        private void ImportCostRangesButtonClick(object sender, EventArgs e)
        {
            //Ask for folder location
            if (this.UtilitySelectExportFolderDialog.ShowDialog() == DialogResult.OK) //Only execute if dialog was ended with a click on ok.
            {
                Context.ImportCostRangesCSV(this.UtilitySelectExportFolderDialog.SelectedPath);
            }
        }

        private void ExportBuildingPartButtonClick(object sender, EventArgs e)
        {
            //Ask for folder location
            if (this.UtilitySelectExportFolderDialog.ShowDialog() == DialogResult.OK) //Only execute if dialog was ended with a click on ok.
            {
                Context.ExportBuildingPartsToCSV(this.UtilitySelectExportFolderDialog.SelectedPath);
            }
        }

        private void ImportBuildingPartButton_Click(object sender, EventArgs e)
        {
            //Ask for folder location
            if (this.UtilitySelectExportFolderDialog.ShowDialog() == DialogResult.OK) //Only execute if dialog was ended with a click on ok.
            {
                //Make sure there is exactly 1 file for each kind of thicknessRanges
                var csvFiles = Directory.GetFiles(this.UtilitySelectExportFolderDialog.SelectedPath, "*.csv");

                if (csvFiles.Where(csvfile => csvfile.EndsWith("KG3x0_KG3xxs.csv")).Count() != 1)
                {
                    FormsHelper.ShowErrorMessage("Make sure there is exactly 1 csv called \"KG3x0_KG3xxs.csv\" in the selected folder.",
                        "Can't import csvs");
                    return;
                }
                if (csvFiles.Where(csvfile => csvfile.EndsWith("KG3x0Options.csv")).Count() != 1)
                {
                    FormsHelper.ShowErrorMessage("Make sure there is exactly 1 csv called \"KG3x0Options.csv\" in the selected folder.",
                        "Can't import csvs");
                    return;
                }
                if (csvFiles.Where(csvfile => csvfile.EndsWith("KG3xx_LayerTypeNames.csv")).Count() != 1)
                {
                    FormsHelper.ShowErrorMessage("Make sure there is exactly 1 csv called \"KG3xx_LayerTypeNames.csv\" in the selected folder.",
                        "Can't import csvs");
                    return;
                }
                if (csvFiles.Where(csvfile => csvfile.EndsWith("KG3xxOptions.csv")).Count() != 1)
                {
                    FormsHelper.ShowErrorMessage("Make sure there is exactly 1 csv called \"KG3xxOptions.csv\" in the selected folder.",
                        "Can't import csvs");
                    return;
                }
                if (csvFiles.Where(csvfile => csvfile.EndsWith("VariationParams.csv")).Count() != 1)
                {
                    FormsHelper.ShowErrorMessage("Make sure there is exactly 1 csv called \"VariationParams.csv\" in the selected folder.",
                        "Can't import csvs");
                    return;
                }

                //Import files if no error has been thrown
                Context.ImportBuildingPartsCSV(this.UtilitySelectExportFolderDialog.SelectedPath);
            }
        }


        private void DeleteUnneccessaryKG3xxsButton_Click(object sender, EventArgs e)
        {
            Context.DeleteUnneccessaryKG3xxs();
            Context.DeleteUnneccessaryThicknessRanges();
        }
    }
}
