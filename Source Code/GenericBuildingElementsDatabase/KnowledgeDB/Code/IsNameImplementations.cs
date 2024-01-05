using System;
using System.Collections.Generic;
using System.Text;

using SharedDBLibrary;

namespace KnowledgeDB
{
    public class KG3x0Name : IsName, IHasConstructor<string, int, int, string, bool, bool, bool, bool>
    {
        public SurfaceR SurfaceR { get; set; }
        public int SurfaceRId { get; set; }
        public ICollection<KG3x0Name_KG3xxName> KG3x0Name_KG3xxNames { get; set; }
        public string Category { get; set; }
        public bool IsExterior { get; set; }
        public bool LoadBearing { get; set; }
        public bool AboveGround { get; set; }
        public bool Sloped { get; set; }
        public KG3x0Name() { }
        public void Constructor(string name, int surfaceId, int id, string category, bool isExterior, bool loadBearing, bool aboveGround, bool sloped)
        {
            this.Id = id;
            this.Name = name;
            this.SurfaceRId = surfaceId;
            this.Category = category;
            this.IsExterior = isExterior;
            this.LoadBearing = loadBearing;
            this.AboveGround = aboveGround;
            this.Sloped = sloped;
        }
        public KG3x0Name(string name) : base(name) { }
    }
    public class KG3xxName : IsName, IHasConstructor<string, int, int>
    {
        public ICollection<KG3x0Name_KG3xxName> KG3x0Name_KG3xxNames { get; set; }
        public ICollection<KG3xxName_Layer> KG3xxName_Layers { get; set; }
        public ReplacementOrder ReplacementOrder { get; set; } //Preferred Placement Order, not actually used during LCA
        public int ReplacementOrderId { get; set; }
        public KG3xxName() { }
        public KG3xxName(string name) : base(name) { }
        public void Constructor(string name, int replacementOrderId, int id)
        {
            this.Id = id;
            this.Name = name;
            this.ReplacementOrderId = replacementOrderId;
        }
    }
    public class ConstructionTypeName : IsName
    {
        public ConstructionTypeName(string name) : base(name) { }
        public ConstructionTypeName() { }
    }

    public class LayerTypeName : IsName, IHasConstructor<string, bool, int>
    {

        public ICollection<KG3xxOption_LayerTypeName> KG3xx_LayerTypeNames { get; set; }
        public bool Is2Component { get; set; }
        public LayerTypeName() { }
        public LayerTypeName(string name) : base(name) { }
        public void Constructor(string name, bool is2Component, int id)
        {
            this.Name = name;
            this.Is2Component = is2Component;
            this.Id = id;
        }
    }

    public class VariationTarget : IsName
    {
        public VariationTarget() { }
        public VariationTarget(string name) : base(name) { }
    }

    public class EnergyStandardName : IsName
    {
        public EnergyStandardName() { }
        public EnergyStandardName(string name) : base(name) { }
    }
}
