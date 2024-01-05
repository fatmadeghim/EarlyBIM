using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace KnowledgeDB
{
    public class DefaultThicknessRange : IThickness, IKnowledgeCSVExportable
    {
        public KG3xxName KG3xxName { get; set; }
        public int KG3xxNameId { get; set; }
        public LayerTypeName LayerTypeName { get; set; }
        public int LayerTypeNameId { get; set; }
        public double ThicknessMin { get; set; }
        public double ThicknessAverage { get; set; }
        public double ThicknessMax { get; set; }
        public DefaultThicknessRange() { } //Empty Constructor for DB creation

        //Check which new entries are needed and build them. Only use this to create new entries
        public static void BuildDefaultThicknessRanges(KG3xxName kg3xxName, List<LayerTypeName> layerTypeNames, 
                                                                              KnowledgeContext context)
        {
            foreach(var ltName in layerTypeNames)
            {
                var existingRange = context.DefaultThicknessRanges.Where(dtf => dtf.KG3xxNameId == kg3xxName.Id &&
                                                                         dtf.LayerTypeNameId == ltName.Id).FirstOrDefault();
                if (existingRange == null)
                {
                    context.DefaultThicknessRanges.Add(new DefaultThicknessRange(kg3xxName, ltName));
                    context.SaveChanges(); //Needs to be saved in order to be found in the next layer's search for existingRange.
                }
            }
        }

        private DefaultThicknessRange(KG3xxName kg3xxName, LayerTypeName ltName)
        {
            this.KG3xxName = kg3xxName;
            this.LayerTypeName = ltName;
            ThicknessMin = 0;
            ThicknessAverage = 0;
            ThicknessMax = 0;
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
            return "KG3xxName;LayerTypeName;ThicknessMin;ThicknessAverage;ThicknessMax";
        }

        public string getCSVHeadline()
        {
            return "KG3xxName;LayerTypeName;ThicknessMin;ThicknessAverage;ThicknessMax";
        }

        public string getCSVLine(KnowledgeContext knowledgeContext)
        {
            return knowledgeContext.KG3xxNames.Where(kg3xxN => kg3xxN.Id == KG3xxNameId).Select(kg3xxN => kg3xxN.Name).FirstOrDefault() + ";" +
                   knowledgeContext.LayerTypeNames.Where(ltn => ltn.Id == LayerTypeNameId).Select(ltn => ltn.Name).FirstOrDefault() + ";" +
                   ThicknessMin.ToString() + ";" + ThicknessAverage.ToString() + ";" + ThicknessMax.ToString();
                   
        }
    }
}
