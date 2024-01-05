using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeDB
{
    public interface IRange
    {
        public KG3xxName KG3xxName { get; set; }
        public int KG3xxNameId { get; set; }
        public LayerTypeName LayerTypeName { get; set; }
        public int LayerTypeNameId { get; set; }

        public double getMin();
        public double getAvg();
        public double getMax();
        public void setMin(double value);
        public void setAvg(double value);
        public void setMax(double value);
        public List<double> getRangeList();
    }
}
