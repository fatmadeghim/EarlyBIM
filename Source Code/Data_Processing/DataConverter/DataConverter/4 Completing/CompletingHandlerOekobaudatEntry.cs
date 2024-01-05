using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DataConverter
{
    public class CompletingHandlerOekobaudatEntry
    {
        /***
        Completes OekobaudatEntries with Information from eLCA (Baustoffkonfiguration)
        :param path: path to eLCA file
        :param oeEntries: List of Oekobaudat Entries
        :param UUIDPosition: position of UUID in the general information of oekobaudatEntries
        :param allEntries: List of all singleModEntries
        :param positionsWithEndOfLife: positions of relevant singleModEntries including end  of life in the List of all singleModEntries
        :param generalUnitPos: position of unit in the general information of SingleModEntries and OekobaudatEntries
        :param generalDensityPos: position of density in the general information of SingleModEntries and OekobaudatEntries
        :param generalThicknessPos: position of thickness in the general information of SingleModEntries and OekobaudatEntries
        ***/
        public static void CompleteOekobaudatEntries(
            string path_eLCA, 
            List<OekobaudatEntry> oeEntries, 
            int generalUUIDPos, 
            List<SingleModEntry> allEntries, 
            List<int> positionsWithEndOfLife, 
            int generalUnitPos,
            int generalDensityPos,
            int generalThicknessPos,
            int generalAreaWeightPos)
        {
            //setup of variables from csv file at given path
            List<string> Header = new List<string>();
            List<List<string>> Entries = new List<List<string>>();
            SetupCompleter(ref Header, ref Entries, path_eLCA);
            
            //find positions of UUIDs etc in the header
            int UUIDA13Pos = Header.IndexOf("UUID [A1-3]");
            int UUIDC3Pos = Header.IndexOf("UUID [C3]");
            int UUIDC4Pos = Header.IndexOf("UUID [C4]");
            int UUIDDPos = Header.IndexOf("UUID [D]");
            int referenceValueC3Pos = Header.IndexOf("Bezugsgröße [C3]");
            int referenceValueC4Pos = Header.IndexOf("Bezugsgröße [C4]");
            int referenceValueDPos = Header.IndexOf("Bezugsgröße [D]");
            int densityPos = Header.IndexOf("Rohdichte");
            int serviceLifePos = Header.IndexOf("Min. Nutzungsdauer");

            //extract all SingleModEntries in KG300 including end of life
            var kg300entries = new List<SingleModEntry>();
            foreach(var position in positionsWithEndOfLife)
            {
                kg300entries.Add(allEntries[position]);
            }
            
            //go through all oeEntries and check if completing is possible
            foreach (var entry in oeEntries)
            {
                //check if completeEntry in csv file present
                var currentUUID = entry.GeneralInformation[generalUUIDPos];
                var entryUnit = entry.GeneralInformation[generalUnitPos];
                List<string> completeEntry = Entries.Find(n => n[UUIDA13Pos].Equals(currentUUID));
                //if yes, try to complete
                if (completeEntry != null)
                {
                    //Indicators
                    if (entry.IndicatorsC3 == null && entry.IndicatorsC4 == null)
                    {
                        entry.IndicatorsC3 = ModuleCompleter(entry, kg300entries, generalUUIDPos, UUIDC3Pos, referenceValueC3Pos, completeEntry, generalUnitPos, densityPos, entryUnit, generalDensityPos, generalThicknessPos, generalAreaWeightPos, "C3");
                        entry.IndicatorsC4 = ModuleCompleter(entry, kg300entries, generalUUIDPos, UUIDC4Pos, referenceValueC4Pos, completeEntry, generalUnitPos, densityPos, entryUnit, generalDensityPos, generalThicknessPos, generalAreaWeightPos, "C4");
                    }
                    if (entry.IndicatorsD == null)
                    {
                        entry.IndicatorsD = ModuleCompleter(entry, kg300entries, generalUUIDPos, UUIDDPos, referenceValueDPos, completeEntry, generalUnitPos, densityPos, entryUnit, generalDensityPos, generalThicknessPos, generalAreaWeightPos, "D");
                    }
                    //ServiceLife
                    entry.ServiceLife = CompleteServiceLife(completeEntry, serviceLifePos);
                }
            }
        }//CompleteOekobaudatEntries

        /***
        Helper function for "CompleteOekobaudatEntries" to setup certain values from the given file (all params passed as ref)
        :param Header: reference to List of strings that contains the header of the csv file
        :param Entries: reference to List of List of string with Entries of the csv file
        :param path: string with path to csv file that contains all the information mentioned above
        ***/
        public static void SetupCompleter(
            ref List<string> Header,
            ref List<List<string>> Entries,
            string path)
        {
            //parse the first line of the given csv file to get the header
            var parts = File.ReadAllLines(path, Encoding.UTF8).First<string>().Split(';');
            //delete all \ in the parsed strings
            for (int i = 0; i < parts.Count(); i++)
            {
                parts[i] = parts[i].Replace("\"", "");
            }
            Header = parts.ToList();
            //read all other lines in csv file to get the "content" lines
            var list = File.ReadAllLines(path, Encoding.UTF8).Skip(1);
            Entries = new List<List<string>>();
            foreach (var line in list)
            {
                var pieces = line.Split(';');
                for (int i = 0; i < pieces.Count(); i++)
                {
                    pieces[i] = pieces[i].Replace("\"", "");
                }
                Entries.Add(pieces.ToList());
            }
        }

        /***
        Helper function for "CompleteOekobaudatEntries" to complete one module
        :param entry: current OekobaudatEntry that is to be completed
        :param kg300entries: List of all singleModEntries that are relevant for completing
        :param generalUUIDPos: UUID Position in GeneralInformation of OekobaudatEntry 
        :param specificUUIDPos: UUID Position of the specific module in the csv file
        :param specificReferenceValuePos: reference value position of the specific moduel in the csv file
        :param completeEntry: complete entry of CSV file that corresponds to the examined OekobaudatEntry
        :param generalUnitPos: position of unit in GeneralInformation of OekobaudatEntry
        :param completeEntryDensityPos: position of density in the param "completeEntry"
        :param entryUnit: string with unit of examinded OekobaudatEntry
        :param generalDensityPos: position of density in GeneralInformation of OekobaudatEntry
        :param generalThicknessPos: position of thickness in GeneralInformation of OekobaudatEntry
        :param generalAreaWeighPos: position of area weight in GeneralInformation of OekobaudatEntry
        :param module: string with module for documenting changes in entry
        :return: List of double with the indicators for completing
        ***/
        private static List<double> ModuleCompleter(
            OekobaudatEntry entry,
            List<SingleModEntry> kg300entries, 
            int generalUUIDPos, 
            int specificUUIDPos, 
            int specificReferenceValuePos,
            List<string> completeEntry, 
            int generalUnitPos, 
            int completeEntryDensityPos,
            string entryUnit,
            int generalDensityPos,
            int generalThicknessPos,
            int generalAreaWeighPos,
            string module)
        {
            //check if a UUID is stored for the considered module
            if(completeEntry[specificUUIDPos] == String.Empty)
            {
                //no data set for the considered module
                return null;
            }
            else if(completeEntry[specificUUIDPos] == "Null-Entry")
            {
                //key word for generating indicators with 0 for all indicators
                var nullIndicators = new List<double>();
                for(int i = 0; i<  kg300entries[0].Indicators.Count; i++)
                {
                    entry.ChangesToEntry += "Added module " + module + " from eLCA (Null-Entry), ";
                    nullIndicators.Add(0.0);
                }
                return nullIndicators;
            }
            //try to find the singleModEntry that can complete the given entry
            SingleModEntry completer = kg300entries.Find(n => n.GeneralInformation[generalUUIDPos].Equals(completeEntry[specificUUIDPos]));
            if (completer != null)
            {
                var completerUnit = completer.GeneralInformation[generalUnitPos];
                var referenceValue = ConversionHandler.ConvertStringToDouble(completeEntry[specificReferenceValuePos]);
                //check if unit of completer is the same as for the entry (if not, completer needs to be converted)
                if (completerUnit == entryUnit)
                {
                    entry.ChangesToEntry += "Added module " + module + " from eLCA, ";
                    return completer.Indicators;
                }
                else
                {
                    //copy completer to modify the copy to the correct unit
                    var completerCorrectUnit = new SingleModEntry(completer);
                    //find conversion factor (depends on completer unit and entry unit and on information given)
                    double conversionFactor = 0.0;
                    
                    if(entryUnit == "m3")
                    {
                        if(completerUnit == "kg")
                        {
                            //if density in complete entry given
                            if (completeEntry[completeEntryDensityPos] != String.Empty)
                            {
                                conversionFactor = ConversionHandler.ConvertStringToDouble(completeEntry[completeEntryDensityPos]);
                            }
                            else if (entry.GeneralInformation[generalDensityPos] != String.Empty)
                            {
                                conversionFactor = ConversionHandler.ConvertStringToDouble(entry.GeneralInformation[generalDensityPos]);
                            }
                            else
                            {
                                Console.WriteLine("Entry with UUID " +  entry.GeneralInformation[generalUUIDPos] + " needs a conversion factor from kg to m3");
                                return null;
                            }
                        }
                        else if(completerUnit == "qm")
                        {
                            if(entry.GeneralInformation[generalThicknessPos] != String.Empty)
                            {
                                double thickness = ConversionHandler.ConvertStringToDouble(entry.GeneralInformation[generalThicknessPos]);
                                conversionFactor = (1 / thickness);
                            }
                            else
                            {
                                Console.WriteLine("Entry with UUID " +  entry.GeneralInformation[generalUUIDPos] + " needs a conversion factor from qm to m3");
                                return null;
                            }
                        }
                        else
                        {
                            //throw error here as no other things implemented
                            throw new NotImplementedException("This conversion is not implemented.");
                        }
                    }
                    else if(entryUnit == "qm")
                    {
                        if (completerUnit == "kg")
                        {
                            if (entry.GeneralInformation[generalAreaWeighPos] != String.Empty)
                            {
                                conversionFactor = ConversionHandler.ConvertStringToDouble(entry.GeneralInformation[generalAreaWeighPos]);
                            }
                            else
                            {
                                Console.WriteLine("Entry with UUID " + entry.GeneralInformation[generalUUIDPos] + " needs a conversion factor from kg to qm");
                                return null;
                            }
                        }
                        else if (completerUnit == "m3")
                        {
                            Console.WriteLine("Not implemented m3 to qm");
                            return null;
                        }
                        else
                        {
                            //throw error here as no other things implemented
                            throw new NotImplementedException("This conversion is not implemented.");
                        }
                    }
                    else
                    {
                        //throw error here as no other things implemented
                        throw new NotImplementedException("This conversion is not implemented.");
                    }
                    if (conversionFactor > 0.0)
                    {
                        if(referenceValue != 1.0)
                        {
                            conversionFactor *= referenceValue;
                        }
                        completerCorrectUnit.UpdateIndicators(generalUnitPos, completerCorrectUnit.GeneralInformation[generalUnitPos], conversionFactor);
                        entry.ChangesToEntry += "Added module " + module + " from eLCA, ";
                        return completerCorrectUnit.Indicators;
                    }
                }
            }
            //if no completing entry found, return null (so far never happened)
            return null;
        }

        /***
        Completes the general Information of the input entries if possible (through internal conversions)
        :param oekobaudatEntries: List of all oekobaudat entries
        :param areaWeightPos: area Weight Position in GeneralInformation of OekobaudatEntry
        :param densityPos: area Weight Position in GeneralInformation of OekobaudatEntry
        :param thicknessPos: area Weight Position in GeneralInformation of OekobaudatEntry
        ***/

        public static void CompleteGeneralInformation(
            List<OekobaudatEntry> oekobaudatEntries, 
            int areaWeightPos, 
            int densityPos, 
            int thicknessPos)
        {
            for(int i = 0; i < oekobaudatEntries.Count; i++)
            {
                var current = oekobaudatEntries[i];
                //check which entries are present
                bool areaWeightBool = (current.GeneralInformation[areaWeightPos] != String.Empty);
                bool densityBool = (current.GeneralInformation[densityPos] != String.Empty);
                bool thicknessBool = (current.GeneralInformation[thicknessPos] != String.Empty);
                
                //if no area weight, but all others
                if (!areaWeightBool && densityBool && thicknessBool) 
                {
                    double areaWeight = ConversionHandler.ConvertStringToDouble(current.GeneralInformation[densityPos]) * ConversionHandler.ConvertStringToDouble(current.GeneralInformation[thicknessPos]);
                    current.GeneralInformation[areaWeightPos] = Convert.ToString(areaWeight);
                    current.ChangesToEntry += "Added area weight through relation to density and thickness, ";
                }
                //if no density, but all others
                if (areaWeightBool && !densityBool && thicknessBool) 
                {
                    double density = ConversionHandler.ConvertStringToDouble(current.GeneralInformation[areaWeightPos]) / ConversionHandler.ConvertStringToDouble(current.GeneralInformation[thicknessPos]);
                    current.GeneralInformation[densityPos] = Convert.ToString(density);
                    current.ChangesToEntry += "Added density through relation to thickness and area weight, ";
                }
                //if no thickness, but all others
                if (areaWeightBool && densityBool && !thicknessBool) 
                {
                    double thickness = ConversionHandler.ConvertStringToDouble(current.GeneralInformation[areaWeightPos]) / ConversionHandler.ConvertStringToDouble(current.GeneralInformation[densityPos]);
                    current.GeneralInformation[thicknessPos] = Convert.ToString(thickness);
                    current.ChangesToEntry += "Added thickness through relation to density and area weight, ";
                }
            }
        }

        /***
        Completes the expected service life from eLCA of one entry
        :param completeEntry: one line in the eLCA that corresponds to current entry
        :param serviceLifePos: position of service Life in the entry above
        :return: service life of current entry
        ***/
        private static int CompleteServiceLife(List<string> completeEntry, int serviceLifePos)
        {
            string serviceLife = completeEntry[serviceLifePos];
            if(serviceLife != String.Empty) { return Convert.ToInt32(serviceLife); }
            else { return 0; }
        }

        /***
        Updates service life from an external csv files if service life is 0 or 50 and exports the entries that still need improving
        :param oekobaudatEntries: list of all OekobaudatEntries
        :param structure: Structure of Oekobaudat for exporting
        :param generalUUIDPos: position of UUID in general information of oekobaudat entry
        :param pathManualServiceLife0: path to csv with entries of manual service life 0
        :param pathManualServiceLife50: path to csv with entries of manual service life 50
        ***/
        public static void UpdateServiceLife(List<OekobaudatEntry> oekobaudatEntries, StructureOekobaudat structure, int generalUUIDPos, string pathManualServiceLife0, string pathManualServiceLife50)
        {
            //import from csv file
            var entriesSL0 = CsvImportHandler.ReadFromCSV(pathManualServiceLife0, true, Encoding.UTF8);
            var entriesSL50 = CsvImportHandler.ReadFromCSV(pathManualServiceLife50, true, Encoding.UTF8);

            //static positions of UUID and service life in the imported entriesSL0 and entriesSL50
            var UUIDpositionImport = 0;
            var serviceLifePositionImport = 20;

            //for export
            var oekobaudatEntriesSL0 = new List<OekobaudatEntry>();
            var oekobaudatEntriesSL50 = new List<OekobaudatEntry>();

            //iterate over all entries
            for (int i = 0; i < oekobaudatEntries.Count; i++)
            {
                var current = oekobaudatEntries[i];
                //if service life 0 complete with entriesSL0
                if(current.ServiceLife == 0)
                {
                    var completingEntry = entriesSL0.Find(n => n[UUIDpositionImport] == current.GeneralInformation[generalUUIDPos]);
                    if (completingEntry != null)
                    {
                        current.ServiceLife = Convert.ToInt32(completingEntry[serviceLifePositionImport]);
                        current.ChangesToEntry += "Manually updated service life from 0 to " + completingEntry[serviceLifePositionImport] + ", ";
                    }
                    else
                    {
                        oekobaudatEntriesSL0.Add(current);
                    }
                }
                //if service life 50 complete with entriesSL50
                else if(current.ServiceLife == 50)
                {
                    var completingEntry = entriesSL50.Find(n => n[UUIDpositionImport] == current.GeneralInformation[generalUUIDPos]);
                    if (completingEntry != null)
                    {
                        current.ServiceLife = Convert.ToInt32(completingEntry[serviceLifePositionImport]);
                        current.ChangesToEntry += "Manually updated service life from 50 to " + completingEntry[serviceLifePositionImport] + ", ";
                    }
                    else
                    {
                        oekobaudatEntriesSL50.Add(current);
                    }
                }
            }
            if(oekobaudatEntriesSL0.Count > 0)
            {
                CsvExportHandler.ExportOekobaudatEntries("EntriesWithServiceLife0AfterManual", oekobaudatEntriesSL0, structure);
            }
            if (oekobaudatEntriesSL50.Count > 0)
            {
                CsvExportHandler.ExportOekobaudatEntries("EntriesWithServiceLife50AfterManual", oekobaudatEntriesSL50, structure);
            }
        }//UpdateServiceLife
    }
}
