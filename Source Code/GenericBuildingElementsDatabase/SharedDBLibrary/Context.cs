using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace SharedDBLibrary
{
    public class Context : DbContext
    {
        //Non DbSet Variables
        protected string filepath;

        public Context() //This constructor is used when adding a migration and creating the DB 
        { }

        public Context(string filepath)
        {
            this.filepath = filepath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=" + filepath);
        }

    }
}
