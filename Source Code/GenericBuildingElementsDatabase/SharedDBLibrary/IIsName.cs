using System;
using System.Collections.Generic;
using System.Text;

namespace SharedDBLibrary
{
    public abstract class IsName : IHasName, IHasID, IHasConstructor<string>, IHasConstructor<string, int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual string getName()
        {
            return Name;
        }
        public IsName() { }
        public IsName(string name)
        {
            Name = name;
        }

        public void Constructor(string name)
        {
            Name = name;
        }

        public void Constructor(string name, int id)
        {
            this.Name = name;
            this.Id = id;
        }
    }
}
