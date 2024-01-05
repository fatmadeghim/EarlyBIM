using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataConverter
{
    public class CsvExportHandler
    {
        /***
            Generates the desired storage location folder (DataConverter\CSVoutput) and returns the needed file path
            :param fileName: string with desired name of file
            :return: string with file path to desired location
        ***/
        public static string SetStorageLocation(string fileName)
        {
            //create path to desired folder
            var filePath = Path.GetFullPath("../../../../../CSVoutput");
            //create folder if not already existent
            Directory.CreateDirectory(filePath);
            //add file name
            filePath = filePath + "/" + fileName;
            return filePath;
        }

        /***
            Exports the header and list body to the given filePath
            :param filePath: string to FilePath
            :param header: string with header
            :param body: List of strings with body of Export
        ***/
        public static void Export(string filePath, string header, List<string> body)
        {
            //Build strings to store in file as csv (with ; as separator)                
            StringBuilder sbOutput = new StringBuilder();
            //first line with header
            sbOutput.AppendLine(header);
            //other lines with data according to PositionList
            for (int i = 0; i < body.Count(); i++)
            {
                sbOutput.AppendLine(body[i]);
            }

            // Create and write the csv file
            try
            {
                File.WriteAllText(filePath, sbOutput.ToString(), Encoding.UTF8);
            }
            catch (Exception e)
            {
                Console.WriteLine("File was not found:" + e.Message);
            }
        }

        /***
            Exports the entries from positions WITH indicators (by using KeyValue Pair for multiples)
            Important: in opposition to function "ExportPositionWithMultiples" this works as well if not all multiples are in the positions list
            :param name: name of file
            :param positions: List of positions that shall be exported (corresponds to positions in List<SingleModEntry>)
            :param entries: List of SingleModEntries
            :param multiples: Key Value Pair for multiples
            :param structure: Structure of Oekobaudat
            :return: just for testing!
        ***/
        public static List<string> ExportPositionsKVMult(string name, List<int> positions, List<SingleModEntry> entries, List<KeyValuePair<int, List<int>>> multiples, StructureOekobaudat structure)
        {
            //set file path
            string fileName = "Data" + name + "Mult.csv";
            var strFilePath = SetStorageLocation(fileName);

            //header
            int maxRep = 1;
            if(multiples.Count > 0) 
            {
                maxRep += multiples.Max(n => n.Value.Count());
            }
            var header = structure.ToCSVString(maxRep);

            //body
            List<string> body = new List<string>();
            for (int i = 0; i < positions.Count(); i++)
            {
                var currentPos = positions[i];
                string line = string.Join(Constants.strSeparator, entries[currentPos].GeneralInformation) + ";";
                //check if multiple
                var mult = multiples.FirstOrDefault(pair => pair.Key == currentPos);
                if (mult.Value != null)
                {
                    line += string.Join(Constants.strSeparator, entries[mult.Key].Indicators);

                    foreach (var m in mult.Value)
                    {
                        line += ";" + entries[m].GeneralInformation[structure.ModulePos] + ";" + string.Join(Constants.strSeparator, entries[m].Indicators);
                        i++;
                    }
                }
                else
                {
                    line += string.Join(Constants.strSeparator, entries[currentPos].Indicators);
                }
                body.Add(line);
            }
            Export(strFilePath, header, body);
            return body;
        }//ExportPositionsKVMult

        /***
            Exports the entries from positions WITH indicators (by using KeyValue Pair for multiples)
            Important: in opposition to function "ExportPositionWithMultiples" this works as well if not all multiples are in the positions list
            :param name: name of file
            :param positions: List of positions that shall be exported (corresponds to positions in List<SingleModEntry>)
            :param entries: List of SingleModEntries
            :param structure: Structure of Oekobaudat
            :return: just for testing!
        ***/
        public static List<string> ExportPositionsMult(string name, List<int> positions, List<SingleModEntry> entries, StructureOekobaudat structure)
        {
            var multiples = MultipleHandler.FindMultiples(positions, structure.UUIDPos, entries);
            return ExportPositionsKVMult(name, positions, entries, multiples, structure);
        }//ExportPositionsMult


        /***
            Exports the given oekobaudat entries to a CSV file
            :param name: name of file
            :param entries: List of all Oekobaudat entries that shall be exported
            :param structure: Structure of Oekobaudat
        ***/
        public static void ExportOekobaudatEntries(string name, List<OekobaudatEntry> entries, StructureOekobaudat structure)
        {
            //a) set file path
            string fileName = "Data" + name + ".csv";
            var strFilePath = SetStorageLocation(fileName);

            //b) build strings
            var header = string.Join(Constants.strSeparator, structure.GeneralInformation.GetRange(0, structure.GeneralInformation.Count-1)) + "; A1-A3 existing; C3 existing; C4 existing; D existing";
            var body = new List<string>();
            //other lines with data according to PositionList
            for (int i = 0; i < entries.Count(); i++)
            {
                body.Add(entries[i].ToCSVString());
            }
            Export(strFilePath, header, body);
        }

        /***
            Exports the given oekobaudat entries with layer types and kgs to a CSV file
            :param name: name of file
            :param entries: List of all Oekobaudat entries that shall be exported
            :param structure: Structure of Oekobaudat
        ***/
        public static void ExportOekobaudatEntriesWithLayerTypesAndKG(string name, List<OekobaudatEntry> entries, StructureOekobaudat structure)
        {
            //a) set file path
            string fileName = "Data" + name + "WithLayerTypesAndKGs.csv";
            var strFilePath = SetStorageLocation(fileName);

            //b) build strings
            var header = string.Join(Constants.strSeparator, structure.GeneralInformation.GetRange(0, structure.GeneralInformation.Count - 1)) + "; A1-A3 existing; C3 existing; C4 existing; D existing; Layers; KGs; Changes to entry";
            var body = new List<string>();
            //other lines with data according to PositionList
            foreach(var entry in entries)
            {
                body.Add(entry.ToCSVString()+ ";" + entry.GetLayerTypesAndKGsForCsv() + entry.ChangesToEntry);
            }
            Export(strFilePath, header, body);
        }//ExportOekobaudatEntriesWithLayerTypesAndKG
    }//ExportEntries
}//Namespace: DataConverter