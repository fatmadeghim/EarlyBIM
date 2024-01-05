using System;
using System.Collections.Generic;
using System.Text;
using KnowledgeDB;

namespace DataConverter
{
    public class GenerateUnits
    {
        public static List<Unit> GenUnits()
        {
            var units = new List<Unit>()
            {
                new Unit(1, "m3"),
                new Unit(1, "qm"),

                //not allowed units:
                //new Unit(1, "kg"),
                //new Unit(1, "m"),
                //new Unit(1, "pcs"),
            };

            return units;
        }
    }
}
