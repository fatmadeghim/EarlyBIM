using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeDB
{
    public interface IThickness : IRange
    {
        public double ThicknessMin { get; set; }
        public double ThicknessAverage { get; set; }
        public double ThicknessMax { get; set; }
    }
}
