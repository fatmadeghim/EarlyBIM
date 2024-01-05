using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataConverter
{
    public class ApplicationCSVParser
    {

        /***
        Parses all lines (except the first) of the input applications file extracts the keywords and related KG from it
        :param path: string with path to applications csv file
        :param keywords: reference to variable whereto extracted data shall be written
        :param related: reference to variable whereto extracted data shall be written
        ***/
        public static void ParsingKeywords(string path, ref Dictionary<int, List<string>> keywords, ref Dictionary<int, List<int>> related)
        {
            var list = CsvImportHandler.ReadFromCSV(path, true,Encoding.UTF8);

            foreach (var line in list)
            {
                
                if (line[0] != String.Empty)
                {
                    keywords.Add(Convert.ToInt32(line[0]), line[1].Split(',').ToList<string>());

                }
                if (line[2] != String.Empty)
                {
                    related.Add(Convert.ToInt32(line[0]), line[2].Split(',').Select(i => int.Parse(i)).ToList());
                }
            }
        }
    }
}
