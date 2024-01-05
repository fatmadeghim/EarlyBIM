using System.Diagnostics;
using System;


namespace KnowledgeDbGUI
{
    partial class KnowledgeDbNavigator
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            NavigatorTabControl = new System.Windows.Forms.TabControl();
            CreateDataTab = new System.Windows.Forms.TabPage();
            CreateResetButton = new System.Windows.Forms.Button();
            CreateWriteToDBButton = new System.Windows.Forms.Button();
            CreateKG3xxTablePanel = new System.Windows.Forms.TableLayoutPanel();
            CreateInstructionText3 = new System.Windows.Forms.TextBox();
            CreateConstructionTypeComboBox = new System.Windows.Forms.ComboBox();
            CreateInstructionText2 = new System.Windows.Forms.TextBox();
            CreateKG3x0ComboBox = new System.Windows.Forms.ComboBox();
            CreateInstructionsText1 = new System.Windows.Forms.TextBox();
            DefaultThicknessRangesTabPage = new System.Windows.Forms.TabPage();
            DefaultThicknessRangeWriteButton = new System.Windows.Forms.Button();
            DefaultThicknessRangesTable = new System.Windows.Forms.TableLayoutPanel();
            ThicknessRangesTab = new System.Windows.Forms.TabPage();
            ThicknessWriteButton = new System.Windows.Forms.Button();
            ThicknessRangeTable = new System.Windows.Forms.TableLayoutPanel();
            TwoCompUncertaintiesTab = new System.Windows.Forms.TabPage();
            TwoComponentWriteButton = new System.Windows.Forms.Button();
            TwoCompUncertaintyTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            CostRangeTabPage = new System.Windows.Forms.TabPage();
            CostRangeWritebutton = new System.Windows.Forms.Button();
            CostRangesTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            ViewDataTabPage = new System.Windows.Forms.TabPage();
            textBox9 = new System.Windows.Forms.TextBox();
            textBox8 = new System.Windows.Forms.TextBox();
            textBox7 = new System.Windows.Forms.TextBox();
            ViewCTypeFilter = new System.Windows.Forms.CheckedListBox();
            ViewKg3x0Filter = new System.Windows.Forms.CheckedListBox();
            ViewTable = new System.Windows.Forms.TableLayoutPanel();
            ViewTextCTypeFilter = new System.Windows.Forms.TextBox();
            ViewTextKG3x0Filter = new System.Windows.Forms.TextBox();
            UtilityTabPage = new System.Windows.Forms.TabPage();
            UtilityDefaultHeightTextbox = new System.Windows.Forms.TextBox();
            UtilityDefaultHeightSetter = new System.Windows.Forms.NumericUpDown();
            UtilityImportBuildingPartsButton = new System.Windows.Forms.Button();
            UtilityExportBuildingPartsButton = new System.Windows.Forms.Button();
            UtilityImportCostRangesButton = new System.Windows.Forms.Button();
            UtilityExportCostRangesButton = new System.Windows.Forms.Button();
            UtilityDeleteUnusedKG3xxsButton = new System.Windows.Forms.Button();
            UtilityImportThicknessesButton = new System.Windows.Forms.Button();
            UtilityExportThicknessesButton = new System.Windows.Forms.Button();
            SelectDbDialog = new System.Windows.Forms.OpenFileDialog();
            UtilitySelectExportFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            NavigatorTabControl.SuspendLayout();
            CreateDataTab.SuspendLayout();
            DefaultThicknessRangesTabPage.SuspendLayout();
            ThicknessRangesTab.SuspendLayout();
            TwoCompUncertaintiesTab.SuspendLayout();
            CostRangeTabPage.SuspendLayout();
            ViewDataTabPage.SuspendLayout();
            UtilityTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)UtilityDefaultHeightSetter).BeginInit();
            SuspendLayout();
            // 
            // NavigatorTabControl
            // 
            NavigatorTabControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            NavigatorTabControl.Controls.Add(CreateDataTab);
            NavigatorTabControl.Controls.Add(DefaultThicknessRangesTabPage);
            NavigatorTabControl.Controls.Add(ThicknessRangesTab);
            NavigatorTabControl.Controls.Add(TwoCompUncertaintiesTab);
            NavigatorTabControl.Controls.Add(CostRangeTabPage);
            NavigatorTabControl.Controls.Add(ViewDataTabPage);
            NavigatorTabControl.Controls.Add(UtilityTabPage);
            NavigatorTabControl.Location = new System.Drawing.Point(2, 4);
            NavigatorTabControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            NavigatorTabControl.Name = "NavigatorTabControl";
            NavigatorTabControl.SelectedIndex = 0;
            NavigatorTabControl.Size = new System.Drawing.Size(968, 700);
            NavigatorTabControl.TabIndex = 0;
            NavigatorTabControl.SelectedIndexChanged += CostRangeWriteButton_Click;
            // 
            // CreateDataTab
            // 
            CreateDataTab.Controls.Add(CreateResetButton);
            CreateDataTab.Controls.Add(CreateWriteToDBButton);
            CreateDataTab.Controls.Add(CreateKG3xxTablePanel);
            CreateDataTab.Controls.Add(CreateInstructionText3);
            CreateDataTab.Controls.Add(CreateConstructionTypeComboBox);
            CreateDataTab.Controls.Add(CreateInstructionText2);
            CreateDataTab.Controls.Add(CreateKG3x0ComboBox);
            CreateDataTab.Controls.Add(CreateInstructionsText1);
            CreateDataTab.Location = new System.Drawing.Point(4, 29);
            CreateDataTab.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            CreateDataTab.Name = "CreateDataTab";
            CreateDataTab.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            CreateDataTab.Size = new System.Drawing.Size(960, 667);
            CreateDataTab.TabIndex = 0;
            CreateDataTab.Text = "Create Data";
            CreateDataTab.UseVisualStyleBackColor = true;
            // 
            // CreateResetButton
            // 
            CreateResetButton.Location = new System.Drawing.Point(613, 9);
            CreateResetButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            CreateResetButton.Name = "CreateResetButton";
            CreateResetButton.Size = new System.Drawing.Size(163, 96);
            CreateResetButton.TabIndex = 3;
            CreateResetButton.Text = "Reset";
            CreateResetButton.UseVisualStyleBackColor = true;
            CreateResetButton.Click += CreateResetButton_Click;
            // 
            // CreateWriteToDBButton
            // 
            CreateWriteToDBButton.Location = new System.Drawing.Point(783, 9);
            CreateWriteToDBButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            CreateWriteToDBButton.Name = "CreateWriteToDBButton";
            CreateWriteToDBButton.Size = new System.Drawing.Size(163, 96);
            CreateWriteToDBButton.TabIndex = 3;
            CreateWriteToDBButton.Text = "Write to DB";
            CreateWriteToDBButton.UseVisualStyleBackColor = true;
            CreateWriteToDBButton.Click += CreateWriteToDBButton_Click;
            // 
            // CreateKG3xxTablePanel
            // 
            CreateKG3xxTablePanel.AutoSize = true;
            CreateKG3xxTablePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            CreateKG3xxTablePanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            CreateKG3xxTablePanel.ColumnCount = 1;
            CreateKG3xxTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1635F));
            CreateKG3xxTablePanel.Location = new System.Drawing.Point(7, 120);
            CreateKG3xxTablePanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            CreateKG3xxTablePanel.Name = "CreateKG3xxTablePanel";
            CreateKG3xxTablePanel.RowCount = 4;
            CreateKG3xxTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            CreateKG3xxTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            CreateKG3xxTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            CreateKG3xxTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 240F));
            CreateKG3xxTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            CreateKG3xxTablePanel.Size = new System.Drawing.Size(1637, 725);
            CreateKG3xxTablePanel.TabIndex = 2;
            // 
            // CreateInstructionText3
            // 
            CreateInstructionText3.Location = new System.Drawing.Point(7, 81);
            CreateInstructionText3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            CreateInstructionText3.Name = "CreateInstructionText3";
            CreateInstructionText3.ReadOnly = true;
            CreateInstructionText3.Size = new System.Drawing.Size(173, 27);
            CreateInstructionText3.TabIndex = 0;
            CreateInstructionText3.Text = "3. Select LayerTypes for KG3xxes\r\n";
            // 
            // CreateConstructionTypeComboBox
            // 
            CreateConstructionTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            CreateConstructionTypeComboBox.FormattingEnabled = true;
            CreateConstructionTypeComboBox.Location = new System.Drawing.Point(187, 43);
            CreateConstructionTypeComboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            CreateConstructionTypeComboBox.Name = "CreateConstructionTypeComboBox";
            CreateConstructionTypeComboBox.Size = new System.Drawing.Size(138, 28);
            CreateConstructionTypeComboBox.TabIndex = 1;
            CreateConstructionTypeComboBox.SelectedIndexChanged += CreateConstructionTypeComboBox_CTypeSelected;
            // 
            // CreateInstructionText2
            // 
            CreateInstructionText2.Location = new System.Drawing.Point(7, 43);
            CreateInstructionText2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            CreateInstructionText2.Name = "CreateInstructionText2";
            CreateInstructionText2.ReadOnly = true;
            CreateInstructionText2.Size = new System.Drawing.Size(173, 27);
            CreateInstructionText2.TabIndex = 0;
            CreateInstructionText2.Text = "2. Select Construction Type\r\n";
            // 
            // CreateKG3x0ComboBox
            // 
            CreateKG3x0ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            CreateKG3x0ComboBox.FormattingEnabled = true;
            CreateKG3x0ComboBox.Location = new System.Drawing.Point(187, 4);
            CreateKG3x0ComboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            CreateKG3x0ComboBox.Name = "CreateKG3x0ComboBox";
            CreateKG3x0ComboBox.Size = new System.Drawing.Size(138, 28);
            CreateKG3x0ComboBox.TabIndex = 1;
            CreateKG3x0ComboBox.SelectedIndexChanged += CreateKG3x0ComboBox_KG3x0Selected;
            // 
            // CreateInstructionsText1
            // 
            CreateInstructionsText1.Location = new System.Drawing.Point(7, 4);
            CreateInstructionsText1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            CreateInstructionsText1.Name = "CreateInstructionsText1";
            CreateInstructionsText1.ReadOnly = true;
            CreateInstructionsText1.Size = new System.Drawing.Size(173, 27);
            CreateInstructionsText1.TabIndex = 0;
            CreateInstructionsText1.Text = "1. Select KG3x0";
            // 
            // DefaultThicknessRangesTabPage
            // 
            DefaultThicknessRangesTabPage.Controls.Add(DefaultThicknessRangeWriteButton);
            DefaultThicknessRangesTabPage.Controls.Add(DefaultThicknessRangesTable);
            DefaultThicknessRangesTabPage.Location = new System.Drawing.Point(4, 29);
            DefaultThicknessRangesTabPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            DefaultThicknessRangesTabPage.Name = "DefaultThicknessRangesTabPage";
            DefaultThicknessRangesTabPage.Size = new System.Drawing.Size(960, 667);
            DefaultThicknessRangesTabPage.TabIndex = 4;
            DefaultThicknessRangesTabPage.Text = "DefaultThicknessRanges";
            // 
            // DefaultThicknessRangeWriteButton
            // 
            DefaultThicknessRangeWriteButton.Dock = System.Windows.Forms.DockStyle.Right;
            DefaultThicknessRangeWriteButton.Location = new System.Drawing.Point(878, 0);
            DefaultThicknessRangeWriteButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            DefaultThicknessRangeWriteButton.Name = "DefaultThicknessRangeWriteButton";
            DefaultThicknessRangeWriteButton.Size = new System.Drawing.Size(82, 667);
            DefaultThicknessRangeWriteButton.TabIndex = 1;
            DefaultThicknessRangeWriteButton.Text = "Write to DB";
            DefaultThicknessRangeWriteButton.UseVisualStyleBackColor = true;
            DefaultThicknessRangeWriteButton.Click += DefaultThicknessRangeWriteButton_Click;
            // 
            // DefaultThicknessRangesTable
            // 
            DefaultThicknessRangesTable.AutoScroll = true;
            DefaultThicknessRangesTable.ColumnCount = 6;
            DefaultThicknessRangesTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            DefaultThicknessRangesTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            DefaultThicknessRangesTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 229F));
            DefaultThicknessRangesTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            DefaultThicknessRangesTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            DefaultThicknessRangesTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1366F));
            DefaultThicknessRangesTable.Location = new System.Drawing.Point(3, 4);
            DefaultThicknessRangesTable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            DefaultThicknessRangesTable.Name = "DefaultThicknessRangesTable";
            DefaultThicknessRangesTable.RowCount = 2;
            DefaultThicknessRangesTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            DefaultThicknessRangesTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            DefaultThicknessRangesTable.Size = new System.Drawing.Size(2086, 1333);
            DefaultThicknessRangesTable.TabIndex = 2;
            // 
            // ThicknessRangesTab
            // 
            ThicknessRangesTab.Controls.Add(ThicknessWriteButton);
            ThicknessRangesTab.Controls.Add(ThicknessRangeTable);
            ThicknessRangesTab.Location = new System.Drawing.Point(4, 29);
            ThicknessRangesTab.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            ThicknessRangesTab.Name = "ThicknessRangesTab";
            ThicknessRangesTab.Size = new System.Drawing.Size(960, 667);
            ThicknessRangesTab.TabIndex = 2;
            ThicknessRangesTab.Text = "ThicknessRanges";
            // 
            // ThicknessWriteButton
            // 
            ThicknessWriteButton.Dock = System.Windows.Forms.DockStyle.Right;
            ThicknessWriteButton.Location = new System.Drawing.Point(874, 0);
            ThicknessWriteButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            ThicknessWriteButton.Name = "ThicknessWriteButton";
            ThicknessWriteButton.Size = new System.Drawing.Size(86, 667);
            ThicknessWriteButton.TabIndex = 3;
            ThicknessWriteButton.Text = "Write to DB";
            ThicknessWriteButton.UseVisualStyleBackColor = true;
            ThicknessWriteButton.Click += ThicknessWriteButton_Click;
            // 
            // ThicknessRangeTable
            // 
            ThicknessRangeTable.AutoScroll = true;
            ThicknessRangeTable.ColumnCount = 8;
            ThicknessRangeTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            ThicknessRangeTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            ThicknessRangeTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 229F));
            ThicknessRangeTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 571F));
            ThicknessRangeTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            ThicknessRangeTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 137F));
            ThicknessRangeTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 137F));
            ThicknessRangeTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 727F));
            ThicknessRangeTable.Location = new System.Drawing.Point(0, 0);
            ThicknessRangeTable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            ThicknessRangeTable.Name = "ThicknessRangeTable";
            ThicknessRangeTable.RowCount = 1;
            ThicknessRangeTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1333F));
            ThicknessRangeTable.Size = new System.Drawing.Size(2086, 1333);
            ThicknessRangeTable.TabIndex = 0;
            // 
            // TwoCompUncertaintiesTab
            // 
            TwoCompUncertaintiesTab.Controls.Add(TwoComponentWriteButton);
            TwoCompUncertaintiesTab.Controls.Add(TwoCompUncertaintyTableLayoutPanel);
            TwoCompUncertaintiesTab.Location = new System.Drawing.Point(4, 29);
            TwoCompUncertaintiesTab.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            TwoCompUncertaintiesTab.Name = "TwoCompUncertaintiesTab";
            TwoCompUncertaintiesTab.Size = new System.Drawing.Size(960, 667);
            TwoCompUncertaintiesTab.TabIndex = 3;
            TwoCompUncertaintiesTab.Text = "TwoComponentUncertainties";
            // 
            // TwoComponentWriteButton
            // 
            TwoComponentWriteButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            TwoComponentWriteButton.Location = new System.Drawing.Point(867, 4);
            TwoComponentWriteButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            TwoComponentWriteButton.Name = "TwoComponentWriteButton";
            TwoComponentWriteButton.Size = new System.Drawing.Size(86, 655);
            TwoComponentWriteButton.TabIndex = 1;
            TwoComponentWriteButton.Text = "Write to DB";
            TwoComponentWriteButton.UseVisualStyleBackColor = true;
            TwoComponentWriteButton.Click += TwoComponentWriteButton_Click;
            // 
            // TwoCompUncertaintyTableLayoutPanel
            // 
            TwoCompUncertaintyTableLayoutPanel.AutoScroll = true;
            TwoCompUncertaintyTableLayoutPanel.ColumnCount = 11;
            TwoCompUncertaintyTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            TwoCompUncertaintyTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            TwoCompUncertaintyTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 229F));
            TwoCompUncertaintyTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            TwoCompUncertaintyTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            TwoCompUncertaintyTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            TwoCompUncertaintyTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            TwoCompUncertaintyTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            TwoCompUncertaintyTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            TwoCompUncertaintyTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            TwoCompUncertaintyTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 796F));
            TwoCompUncertaintyTableLayoutPanel.Location = new System.Drawing.Point(0, 4);
            TwoCompUncertaintyTableLayoutPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            TwoCompUncertaintyTableLayoutPanel.Name = "TwoCompUncertaintyTableLayoutPanel";
            TwoCompUncertaintyTableLayoutPanel.RowCount = 1;
            TwoCompUncertaintyTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            TwoCompUncertaintyTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            TwoCompUncertaintyTableLayoutPanel.Size = new System.Drawing.Size(2086, 1333);
            TwoCompUncertaintyTableLayoutPanel.TabIndex = 0;
            // 
            // CostRangeTabPage
            // 
            CostRangeTabPage.Controls.Add(CostRangeWritebutton);
            CostRangeTabPage.Controls.Add(CostRangesTableLayoutPanel);
            CostRangeTabPage.Location = new System.Drawing.Point(4, 29);
            CostRangeTabPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            CostRangeTabPage.Name = "CostRangeTabPage";
            CostRangeTabPage.Size = new System.Drawing.Size(960, 667);
            CostRangeTabPage.TabIndex = 6;
            CostRangeTabPage.Text = "Cost Ranges";
            CostRangeTabPage.UseVisualStyleBackColor = true;
            // 
            // CostRangeWritebutton
            // 
            CostRangeWritebutton.Dock = System.Windows.Forms.DockStyle.Right;
            CostRangeWritebutton.Location = new System.Drawing.Point(874, 0);
            CostRangeWritebutton.Name = "CostRangeWritebutton";
            CostRangeWritebutton.Size = new System.Drawing.Size(86, 667);
            CostRangeWritebutton.TabIndex = 6;
            CostRangeWritebutton.Text = "Write to DB";
            CostRangeWritebutton.UseVisualStyleBackColor = true;
            CostRangeWritebutton.Click += CostRangeWriteButton_Click;
            // 
            // CostRangesTableLayoutPanel
            // 
            CostRangesTableLayoutPanel.ColumnCount = 9;
            CostRangesTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            CostRangesTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            CostRangesTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            CostRangesTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 343F));
            CostRangesTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            CostRangesTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            CostRangesTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 137F));
            CostRangesTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 137F));
            CostRangesTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1070F));
            CostRangesTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            CostRangesTableLayoutPanel.Location = new System.Drawing.Point(3, 0);
            CostRangesTableLayoutPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            CostRangesTableLayoutPanel.Name = "CostRangesTableLayoutPanel";
            CostRangesTableLayoutPanel.RowCount = 1;
            CostRangesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            CostRangesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            CostRangesTableLayoutPanel.Size = new System.Drawing.Size(2086, 1338);
            CostRangesTableLayoutPanel.TabIndex = 5;
            // 
            // ViewDataTabPage
            // 
            ViewDataTabPage.Controls.Add(textBox9);
            ViewDataTabPage.Controls.Add(textBox8);
            ViewDataTabPage.Controls.Add(textBox7);
            ViewDataTabPage.Controls.Add(ViewCTypeFilter);
            ViewDataTabPage.Controls.Add(ViewKg3x0Filter);
            ViewDataTabPage.Controls.Add(ViewTable);
            ViewDataTabPage.Controls.Add(ViewTextCTypeFilter);
            ViewDataTabPage.Controls.Add(ViewTextKG3x0Filter);
            ViewDataTabPage.Location = new System.Drawing.Point(4, 29);
            ViewDataTabPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            ViewDataTabPage.Name = "ViewDataTabPage";
            ViewDataTabPage.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            ViewDataTabPage.Size = new System.Drawing.Size(960, 667);
            ViewDataTabPage.TabIndex = 1;
            ViewDataTabPage.Text = "View Data";
            ViewDataTabPage.UseVisualStyleBackColor = true;
            // 
            // textBox9
            // 
            textBox9.BackColor = System.Drawing.Color.FromArgb(192, 255, 255);
            textBox9.Location = new System.Drawing.Point(735, 81);
            textBox9.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            textBox9.Name = "textBox9";
            textBox9.ReadOnly = true;
            textBox9.Size = new System.Drawing.Size(218, 27);
            textBox9.TabIndex = 0;
            textBox9.Text = "Structural & Therm. Variation Layer";
            // 
            // textBox8
            // 
            textBox8.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);
            textBox8.Location = new System.Drawing.Point(735, 43);
            textBox8.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            textBox8.Name = "textBox8";
            textBox8.ReadOnly = true;
            textBox8.Size = new System.Drawing.Size(218, 27);
            textBox8.TabIndex = 0;
            textBox8.Text = "Structural Variation Layer";
            // 
            // textBox7
            // 
            textBox7.BackColor = System.Drawing.Color.FromArgb(255, 192, 192);
            textBox7.Location = new System.Drawing.Point(735, 4);
            textBox7.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            textBox7.Name = "textBox7";
            textBox7.ReadOnly = true;
            textBox7.Size = new System.Drawing.Size(218, 27);
            textBox7.TabIndex = 0;
            textBox7.Text = "Thermal Variation Layer";
            // 
            // ViewCTypeFilter
            // 
            ViewCTypeFilter.CheckOnClick = true;
            ViewCTypeFilter.FormattingEnabled = true;
            ViewCTypeFilter.Location = new System.Drawing.Point(470, 4);
            ViewCTypeFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            ViewCTypeFilter.Name = "ViewCTypeFilter";
            ViewCTypeFilter.Size = new System.Drawing.Size(147, 48);
            ViewCTypeFilter.TabIndex = 3;
            // 
            // ViewKg3x0Filter
            // 
            ViewKg3x0Filter.CheckOnClick = true;
            ViewKg3x0Filter.FormattingEnabled = true;
            ViewKg3x0Filter.Location = new System.Drawing.Point(161, 4);
            ViewKg3x0Filter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            ViewKg3x0Filter.Name = "ViewKg3x0Filter";
            ViewKg3x0Filter.Size = new System.Drawing.Size(147, 48);
            ViewKg3x0Filter.TabIndex = 3;
            // 
            // ViewTable
            // 
            ViewTable.AutoScroll = true;
            ViewTable.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            ViewTable.ColumnCount = 1;
            ViewTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 2241F));
            ViewTable.Location = new System.Drawing.Point(3, 137);
            ViewTable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            ViewTable.Name = "ViewTable";
            ViewTable.RowCount = 1;
            ViewTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            ViewTable.Size = new System.Drawing.Size(2171, 1200);
            ViewTable.TabIndex = 2;
            // 
            // ViewTextCTypeFilter
            // 
            ViewTextCTypeFilter.Location = new System.Drawing.Point(315, 4);
            ViewTextCTypeFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            ViewTextCTypeFilter.Name = "ViewTextCTypeFilter";
            ViewTextCTypeFilter.ReadOnly = true;
            ViewTextCTypeFilter.Size = new System.Drawing.Size(147, 27);
            ViewTextCTypeFilter.TabIndex = 0;
            ViewTextCTypeFilter.Text = "Filter ConstructionType";
            // 
            // ViewTextKG3x0Filter
            // 
            ViewTextKG3x0Filter.Location = new System.Drawing.Point(7, 4);
            ViewTextKG3x0Filter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            ViewTextKG3x0Filter.Name = "ViewTextKG3x0Filter";
            ViewTextKG3x0Filter.ReadOnly = true;
            ViewTextKG3x0Filter.Size = new System.Drawing.Size(147, 27);
            ViewTextKG3x0Filter.TabIndex = 0;
            ViewTextKG3x0Filter.Text = "Filter KG3x0";
            // 
            // UtilityTabPage
            // 
            UtilityTabPage.Controls.Add(UtilityDefaultHeightTextbox);
            UtilityTabPage.Controls.Add(UtilityDefaultHeightSetter);
            UtilityTabPage.Controls.Add(UtilityImportBuildingPartsButton);
            UtilityTabPage.Controls.Add(UtilityExportBuildingPartsButton);
            UtilityTabPage.Controls.Add(UtilityImportCostRangesButton);
            UtilityTabPage.Controls.Add(UtilityExportCostRangesButton);
            UtilityTabPage.Controls.Add(UtilityDeleteUnusedKG3xxsButton);
            UtilityTabPage.Controls.Add(UtilityImportThicknessesButton);
            UtilityTabPage.Controls.Add(UtilityExportThicknessesButton);
            UtilityTabPage.Location = new System.Drawing.Point(4, 29);
            UtilityTabPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            UtilityTabPage.Name = "UtilityTabPage";
            UtilityTabPage.Size = new System.Drawing.Size(960, 667);
            UtilityTabPage.TabIndex = 5;
            UtilityTabPage.Text = "Utility Functions";
            // 
            // UtilityDefaultHeightTextbox
            // 
            UtilityDefaultHeightTextbox.Location = new System.Drawing.Point(7, 258);
            UtilityDefaultHeightTextbox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            UtilityDefaultHeightTextbox.Name = "UtilityDefaultHeightTextbox";
            UtilityDefaultHeightTextbox.ReadOnly = true;
            UtilityDefaultHeightTextbox.Size = new System.Drawing.Size(157, 27);
            UtilityDefaultHeightTextbox.TabIndex = 6;
            UtilityDefaultHeightTextbox.Text = "Textbox Height";
            // 
            // UtilityDefaultHeightSetter
            // 
            UtilityDefaultHeightSetter.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            UtilityDefaultHeightSetter.Location = new System.Drawing.Point(170, 259);
            UtilityDefaultHeightSetter.Minimum = new decimal(new int[] { 15, 0, 0, 0 });
            UtilityDefaultHeightSetter.Name = "UtilityDefaultHeightSetter";
            UtilityDefaultHeightSetter.Size = new System.Drawing.Size(157, 27);
            UtilityDefaultHeightSetter.TabIndex = 5;
            UtilityDefaultHeightSetter.Value = new decimal(new int[] { 25, 0, 0, 0 });
            // 
            // UtilityImportBuildingPartsButton
            // 
            UtilityImportBuildingPartsButton.Location = new System.Drawing.Point(170, 175);
            UtilityImportBuildingPartsButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            UtilityImportBuildingPartsButton.Name = "UtilityImportBuildingPartsButton";
            UtilityImportBuildingPartsButton.Size = new System.Drawing.Size(157, 77);
            UtilityImportBuildingPartsButton.TabIndex = 4;
            UtilityImportBuildingPartsButton.Text = "Import Building Parts from csv";
            UtilityImportBuildingPartsButton.UseVisualStyleBackColor = true;
            UtilityImportBuildingPartsButton.Click += ImportBuildingPartButton_Click;
            // 
            // UtilityExportBuildingPartsButton
            // 
            UtilityExportBuildingPartsButton.Location = new System.Drawing.Point(7, 175);
            UtilityExportBuildingPartsButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            UtilityExportBuildingPartsButton.Name = "UtilityExportBuildingPartsButton";
            UtilityExportBuildingPartsButton.Size = new System.Drawing.Size(157, 77);
            UtilityExportBuildingPartsButton.TabIndex = 3;
            UtilityExportBuildingPartsButton.Text = "Export Building Parts to csv";
            UtilityExportBuildingPartsButton.UseVisualStyleBackColor = true;
            UtilityExportBuildingPartsButton.Click += ExportBuildingPartButtonClick;
            // 
            // UtilityImportCostRangesButton
            // 
            UtilityImportCostRangesButton.Location = new System.Drawing.Point(170, 89);
            UtilityImportCostRangesButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            UtilityImportCostRangesButton.Name = "UtilityImportCostRangesButton";
            UtilityImportCostRangesButton.Size = new System.Drawing.Size(157, 77);
            UtilityImportCostRangesButton.TabIndex = 2;
            UtilityImportCostRangesButton.Text = "Import Cost Ranges from csv";
            UtilityImportCostRangesButton.UseVisualStyleBackColor = true;
            UtilityImportCostRangesButton.Click += ImportCostRangesButtonClick;
            // 
            // UtilityExportCostRangesButton
            // 
            UtilityExportCostRangesButton.Location = new System.Drawing.Point(7, 89);
            UtilityExportCostRangesButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            UtilityExportCostRangesButton.Name = "UtilityExportCostRangesButton";
            UtilityExportCostRangesButton.Size = new System.Drawing.Size(157, 77);
            UtilityExportCostRangesButton.TabIndex = 1;
            UtilityExportCostRangesButton.Text = "Export Cost Ranges to csv";
            UtilityExportCostRangesButton.UseVisualStyleBackColor = true;
            UtilityExportCostRangesButton.Click += ExportCostRangesButtonClick;
            // 
            // UtilityDeleteUnusedKG3xxsButton
            // 
            UtilityDeleteUnusedKG3xxsButton.Location = new System.Drawing.Point(334, 4);
            UtilityDeleteUnusedKG3xxsButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            UtilityDeleteUnusedKG3xxsButton.Name = "UtilityDeleteUnusedKG3xxsButton";
            UtilityDeleteUnusedKG3xxsButton.Size = new System.Drawing.Size(157, 77);
            UtilityDeleteUnusedKG3xxsButton.TabIndex = 0;
            UtilityDeleteUnusedKG3xxsButton.Text = "Delete unused KG3xxs and Thicknesses";
            UtilityDeleteUnusedKG3xxsButton.UseVisualStyleBackColor = true;
            UtilityDeleteUnusedKG3xxsButton.Click += DeleteUnneccessaryKG3xxsButton_Click;
            // 
            // UtilityImportThicknessesButton
            // 
            UtilityImportThicknessesButton.Location = new System.Drawing.Point(170, 4);
            UtilityImportThicknessesButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            UtilityImportThicknessesButton.Name = "UtilityImportThicknessesButton";
            UtilityImportThicknessesButton.Size = new System.Drawing.Size(157, 77);
            UtilityImportThicknessesButton.TabIndex = 0;
            UtilityImportThicknessesButton.Text = "Import Thicknesses from csv";
            UtilityImportThicknessesButton.UseVisualStyleBackColor = true;
            UtilityImportThicknessesButton.Click += ImportThicknessrangesButton_Click;
            // 
            // UtilityExportThicknessesButton
            // 
            UtilityExportThicknessesButton.Location = new System.Drawing.Point(7, 4);
            UtilityExportThicknessesButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            UtilityExportThicknessesButton.Name = "UtilityExportThicknessesButton";
            UtilityExportThicknessesButton.Size = new System.Drawing.Size(157, 77);
            UtilityExportThicknessesButton.TabIndex = 0;
            UtilityExportThicknessesButton.Text = "Export Thicknesses to csv";
            UtilityExportThicknessesButton.UseVisualStyleBackColor = true;
            UtilityExportThicknessesButton.Click += ExportThicknessrangesButton_Click;
            // 
            // SelectDbDialog
            // 
            SelectDbDialog.FileName = "Select Database";
            // 
            // UtilitySelectExportFolderDialog
            // 
            UtilitySelectExportFolderDialog.Description = "Select csv location Folder";
            UtilitySelectExportFolderDialog.UseDescriptionForTitle = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoScroll = true;
            tableLayoutPanel1.ColumnCount = 8;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 500F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // KnowledgeDbNavigator
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(974, 703);
            Controls.Add(NavigatorTabControl);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "KnowledgeDbNavigator";
            Text = "EarlyBIM ExpertGUI";
            NavigatorTabControl.ResumeLayout(false);
            CreateDataTab.ResumeLayout(false);
            CreateDataTab.PerformLayout();
            DefaultThicknessRangesTabPage.ResumeLayout(false);
            ThicknessRangesTab.ResumeLayout(false);
            TwoCompUncertaintiesTab.ResumeLayout(false);
            CostRangeTabPage.ResumeLayout(false);
            ViewDataTabPage.ResumeLayout(false);
            ViewDataTabPage.PerformLayout();
            UtilityTabPage.ResumeLayout(false);
            UtilityTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)UtilityDefaultHeightSetter).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl NavigatorTabControl;
        private System.Windows.Forms.TabPage CreateDataTab;
        private System.Windows.Forms.TabPage ViewDataTabPage;
        private System.Windows.Forms.OpenFileDialog SelectDbDialog;
        private System.Windows.Forms.TextBox CreateInstructionsText1;
        private System.Windows.Forms.ComboBox CreateKG3x0ComboBox;
        private System.Windows.Forms.TextBox CreateInstructionText2;
        private System.Windows.Forms.ComboBox CreateConstructionTypeComboBox;
        private System.Windows.Forms.TableLayoutPanel CreateKG3xxTablePanel;
        private System.Windows.Forms.TextBox CreateInstructionText3;
        private System.Windows.Forms.Button CreateWriteToDBButton;
        private System.Windows.Forms.TableLayoutPanel ViewTable;
        private System.Windows.Forms.TextBox ViewTextCTypeFilter;
        private System.Windows.Forms.TextBox ViewTextKG3x0Filter;
        private System.Windows.Forms.CheckedListBox ViewCTypeFilter;
        private System.Windows.Forms.CheckedListBox ViewKg3x0Filter;
        private System.Windows.Forms.TabPage ThicknessRangesTab;
        private System.Windows.Forms.TableLayoutPanel ThicknessRangeTable;
        private System.Windows.Forms.Button ThicknessWriteButton;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.TabPage TwoCompUncertaintiesTab;
        private System.Windows.Forms.Button TwoComponentWriteButton;
        private System.Windows.Forms.TableLayoutPanel TwoCompUncertaintyTableLayoutPanel;
        private System.Windows.Forms.Button CreateResetButton;
        private System.Windows.Forms.TabPage DefaultThicknessRangesTabPage;
        private System.Windows.Forms.TableLayoutPanel DefaultThicknessRangesTable;
        private System.Windows.Forms.Button DefaultThicknessRangeWriteButton;
        private System.Windows.Forms.FolderBrowserDialog UtilitySelectExportFolderDialog;
        private System.Windows.Forms.TabPage CostRangeTabPage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel CostRangesTableLayoutPanel;
        private System.Windows.Forms.TabPage UtilityTabPage;
        private System.Windows.Forms.Button UtilityImportCostRangesButton;
        private System.Windows.Forms.Button UtilityExportCostRangesButton;
        private System.Windows.Forms.Button UtilityDeleteUnusedKG3xxsButton;
        private System.Windows.Forms.Button UtilityImportThicknessesButton;
        private System.Windows.Forms.Button UtilityExportThicknessesButton;
        private System.Windows.Forms.Button UtilityImportBuildingPartsButton;
        private System.Windows.Forms.Button UtilityExportBuildingPartsButton;
        private System.Windows.Forms.TextBox UtilityDefaultHeightTextbox;
        private System.Windows.Forms.NumericUpDown UtilityDefaultHeightSetter;
        private System.Windows.Forms.Button CostRangeWritebutton;
    }
}



