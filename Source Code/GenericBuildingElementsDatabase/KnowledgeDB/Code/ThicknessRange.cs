using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

using SharedDBLibrary;

namespace KnowledgeDB
{
    public class ThicknessRange : IThickness, IKnowledgeCSVExportable
    {
        public int KG3xxNameId { get; set; }
        public KG3xxName KG3xxName {get; set; }
        public int LayerTypeNameId { get; set; }
        public LayerTypeName LayerTypeName { get; set; }
        public int LayerId { get; set; }
        public Layer Layer { get; set; }
        public double ThicknessMin { get; set; }
        public double ThicknessAverage { get; set; }
        public double ThicknessMax { get; set; }
        public bool IsDefault { get; set; }
        public ThicknessRange() { } //default constructor for EF to work
        public static void BuildThicknessRanges(KG3xxName kgName, List<LayerTypeName> ltNames, KnowledgeContext context)
        {
            foreach (var ltName in ltNames)
            {
                var layers = (from layer in context.Layers
                              join lay_lt in context.Layer_StandardLayerTypes on layer.Id equals lay_lt.Id1
                              join st_lay in context.StandardLayerTypes on lay_lt.Id2 equals st_lay.Id
                              join kg_lay in context.KG3xxName_Layers on lay_lt.Id1 equals kg_lay.Id2
                              where kgName.Id == kg_lay.Id1 && ltName.Id == st_lay.NameId
                              select layer).ToList();

                foreach (var layer in layers)
                {
                    var existingRange = context.ThicknessRanges.Where(tRange => tRange.KG3xxNameId == kgName.Id && tRange.LayerId == layer.Id &&
                                                                                tRange.LayerTypeNameId == ltName.Id).FirstOrDefault();
                    if (existingRange == null)
                    {
                        var defaultThickness = context.DefaultThicknessRanges.Where(dtr => dtr.KG3xxNameId == kgName.Id &&
                                                                                    dtr.LayerTypeNameId == ltName.Id).FirstOrDefault();
                        context.ThicknessRanges.Add(new ThicknessRange(kgName, ltName, layer, defaultThickness, context.FindUnitOfLayer(layer)));
                        context.SaveChanges(); //Needs to be saved in order to be found in the next layer's search for existingRange.
                    }
                }
            }
        }

        //Constructor is private because ThicknessRanges should only be created using the BuildThicknessRanges method
        private ThicknessRange(KG3xxName name, LayerTypeName ltName, Layer layer, DefaultThicknessRange defaultThickness, Unit unit)
        {
            this.KG3xxName = name;
            this.LayerTypeName = ltName;
            this.Layer = layer;
            this.IsDefault = true;
            if (unit.ReferenceUnit == "qm")
            {
                this.IsDefault = false;
                ThicknessMin = 1.0;
                ThicknessAverage = 1.0;
                ThicknessMax = 1.0;
            }
            else
            {
                ThicknessMin = defaultThickness.ThicknessMin;
                ThicknessAverage = defaultThickness.ThicknessAverage;
                ThicknessMax = defaultThickness.ThicknessMax;
            }
        }

        public List<double> getRangeList()
        {
            return new List<double>() { ThicknessMin, ThicknessAverage, ThicknessMax };
        }
        public double getMin() { return ThicknessMin; }
        public double getAvg() { return ThicknessAverage; }
        public double getMax() { return ThicknessMax; }
        public void setMin(double value) { ThicknessMin = value; }
        public void setAvg(double value) { ThicknessAverage = value; }
        public void setMax(double value) { ThicknessMax = value; }

        //CSV Export
        public static string getCSVHeadlineStatic()
        {
            return "KG3xxName;LayerTypeName;LayerName;LayerUUID;ThicknessMin;ThicknessAverage;ThicknessMax;IsDefault";
        }

        public string getCSVHeadline()
        {
            return "KG3xxName;LayerTypeName;LayerName;LayerUUID;ThicknessMin;ThicknessAverage;ThicknessMax;IsDefault";
        }

