using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;

using KnowledgeDB;
using GUIHelpFunctions;


namespace KnowledgeDbGUI.ViewTab
{
    public class ViewTableKG3x0Table
    {
        public string Name { get; set; }

        private TextBox nameTextBox;

        private TextBox cTypeTextBox;

        private TextBox modifyTextBox;

        private TextBox deleteTextBox;

        private List<TextBox> headLineTextBoxes;

        private List<ViewTableKG3x0TableEntry> kg3x0TableEntries;

        private List<int> cutOffColumns; //Columns where KG3xxOptions end, important for LayoutManagement
        public TableLayoutPanel Table { get; set; } //to be deleted
        
        public ViewTableKG3x0Table(string name)
        {
            this.Name = name;
            this.headLineTextBoxes = new List<TextBox>();
            this.cutOffColumns = new List<int>();
            this.kg3x0TableEntries = new List<ViewTableKG3x0TableEntry>();
            nameTextBox = FormsHelper.CreateTextBox(name, 400, 40);

            Table = FormsHelper.CreateTable(1, 1);
            Table.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            nameTextBox = FormsHelper.CreateTextBox(name, 400, 40);
            Table.Controls.Add(nameTextBox, 0, 0);
            //Table.Size = new System.Drawing.Size(1850, 200);

            Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));

            //Table.AutoScroll = true;
            Table.AutoSize = true;
        }

        public void BuildTable(KnowledgeContext context, List<string> cTypeFilter, EventHandler resetEventHandler, Action<KG3x0Option> importAction)
        {
            //Create Headline
            Table.RowCount++; //Headline For second Row
            cTypeTextBox = FormsHelper.CreateTextBox("Construction Type", 60);

            foreach (var kg3xx in context.FindKG3xxNamesWhereKg3x0NameIs(Name))
            {
                headLineTextBoxes.Add(FormsHelper.CreateTextBox(kg3xx.Name, 60));
                cutOffColumns.Add(1);
            }
            cutOffColumns.Add(1); //Cutoffcolumns Entries initialized with 1, these will be updated when the entries are loaded into the Table

            deleteTextBox = FormsHelper.CreateTextBox("Delete", 50);

            modifyTextBox = FormsHelper.CreateTextBox("Import", 50);

            LoadEntries(context, cTypeFilter, resetEventHandler, importAction);
        }

        private void LoadEntries(KnowledgeContext context, List<string> cTypeFilter, EventHandler resetEventHandler, Action<KG3x0Option> importAction)
        {
            //Collect Kg3x0Options for given Construction Type
            var kg3x0Options = (from kg3x0 in context.KG3x0Options
                                join kg3x0n in context.KG3x0Names on kg3x0.NameId equals kg3x0n.Id
                                join ctype in context.ConstructionTypeNames on kg3x0.ConstructionTypeNameId equals ctype.Id
                                where cTypeFilter.Contains(ctype.Name) &&
                                      kg3x0n.Name == Name
                                select kg3x0).ToList();

            //Create entry for each KG3x0Option
            foreach (var kg3x0 in kg3x0Options)
            {
                var entry = new ViewTableKG3x0TableEntry(kg3x0, context, resetEventHandler, importAction);
                kg3x0TableEntries.Add(entry);
            }
        }

        public void Show(TableLayoutPanel higherLevelTable, int row) 
        {
            if (kg3x0TableEntries.Count == 0) //Case: No entries for given filters
            {
                Table.Controls.Add(nameTextBox, 0, 30);
                Table.Controls.Add(FormsHelper.CreateTextBox("No entries for given filters", 200));
                higherLevelTable.Controls.Add(Table, 0, row);
                return;
            }

            //Put Entries into Table
            var localrownumber = 2;
            for (var index = 0; index < cutOffColumns.Count-1; index++)
            {
                foreach(var entry in kg3x0TableEntries)
                {
                    cutOffColumns = entry.InsertIntoTable(localrownumber, Table, cutOffColumns, index);
                    localrownumber++;
                }
                localrownumber = 2;
            }

            //Set final columncount
            Table.ColumnCount = cutOffColumns.Last() + 2;

            //Put static elements into Table
            foreach(var entry in kg3x0TableEntries)
            {
                Table.Controls.Add(entry.CTypeTextBox, 0, localrownumber);
                Table.Controls.Add(entry.ImportToCreateButton, cutOffColumns.Last(), localrownumber);
                Table.Controls.Add(entry.DeleteButton, cutOffColumns.Last()+1, localrownumber);
                localrownumber++;
            }

            //Put Headline into Table
            Table.Controls.Add(cTypeTextBox, 0, 1);
            for (int i = 0; i < headLineTextBoxes.Count; i++)
            {
                Table.Controls.Add(headLineTextBoxes[i], cutOffColumns[i], 1);
            }
            Table.Controls.Add(modifyTextBox, cutOffColumns.Last(), 1);
            Table.Controls.Add(deleteTextBox, cutOffColumns.Last()+1, 1);

            //Layout
            //Make Headline Textboxes bigger
            Table.SetColumnSpan(nameTextBox, Table.ColumnCount);
            for (int i = 0; i < headLineTextBoxes.Count; i++)
            {
                {
                    Table.SetColumnSpan(headLineTextBoxes[i], cutOffColumns[i + 1] - cutOffColumns[i]);
                }
            }
            //Limit Line Height
            Table.RowStyles.Clear();
            Table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            for(var i = 0; i <= cutOffColumns.Last()+1; i++)
            {
                Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            }

            //Put into higher order table
            higherLevelTable.Controls.Add(Table, 0, row);
        }
    }
}
