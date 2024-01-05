using System;
using System.Collections.Generic;
using System.Text;

namespace DataConverter
{
    public class ThermalConductivityMap
    {
        public string Category { get; set; }
        public bool RelevanceOfThermalConductivity { get; set; }
        public double ThermalConductivity { get; set; }

        public ThermalConductivityMap(string[] readInput)
        {
            //Category
            Category = readInput[0];

            //RelevanceOfThermalConductivity
            if (readInput[1].Contains("x"))
            {
                RelevanceOfThermalConductivity = true;
            }
            else if (readInput[1].Contains("o"))
            {
                RelevanceOfThermalConductivity = false;
            }
            else
            {
                RelevanceOfThermalConductivity = false;
                //throw error as no other option implemented
                throw new NotImplementedException("This option is not implemented");
            }

            //ThermalConductivity
            if (RelevanceOfThermalConductivity)
            {
                ThermalConductivity = ConversionHandler.ConvertStringToDouble(readInput[2]);
            }
            else
            {
                ThermalConductivity = 0.0;
            }
        }
    }
}
