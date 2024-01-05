using System;
using System.Collections.Generic;
using System.Text;

using SharedDBLibrary;

namespace KnowledgeDB
{
    public class StandardLayerType : IHasID, IHasForeignName<LayerTypeName>, IHasConstructor<int, int>
    {
        public int Id { get; set; }
        public LayerTypeName Name { get; set; }
        public int NameId { get; set; }
        public ICollection<Layer_StandardLayerType> Layer_StandardLayerTypes { get; set; }
        public StandardLayerType() { }

        public StandardLayerType(LayerTypeName name)
        {
            Name = name;
        }

        public string getName()
        {
            return Name.getName();
        }

        public void Constructor(int nameId, int id)
        {
            this.NameId = nameId;
            this.Id = id;
        }
    }

}
