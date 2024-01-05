using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataConverter
{
    public class OekobaudatCSVParser
    {
        /***
            Parses the first line of the input oekobaudat file and returns it in form of type "StructureOekobaudat"
            :param path: string with path to oekobaudat csv file
            :return: StructureOekobaudat
        ***/
        public static StructureOekobaudat ParsingStructure(string path)
        {
            //to display umlauts correctly, UTF7 is needed
            var header = File.ReadAllLines(path, Encoding.UTF7).First<string>();
            string[] parts = header.Split(';');
            for (int i = 0; i < parts.Count(); i++)
            {
                parts[i] = parts[i].Replace("\"", "");
            }
            return new StructureOekobaudat(parts.ToList<string>());
        }

        /***
            Parses all lines (except the first)of the input oekobaudat file and creates an object of SingleModEntry for each line
            :param path: string with path to oekobaudat csv file
            :param modulePosition: int with position of module (needed for break between general informtion and data)
            :return: List<SingleModEntry>
        ***/
        public static List<SingleModEntry> ParsingEntries(string path, int modulePosition)
        {
            var list = File.ReadAllLines(path, Encoding.UTF7).Skip(1);
            
            List<SingleModEntry> entries = new List<SingleModEntry>();
            foreach (var line in list)
            {
                entries.Add(SingleModEntry.ParseFideCSV(line, modulePosition));
            }

            return entries;
        }
    }//CSVParser
}//DataConverter