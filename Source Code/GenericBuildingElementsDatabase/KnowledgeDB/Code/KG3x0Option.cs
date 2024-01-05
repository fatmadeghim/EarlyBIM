using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

using SharedDBLibrary;

namespace KnowledgeDB
{
    public class KG3x0Option : IHasForeignName<KG3x0Name>, IHasID, IKnowledgeCSVExportable
    {
        public int Id { get; set; }
        public KG3x0Name Name{ get; set; }
        public int NameId { get; set; }
        public ConstructionTypeName ConstructionTypeName { get; set; }
        public int ConstructionTypeNameId { get; set; }
        public ICollection<KG3x0Option_KG3xxOption> KG3x0_KG3xxs { get; set; }
        public KG3x0Option()
        { }
        public string getName()
        {
            return Name.getName();
        }
        public string getCSVHeadline()
        {
            return "Id;KG3x0Name;ConstructionTypeName";
        }

        public string getCSVLine(KnowledgeContext context)
        {
            return Id.ToString() + ";" +
                   context.KG3x0Names.Where(kgN => kgN.Id == NameId).Select(kgN => kgN.Name).FirstOrDefault() + ";" +
                   context.ConstructionTypeNames.Where(ctN => ctN.Id == ConstructionTypeNameId).Select(ctN => ctN.Name).FirstOrDefault();
        }
    }
}
