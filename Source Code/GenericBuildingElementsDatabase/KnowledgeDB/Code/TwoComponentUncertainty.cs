using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SharedDBLibrary;

namespace KnowledgeDB
{
    public class TwoComponentUncertainty : IThickness, IHasConstructor<int, int, double, double, double, double, double, double>, IKnowledgeCSVExportable
    {
        public KG3xxName KG3xxName { get; set; }
        public int KG3xxNameId { get; set; }
        public LayerTypeName LayerTypeName { get; set; }
        public int LayerTypeNameId { get; set; }
        public double Component2PercentageMin { get; set; }
        public double Component2PercentageAverage { get; set; }
        public double Component2PercentageMax { get; set; }
        public double ThicknessMin { get; set; }
        public double ThicknessAverage { get; set; }
        public double ThicknessMax { get; set; }
        public TwoComponentUncertainty() { } //Default constructor needed for EF to work, not meant to be used!

        //Checks for new entries that are needed for the given KG3xxName LayertypeName combinations. Only use this to create new entries.
        public static void BuildTwoComponentUncertainties(KG3xxName kgName, List<LayerTypeName> ltNames, KnowledgeContext context)
        {
            foreach (var ltName in ltNames)
            {
                    var existingRange = context.TwoComponentUncertainties.Where(uncert => uncert.KG3xxNameId == kgName.Id &&
                                                                                uncert.LayerTypeNameId == ltName.Id).FirstOrDefault();
                    if (existingRange == null)
                    {
                        context.TwoComponentUncertainties.Add(new TwoComponentUncertainty(kgName, ltName));
                        context.SaveChanges();
                    }
 
            }
        }

        //Private constructor because this is not meant to be called from outside. Use BuildTwoComponentUncertainties to create new uncercertainties
        private TwoComponentUncertainty(KG3xxName kgName, LayerTypeName ltName)
        {
            this.KG3xxName = kgName;
            this.LayerTypeName = ltName;
            this.Component2PercentageMin = 0.0;
            this.Component2PercentageAverage = 0.0;
            this.Component2PercentageMax = 0.0;
            this.ThicknessMin = 0.0;
            this.ThicknessAverage = 0.0;
            this.ThicknessMax = 0.0;
        }

        public List<double> getRangeList()
        {
            return new List<double>() { ThicknessMin, ThicknessAverage, ThicknessMax };
        }

        public List<double> getComp2PercentageList()
        {
            return new List<double>() { Component2PercentageMin, Component2PercentageAverage, Component2PercentageMax };
        }

        public double getMin() { return ThicknessMin; }
        public double getAvg() { return ThicknessAverage; }
        public double getMax() { return ThicknessMax; }
        public void setMin(double value) { ThicknessMin = value; }
        public void setAvg(double value) { ThicknessAverage = value; }
        public void setMax(double value) { ThicknessMax = value; }

        public void Constructor(int kg3xxNameId, 
                                int layerTypeNameId,
                                double comp2PercentageMin,
                                double comp2PercentageAvg,
                                double comp2PercentageMax,
                                double thicknessMin,
                                double thicknessAvg,
                                double thicknessMax)
        {
            this.KG3xxNameId = kg3xxNameId;
            this.LayerTypeNameId = layerTypeNameId;
            this.Component2PercentageMin = comp2PercentageMin;
            this.Component2PercentageAverage = comp2PercentageAvg;
            this.Component2PercentageMax = comp2PercentageMax;
            this.ThicknessMin = thicknessMin;
            this.ThicknessAverage = thicknessAvg;
            this.ThicknessMax = thicknessMax;
        }

        //CSV Export
        public static string getCSVHeadlineStatic()
        {
            return "KG3xxName;LayerTypeName;ThicknessMin;ThicknessAverage;ThicknessMax;Comp2PercentageMin;Comp2PercentageAverage;Comp2PercentageMax";
        }

        public string getCSVHeadline()
        {
            return "KG3xxName;LayerTypeName;ThicknessMin;ThicknessAverage;ThicknessMax;Comp2PercentageMin;Comp2PercentageAverage;Comp2PercentageMax";
        }

        public string getCSVLine(KnowledgeContext knowledgeContext)
        {
            return knowledgeContext.KG3xxNames.Where(kg3xxN => kg3xxN.Id == KG3xxNameId).Select(kg3xxN => kg3xxN.Name).FirstOrDefault() + ";" +
                   knowledgeContext.LayerTypeNames.Where(ltn => ltn.Id == LayerTypeNameId).Select(ltn => ltn.Name).FirstOrDefault() + ";" +                
                   ThicknessMin.ToString() + ";" + ThicknessAverage.ToString() + ";" + ThicknessMax.ToString() + ";" + 
                   Component2PercentageMin.ToString() + ";" + Component2PercentageAverage.ToString() + ";" + Component2PercentageMax.ToString();

        }
    }
}
