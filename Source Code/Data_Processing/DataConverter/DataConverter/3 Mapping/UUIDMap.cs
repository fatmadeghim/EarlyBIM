using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConverter
{
    public class UUIDMap
    {
        public string UUID { get; set; }
        public bool UsefulInEarlyDesignPhases { get; set; }
        public List<string> Layers { get; set; }
        public List<string> KGs { get; set; }

        public UUIDMap(string[] readInput)
        {
            //UUID
            UUID = readInput[0];
            //KGs
            KGs = readInput[20].Split(',').ToList();
            for (int i = 0; i < KGs.Count; i++)
            {
                KGs[i] = KGs[i].Trim();
            }
            //Layers
            Layers = readInput[21].Split(',').ToList();
            for (int i = 0; i < Layers.Count; i++)
            {
                Layers[i] = Layers[i].Trim();
            }
            //UsefulInEarlyDesignPhases
            if (readInput[24].Contains("x"))
            {
                UsefulInEarlyDesignPhases = true;
            }
            else if (readInput[24].Contains("o"))
            {
                UsefulInEarlyDesignPhases = false;
            }
            else
            {
                UsefulInEarlyDesignPhases = false;
                //throw error as no other option implemented
                //throw new NotImplementedException("This option is not implemented");
            }
        }
    }
}
