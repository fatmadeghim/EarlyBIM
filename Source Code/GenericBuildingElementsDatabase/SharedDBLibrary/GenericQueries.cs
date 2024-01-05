using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SharedDBLibrary
{
    public static class GenericQueries
    {
        public static IQueryable<T2> FindT2sWhereT1Is<T1,T2,T3>(this DbSet<T3> mTmSet, T1 t1, DbSet<T2> t2Set) where T3: class, IManyToMany<T1, T2>
                                                                                                               where T1: class, IHasID
                                                                                                               where T2: class, IHasID
        {
            return (from mTm in mTmSet
                    join t2 in t2Set on mTm.Id2 equals t2.Id
                    where mTm.Id1 == t1.Id
                    select t2);
        }

        public static IQueryable<T1> FindT1sWhereT2Is<T1, T2, T3>(this DbSet<T3> mTmSet, DbSet<T1> t1Set, T2 t2) where T3 : class, IManyToMany<T1, T2>
                                                                                                                 where T1 : class, IHasID
                                                                                                                 where T2 : class, IHasID
        {
            return (from mTm in mTmSet
                    join t1 in t1Set on mTm.Id1 equals t1.Id
                    where mTm.Id2 == t2.Id
                    select t1);
        }



        public static IQueryable<T1> FindT1sWhereT2IsName<T1, T2, T3>(DbSet<T3> mTmSet, DbSet<T1> t1Set, DbSet<T2> t2Set, string name)
                                                                                               where T3 : class, IManyToManyT2HasName<T1, T2>
                                                                                               where T1 : class, IHasID
                                                                                               where T2 : IsName, IHasID
        {
            return (from t1 in t1Set
                    join mTm in mTmSet on t1.Id equals mTm.Id1
                    join t2 in t2Set on mTm.Id2 equals t2.Id
                    where t2.Name.Equals(name)
                    select t1);
        }

        public static IQueryable<T2> FindT2sWhereT1IsName<T1, T2, T3>(DbSet<T3> mTmSet, DbSet<T1> t1Set, DbSet<T2> t2Set, string name)
                                                                                    where T3 : class, IManyToManyT1HasName<T1, T2>
                                                                                    where T1 : IsName, IHasID
                                                                                    where T2 : class, IHasID
        {
            return (from t2 in t2Set
                    join mTm in mTmSet on t2.Id equals mTm.Id2
                    join t1 in t1Set on mTm.Id1 equals t1.Id
                    where t1.Name.Equals(name)
                    select t2);
        }

        public static List<T1> GetNameObjectsFromListOfStrings<T1>(this DbSet<T1> dbSet, List<string> strings) where T1: IsName
        {
            var objectlist = new List<T1>();
            foreach (var name in strings)
            {
                objectlist.Add(dbSet.Where(nameObj => nameObj.Name.Equals(name)).First());
            }
            return objectlist;
        }

        public static IQueryable<T1> GetObjectsWithName<T1, T2>(this DbSet<T1> namedObjects, DbSet<T2> names, string name) where T1: class, IHasForeignName<T2>
                                                                                                                                            where T2: IsName
        {
            return (from obj in namedObjects
                    join nme in names on obj.NameId equals nme.Id
                    where nme.Name.Equals(name)
                    select obj);
        }
    }
}
