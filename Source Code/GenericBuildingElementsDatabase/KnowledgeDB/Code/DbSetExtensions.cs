using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeDB
{
    public static class DbSetExtensions
    {
        public static void LinkT1ToManyT2s(this DbSet<KG3xxOption_LayerTypeName> mtmSet, 
                                           KG3xxOption t1, 
                                           List<LayerTypeName> t2s, 
                                           List<int> positions, 
                                           List<int> accessibilities)
                                                               
        {
            var index = 0;
            foreach (var t2 in t2s)
            {
                var manyToManyEntry = new KG3xxOption_LayerTypeName()
                {
                    Type1 = t1,
                    Type2 = t2,
                    Position = positions[index],
                    AccessOrder = accessibilities[index]
                };
                index++;
                mtmSet.Add(manyToManyEntry);
            }
        }

        public static void LinkT1ToManyT2s(this DbSet<KG3xxOption_LayerTypeName> mtmSet,
                                           KG3xxOption t1,
                                           List<LayerTypeName> t2s,
                                           List<int> positions,
                                           List<int> accessibilities,
                                           List<bool> hasExposureQualities)

        {
            var index = 0;
            foreach (var t2 in t2s)
            {
                var manyToManyEntry = new KG3xxOption_LayerTypeName()
                {
                    Type1 = t1,
                    Type2 = t2,
                    Position = positions[index],
                    AccessOrder = accessibilities[index],
                    HasExposureQuality = hasExposureQualities[index]
                };
                index++;
                mtmSet.Add(manyToManyEntry);
            }
        }
    }
}
