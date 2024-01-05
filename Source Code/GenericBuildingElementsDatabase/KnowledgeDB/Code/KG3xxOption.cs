using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;

using SharedDBLibrary;

namespace KnowledgeDB
{
    public class KG3xxOption : IHasForeignName<KG3xxName>, IHasID, IKnowledgeCSVExportable
    {
        public int Id { get; set; }
        public KG3xxName Name { get; set; }
        public int NameId { get; set; }
        public ICollection<KG3x0Option_KG3xxOption> KG3x0_KG3xxs{get; set; }
        public ICollection<KG3xxOption_LayerTypeName> KG3xx_LayerTypeNames { get; set; }
        public ReplacementOrder ReplacementOrder { get; set; } //Used for calculations in the LCA
        public int ReplacementOrderId { get; set; }
        public KG3xxOption() { }
        public KG3xxOption(KG3xxName kG3xxName)
        {
            this.Name = kG3xxName;
        }

        //Pseudo constructor, either creates a new KG3xx, puts it into the context or returns an already existing KG3xx Object from the context with the
        //desired properties.
        public static KG3xxOption BuildKG3xx(KnowledgeContext context, 
                                             string name, 
                                             List<string> layerTypeNames,
                                             List<int> positions, 
                                             List<int> accessibilities,
                                             List<bool> hasExposureQualities,
                                             ReplacementOrder replacementOrder)
        {
            //Get all kg3xxs with same name
            var existing = from kg in context.KG3xxOptions
                           join kg3xxName in context.KG3xxNames on kg.NameId equals kg3xxName.Id
                           where kg3xxName.Name == name
                           select kg;

            if (!existing.Any()) //Empty existing -> KG3xx doesn't exist
            {
                return new KG3xxOption(context, name, layerTypeNames, positions, accessibilities, hasExposureQualities, replacementOrder);
            }
            else 
            {
                var orderedLayerNames = UtilityFunctions.OrderListByPositionList(layerTypeNames, positions);
                var orderedExposureQualities = UtilityFunctions.OrderListByPositionList(hasExposureQualities, positions);

                foreach (var exKg3xx in existing)
                {
                    var exCheckParams = (from mTm in context.KG3xx_LayerTypeNames
                                        join lTName in context.LayerTypeNames on mTm.Id2 equals lTName.Id
                                        where mTm.Id1 == exKg3xx.Id
                                        orderby mTm.Position ascending
                                        select new { lTName.Name, mTm.HasExposureQuality }).ToList();

                    //Unpack check Params
                    var exLayerNames = new List<string>();
                    var exExposureQuals = new List<bool>();
                    foreach(var exCheckParam in exCheckParams)
                    {
                        exLayerNames.Add(exCheckParam.Name);
                        exExposureQuals.Add(exCheckParam.HasExposureQuality);
                    }

                    //Same content and same order -> KG3xx already exists
                    if (exLayerNames.SequenceEqual(orderedLayerNames) && exExposureQuals.SequenceEqual(exExposureQuals)) 
                                    { return context.KG3xxOptions.Where(kg => kg.Id.Equals(exKg3xx.Id)).First(); }
                }
                //No existing KG3xx matches new one
                return new KG3xxOption(context, name, layerTypeNames, positions, accessibilities, hasExposureQualities, replacementOrder);
            }
        }

        //Constructor is private because you should always use BuildKG3xx to ensure the KG3xx you're trying to create doesn't exist yet
        private KG3xxOption(KnowledgeContext context, 
                            string name, 
                            List<string> layerTypeNames, 
                            List<int> positions, 
                            List<int> accessibilities,
                            List<bool> hasExposureQualities,
                            ReplacementOrder replacementOrder)
        {
            this.Name = context.KG3xxNames.Where(kg3xxN => kg3xxN.Name.Equals(name)).First();
            this.ReplacementOrder = replacementOrder;
            var layernames = context.LayerTypeNames.GetNameObjectsFromListOfStrings<LayerTypeName>(layerTypeNames);
            context.KG3xx_LayerTypeNames.LinkT1ToManyT2s(this, layernames, positions, accessibilities, hasExposureQualities);
            context.KG3xxOptions.Add(this);
            context.SaveChanges(); 
        }

        public string getName()
        {
            return Name.Name;
        }

        public static bool ExistsInContext(KnowledgeContext context, string name, List<string> orderedLayerNames)
        {
            //Get all kg3xxs with same name
            var existing = from kg3xx in context.KG3xxOptions
                           join kg3xxName in context.KG3xxNames on kg3xx.Id equals kg3xxName.Id
                           where kg3xxName.Name.Equals(name)
                           select kg3xx;

            if (!existing.Any())
            {
                return false;
            }
            else
            {
                foreach (var kg3xx in existing)
                {
                    var exLayerNames = (from mTm in context.KG3xx_LayerTypeNames
                                        join lTName in context.LayerTypeNames on mTm.Id2 equals lTName.Id
                                        where mTm.Id1 == kg3xx.Id
                                        orderby mTm.Position
                                        select lTName.Name).ToList();
                    if (exLayerNames.Count != orderedLayerNames.Count) { return false; }
                    if (exLayerNames.SequenceEqual(orderedLayerNames)){ return true; }
                }
                return false;
            }
        }

        //CSV Export

        public string getCSVHeadline()
        {
            return "Id;KG3xxName;ReplaceOrder";
        }

        public string getCSVLine(KnowledgeContext context)
        {
            return Id.ToString() + ";" +
                   context.KG3xxNames.Where(kgN => kgN.Id == NameId).Select(kgN => kgN.Name).FirstOrDefault() + ";" +
                   context.ReplacementOrders.Where(rO => rO.Id == ReplacementOrderId).Select(rO => rO.Order).FirstOrDefault();
        }
    }
}
