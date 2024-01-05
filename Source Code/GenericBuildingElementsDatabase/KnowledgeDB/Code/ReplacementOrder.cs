using System;
using System.Collections.Generic;
using System.Text;
using SharedDBLibrary;

namespace KnowledgeDB
{
    public class ReplacementOrder: IHasID, IHasConstructor<string, int>
    {
        public int Id { get; set; }
        public string Order { get; set; }
        public ReplacementOrder() { }
        public void Constructor(string order, int id)
        {
            this.Order = order;
            this.Id = id;
        }
    }
}
