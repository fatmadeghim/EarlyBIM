using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using SharedDBLibrary;

namespace KnowledgeDB
{
    public class KG3x0Option_KG3xxOption : IManyToManyT1HasName<KG3x0Option, KG3xxOption>,
                               IManyToManyT2HasName<KG3x0Option, KG3xxOption>,
                               IHasPosition,
                               IKnowledgeCSVExportable
    {
        public int Id1 { get; set; }
        public KG3x0Option Type1 { get; set; }
        public int Id2 { get; set; }
        public KG3xxOption Type2 { get; set; }
        public int Position { get; set; }

        public KG3x0Option_KG3xxOption() { }
        public KG3x0Option_KG3xxOption(KG3x0Option kG3x0, KG3xxOption kG3xx)
        {
            Id1 = kG3x0.Id;
            Type1 = kG3x0;
            Id2 = kG3xx.Id;
            Type2 = kG3xx;
        }

        public string getT1Name()
        {
            return Type1.getName();
        }

        public string getT2Name()
        {
            return Type2.getName();
        }

        public string getCSVHeadline()
        {
            return "Id1;Id2;Postion";
        }

        public string getCSVLine(KnowledgeContext context)
        {
            return Id1.ToString() + ";" +
                   Id2.ToString() + ";" +
                   Position.ToString();
        }
    }

    public class KG3xxOption_LayerTypeName : IManyToManyT1HasName<KG3xxOption, LayerTypeName>,
                                       IManyToManyT2HasName<KG3xxOption, LayerTypeName>,
                                       IHasID,
                                       IHasPosition,
                                       IKnowledgeCSVExportable
    {
        public int Id { get; set; }
        public int Id1 { get; set; }
        public KG3xxOption Type1 { get; set; }
        public int Id2 { get; set; }
        public LayerTypeName Type2 { get; set; }
        public int Position { get; set; }
        public int AccessOrder { get; set; }
        public bool HasExposureQuality { get; set; }
        public KG3xxOption_LayerTypeName() { }
        public KG3xxOption_LayerTypeName(KG3xxOption kG3xx, LayerTypeName layerTypeName, bool hasExposureQuality)
        {
            Id1 = kG3xx.Id;
            Type1 = kG3xx;
            Id2 = layerTypeName.Id;
            Type2 = layerTypeName;
            HasExposureQuality = hasExposureQuality;
        }

        public string getT1Name()
        {
            return Type1.getName();
        }
        public string getT2Name()
        {
            return Type2.getName();
        }

        public string getCSVHeadline()
        {
            return "Id;Id1;LayerTypeName;Position;AccessOrder";
        }

        public string getCSVLine(KnowledgeContext context)
        {
            return Id.ToString() + ";" +
                   Id1.ToString() + ";" +
                   context.LayerTypeNames.Where(ltn => ltn.Id == Id2).Select(ltn => ltn.Name).FirstOrDefault() + ";" +
                   Position.ToString() + ";" +
                   AccessOrder.ToString();
        }
    }

    public class KG3x0Name_KG3xxName : IManyToManyT1HasName<KG3x0Name, KG3xxName>,
                                       IManyToManyT2HasName<KG3x0Name, KG3xxName>,
                                       IHasPosition, IHasConstructor<int, int, int>
    {
        public int Id1 { get; set; }
        public KG3x0Name Type1 { get; set; }
        public int Id2 { get; set; }
        public KG3xxName Type2 { get; set; }
        public int Position { get; set; }
        public string getT1Name() { return Type1.getName(); }
        public string getT2Name() { return Type2.getName(); }
        public KG3x0Name_KG3xxName() { }
        public void Constructor(int id1, int id2, int position)
        {
            this.Id1 = id1;
            this.Id2 = id2;
            this.Position = position;
        }
    }

    public class Layer_StandardLayerType : IManyToManyT1HasName<Layer, StandardLayerType>,
                                       IManyToManyT2HasName<Layer, StandardLayerType>
    {
        public int Id1 { get; set; }
        public Layer Type1 { get; set; }
        public int Id2 { get; set; }
        public StandardLayerType Type2 { get; set; }
        public string getT1Name() { return Type1.getName(); }
        public string getT2Name() { return Type2.getName(); }
        public Layer_StandardLayerType() { }
        public Layer_StandardLayerType(Layer layer, StandardLayerType standardLayerType)
        {
            Id1 = layer.Id;
            Type1 = layer;
            Id2 = standardLayerType.Id;
            Type2 = standardLayerType;
        }
    }

    public class KG3xxName_Layer : IManyToManyT1HasName<KG3xxName, Layer>,
                                   IManyToManyT2HasName<KG3xxName, Layer>
    {
        public int Id1 { get; set; }
        public KG3xxName Type1 { get; set; }
        public int Id2 { get; set; }
        public Layer Type2 { get; set; }
        public int LifeSpan { get; set; }
        public string getT1Name() { return Type1.getName(); }
        public string getT2Name() { return Type2.getName(); }
        public KG3xxName_Layer() { }
        public KG3xxName_Layer(KG3xxName kG3xxName, Layer layer, int lifeSpan)
        {
            Id1 = kG3xxName.Id;
            Type1 = kG3xxName;
            Id2 = layer.Id;
            Type2 = layer;
            LifeSpan = lifeSpan;
        }
    }

    public class KG3x0Name_EnergyStandardName : IManyToManyT1HasName<KG3x0Name, EnergyStandardName>,
                                                IManyToManyT2HasName<KG3x0Name, EnergyStandardName>,
                                                IHasConstructor<int, int, double>
    {
        public int Id1 { get; set; }
        public KG3x0Name Type1 { get; set; }
        public int Id2 { get; set; }
        public EnergyStandardName Type2 { get; set; }
        public double RequiredU { get; set; }
        public string getT1Name() { return Type1.getName(); }
        public string getT2Name() { return Type2.getName(); }

        public KG3x0Name_EnergyStandardName() { } //Empty Constructor for DB Creation
        public void Constructor(int id1, int id2, double uValue)
        {
            this.Id1 = id1;
            this.Id2 = id2;
            this.RequiredU = uValue;
        }
    }

    public class EnergyStandardName_WindowLayerTypeName : IManyToManyT1HasName<EnergyStandardName, LayerTypeName>,
                                                          IManyToManyT2HasName<EnergyStandardName, LayerTypeName>
    {
        public EnergyStandardName Type1 { get; set; }
        public int Id1 { get; set; }
        public LayerTypeName Type2 { get; set; }
        public int Id2 { get; set; }
        public string getT1Name() { return Type1.Name; }
        public string getT2Name() { return Type2.Name; }
    }
                                    
}