using System;
using System.Collections.Generic;
using System.Text;

namespace DataConverter.Old_Functions
{
    class OldFunctions
    {

        /***
       Exports the given entries through positionList and the corresponding List of SingleModEntries to a CSV file,
       taking into account that there are multiples and writes the multiples in one line
       Important: in opposition to function "ExportPositionWithMultiples" this works as well if not all multiples are in the positions list
       :param name: name of file
       :param positions: List of positions that shall be exported (corresponds to positions in List<SingleModEntry>
       :param structure: Structure of Oekobaudat
       :param multiples: List of List of indices of multiples
       :return: just for testing!
       ***/
        /*
       public static List<string> ExportPositionWithMultipleNonConsecutive(string name, List<int> positions, List<SingleModEntry> entries, StructureOekobaudat structure, List<List<int>> multiples)
       {
           //set file path
           string fileName = "Data" + name + "Mult.csv";
           var strFilePath = SetStorageLocation(fileName);

           //find max number of entries in multiples (might be too big, but doesn't really matter)
           int maxNumberOfEntries = multiples.Max(n => n.Count());

           //header
           var header = structure.ToCSVString(maxNumberOfEntries);

           //body
           List<string> body = new List<string>();
           bool lastEntryWritten = false;
           //entries with data according to positions and multiples
           for (int i = 0; i < positions.Count() - 1; i++)
           {
               var current = entries[positions[i]];
               var next = entries[positions[i + 1]];

               string line = string.Join(Constants.strSeparator, current.GeneralInformation);

               //check if multiple entries
               if (current.GeneralInformation[structure.UUIDPos] == next.GeneralInformation[structure.UUIDPos])
               {
                   while (current.GeneralInformation[structure.UUIDPos] == next.GeneralInformation[structure.UUIDPos])
                   {
                       line += ";" + string.Join(Constants.strSeparator, entries[positions[i]].Indicators);
                       line += ";" + entries[positions[i]].GeneralInformation.Last();
                       i++;
                       if (i >= positions.Count - 1)
                       {
                           lastEntryWritten = true;
                           break;
                       }

                       current = entries[positions[i]];
                       next = entries[positions[i + 1]];
                   }
                   line += ";" + string.Join(Constants.strSeparator, current.Indicators);
               }
               //no multiple entry
               else
               {
                   line += ";" + string.Join(Constants.strSeparator, current.Indicators);
               }
               body.Add(line);
           }

           if (!lastEntryWritten)
           {
               var last = entries[positions.Last()];
               string line = string.Join(Constants.strSeparator, last.GeneralInformation);
               line += ";" + string.Join(Constants.strSeparator, last.Indicators);
               body.Add(line);
           }

           Export(strFilePath, header, body);
           return body;
       }//ExportPositionWithMultipleNonConsecutive




       */


        /***
        Exports the given entries through positionList and the corresponding List of SingleModEntries to a CSV file,
        taking into account that there are multiples and writes the multiples in one line
        :param name: name of file
        :param positions: List of positions that shall be exported (corresponds to positions in List<SingleModEntry>
        :param structure: Structure of Oekobaudat
        :param multiples: List of List of indices of multiples
        ***/
        /*
        public static void ExportPositionWithMultiples(string name, List<int> positions, List<SingleModEntry> entries, StructureOekobaudat structure, List<List<int>> multiples)
        {
            //set file path
            string fileName = "Data" + name + "Mult.csv";
            var strFilePath = SetStorageLocation(fileName);

            //find max number of entries in multiples
            int maxNumberOfEntries = multiples.Max(n => n.Count());

            //header
            var header = structure.ToCSVString(maxNumberOfEntries);

            //body
            List<string> body = new List<string>();
            //entries with data according to positions and multiples
            int currentMultiple = 0;
            for (int i = 0; i < positions.Count(); i++)
            {
                string line = string.Join(Constants.strSeparator, entries[positions[i]].GeneralInformation);
                //check if multiple entries
                if (positions[i] == multiples[currentMultiple][0])
                {
                    for (int j = 0; j < multiples[currentMultiple].Count(); j++)
                    {
                        if (j != 0)
                        {
                            line += ";" + entries[positions[i]].GeneralInformation.Last();
                        }
                        line += ";" + string.Join(Constants.strSeparator, entries[positions[i]].Indicators);
                        i++;
                    }
                    currentMultiple++;
                    i--;
                }
                //no multiple entry
                else
                {
                    line += ";" + string.Join(Constants.strSeparator, entries[positions[i]].Indicators);
                }
                body.Add(line);
            }//for (int i = 0; i < positionKG.Count(); i++)
            Export(strFilePath, header, body);
        }//ExportPositionWithMultiples
        */



        /***
        Exports the given entries through positionList and the corresponding List of SingleModEntries to a CSV file
        :param name: name of file
        :param positions: List of positions that shall be exported (corresponds to positions in List<SingleModEntry>
        :param structure: Structure of Oekobaudat
        ***/
        /*
        public static void ExportPosition(string name, List<int> positions, List<SingleModEntry> entries, StructureOekobaudat structure)
        {

            //a) set file path
            string fileName = "Data" + name + ".csv";
            var strFilePath = SetStorageLocation(fileName);

            //b) build strings
            var header = structure.ToCSVString();
            var body = new List<string>();
            //other lines with data according to PositionList
            for (int i = 0; i < positions.Count(); i++)
            {
                body.Add(entries[positions[i]].ToCSVString());
            }
            Export(strFilePath, header, body);
        }
        */

