using System;
using System.Collections.Generic;
using System.Text;
using SharedDBLibrary;

namespace KnowledgeDB
{
    public class TwoComponentLayerType : IHasID, IHasForeignName<LayerTypeName>, IHasConstructor<int, int, int, int, bool, bool, double, bool>
    {
        public int Id { get; set; }
        public LayerTypeName Name { get; set; }
        public int NameId { get; set; }
        public StandardLayerType Component1 { get; set; }
        public int Component1Id { get; set; }
        public StandardLayerType Component2 { get; set; }
        public int Component2Id { get; set; }
        public bool SeparateReplacement { get; set; }
        public bool HasFixedLambda { get; set; } 
        public double FixedLambda { get; set; }
        public bool CostsByPercentage { get; set; }

        public TwoComponentLayerType() { }
        public string getName() { return Name.Name; }
        public void Constructor(int nameId, int comp1Id, int comp2Id, int id, bool separateReplacement, bool hasFixedLambda, 
                                double fixedLambda, bool costsByPercentage)
        {
            this.NameId = nameId;
            this.Component1Id = comp1Id;
            this.Component2Id = comp2Id;
            this.Id = id;
            this.SeparateReplacement = separateReplacement;
            this.HasFixedLambda = hasFixedLambda;
            this.FixedLambda = fixedLambda;
            this.CostsByPercentage = costsByPercentage;
        }

    }
}
