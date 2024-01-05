using System;
using System.Collections.Generic;
using System.Text;
using KnowledgeDB;

namespace DataConverter
{
    public class GenerateOekobaudatData
    {
        public static List<OekobaudatData> GenOekobaudat(
            List<Unit> units,
            StructureOekobaudat structure,
            List<OekobaudatEntry> oekobaudatEntries)
        {
            //get positions
            int namePos = structure.FindIndex("Name");
            int UUIDPos = structure.FindIndex("UUID");
            int modPos = structure.FindIndex("Modul");
            int unitPos = structure.FindIndex("Bezugseinheit");

            var result = new List<OekobaudatData>();
            for(int i = 0; i < oekobaudatEntries.Count; i++)
            {
                var current = oekobaudatEntries[i];
                var unit = units.Find(n => n.ReferenceUnit == current.GeneralInformation[unitPos]);
                if (unit != null)
                {
                    //assign GWP values if module present
                    double GWPA1_A3 = 0;
                    double GWPC3 = 0;
                    double GWPC4 = 0;
                    double GWPD = 0;
                    if (current.IndicatorsA1_A3 != null)
                    {
                        GWPA1_A3 = current.IndicatorsA1_A3[0];
                    }
                    if (current.IndicatorsC3 != null)
                    {
                        GWPC3 = current.IndicatorsC3[0];
                    }
                    if (current.IndicatorsC4 != null)
                    {
                        GWPC4 = current.IndicatorsC4[0];
                    }
                    if (current.IndicatorsD != null)
                    {
                        GWPD = current.IndicatorsD[0];
                    }

                    var dataEntry = new OekobaudatData(current.GeneralInformation[UUIDPos], unit, GWPA1_A3, GWPC3, GWPC4, GWPD);
                    result.Add(dataEntry);
                }
            }
            return result;
        }
    }
}
