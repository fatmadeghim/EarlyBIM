using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConverter
{
    public class OekobaudatEntry
    {
        public List<string> GeneralInformation { get; set; }
        public List<double> IndicatorsA1_A3 { get; set; }
        public List<double> IndicatorsC3 { get; set; }
        public List<double> IndicatorsC4 { get; set; }
        public List<double> IndicatorsD { get; set; }
        public List<KeyValuePair<string, List<double>>> OtherIndicators { get; set; }
        public List<string> LayerTypes { get; set; }
        public List<string> KGs { get; set; }
        public int ServiceLife { get; set; }
        public double ThermalConductivity { get; set; }
        public double ReverseOfR { get; set; } // = 1/R = lambda/d
        public string ChangesToEntry { get; set; }
        public string EstimatedThickness { get; set; }

        //empty constructor
        public OekobaudatEntry() { }
        //constructor from list of SingleModEntries
        public OekobaudatEntry(List<SingleModEntry> entries, int modPos, int unitPos, int valPos)
        {
            //check for general mistakes
            if (entries == null || entries.Count == 0 || modPos <= 0 || unitPos <= 0 || valPos <= 0)
            {
                throw new ArgumentNullException("Input not valid - can't create entry");
            }

            //check if all entries in the same Unit
            var unit = entries[0].GeneralInformation[unitPos];
            for (int i = 0; i < entries.Count; i++)
            {
                var current = entries[i];

                if (current.GeneralInformation[valPos] != "1")
                {
                    throw new InvalidOperationException("Entries were in wrong unit and/or value");
                }

                if (current.GeneralInformation[unitPos] != unit)
                {
                    throw new InvalidOperationException("Entries were in wrong unit and/or value");
                }
            }

            //assign general values
            GeneralInformation = entries[0].GeneralInformation.GetRange(0, entries[0].GeneralInformation.Count - 1);
            OtherIndicators = new List<KeyValuePair<string, List<double>>>();
            ServiceLife = 0;
            ThermalConductivity = 0.0;
            ChangesToEntry = entries[0].ChangesToEntry;
            EstimatedThickness = entries[0].EstimatedThickness;

            //adding entries to correct module if possible or else to OtherIndicators
            foreach(var entry in entries)
            {
                switch (entry.GeneralInformation[modPos])
                {
                    case "A1-A3":
                        IndicatorsA1_A3 = entry.Indicators;
                        break;
                    case "C3":
                        IndicatorsC3 = entry.Indicators;
                        break;
                    case "C4":
                        IndicatorsC4 = entry.Indicators;
                        break;
                    case "D":
                        IndicatorsD = entry.Indicators;
                        break;
                    default:
                        OtherIndicators.Add(new KeyValuePair<string, List<double>>(entry.GeneralInformation[modPos], entry.Indicators) { });
                        break;
                }
            }

            //all valid entries must have A1-A3
            if(IndicatorsA1_A3 == null)
            {
                //check if A1 + A2 + A3 exist
                List<double> indicatorsA1 = OtherIndicators.Find(n => n.Key == "A1").Value;
                List<double> indicatorsA2 = OtherIndicators.Find(n => n.Key == "A2").Value;
                List<double> indicatorsA3 = OtherIndicators.Find(n => n.Key == "A3").Value;
                
                if (indicatorsA1 != null && indicatorsA2 != null && indicatorsA3 != null)
                {
                    IndicatorsA1_A3 = new List<double>();
                    for (int i = 0; i < indicatorsA1.Count; i++)
                    {
                        IndicatorsA1_A3.Add(indicatorsA1[i] + indicatorsA2[i] + indicatorsA3[i]);
                    }
                    ChangesToEntry += "Manually sumed up indicators A1 to A3, ";
                }
            }
        }//Constructor

        //constructor with every input seperate
        public OekobaudatEntry(List<string> generalInformation, List<double> indicatorsA1_A3, 
            List<double> indicatorsC3, List<double> indicatorsC4, List<double> indicatorsD,
            List<KeyValuePair<string, List<double>>> otherIndicators, List<string> layerTypes, List<string> kgs,
            int serviceLife, double thermalConductivity, string changesToEntry)
        {
            GeneralInformation = generalInformation;
            IndicatorsA1_A3 = indicatorsA1_A3;
            IndicatorsC3 = indicatorsC3;
            IndicatorsC4 = indicatorsC4;
            IndicatorsD = indicatorsD;
            OtherIndicators = otherIndicators;
            LayerTypes = layerTypes;
            KGs = kgs;
            ServiceLife = serviceLife;
            ThermalConductivity = thermalConductivity;
            ChangesToEntry = changesToEntry;
        }


        /***
            Function to export one OekobaudatEntry with predefined information to a csv file
            :return: string with all predefined information of one OekobaudatEntry
        ***/

        public string ToCSVString()
        {
            var CSVString = string.Join(Constants.strSeparator, GeneralInformation) + ";";

            if(IndicatorsA1_A3!= null) {CSVString += "true;";}
            else { CSVString += "false;"; }

            if (IndicatorsC3 != null) { CSVString += "true;"; }
            else { CSVString += "false;"; }

            if (IndicatorsC4 != null) { CSVString += "true;"; }
            else { CSVString += "false;"; }

            if (IndicatorsD != null) { CSVString += "true"; }
            else { CSVString += "false"; }

            return CSVString;
        }
        /***
            Function that puts all layertypes and KGs for the OekobaudatEntry into a string for csv export
            :return: string with all predefined information of one OekobaudatEntry
        ***/
        public string GetLayerTypesAndKGsForCsv()
        {
            string CSVString = "";

            //add layertypes
            for (int i = 0; i < LayerTypes.Count-1; i++)
            {
                CSVString += LayerTypes[i] + ", ";
            }
            if(LayerTypes.Count >= 1)
            {
                CSVString += LayerTypes[^1] + ";";
            }
            else
            {
                CSVString += "No Layer Types;";
            }

            //add kgs
            for (int i = 0; i < KGs.Count - 1; i++)
            {
                CSVString += KGs[i] + ", ";
            }
            if (KGs.Count >= 1)
            {
                CSVString += KGs[^1] + ";";
            }
            else
            {
                CSVString += "No KGs";
            }
            return CSVString;
        }
    }
}
