using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using SharedDBLibrary;

namespace KnowledgeDB
{
    public class Layer : IHasName, IHasID
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string UUID { get; set; }
        public ICollection<Layer_StandardLayerType> Layer_StandardLayerTypes { get; set; } 
        public ICollection<KG3xxName_Layer> KG3xxName_Layers { get; set; }
        public int OekobaudatDataId { get; set; }
        public OekobaudatData OekobaudatData { get; set; }
        public double Lambda { get; set; }
        public double EstimatedThickness { get; set; } //Thickness used for layers whose GWP is given in m². Since we use a thickness of 1m for calculations,
                                                       //we use this parameter for IFC exports.

        public Layer() { }
        public Layer(string name, string UUID, ICollection<KG3xxName> kG3xxNames, ICollection<StandardLayerType> layerTypes, OekobaudatData data, double lambda, int lifeSpan)
        {
            Name = name;
            this.UUID = UUID;

            KG3xxName_Layers = new List<KG3xxName_Layer>();
            foreach(var entry in kG3xxNames)
            {
                KG3xxName_Layers.Add(new KG3xxName_Layer(entry, this, lifeSpan));
            }

            Layer_StandardLayerTypes = new List<Layer_StandardLayerType>();
            foreach (var entry in layerTypes)
            {
                Layer_StandardLayerTypes.Add(new Layer_StandardLayerType(this, entry));
            }

            OekobaudatData = data;
            Lambda = lambda;
        }
        public Layer(string name, string UUID, string category, OekobaudatData data, double lambda, double estimatedThickness = 0.0)
        {
            Name = name;
            this.UUID = UUID;
            Category = category;
            OekobaudatData = data;
            Lambda = lambda;
            EstimatedThickness = estimatedThickness;
        }
        public string getName()
        {
            return Name;
        }

        public int GetLifeSpan(KnowledgeContext knowledgeContext, KG3xxName kg3xxName)
        {
            var lifeSpan = (from kg3xxname_lay in knowledgeContext.KG3xxName_Layers
                            where kg3xxname_lay.Id1 == kg3xxName.Id &&
                                  kg3xxname_lay.Id2 == Id
                            select kg3xxname_lay.LifeSpan).FirstOrDefault();
            return lifeSpan;
        }
    }
}
