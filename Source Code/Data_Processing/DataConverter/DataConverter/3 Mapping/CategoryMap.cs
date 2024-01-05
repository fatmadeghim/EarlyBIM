using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConverter
{
    public class CategoryMap
    {
        public string Category { get; set; }
        public bool ManualMapping { get; set; }
        public bool UsefulInEarlyDesignPhases { get; set; }
        public List<string> Layers { get; set; }
        public List<string> KGs { get; set; }
        
        public CategoryMap(string[] readInput)
        {
            //Category
            Category = readInput[0];
            //KGs
            KGs = readInput[1].Split(',').ToList();
            for(int i = 0; i < KGs.Count; i++)
            {
                KGs[i] = KGs[i].Trim();
            }
            //Layers
            Layers = readInput[2].Split(',').ToList();
            for (int i = 0; i < Layers.Count; i++)
            {
                Layers[i] = Layers[i].Trim();
            }
            //ManualMapping and UsefulInEarlyDesignPhases
            if (readInput[5].Contains("x") && (readInput[4] == string.Empty || readInput[4] == " "))
            {
                ManualMapping = false;
                UsefulInEarlyDesignPhases = true;
            }
            else if (readInput[5].Contains("o"))
            {
                ManualMapping = false;
                UsefulInEarlyDesignPhases = false;
            }
            else if (readInput[4] != string.Empty && readInput[4] != " ")
            {
                ManualMapping = true;
                UsefulInEarlyDesignPhases = true;
            }
            else
            {
                ManualMapping = false;
                UsefulInEarlyDesignPhases = false;
                //throw error as no other option implemented
                throw new NotImplementedException("This option is not implemented");
            }
        }

        //used only for unit test (todo: update unit test)
        public CategoryMap(string category, bool manual, bool useful)
        {
            Category = category;
            ManualMapping = manual;
            UsefulInEarlyDesignPhases = useful;
        }
    }
}
