using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using KnowledgeDB;
using GUIHelpFunctions;
using System.Windows.Documents;


namespace KnowledgeDbGUI.GenericThicknessTable
{
    public abstract class GenericThicknessTable<T1, T2> where T1: GenericThicknessTableRow<T2>
                                                        where T2 : IRange
    {
        public TableLayoutPanel Table;
        protected List<T1> rows;
        protected List<T1> changedRows;

        public GenericThicknessTable(TableLayoutPanel table)
        {
            this.Table = table;

            //Layout
            table.RowStyles.Clear();
            table.RowStyles.Add((new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, FormsHelper.GetDefaultHeight())));
            table.AutoScroll = true;


            rows = new List<T1>();
            changedRows = new List<T1>();
            BuildHeadline();
        }
        public abstract void BuildHeadline();

        public abstract void BuildTable(KnowledgeContext context);

        public virtual void ResetTable(KnowledgeContext context) //Reloads the table, this is called whenever the Tab is entered
        {
            this.Table.RowStyles.Clear();
            this.Table.RowStyles.Add((new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, FormsHelper.GetDefaultHeight())));

            foreach (var row in rows)
            {
                row.DeleteFromTable(Table);
            }
            rows.Clear();
            changedRows.Clear();

            BuildTable(context);
        }

        public virtual void WriteChanges(KnowledgeContext context)
        {
            foreach (var changedRow in changedRows)
            {
                try
                {
                    changedRow.PrepareChanges();
                }
                catch (Exception e)
                {
                    FormsHelper.ShowErrorMessage("Error in Row " + (rows.IndexOf(changedRow) + 1).ToString() + ": " + e.Message,
                                                 "Can't generate Data");
                    return;
                }
            }
            //Only actually change something if all changes are okay
            foreach (var changedRow in changedRows)
            {
                changedRow.WriteChanges();
            }
            context.SaveChanges();
        }
    }
}
