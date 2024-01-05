using System;
using System.Collections.Generic;
using System.Text;

namespace KnowledgeDB
{
    public interface IKnowledgeCSVExportable
    {
        public string getCSVHeadline();
        public string getCSVLine(KnowledgeContext KnowledgeContext);
    }

    public static class IKnowledgeCSVExportableExtension
    {
        public static string getCSVExport<T>(this List<T> exportables, KnowledgeContext knowledgeContext) where T: IKnowledgeCSVExportable
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(exportables[0].getCSVHeadline());
            foreach (var exportable in exportables)
            {
                try
                {
                    stringBuilder.AppendLine(exportable.getCSVLine(knowledgeContext));
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            return stringBuilder.ToString();
        }
    }
}
