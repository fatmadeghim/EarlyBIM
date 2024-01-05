using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataConverter
{
    class CompletingHandlerSMEntry
    {
        /***
        Adds missing units of the entries with given indices (looks for unit in Referenzfluss-Name)
        :param indicesKG: indices of entries in allEntries
        :param allEntries: list of all singleModEntries
        :param referenceUnitPosition: position of reference unit in general information of SingleModEntry
        :param referenceValuePosition: position of reference value in general information of SingleModEntry
        :param referenceFluxNamePosition: position of reference flux name in general information of SingleModEntry
        :param conversionToKgPosition: position of conversion to kg in general information of SingleModEntry
        :param thicknessPosition: position of thickness in general information of SingleModEntry
         ***/
        public static void AddMissingUnits(List<int> indicesKG, List<SingleModEntry> allEntries, int referenceUnitPosition, int referenceValuePosition, int referenceFluxNamePosition, int conversionToKgPosition, int thicknessPosition)
        {
            //for manual checking: generate and export List with old value and new value after correction
            string header = "Name; UUID; Old Bezugsgröße; Old Bezugseinheit; Old GWP;Corrected Bezugsgröße; Corrected Bezugseinheit;Corrected GWP; Referenzfluss -Name";
            var corrections = new List<string>();

            //go through all entries in SingleModEntries with the given indices and check if unit is null
            for (int i = 0; i < indicesKG.Count; i++)
            {
                var currentEntry = allEntries[indicesKG[i]];

                //for entries with unit = null, search for unit in "Referenzfluss-Name"
                if (currentEntry.GeneralInformation[referenceUnitPosition] == "null")
                {
                    string referenceName = currentEntry.GeneralInformation[referenceFluxNamePosition];

                    //for manual checking old values
                    var oldvalues = string.Join(";", currentEntry.GeneralInformation[referenceValuePosition], currentEntry.GeneralInformation[referenceUnitPosition], currentEntry.Indicators[0]);

                    //go through different possibilities for correction
                    if (referenceName.ToLower().Contains("1 stück"))
                    {
                        currentEntry.GeneralInformation[referenceUnitPosition] = "pcs.";
                    }
                    else if (referenceName.Contains(" 1m²") || referenceName.Contains("1 m²") || referenceName.Contains("1m2") || referenceName.Contains("1 m2"))
                    {
                        currentEntry.GeneralInformation[referenceUnitPosition] = "qm";
                    }
                    else if ((referenceName.Contains("1kg") || referenceName.Contains("1 kg")) && (!referenceName.Contains("kg/m") || !referenceName.Contains("kg)")))
                    {
                        currentEntry.GeneralInformation[referenceUnitPosition] = "kg";
                    }
                    else if (referenceName.Contains(" 1t") || referenceName.Contains(" 1 t") || referenceName.Contains("1 Tonne"))
                    {
                        currentEntry.UpdateIndicators(referenceUnitPosition, referenceValuePosition, conversionToKgPosition, "kg", "1", "1", 0.001);
                    }
                    else if (referenceName.Contains("1 m³") || referenceName.Contains("1m³") || referenceName.Contains("1m3") || referenceName.Contains("1 m3"))
                    {
                        currentEntry.GeneralInformation[referenceUnitPosition] = "m3";
                    }
                    else if (referenceName.Contains("lfm") || referenceName.Contains("1 m ") || referenceName.Contains("1m "))
                    {
                        currentEntry.GeneralInformation[referenceUnitPosition] = "m";
                    }

                    //needs extra check!
                    if (referenceName.Contains(" cm ") || referenceName.Contains(" mm "))
                    {
                        //currentEntry.GeneralInformation[thicknessPosition] = "I could find something here";
                    }

                    corrections.Add(string.Join(";", currentEntry.GeneralInformation[2], currentEntry.GeneralInformation[0]) + ";" + oldvalues + ";" + string.Join(";", currentEntry.GeneralInformation[referenceValuePosition], currentEntry.GeneralInformation[referenceUnitPosition], currentEntry.Indicators[0], referenceName));
                    currentEntry.ChangesToEntry += "Added missing unit (" + currentEntry.GeneralInformation[referenceValuePosition] + currentEntry.GeneralInformation[referenceUnitPosition] + ") from reference flux name, ";
                }
            }
            CsvExportHandler.Export(CsvExportHandler.SetStorageLocation("UnitCorrection.csv"), header, corrections);
        }//AddMissingUnits

        /***
        Adds more information to the referencefluxName from xml
        :param positions: List of int with all positions in entries to consider
        :param entries: List of all SingleModEntries
        :param uuidPosition: position of UUID in generalInformation of SingleModEntry
        :param referenceFluxNamePosition: position of referenceFluxName in generalInformation of SingleModEntry
        :param pathXmlFolder: path to folder where all xml files are saved
         ***/
        public static void AddInformationFromXML(
            List<int> positions,
            List<SingleModEntry> entries,
            int uuidPosition,
            int referenceFluxNamePosition,
            int unitPos,
            string pathXmlFolder)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                var entry = entries[positions[i]];
                //restriction to reduce runtime (could be omitted)
                if(entry.GeneralInformation[unitPos] != "m3")
                {
                    //add additional info to referenceFluxName from xml
                    XmlHandler.AddTechnologyDescription(entry, uuidPosition, referenceFluxNamePosition, pathXmlFolder);

                }
            }
        }

        /***
        Adds missing conversions of the entries with given indices (looks for conversions in Referenzfluss-Name)
        REMARK: at the moment (19.06.21) this function doesn't help (can't find additional information)
        :param indicesKG: indices of entries in allEntries
        :param allEntries: list of all singleModEntries
        :param referenceUnitPosition: position of reference unit in general information of SingleModEntry
        :param referenceFluxNamePosition: position of reference flux name in general information of SingleModEntry
        :param densityPosition: position of density in general information of SingleModEntry
        :param areaWeightPosition: position of area weight in general information of SingleModEntry
        :param thicknessPosition: position of thickness in general information of SingleModEntry
         ***/
        public static void AddMissingConversions(
            List<int> indicesKG, 
            List<SingleModEntry> allEntries, 
            int referenceUnitPosition, 
            int referenceFluxNamePosition, 
            int densityPosition, 
            int areaWeightPosition, 
            int thicknessPosition)
        {
            //for manual checking: generate and export List updated conversions
            string header = "Name; UUID; Referenzfluss-Name; Flaechengewicht (kg/m2); Rohdichte (kg/m3); Schichtdicke (m)";
            var body = new List<string>();
            //go through all entries in SingleModEntries with the given indices
            for (int i = 0; i < indicesKG.Count; i++)
            {
                var currentEntry = allEntries[indicesKG[i]];
                bool change = false;
                //for entries with unit != m3, search for conversion factors in "Referenzfluss-Name"
                if (currentEntry.GeneralInformation[referenceUnitPosition] != "m3")
                {
                    string referenceName = currentEntry.GeneralInformation[referenceFluxNamePosition];
                    //check for density if empty
                    if (currentEntry.GeneralInformation[densityPosition] == "")
                    {
                        string densityUnitUpper = "kg/m³";
                        string densityUnitLower = "kg/m3";
                        int position = 0;
                        if (referenceName.ToLower().Contains(densityUnitLower))
                        {
                            position = referenceName.IndexOf(densityUnitLower);
                        }

                        else if (referenceName.ToLower().Contains(densityUnitUpper))
                        {
                            position = referenceName.IndexOf(densityUnitUpper);
                        }

                        if (position > 0)
                        {
                            string density = ExtractInt(position, referenceName, SubstringBeforePosition);
                            currentEntry.GeneralInformation[densityPosition] = density;
                            change = true;
                        }
                    }
                    //check for area weight if empty
                    if (currentEntry.GeneralInformation[areaWeightPosition] == "")
                    {
                        string areaWeightUnitUpper = "kg/m²";
                        string areaWeightUnitLower = "kg/m2";
                        string areaWeightGramUnitUpper = "g/m²"; //checked AFTER kg to make sure it is not misleadingly taken
                        string areaWeightGramUnitLower = "g/m2";

                        int position = 0;
                        if (referenceName.ToLower().Contains(areaWeightUnitLower))
                        {
                            position = referenceName.IndexOf(areaWeightUnitLower);
                        }

                        else if (referenceName.ToLower().Contains(areaWeightUnitUpper))
                        {
                            position = referenceName.IndexOf(areaWeightGramUnitUpper);
                        }

                        else if (referenceName.ToLower().Contains(areaWeightGramUnitLower))
                        {
                            position = referenceName.IndexOf(areaWeightGramUnitLower);
                            double areaWeight = ConversionHandler.ConvertStringToDouble(ExtractInt(position, referenceName, SubstringBeforePosition)) / 1000;
                            currentEntry.GeneralInformation[areaWeightPosition] = Convert.ToString(areaWeight);
                            change = true;
                            position = 0; //to make sure not twice
                        }

                        else if (referenceName.ToLower().Contains(areaWeightGramUnitUpper))
                        {
                            position = referenceName.IndexOf(areaWeightGramUnitUpper);
                            double areaWeight = ConversionHandler.ConvertStringToDouble(ExtractInt(position, referenceName, SubstringBeforePosition)) / 1000;
                            currentEntry.GeneralInformation[areaWeightPosition] = Convert.ToString(areaWeight);
                            change = true;
                            position = 0; //to make sure not twice
                        }

                        if (position > 0)
                        {
                            string areaWeight = ExtractInt(position, referenceName, SubstringBeforePosition);
                            currentEntry.GeneralInformation[areaWeightPosition] = areaWeight;
                            change = true;
                        }
                    }
                    
                    if (change)
                    {
                        body.Add(string.Join(";", currentEntry.GeneralInformation[2], currentEntry.GeneralInformation[0], referenceName, currentEntry.GeneralInformation[areaWeightPosition], currentEntry.GeneralInformation[densityPosition], currentEntry.GeneralInformation[thicknessPosition]));
                        currentEntry.ChangesToEntry += "Added conversions from ReferenceFluxName, ";
                    }
                }
            }
            CsvExportHandler.Export(CsvExportHandler.SetStorageLocation("AddedConversions.csv"), header, body);
        }//AddMissingConversions

        /***
         * Helper function for AddMissingConversions
         * Separates the integer from a given string just before the given position
         ***/
        public static string ExtractInt(int position, string complete, Func<string, int, string> subStringMethod)
        {
            var subString = subStringMethod(complete, position);
            string numeric = "";
            if (subString.LastIndexOf(',') == -1) //if no comma in the part
            {
                numeric = new String(subString.Where(Char.IsDigit).ToArray());
            }
            else
            {
                int positionComma = subString.LastIndexOf(',');

                string part1 = subString.Substring(0, positionComma);
                string part2 = subString.Substring(positionComma);

                numeric = new String(part1.Where(Char.IsDigit).ToArray());
                numeric += ',';
                numeric += new String(part2.Where(Char.IsDigit).ToArray());
            }

            //check if numeric present in complete string; if not, try to reduce the first numbers 
            // Note: this could theoretically produce wrong results, but it's not likely this will happen with the given texts
            int numPosition = complete.IndexOf(numeric);
            if (numPosition == -1)
            {
                while (numeric.Length > 1)
                {
                    numeric = numeric.Remove(0, 1);
                    if (complete.IndexOf(numeric) != -1) { break; }
                }
                return "";
            }

            //check if no < or > directly before the value (only inf numPos>0) 
            //--> if yes, no valid information and return empty string
            if (numPosition > 0)
            {
                string beforeString = complete.Substring(numPosition - 1, 1);
                if (beforeString.Equals("<") || beforeString.Equals(">"))
                {
                    return "";
                }
            }
            
            return numeric;
        }

        /***
        * Helper functions for Definition of how to deal with subString
        ***/
        public static string SubstringBeforePosition(string complete, int position)
        {
            int length = 8;
            int beginning = position - length;
            if (beginning < 0)
            {
                beginning = 0;
            }
            return complete.Substring(beginning, length);

        }
        public static string StringWithoutLastChar(string complete, int position)
        {
            //return from 0 to length-1
            return complete[0..^1];
        }

        /***
        Adds missing conversion factors (density and area weight) from eLCA file
        :param path: string with path to eLCA file
        :param indicesKG: List of int with all indices for relevant singleModEntries
        :param allEntries: List of all singleModEntries
        :param referenceUnitPosition: int with position of unit in GeneralInformation of SingleModEntry
        :param UUIDPosition: int with position of UUID in GeneralInformation of SingleModEntry
        :param densityPosition: int with position of density in GeneralInformation of SingleModEntry
        :param areaWeightPosition: int with position of area weight in GeneralInformation of SingleModEntry
        ***/

        public static void AddMissingConversionsFrom_eLCA(
            string path,
            List<int> indicesKG,
            List<SingleModEntry> allEntries,
            int referenceUnitPosition,
            int UUIDPosition,
            int densityPosition,
            int areaWeightPosition)
        {
            //setup of variables from csv file at given path
            List<string> Header = new List<string>();
            List<List<string>> Entries = new List<List<string>>();
            CompletingHandlerOekobaudatEntry.SetupCompleter(ref Header, ref Entries, path);
            int completerDensityPos = Header.IndexOf("Rohdichte");
            int UUIDA13Pos = Header.IndexOf("UUID [A1-3]");

            //go through all entries in SingleModEntries with the given indices (all entries with not m3 as info might be needed for 1/R)
            for (int i = 0; i < indicesKG.Count; i++)
            {
                var currentEntry = allEntries[indicesKG[i]];
                if(currentEntry.GeneralInformation[referenceUnitPosition] != "m3")
                {
                    //check if completeEntry in csv file present
                    var currentUUID = currentEntry.GeneralInformation[UUIDPosition];
                    List<string> completeEntry = Entries.Find(n => n[UUIDA13Pos].Equals(currentUUID));
                    if (completeEntry != null)
                    {
                        //if information on density present, it is either in density position or in last or second to last (if not empty string)
                        if (currentEntry.GeneralInformation[densityPosition] == "")
                        {
                            if (completeEntry[completerDensityPos] != string.Empty)
                            {
                                currentEntry.GeneralInformation[densityPosition] = completeEntry[completerDensityPos];
                            }
                            else if (completeEntry.Last() != String.Empty)
                            {
                                string densityUnit = "kg/m3";
                                //last
                                if (completeEntry.Last().Contains(densityUnit))
                                {
                                    currentEntry.GeneralInformation[densityPosition] = ExtractInt(0, completeEntry.Last(), StringWithoutLastChar);
                                }
                                //second to last
                                else if (completeEntry[^2].Contains(densityUnit))
                                {
                                    currentEntry.GeneralInformation[densityPosition] = ExtractInt(0, completeEntry[^2], StringWithoutLastChar);
                                }
                            }
                        }

                        //if information on area weight present, it is either in last or second to last position (if not empty string)
                        if (currentEntry.GeneralInformation[areaWeightPosition] == "")
                        {
                            if (completeEntry.Last() != String.Empty)
                            {
                                string areaWeightUnit = "kg/m2";
                                //last
                                if (completeEntry.Last().Contains(areaWeightUnit))
                                {
                                    currentEntry.GeneralInformation[areaWeightPosition] = ExtractInt(0, completeEntry.Last(), StringWithoutLastChar);
                                }
                                //second to last
                                else if (completeEntry[^2].Contains(areaWeightUnit))
                                {
                                    currentEntry.GeneralInformation[areaWeightPosition] = ExtractInt(0, completeEntry[^2], StringWithoutLastChar);
                                }
                            }
                        }
                        currentEntry.ChangesToEntry += "(Tried to) add general information from eLCA, ";
                    }//if completeEntry
                }//if unit != m3
            }//for
        }//AddMissingConversionsFrom_eLCA
    }
}