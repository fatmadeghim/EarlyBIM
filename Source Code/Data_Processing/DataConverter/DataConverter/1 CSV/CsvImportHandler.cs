using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataConverter
{
    public class CsvImportHandler
    {
        /***
            Reads all lines from a given csv file (through path) and returns them as a list of string[]
            :param path: string with path to csv file
            :param skipFirstLine: bool (true if first line should not be read)
            :param encoding: Encoding of csv file
            :param splitter: by default ';', can be changed if csv file has different splitter
            :return: List<string[]>
        ***/
        public static List<string[]> ReadFromCSV (
            string path,
            bool skipFirstLine,
            Encoding encoding,
            char splitter = ';'
            )
        {
            var list = File.ReadAllLines(path, encoding).Skip(Convert.ToInt32(skipFirstLine));
            List<string[]> result = new List<string[]>();
            foreach (var line in list)
            {
                result.Add(line.Split(splitter));

            }
            return result;
        }

        /***
            Reads all lines except first from a csv file and converts the lines into CategoryMaps
            :param path: string containing the path to the csv file
            :return: List<CategoryMap>
        ***/
        public static List<CategoryMap> ReadCategories(string path)
        {
            //read from CSV file
            var list = CsvImportHandler.ReadFromCSV(path, true, Encoding.UTF8);

            var categoryMaps = new List<CategoryMap>();
            foreach (var line in list)
            {
                categoryMaps.Add(new CategoryMap(line));
            }
            return categoryMaps;
        }

        /***
            Reads all lines except first from a csv file and converts the lines into UUIDMaps
            :param path: string containing the path to the csv file
            :return: List<UUIDMap>
        ***/
        public static List<UUIDMap> ReadUUIDMaps(string path)
        {
            //read from CSV file
            var list = CsvImportHandler.ReadFromCSV(path, true, Encoding.UTF8);

            var UUIDMaps = new List<UUIDMap>();
            foreach (var line in list)
            {
                UUIDMaps.Add(new UUIDMap(line));
            }
            return UUIDMaps;
        }

        /***
            Reads the first line of a given file with the given encoding
            :param path: string containing the path to the csv file
            :return: List<string>
        ***/
        public static List<string> ReadHeader (string path)
        {
            //read from CSV file
            var header = File.ReadAllLines(path, Encoding.UTF8).First<string>();
            string[] parts = header.Split(';');
            for (int i = 0; i < parts.Count(); i++)
            {
                parts[i] = parts[i].Replace("\"", "");
            }
            return parts.ToList();
        }

        /***
            Reads all lines except first from a csv file and converts the lines into UUIDMaps
            :param path: string containing the path to the csv file
            :return: List<UUIDMap>
        ***/
        public static List<ThermalConductivityMap> ReadThermalConductivityMaps(string path)
        {
            //read from CSV file
            var list = CsvImportHandler.ReadFromCSV(path, true, Encoding.UTF8);

            var thermalConductivityMaps = new List<ThermalConductivityMap>();
            foreach (var line in list)
            {
                thermalConductivityMaps.Add(new ThermalConductivityMap(line));
            }
            return thermalConductivityMaps;
        }
    }//CSVHandler
}//DataConverter

