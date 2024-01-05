using System;
using System.Collections.Generic;
using System.Text;
using KnowledgeDB;

namespace DataConverter
{
    class OverviewKGLayerTypeMapping
    {
        /***
            Exports all KG3xxNames and the present possible layer types for each of them
            :param kg3xxNames: list of all KG3xxNames
            :param oekobaudatEntries: list of all oekobaudat entries
        ***/
        public static void ExportKGs(List<KG3xxName> kg3xxNames, List<OekobaudatEntry> oekobaudatEntries)
        {
            var header = "KG3xx;Corresponding LayerTypes";
            var body = new List<string>();
            foreach(var kg3xx in kg3xxNames)
            {
                var layerTypes = "";
                for (int i = 0; i < oekobaudatEntries.Count; i++)
                {
                    var current = oekobaudatEntries[i];
                    if (current.KGs.Contains(kg3xx.Name))
                    {
                        foreach(var layType in current.LayerTypes)
                        {
                            if (!layerTypes.Contains(layType))
                            {
                                layerTypes+=layType+", ";
                            }
                        }
                        
                    }
                }
                body.Add(kg3xx.Name + ";" + layerTypes);
            }
            CsvExportHandler.Export(CsvExportHandler.SetStorageLocation("KG-LayerTypeMaps.csv"), header, body);
        }

        /***
            Exports all LayerTypes and the present possible kg3xxNames for each of them
            :param standardLayerTypes: list of all StandardLayerTypes
            :param oekobaudatEntries: list of all oekobaudat entries
        ***/
        public static void ExportLayers(List<StandardLayerType> standardLayerTypes, List<OekobaudatEntry> oekobaudatEntries)
        {
            var header = "LayerType;Corresponding KG3xxs";
            var body = new List<string>();
            foreach (var layerType in standardLayerTypes)
            {
                var kg3xxs = "";
                for (int i = 0; i < oekobaudatEntries.Count; i++)
                {
                    var current = oekobaudatEntries[i];
                    if (current.LayerTypes.Contains(layerType.Name.Name))
                    {
                        foreach (var kg in current.KGs)
                        {
                            if (!kg3xxs.Contains(kg))
                            {
                                kg3xxs += kg + ", ";
                            }
                        }

                    }
                }
                body.Add(layerType.Name.Name + ";" + kg3xxs);
            }
            CsvExportHandler.Export(CsvExportHandler.SetStorageLocation("LayerType-KgMaps.csv"), header, body);
        }
    }
}
