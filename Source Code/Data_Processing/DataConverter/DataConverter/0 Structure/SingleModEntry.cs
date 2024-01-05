using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConverter
{
    public class SingleModEntry
    {
        public List<string> GeneralInformation { get; set; }
        public List<double> Indicators { get; set; }
        public string ChangesToEntry { get; set; }
        public string EstimatedThickness { get; set; }
        
        //Constructor
        public SingleModEntry(List<string> row, int modulePosition)
        {
            GeneralInformation = row.GetRange(0, modulePosition + 1);
            List<string> stringIndicators = row.GetRange(modulePosition + 1, row.Count - modulePosition - 1);
            Indicators = new List<double>();
            foreach (var value in stringIndicators)
            {
                if (value == "")
                {
                    Indicators.Add(0.0);
                }
                else
                {
                    Indicators.Add(ConversionHandler.ConvertStringToDouble(value));
                }
            }
            ChangesToEntry = "";
        }

        //Constructor
        public SingleModEntry(List<string> generalInformation, List<double> indicators)
        {
            GeneralInformation = generalInformation;
            Indicators = indicators;
            ChangesToEntry = "";
        }

        //Copy Constructor
        public SingleModEntry(SingleModEntry entry)
        {
            if (entry.GeneralInformation != null)
            {
                this.GeneralInformation = new List<string>(entry.GeneralInformation);
            }
            else
            {
                this.GeneralInformation = new List<string>();
            }
            if (entry.Indicators != null)
            {
                this.Indicators = new List<double>(entry.Indicators);
            }
            else
            {
                this.Indicators = new List<double>();
            }
            this.ChangesToEntry = entry.ChangesToEntry;
        }

        /***
            Returns an entity of SingleModEntry from a line of the oekobaudat csv file
            :param line: string from CSV file (one line)
            :param modulePosition: int that states in which column the module is found
            :return: SingleModEntry
        ***/
        public static SingleModEntry ParseFideCSV(string line, int modulePosition)
        {
            string[] parts = line.Split(';');
            for(int i = 0; i < parts.Count(); i++)
            {
                parts[i] = parts[i].Replace("'","");
            }
            return new SingleModEntry(parts.ToList<string>(), modulePosition);
        }


        /***
            Returns the entity of SingleModEntry in form of a string that can be passed to a CSV file
            :return: string
        ***/
        public string ToCSVString()
        {
            return (string.Join(Constants.strSeparator, GeneralInformation) + ";"
                            + string.Join(Constants.strSeparator, Indicators));
        }

        /***
            Updates indicators of the current entry
            :param referenceUnitPos: int with position in GeneralInformation to reference Unit
            :param referenceValuePos: int with position in GeneralInformation to reference Value
            :param conversionToKgPos: int with position in GeneralInformation to conversion in kg
            :param newReferenceUnit: string with new reference Unit
            :param newReferenceValue: string with new reference Value
            :param newConversionToKg: string with new conversion to kg
            :param multiplicationConversionFactor: double value for conversion to multiply (!)
        ***/
        public void UpdateIndicators(int referenceUnitPos, int referenceValuePos, int conversionToKgPos, string newReferenceUnit, string newReferenceValue, string newConversionToKg, double multiplicationConversionFactor)
        {
            this.GeneralInformation[referenceUnitPos] = newReferenceUnit;
            this.GeneralInformation[referenceValuePos] = newReferenceValue;
            this.GeneralInformation[conversionToKgPos] = newConversionToKg;
            for (int i = 0; i <  this.Indicators.Count(); i++)
            {
                this.Indicators[i] *= multiplicationConversionFactor;
            }
        }

        /***
            Updates indicators of the current entry
            :param referenceUnitPos: int with position in GeneralInformation to reference Unit
            :param newReferenceUnit: string with new reference Unit
            :param conversionFactor: double value for conversion to multiply (!)
        ***/
        public void UpdateIndicators(int referenceUnitPos, string newReferenceUnit, double conversionFactor)
        {
            this.GeneralInformation[referenceUnitPos] = newReferenceUnit;
            for (int i = 0; i < this.Indicators.Count(); i++)
            {
                this.Indicators[i] *= conversionFactor;
            }
        }
    }
}
