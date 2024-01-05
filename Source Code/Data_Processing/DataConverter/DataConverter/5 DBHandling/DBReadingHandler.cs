using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KnowledgeDB;

namespace DataConverter
{
    class DBReadingHandler
    {
        public static List<KG3xxName> ReadKG3xxName(string filepathDB)
        {
            using var context = new KnowledgeContext(filepathDB);
            List<KG3xxName> kG3xxNames = context.KG3xxNames.ToList();
            return kG3xxNames;
        }

        public static List<StandardLayerType> ReadStandardLayerTypes(string filepathDB)
        {
            using var context = new KnowledgeContext(filepathDB);
            List<StandardLayerType> standardLayerTypes = context.StandardLayerTypes.ToList();
            foreach(var standardLayerType in standardLayerTypes)
            {
                standardLayerType.Name = context.LayerTypeNames.Find(standardLayerType.NameId);
            }
            return standardLayerTypes;
        }

        //not sure if this function needed/reasonable
        public static List<Unit> ReadUnit(string filepathDB)
        {
            using var context = new KnowledgeContext(filepathDB);
            List<Unit> units = context.Units.ToList();
            return units;
        }
    }
}
