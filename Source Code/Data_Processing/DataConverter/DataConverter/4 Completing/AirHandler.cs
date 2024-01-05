using KnowledgeDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataConverter
{
    public class AirHandler
    {
        /***
       Generates two OekobaudatEntries for air (moving air and static air layer) with zero values for indicators
       :param structure: structure of Oekobaudat
       :return: List of air OekobaudatEntries
       ***/
        public static List<OekobaudatEntry> GenerateAir(StructureOekobaudat structure, List<KnowledgeDB.KG3xxName> allKG3xxs)
        {
            
            var name1 = "moving air";
            var thermalConductivity1 = 0.0;
            var name2 = "static air";
            var thermalConductivity2 = 0.33;
            var movingAir = GenerateValidEmptyEntry(name1, thermalConductivity1, structure, allKG3xxs);
            var staticAir = GenerateValidEmptyEntry(name2, thermalConductivity2, structure, allKG3xxs);
            var airEntries = new List<OekobaudatEntry>() { movingAir, staticAir };
            return airEntries;
        }

        //helper function for GenerateAir
        public static OekobaudatEntry GenerateValidEmptyEntry(string name, double thermalConductivity, StructureOekobaudat structure, List<KnowledgeDB.KG3xxName> allKG3xxs)
        {
            //general information empty except for chosen entities
            var generalInformation = new List<string>();
            for(int i = 0; i< structure.GeneralInformation.Count-1; i++)
            {
                generalInformation.Add("");
            }
            generalInformation[structure.UUIDPos] = "UUID " + name;
            generalInformation[structure.NamePos] = name;
            generalInformation[structure.ReferenceUnitPos] = "m3";
            generalInformation[structure.ReferenceValuePos] = "1";
            //empty indicators
            var emptyIndicators = new List<double>();
            for (int i = 0; i < structure.Indicators.Count; i++)
            {
               emptyIndicators.Add(0.0);
            }
            //only layer with same name as entry and applicable in all kgs
            var layers = new List<string>() { name };
            var kgs = new List<string>();
            foreach(var kg in allKG3xxs)
            {
                kgs.Add(kg.Name);
            }
            int serviceLifeTime = 120; //as max of all service life times (could be infinity as air has no life time as such)
            var entry = new OekobaudatEntry(generalInformation, emptyIndicators, emptyIndicators, emptyIndicators, emptyIndicators, null, layers, kgs, serviceLifeTime, thermalConductivity, "");
            return entry;
        }
    }
}
