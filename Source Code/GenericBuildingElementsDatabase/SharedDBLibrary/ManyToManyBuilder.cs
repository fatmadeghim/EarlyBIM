using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SharedDBLibrary
{
    public static class ManyToManyBuilder
    {
        public static void LinkT1ToManyT2s<T1, T2, T3>(this DbSet<T3> mtmSet, T1 t1, List<T2> t2s) where T3: class, IManyToMany<T1,T2>, new()
                                                                                                   where T1: IHasID
                                                                                                   where T2: IHasID
        {
            foreach (var t2 in t2s)
            {
                var manyToManyEntry = new T3()
                {
                    Type1 = t1,
                    Type2 = t2
                };
                mtmSet.Add(manyToManyEntry);
            }
        }

        public static void LinkT1ToManyT2sWithPositions<T1, T2, T3>(this DbSet<T3> mtmSet, T1 t1, List<T2> t2s, List<int> positions) 
                                                                                                   where T3 : class, IManyToMany<T1, T2>, IHasPosition, new()
                                                                                                   where T1 : IHasID
                                                                                                   where T2 : IHasID
        {
            var index = 0;
            foreach (var t2 in t2s)
            {
                var manyToManyEntry = new T3()
                {
                    Type1 = t1,
                    Type2 = t2,
                    Position = positions[index]
                };
                index++;
                mtmSet.Add(manyToManyEntry);
            }
        }

        public static void LinkT2ToManyT1s<T1, T2, T3>(this DbSet<T3> mtmSet, List<T1> t1s, T2 t2) where T3: class, IManyToMany<T1,T2>, new()
                                                                                                   where T1: IHasID
                                                                                                   where T2: IHasID
        {
            foreach (var t1 in t1s)
            {
                var manyToManyEntry = new T3()
                {
                    Type1 = t1,
                    Type2 = t2,
                };
                mtmSet.Add(manyToManyEntry);
            }
        }

        public static void LinkT1ToManyT2sWithPositionsT2IdOnly<T1, T2, T3>(this DbSet<T3> mtmSet, T1 t1, List<T2> t2s, List<int> positions)
                                                                                           where T3 : class, IManyToMany<T1, T2>, IHasPosition, new()
                                                                                           where T1 : IHasID
                                                                                           where T2 : IHasID
        {
            var index = 0;
            foreach (var t2 in t2s)
            {
                var manyToManyEntry = new T3()
                {
                    Type1 = t1,
                    Id2 = t2.Id,
                    Position = positions[index]
                };
                index++;
                mtmSet.Add(manyToManyEntry);
            }
        }

        public static void LinkT1ToManyT2sWithPositionsT2IdOnly<T1,T2, T3>(this DbSet<T3> mtmSet, T1 t1, List<int> t2s, List<int> positions)
                                                                                           where T3 : class, IManyToMany<T1, T2>, IHasPosition, new()
                                                                                           where T1 : IHasID
                                                                                           where T2 : IHasID
        {
            var index = 0;
            foreach (var t2 in t2s)
            {
                var manyToManyEntry = new T3()
                {
                    Type1 = t1,
                    Id2 = t2,
                    Position = positions[index]
                };
                index++;
                mtmSet.Add(manyToManyEntry);
            }
        }
    }
}
