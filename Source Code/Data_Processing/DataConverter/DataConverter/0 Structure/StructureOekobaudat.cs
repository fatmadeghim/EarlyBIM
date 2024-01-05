using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataConverter
{
    public class StructureOekobaudat
    {
        public List<string> GeneralInformation { get; set; }
        public List<string> Indicators { get; set; }
        public int ModulePos { get; set; }
        public int UUIDPos { get; set; }
        public int ReferenceUnitPos { get; set; }
        public int ReferenceValuePos { get; set; }
        public int ReferenceFluxNamePos { get; set; }
        public int CategoryPos { get; set; }
        public int NamePos { get; set; }
        public int DensityPos { get; set; }
        public int AreaWeightPos { get; set; }
        public int ThicknessPos { get; set; }
        public int BulkDensityPos { get; set; }
        public int ConversionToKgPos { get; set; }
        public int LengthMassPos { get; set; }

        //Constructor from List<string>
        public StructureOekobaudat(List<string> row)
        {
            ModulePos = row.IndexOf("Modul");
            //Assign names of columns to GeneralInformation until column "Modul"
            GeneralInformation = row.GetRange(0, ModulePos + 1);
            //Assign names of columns from indicators to Indicators
            Indicators = row.GetRange(ModulePos + 1, row.Count - ModulePos - 1);
            
            UUIDPos = this.FindIndex("UUID");
            ReferenceUnitPos = this.FindIndex("Bezugseinheit");
            ReferenceValuePos = this.FindIndex("Bezugsgroesse");
            ReferenceFluxNamePos = this.FindIndex("Referenzfluss-Name");
            CategoryPos = this.FindIndex("Kategorie");
            NamePos = this.FindIndex("Name");
            DensityPos = this.FindIndex("Rohdichte (kg/m3)");
            AreaWeightPos = this.FindIndex("Flaechengewicht (kg/m2)");
            ThicknessPos = this.FindIndex("Schichtdicke (m)");
            BulkDensityPos = this.FindIndex("Schuettdichte (kg/m3)");
            ConversionToKgPos = this.FindIndex("Umrechungsfaktor auf 1kg");
            LengthMassPos = this.FindIndex("Laengengewicht (kg/m)");
        }

        /***
            Returns the position of the input string in the generalInformation
            :param name: string
            :return: int with index of input string
        ***/
        public int FindIndex(string name)
        {
            //find position of "Kategorie" in Oekobaudat
            return GeneralInformation.IndexOf(name);
        }

        /***
            Returns the entity of StructureOekobaudat in form of a string that can be passed to a CSV file
            :return: string
        ***/
        public string ToCSVString()
        {
            return (string.Join(Constants.strSeparator, GeneralInformation) + ";" + string.Join(Constants.strSeparator, Indicators));
        }

        /***
            Returns the entity of StructureOekobaudat with a certain number of repetitions of the inidcators
            in form of a string that can be passed to a CSV file
            :param noRepetitionIndicators: int with number of repetition indicators
            :return: string
        ***/
        public string ToCSVString(int noRepetitionIndicators)
        {
            string header = string.Join(Constants.strSeparator, GeneralInformation);
            for (int i = 0; i < noRepetitionIndicators; i++)
            {
                //add "Modul" if not the first indicator entry in row
                if (i != 0)
                {
                    header += ";" + GeneralInformation.Last();
                }
                header += ";" + string.Join(Constants.strSeparator, Indicators);
            }
            return header;
        }
    }//StructureOekobaudat
}//DataConverter