        /***
        Exports the given entries to a CSV file
        :param name: name of file
        :param entries: List of entries that shall be exported
        :param structure: Structure of Oekobaudat
        ***/
        /*
        public static void ExportSingleModEntries(string name, List<SingleModEntry> entries, StructureOekobaudat structure)
        {

            //a) set file path
            string fileName = "Data" + name + ".csv";
            var strFilePath = SetStorageLocation(fileName);

            //b) build strings
            var header = structure.ToCSVString();
            var body = new List<string>();
            //other lines with data according to PositionList
            for (int i = 0; i < entries.Count(); i++)
            {
                body.Add(entries[i].ToCSVString());
            }
            Export(strFilePath, header, body);
        }
        */


        /***
        Exports the given entries to a CSV file
        :param name: name of file
        :param entries: List of entries that shall be exported
        :param structure: Structure of Oekobaudat
        ***/
        /*
        public static void ExportStrings(string name, List<string> entries, StructureOekobaudat structure)
        {

            //a) set file path
            string fileName = "Data" + name + ".csv";
            var strFilePath = SetStorageLocation(fileName);

            //b) build strings
            var header = string.Join(Constants.strSeparator, structure.GeneralInformation) + "; A1-A3 existing; C3 existing; C4 existing; D existing";
            var body = new List<string>();
            //other lines with data according to PositionList
            for (int i = 0; i < entries.Count(); i++)
            {
                body.Add(entries[i]);
            }
            Export(strFilePath, header, body);
        }
        */

        /***
        Exports the given positions with the corresponding application to a csv file with one line per Dataset with the following information:
        UUID, Name, Kategorie, technological applicability, Applications
        :param name: name of file
        :param positions: List of positions that shall be exported (corresponds to positions in List<SingleModEntry>
        :param structure: Structure of Oekobaudat
        :param multiples: List of List of indices of multiples
        ***/
        /*
        public static void ExportWithApplicationMult(string name, List<int> positions, List<SingleModEntry> entries, StructureOekobaudat structure, List<List<int>> multiples)
        {
            //a) set file path
            string fileName = "Data" + name + "OverviewApplication.csv";
            var strFilePath = SetStorageLocation(fileName);

            //b) header
            int columnUUID = structure.FindIndex("UUID");
            int columnName = structure.FindIndex("Name");
            int columnCategory = structure.FindIndex("Kategorie");

            var header = structure.GeneralInformation[columnUUID] + ";" +
                structure.GeneralInformation[columnName] + ";" +
                structure.GeneralInformation[columnCategory] + ";" +
                "Technological Applicability ;" +
                "Applications";

            //c) body
            var body = new List<string>();
            int currentMultiple = 0;
            for (int i = 0; i < positions.Count(); i++)
            {
                body.Add(
                    entries[positions[i]].GeneralInformation[columnUUID] + ";" +
                    entries[positions[i]].GeneralInformation[columnName] + ";" +
                    entries[positions[i]].GeneralInformation[columnCategory] + ";" +
                    entries[positions[i]].TechnologicalApplicability + ";" +
                    entries[positions[i]].ApplicationToCSVString());
                if (positions[i] == multiples[currentMultiple][0])
                {
                    i += multiples[currentMultiple].Count - 1;
                    currentMultiple++;
                }
            }
            Export(strFilePath, header, body);
        }//ExportWithApplicationMult
        */


        /***
        Exports the given positions with the corresponding application to a csv file
        :param name: name of file
        :param positions: List of positions that shall be exported (corresponds to positions in List<SingleModEntry>
        :param structure: Structure of Oekobaudat
        ***/
        /*
        public static void ExportWithApplication(string name, List<int> positions, List<SingleModEntry> entries, StructureOekobaudat structure)
        {
            //a) set file path
            string fileName = "Data" + name + "Application.csv";
            var strFilePath = SetStorageLocation(fileName);

            //b) header
            var header = structure.ToCSVString() + "; Applications";

            //c) body
            var body = new List<string>();
            for (int i = 0; i < positions.Count(); i++)
            {
                body.Add(entries[positions[i]].ToCSVString() + ";" + entries[positions[i]].ApplicationToCSVString());
            }
            //d) export
            Export(strFilePath, header, body);
        }//ExportWithApplication
        */


        /***
        Exports the general information of the given entries through positionList and the corresponding List of SingleModEntries to a CSV file,
        taking into account that there are multiples (written only once)
        Important: in opposition to function "ExportPositionWithMultiples" this works as well if not all multiples are in the positions list
        :param name: name of file
        :param positions: List of positions that shall be exported (corresponds to positions in List<SingleModEntry>)
        :param structure: Structure of Oekobaudat
        :return: just for testing!
        ***/
        /*
        public static List<string> ExportPositionGeneralInfoMult(string name, List<int> positions, List<SingleModEntry> entries, StructureOekobaudat structure)
        {
            //set file path
            string fileName = "Data" + name + "Mult.csv";
            var strFilePath = SetStorageLocation(fileName);

            //header
            var header = structure.ToCSVString();

            //body
            List<string> body = new List<string>();
            var positionsNoDuplicates = Order.EliminateDuplicates(entries, positions, structure.UUIDPos);
            for (int i = 0; i < positionsNoDuplicates.Count(); i++)
            {
                var current = entries[positionsNoDuplicates[i]];
                string line = string.Join(Constants.strSeparator, current.GeneralInformation.GetRange(0, current.GeneralInformation.Count - 1));
                body.Add(line);
            }

            Export(strFilePath, header, body);
            return body;
        }//ExportPositionGeneralInfoMult
        */
    }
}
