using System;
using System.Collections.Generic;
using System.Text;

using SharedDBLibrary;

namespace KnowledgeDB
{ 
    public class OekobaudatData : IHasID
    {
        public int Id { get; set; }
        public string UUID { get; set; }
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
        public double GWPA1_A3 { get; set; }
        public double GWPC3 { get; set; }
        public double GWPC4 { get; set; }
        public double GWPD { get; set; }

        public OekobaudatData() { }

        public OekobaudatData(string UUID, Unit unit, double GWPA1_A3, double GWPC3, double GWPC4, double GWPD)
        {
            this.UUID = UUID;
            Unit = unit;
            this.GWPA1_A3 = GWPA1_A3;
            this.GWPC3 = GWPC3;
            this.GWPC4 = GWPC4;
            this.GWPD = GWPD;
        }

        public double GetGWPSum(bool includeD=false)
        {
            if (includeD)
            {
                return GWPA1_A3 + GWPC3 + GWPC4 + GWPD;
            }
            return GWPA1_A3 + GWPC3 + GWPC4;
        }
    }
}
