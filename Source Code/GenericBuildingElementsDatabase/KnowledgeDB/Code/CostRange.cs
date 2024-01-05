using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;

namespace KnowledgeDB
{
    public class CostRange : IRange, IKnowledgeCSVExportable
    {
        public KG3xxName KG3xxName { get; set; }
        public int KG3xxNameId { get; set; }
        public LayerTypeName LayerTypeName { get; set; }
        public int LayerTypeNameId { get; set; }
        public bool HasExposureQuality { get; set; }
        public Layer Layer { get; set; }
        public int LayerId { get; set; }
        public double CostMin { get; set; }
        public double CostAvg { get; set; }
        public double CostMax { get; set; }

        public double getMin() { return CostMin; }
        public double getAvg() { return CostAvg; }
        public double getMax() { return CostMax; }
        public void setMin(double value) { CostMin = value; }
        public void setAvg(double value) { CostAvg = value; }
        public void setMax(double value) { CostMax = value; }

        public CostRange() { } //Empty Constructor for db creation
        public List<double> getRangeList()
        {
            return new List<double> { CostMin, CostAvg, CostMax };
        }

        public static void BuildCostRanges(KG3xxName kgName, List<LayerTypeName> ltNames, List<bool> hasExposureQualities, KnowledgeContext context)
        {
            foreach (var ltName in ltNames)
            {
                List<Layer> layers;
                if (ltName.Is2Component)
                {
                    var tclt = context.TwoComponentLayerTypes.Where(tclt => tclt.NameId == ltName.Id).FirstOrDefault();
                    layers = (from layer in context.Layers
                              join lay_lt in context.Layer_StandardLayerTypes on layer.Id equals lay_lt.Id1
                              join kg_lay in context.KG3xxName_Layers on lay_lt.Id1 equals kg_lay.Id2
                              where kgName.Id == kg_lay.Id1 &&
                                    (lay_lt.Id2 == tclt.Component1Id || lay_lt.Id2 == tclt.Component2Id)
                              select layer).ToList();
                }
                else
                {
                    layers = (from layer in context.Layers
                              join lay_lt in context.Layer_StandardLayerTypes on layer.Id equals lay_lt.Id1
                              join st_lay in context.StandardLayerTypes on lay_lt.Id2 equals st_lay.Id
                              join kg_lay in context.KG3xxName_Layers on lay_lt.Id1 equals kg_lay.Id2
                              where kgName.Id == kg_lay.Id1 && ltName.Id == st_lay.NameId
                              select layer).ToList();
                }

                foreach (var layer in layers)
                {
                    var existingRange = context.CostRanges.Where(tRange => tRange.KG3xxNameId == kgName.Id && tRange.LayerId == layer.Id &&
                                                                              tRange.LayerTypeNameId == ltName.Id && 
                                                                              tRange.HasExposureQuality == hasExposureQualities[ltNames.IndexOf(ltName)])
                                                                              .FirstOrDefault();
                    if (existingRange == null)
                    {
                        context.CostRanges.Add(new CostRange(kgName, ltName, layer, hasExposureQualities[ltNames.IndexOf(ltName)]));
                        context.SaveChanges(); //Needs to be saved in order to be found in the next layer's search for existingRange.
                    }
                }
            }
        }

        private CostRange(KG3xxName kgName, LayerTypeName ltName, Layer layer, bool hasExposureQuality)
        {
            KG3xxName = kgName;
            LayerTypeName = ltName;
            Layer = layer;
            HasExposureQuality = hasExposureQuality;
            CostMin = 0;
            CostAvg = 0;
            CostMax = 0;
        }

        public string getCSVHeadline()
        {
            return "KG3xxName; LayerTypeName; LayerName; LayerUUID; HasExposureQuality; CostMin; CostAverage; CostMax; " +
                "ThicknessMin; Thickness Average; Thickness Max"; //Thickness is only exported for utility reasons
        }

        public string getCSVLine(KnowledgeContext knowledgeContext)
        {
            IThickness thicknessRange;
            //Check if layertype is 2component
            if (knowledgeContext.LayerTypeNames.Where(ltn => ltn.Id == this.LayerTypeNameId).FirstOrDefault().Is2Component)
            {
                thicknessRange = knowledgeContext.TwoComponentUncertainties.Where(tr =>
                        tr.KG3xxNameId == this.KG3xxNameId && tr.LayerTypeNameId == this.LayerTypeNameId).FirstOrDefault();
            }
            else
            {
                thicknessRange = knowledgeContext.ThicknessRanges.Where(tr =>
                          tr.KG3xxNameId == this.KG3xxNameId && tr.LayerTypeNameId == this.LayerTypeNameId && tr.LayerId == this.LayerId).FirstOrDefault();
            }

            if (thicknessRange == null) 
            {
                return knowledgeContext.KG3xxNames.Where(kg3xxN => kg3xxN.Id == KG3xxNameId).Select(kg3xxN => kg3xxN.Name).FirstOrDefault() + ";" +
                       knowledgeContext.LayerTypeNames.Where(ltn => ltn.Id == LayerTypeNameId).Select(ltn => ltn.Name).FirstOrDefault() + ";" +
                       knowledgeContext.Layers.Where(l => l.Id == LayerId).Select(l => l.Name).FirstOrDefault() + ";" +
                       knowledgeContext.Layers.Where(l => l.Id == LayerId).Select(l => l.UUID).FirstOrDefault() + ";" +
                       HasExposureQuality.ToString() + ";" +
                       CostMin.ToString() + ";" + CostAvg.ToString() + ";" + CostMax.ToString() + ";" +
                       "0" + ";" + "0" + ";" + "0";
            }
            
            return knowledgeContext.KG3xxNames.Where(kg3xxN => kg3xxN.Id == KG3xxNameId).Select(kg3xxN => kg3xxN.Name).FirstOrDefault() + ";" +
                   knowledgeContext.LayerTypeNames.Where(ltn => ltn.Id == LayerTypeNameId).Select(ltn => ltn.Name).FirstOrDefault() + ";" +
                   knowledgeContext.Layers.Where(l => l.Id == LayerId).Select(l => l.Name).FirstOrDefault() + ";" +
                   knowledgeContext.Layers.Where(l => l.Id == LayerId).Select(l => l.UUID).FirstOrDefault() + ";" +
                   HasExposureQuality.ToString() + ";" +
                   CostMin.ToString() + ";" + CostAvg.ToString() + ";" + CostMax.ToString() + ";" +
                   thicknessRange.ThicknessMin.ToString() + ";" + thicknessRange.ThicknessAverage.ToString() + ";" + 
                   thicknessRange.ThicknessMax.ToString();
        }
    }
}
