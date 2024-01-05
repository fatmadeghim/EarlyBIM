using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using SharedDBLibrary;

namespace KnowledgeDB
{
    public class VariationParam : IHasForeignName<VariationTarget>, IKnowledgeCSVExportable
    {
        public int Id { get; set; } 
        public int NameId { get; set; }
        public VariationTarget Name { get; set; }
        public int KG3x0OptionId { get; set; }
        public KG3x0Option KG3X0Option { get; set; }
        public int KG3xxOption_LayerTypeNameId { get; set; }
        public KG3xxOption_LayerTypeName KG3xxOption_LayerTypeName { get; set; }

        public VariationParam() { } //Empty Constructor for DB creation

        public string getName()
        {
            return Name.Name;
        }

        public string getCSVHeadline()
        {
            return "Id;VariationTarget;KG3x0OptionId;KG3xxOption_LayerTypeNameId";
        }

        public string getCSVLine(KnowledgeContext context)
        {
            return Id.ToString() + ";" +
                   context.VariationTargets.Where(vt => vt.Id == NameId).Select(vt => vt.Name).FirstOrDefault() + ";" +
                   KG3x0OptionId.ToString() + ";" +
                   KG3xxOption_LayerTypeNameId.ToString();
        }
    }
}