        public string getCSVLine(KnowledgeContext knowledgeContext)
        {
            return knowledgeContext.KG3xxNames.Where(kg3xxN => kg3xxN.Id == KG3xxNameId).Select(kg3xxN => kg3xxN.Name).FirstOrDefault() + ";" +
                   knowledgeContext.LayerTypeNames.Where(ltn => ltn.Id == LayerTypeNameId).Select(ltn => ltn.Name).FirstOrDefault() + ";" +
                   knowledgeContext.Layers.Where(l => l.Id == LayerId).Select(l => l.Name).FirstOrDefault() + ";" +
                   knowledgeContext.Layers.Where(l => l.Id == LayerId).Select(l => l.UUID).FirstOrDefault() + ";" +
                   ThicknessMin.ToString() + ";" + ThicknessAverage.ToString() + ";" + ThicknessMax.ToString() + ";" + IsDefault.ToString();

        }
    }

    //The following class is used to store non default thicknessranges while updating the DB's layers (which causes thicknessrange entries to get lost)
    public class TempThicknessRangeStorage
    {
        public string KG3xxName { get; set; }
        public string LayerTypeName { get; set; }
        public string LayerName { get; set; }
        public double ThicknessMin { get; set; }
        public double ThicknessAvg { get; set; }
        public double ThicknessMax { get; set; }

        public TempThicknessRangeStorage(ThicknessRange thicknessRange, KnowledgeContext context)
        {
            this.KG3xxName = context.KG3xxNames.Where(kgn => kgn.Id == thicknessRange.KG3xxNameId).Select(kgn => kgn.Name).FirstOrDefault();
            this.LayerTypeName = context.LayerTypeNames.Where(ltn => ltn.Id == thicknessRange.LayerTypeNameId).Select(ltn => ltn.Name).FirstOrDefault();
            this.LayerName = context.Layers.Where(l => l.Id == thicknessRange.LayerId).Select(l => l.Name).FirstOrDefault();
            this.ThicknessMin = thicknessRange.ThicknessMin;
            this.ThicknessAvg = thicknessRange.ThicknessAverage;
            this.ThicknessMax = thicknessRange.ThicknessMax;
        }

        public void RestoreToDB(KnowledgeContext context)
        {
            //Check if default entry already exists
            var exTr = context.ThicknessRanges.Where(tr =>
                          tr.KG3xxNameId == context.KG3xxNames.Where(kgn => kgn.Name.Equals(this.KG3xxName)).Select(kgn => kgn.Id).FirstOrDefault() &&
                          tr.LayerTypeNameId == context.LayerTypeNames.Where(ltn => ltn.Name.Equals(this.LayerTypeName)).Select(ltn => ltn.Id).FirstOrDefault() &&
                          tr.LayerId == context.Layers.Where(l => l.Name.Equals(this.LayerName)).Select(l => l.Id).FirstOrDefault()).FirstOrDefault();

            if(exTr != null)
            {
                exTr.ThicknessMin = this.ThicknessMin;
                exTr.ThicknessAverage = this.ThicknessAvg;
                exTr.ThicknessMax = this.ThicknessMax;
                exTr.IsDefault = false;
            }
            else
            {
                var tr = new ThicknessRange()
                {
                    KG3xxNameId = context.KG3xxNames.Where(kgn => kgn.Name.Equals(this.KG3xxName)).Select(kgn => kgn.Id).FirstOrDefault(),
                    LayerTypeNameId = context.LayerTypeNames.Where(ltn => ltn.Name.Equals(this.LayerTypeName)).Select(ltn => ltn.Id).FirstOrDefault(),
                    LayerId = context.Layers.Where(l => l.Name.Equals(this.LayerName)).Select(l => l.Id).FirstOrDefault(),
                    ThicknessMin = this.ThicknessMin,
                    ThicknessAverage = this.ThicknessAvg,
                    ThicknessMax = this.ThicknessMax,
                    IsDefault = false,
                };
                context.ThicknessRanges.Add(tr);
            }
            context.SaveChanges();
        }
    }
}
