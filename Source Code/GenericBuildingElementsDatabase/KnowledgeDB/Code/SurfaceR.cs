using System;
using System.Collections.Generic;
using System.Text;

using SharedDBLibrary;

namespace KnowledgeDB
{
    public class SurfaceR : IHasID, IHasConstructor<double, double, int>
    {
        public int Id { get; set; }
        public double Rsi { get; set; }
        public double Rse { get; set; }

        public void Constructor(double rsi, double rse, int id)
        {
            this.Rsi = rsi;
            this.Rse = rse;
            this.Id = id;
        }
    }
}
