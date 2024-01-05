using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataConverter
{
    public static class ClearDBHandler
    {
        public static void Clear<T>(this DbSet<T> dbSet) where T : class
        {
            //dbSet.RemoveRange(dbSet);
            if (dbSet != null) 
            { 
                foreach (var member in dbSet)
                {
                    dbSet.Remove(member);
                }
            }
        }
    }
}
