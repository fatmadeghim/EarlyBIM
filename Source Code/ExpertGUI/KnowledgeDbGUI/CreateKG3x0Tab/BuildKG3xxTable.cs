using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using KnowledgeDB;
using System.Diagnostics;

namespace KnowledgeDbGUI.CreateKG3x0Tab
{
    public class BuildKG3xxTable
    {
        public TableLayoutPanel Table { get; set; }
        public List<CreateKG3xxTableColumn> Columns { get; set; } = new List<CreateKG3xxTableColumn>();

        public BuildKG3xxTable(TableLayoutPanel table)
        {
            this.Table = table;
            table.AutoScroll = false;
        }
        public void Load(string kG3x0Name, KnowledgeContext context, CreateDataManager createDataManager)
        {
            var kg3xxNames = context.FindKG3xxNamesWhereKg3x0NameIs(kG3x0Name);

            //Makes each column the same size
            for (var i = 0; i < kg3xxNames.Count; i++) 
            {
                //Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.0F / kg3xxNames.Count));
                Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1900 / kg3xxNames.Count));
            }
            Table.ColumnStyles.RemoveAt(0); //Windows Forms requires a columnstyle at initialization. We remove this style after adding our own styles above.
            
            //Add columns
            var columncount = 0;
            foreach (var kg3xxname in kg3xxNames)
            {
                var layerTypeNames = context.LayerTypeNames.Select(lt => lt.Name).ToList();
                layerTypeNames.Sort();
                var column = new CreateKG3xxTableColumn(kg3xxname,
                                                        context.ReplacementOrders.Select(ro => ro.Order).ToList(),
                                                        context.ReplacementOrders.Where(ro => ro.Id == kg3xxname.ReplacementOrderId).Select
                                                                                                            (ro => ro.Order).FirstOrDefault(),
                                                        layerTypeNames,
                                                        context.VariationTargets.Select(vt => vt.Name).ToList(),
                                                        createDataManager);
                Columns.Add(column);
                column.Show(Table, columncount);
                columncount += 1;
            }
            Table.ColumnCount -= 1;
        }

        public void Reset()
        {
            Columns.Clear();
            Table.Controls.Clear();
            Table.ColumnCount = 1;
        }

        //Load a KG3x0Option into the table 
        public void ImportKG3x0Option(KG3x0Option kg3x0Option, KnowledgeContext context)
        {
            var kg3xxs = (from kg3xx in context.KG3xxOptions
                          join kg3x0_kg3xx in context.KG3x0_KG3xxs on kg3xx.Id equals kg3x0_kg3xx.Id2
                          where kg3x0_kg3xx.Id1 == kg3x0Option.Id
                          orderby kg3x0_kg3xx.Position
                          select kg3xx).ToList();

            var varParams = context.VariationParams.Where(vp => vp.KG3x0OptionId == kg3x0Option.Id).ToList();

            for (int i = 0; i < Columns.Count; i++)
            {
                Columns[i].ImportKG3xx(kg3xxs[i], varParams, context);
            }
        }

        public List<KG3xxOption> BuildKG3xxes(KnowledgeContext context)
        {
            var kg3xxs = new List<KG3xxOption>();
            foreach (var column in Columns)
            {
                kg3xxs.Add(column.BuildKG3xx(context));
            }
            return kg3xxs;
        }
    }
}
