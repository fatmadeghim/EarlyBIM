using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

using SharedDBLibrary;

namespace KnowledgeDB
{
    public static class CSVHandler
        //Class that handles CSVFiles for initial data creation
    {

        public static List<T> GenerateEntriesStringConstructor<T>(string path) where T: IHasConstructor<string, int>, new()
        {
            var lines = ReadFromCSV(path, true, Encoding.UTF8);
            var createdItems = new List<T>();
            foreach (var line in lines)
            {
                var item = new T();
                item.Constructor(line[1], int.Parse(line[0]));
                createdItems.Add(item);
            }
            return createdItems;
        }

        public static List<T> GenerateEntriesTwoDoubleConstructor<T>(string path) where T : IHasConstructor<double, double, int>, new()
        {
            var lines = ReadFromCSV(path, true, Encoding.UTF8);
            var createdItems = new List<T>();
            foreach (var line in lines)
            {
                var item = new T();
                item.Constructor(Double.Parse(line[1]), Double.Parse(line[2]), int.Parse(line[0]));
                createdItems.Add(item);
            }
            return createdItems;
        }

        public static List<T> GenerateEntriesStringIntConstructor<T>(string path) where T : IHasConstructor<string, int, int>, new()
        {
            var lines = ReadFromCSV(path, true, Encoding.UTF8);
            var createdItems = new List<T>();
            var indexcounter = 1;
            foreach (var line in lines)
            {
                var item = new T();
                item.Constructor(line[1], Int32.Parse(line[2]), int.Parse(line[0]));
                createdItems.Add(item);
                indexcounter++;
            }
            return createdItems;
        }

        public static List<T> GenerateEntriesThreeIntNoIdConstructor<T>(string path) where T : IHasConstructor<int, int, int>, new()
        {
            var lines = ReadFromCSV(path, true, Encoding.UTF8);
            var createdItems = new List<T>();
            var indexcounter = 1;
            foreach (var line in lines)
            {
                var item = new T();
                item.Constructor(Int32.Parse(line[0]), Int32.Parse(line[1]), Int32.Parse(line[2]));
                createdItems.Add(item);
                indexcounter++;
            }
            return createdItems;
        }

        public static List<T> GenerateEntriesStringBoolConstructor<T>(string path) where T : IHasConstructor<string, bool, int>, new()
        {
            var lines = ReadFromCSV(path, true, Encoding.UTF8);
            var createdItems = new List<T>();
            var indexcounter = 1;
            foreach (var line in lines)
            {
                var item = new T();
                item.Constructor(line[1], Boolean.Parse(line[2]), int.Parse(line[0]));
                createdItems.Add(item);
                indexcounter++;
            }
            return createdItems;
        }

        public static List<T> GenerateEntriesTwoIntDoubleConstructor<T>(string path) where T : IHasConstructor<int, int, double>, new()
        {
            var lines = ReadFromCSV(path, true, Encoding.UTF8);
            var createdItems = new List<T>();
            var indexcounter = 1;
            foreach (var line in lines)
            {
                var item = new T();
                item.Constructor(int.Parse(line[0]), int.Parse(line[1]), Double.Parse(line[2]));
                createdItems.Add(item);
                indexcounter++;
            }
            return createdItems;
        }

        public static List<T> GenerateEntriesKg3x0Names<T>(string path) where T : IHasConstructor<string, int, int, string, bool, bool, bool, bool>, new()
        {
            var lines = ReadFromCSV(path, true, Encoding.UTF8);
            var createdItems = new List<T>();
            var indexcounter = 1;
            foreach (var line in lines)
            {
                var item = new T();
                item.Constructor(line[1], int.Parse(line[2]), indexcounter, line[3], bool.Parse(line[4]), bool.Parse(line[5]), bool.Parse(line[6]), bool.Parse(line[7]));
                createdItems.Add(item);
                indexcounter++;
            }
            return createdItems;
        }

        public static List<EnergyStandardName_WindowLayerTypeName> GenerateEnergyStandardWindowLayerTypeLinks(string path, Dictionary<string, int> LayerTypeNameIds)
        {
            var lines = ReadFromCSV(path, true, Encoding.UTF8);
            var createdItems = new List<EnergyStandardName_WindowLayerTypeName>();
            
            foreach(var line in lines)
            {
                List<int> windowIds = new List<int>();
                //Create a list of all LayerTypeNameIds
                for(int i = 1; i<line.Length; i++)
                {
                    windowIds.Add(LayerTypeNameIds[(line[i])]);
                }
                foreach(var id in windowIds)
                {
                    var item = new EnergyStandardName_WindowLayerTypeName()
                    {
                        Id1 = int.Parse(line[0]),
                        Id2 = id
                    };
                    createdItems.Add(item);
                }
            }
            return createdItems;
        }

        public static List<T> GenerateEntriesTwoIntSixDouble<T>(string path) where T: IHasConstructor<int, int, double, double, double, double, double, double>, new()
        {
            {
                var lines = ReadFromCSV(path, true, Encoding.UTF8);
                var createdItems = new List<T>();
                var indexcounter = 1;
                foreach (var line in lines)
                {
                    var item = new T();
                    item.Constructor(int.Parse(line[0]), 
                                     int.Parse(line[1]), 
                                     Double.Parse(line[2]), 
                                     Double.Parse(line[3]), 
                                     Double.Parse(line[4]), 
                                     Double.Parse(line[5]), 
                                     Double.Parse(line[6]),
                                     Double.Parse(line[7]));
                    createdItems.Add(item);
                    indexcounter++;
                }
                return createdItems;
            }
        }

        public static List<DefaultThicknessRange> ImportDefaultThicknessRangesCSV(string path, KnowledgeContext knowledgeContext)
        {
            var lines = ReadFromCSV(path, true, Encoding.GetEncoding("ISO-8859-1"));
            var createdRanges = new List<DefaultThicknessRange>();
            foreach (var line in lines)
            {
                var dtr = new DefaultThicknessRange()
                {
                    KG3xxName = knowledgeContext.KG3xxNames.Where(kgn => kgn.Name.Equals(line[0])).FirstOrDefault(),
                    LayerTypeName = knowledgeContext.LayerTypeNames.Where(ltn => ltn.Name.Equals(line[1])).FirstOrDefault(),
                    ThicknessMin = Double.Parse(line[2]),
                    ThicknessAverage = Double.Parse(line[3]),
                    ThicknessMax = Double.Parse(line[4])
                };
                createdRanges.Add(dtr);
            }
            return createdRanges;
        }

        public static List<ThicknessRange> ImportThicknessRangesCSV(string path, KnowledgeContext knowledgeContext)
        {
            var lines = ReadFromCSV(path, true, Encoding.GetEncoding("ISO-8859-1"));
            var createdRanges = new List<ThicknessRange>();
            foreach (var line in lines)
            {
                var tr = new ThicknessRange()
                {
                    KG3xxName = knowledgeContext.KG3xxNames.Where(kgn => kgn.Name.Equals(line[0])).FirstOrDefault(),
                    LayerTypeName = knowledgeContext.LayerTypeNames.Where(ltn => ltn.Name.Equals(line[1])).FirstOrDefault(),
                    Layer = knowledgeContext.Layers.Where(l => l.UUID.Equals(line[3])).FirstOrDefault(),
                    ThicknessMin = Double.Parse(line[4]),
                    ThicknessAverage = Double.Parse(line[5]),
                    ThicknessMax = Double.Parse(line[6]),
                    IsDefault = Boolean.Parse(line[7])
                };
                //knowledgeContext.ThicknessRanges.Add(tr);
                createdRanges.Add(tr);
            }
            return createdRanges;
        }

        public static List<TwoComponentUncertainty> ImportTwoCompUncertaintiesCSV(string path, KnowledgeContext knowledgeContext)
        {
            var lines = ReadFromCSV(path, true, Encoding.GetEncoding("ISO-8859-1"));
            var createdRanges = new List<TwoComponentUncertainty>();
            foreach (var line in lines)
            {
                var tcu = new TwoComponentUncertainty()
                {
                    KG3xxName = knowledgeContext.KG3xxNames.Where(kgn => kgn.Name.Equals(line[0])).FirstOrDefault(),
                    LayerTypeName = knowledgeContext.LayerTypeNames.Where(ltn => ltn.Name.Equals(line[1])).FirstOrDefault(),
                    ThicknessMin = Double.Parse(line[2]),
                    ThicknessAverage = Double.Parse(line[3]),
                    ThicknessMax = Double.Parse(line[4]),
                    Component2PercentageMin = Double.Parse(line[5]),
                    Component2PercentageAverage = Double.Parse(line[6]),
                    Component2PercentageMax = Double.Parse(line[7])
                };
                createdRanges.Add(tcu);
            }
            return createdRanges;
        }
        
        public static List<CostRange> ImportCostRangesCSV(string path, KnowledgeContext knowledgeContext)
        {
            var lines = ReadFromCSV(path, true, Encoding.GetEncoding("ISO-8859-1"));
            var createdRanges = new List<CostRange>();
            foreach (var line in lines)
            {
                var cr = new CostRange()
                {
                    KG3xxName = knowledgeContext.KG3xxNames.Where(kgn => kgn.Name.Equals(line[0])).FirstOrDefault(),
                    LayerTypeName = knowledgeContext.LayerTypeNames.Where(ltn => ltn.Name.Equals(line[1])).FirstOrDefault(),
                    Layer = knowledgeContext.Layers.Where(lay => lay.UUID.Equals(line[3])).FirstOrDefault(),
                    HasExposureQuality = bool.Parse(line[4]),
                    CostMin = double.Parse(line[5]),
                    CostAvg = double.Parse(line[6]),
                    CostMax = double.Parse(line[7])
                };
                createdRanges.Add(cr);
            }
            return createdRanges;
        }
        
        public static List<T> ImportOnlyIDObjectCSV<T>(string path) where T: class, IHasID, new()
        {
            var lines = ReadFromCSV(path, true, Encoding.GetEncoding("ISO-8859-1"));
            var createdObjects = new List<T>();
            foreach (var line in lines)
            {
                var obj = new T()
                {
                    Id = int.Parse(line[0])
                };
                createdObjects.Add(obj);
            }
            return createdObjects;
        }

        public static void RestoreVariationParamLinks(string path, 
                                                      List<VariationParam> varParams, 
                                                      List<KG3x0Option> kg3x0Options, 
                                                      List<KG3xxOption_LayerTypeName> kg3xx_ltns,
                                                      KnowledgeContext context)
        {
            var lines = ReadFromCSV(path, true, Encoding.GetEncoding("ISO-8859-1"));
            for(int i = 0; i < lines.Count; i++)
            {
                varParams[i].Name = context.VariationTargets.Where(vt => vt.Name.Equals(lines[i][1])).FirstOrDefault();
                varParams[i].KG3X0Option = kg3x0Options.Where(kg3x0 => kg3x0.Id == int.Parse(lines[i][2])).FirstOrDefault();
                varParams[i].KG3xxOption_LayerTypeName = kg3xx_ltns.Where(kg_ltn => kg_ltn.Id == int.Parse(lines[i][3])).FirstOrDefault();
            }
        }

        public static void RestoreKG3x0OptionLinks(string path, List<KG3x0Option> kg3x0Options, KnowledgeContext context)
        {
            var lines = ReadFromCSV(path, true, Encoding.GetEncoding("ISO-8859-1"));
            for (int i = 0; i < lines.Count; i++)
            {
                kg3x0Options[i].Name = context.KG3x0Names.Where(kgn => kgn.Name.Equals(lines[i][1])).FirstOrDefault();
                kg3x0Options[i].ConstructionTypeName = context.ConstructionTypeNames.Where(ctn => ctn.Name.Equals(lines[i][2])).FirstOrDefault();
            }
        }

        public static void RestoreKG3xxOptionsLinks(string path, List<KG3xxOption> kg3xxOptions, KnowledgeContext context)
        {
            var lines = ReadFromCSV(path, true, Encoding.GetEncoding("ISO-8859-1"));
            for (int i = 0; i < lines.Count; i++)
            {
                kg3xxOptions[i].Name = context.KG3xxNames.Where(kgn => kgn.Name.Equals(lines[i][1])).FirstOrDefault();
                kg3xxOptions[i].ReplacementOrder = context.ReplacementOrders.Where(rO => rO.Order.Equals(lines[i][2])).FirstOrDefault();
            }
        }

        public static void RestoreKG3xxLayerTypeNameLinks(string path, 
                                                          List<KG3xxOption_LayerTypeName> kg3xx_ltns,
                                                          List<KG3xxOption> kg3xxs,
                                                          KnowledgeContext context)
        {
            var lines = ReadFromCSV(path, true, Encoding.GetEncoding("ISO-8859-1"));
            for (int i = 0; i < lines.Count; i++)
            {
                kg3xx_ltns[i].Type1 = kg3xxs.Where(kgn => kgn.Id == int.Parse(lines[i][1])).FirstOrDefault();
                kg3xx_ltns[i].Type2 = context.LayerTypeNames.Where(ltn => ltn.Name.Equals(lines[i][2])).FirstOrDefault();
                kg3xx_ltns[i].Position = int.Parse(lines[i][3]);
                kg3xx_ltns[i].AccessOrder = int.Parse(lines[i][4]);
            }
        }

        public static List<KG3x0Option_KG3xxOption> RestoreKG3x0_KG3xxs(string path,
                                                                 List<KG3x0Option> kg3x0s,
                                                                 List<KG3xxOption> kg3xxs)
        {
            var lines = ReadFromCSV(path, true, Encoding.GetEncoding("ISO-8859-1"));
            var kg3x0s_kg3xxs = new List<KG3x0Option_KG3xxOption>();
            for (int i = 0; i < lines.Count; i++)
            {
                var kg3x0_kg3xx = new KG3x0Option_KG3xxOption()
                {
                    Type1 = kg3x0s.Where(kg3x0 => kg3x0.Id == int.Parse(lines[i][0])).FirstOrDefault(),
                    Type2 = kg3xxs.Where(kg3xx => kg3xx.Id == int.Parse(lines[i][1])).FirstOrDefault(),
                    Position = int.Parse(lines[i][2])
                };
                kg3x0s_kg3xxs.Add(kg3x0_kg3xx);
            }
            return kg3x0s_kg3xxs;
        }

        public static List<string[]> ReadFromCSV(string path, bool skipFirstLine, Encoding encoding, char splitter = ';')
        {
            var list = File.ReadAllLines(path, encoding).Skip(Convert.ToInt32(skipFirstLine));
            List<string[]> result = new List<string[]>();
            foreach (var line in list)
            {
                result.Add(line.Split(splitter));

            }
            return result;
        }
    }

    //Layertypes need their own class as they are dependent on each other and therefore have to be created together
    public class LayerTypeCreator
    {
        public List<StandardLayerType> StandardLayerTypes { get; set; }
        public List<TwoComponentLayerType> TwoComponentLayerTypes { get; set; }
        public List<LayerTypeName> LayerTypeNames { get; set; }
        public Dictionary<string, int> LayerTypeNameIds { get; set; }

        public LayerTypeCreator(string filepathStandard, string filepathTwoComponent)
        {
            StandardLayerTypes = new List<StandardLayerType>();
            TwoComponentLayerTypes = new List<TwoComponentLayerType>();
            LayerTypeNames = new List<LayerTypeName>();
            LayerTypeNameIds = new Dictionary<string, int>();

            var standardlines = CSVHandler.ReadFromCSV(filepathStandard, true, Encoding.UTF8);
            var indexcounter = 1;

            foreach (var line in standardlines)
            {
                var layertypeName = new LayerTypeName();
                layertypeName.Constructor(line[1], false, int.Parse(line[0]));
                LayerTypeNames.Add(layertypeName);
                var standardLayerType = new StandardLayerType();
                standardLayerType.Constructor(int.Parse(line[0]), int.Parse(line[0]));
                StandardLayerTypes.Add(standardLayerType);
                LayerTypeNameIds.Add(layertypeName.Name, indexcounter);
                indexcounter++;
            }

            var twoComponentLines = CSVHandler.ReadFromCSV(filepathTwoComponent, true, Encoding.UTF8);


            foreach (var line in twoComponentLines)
            {
                var layertypeName = new LayerTypeName();
                layertypeName.Constructor(line[0], true, indexcounter);
                LayerTypeNames.Add(layertypeName);
                var twoComponentLayertype = new TwoComponentLayerType();
                twoComponentLayertype.Constructor(indexcounter, LayerTypeNameIds[line[1]], LayerTypeNameIds[line[2]], TwoComponentLayerTypes.Count+1, bool.Parse(line[3]), 
                                                                bool.Parse(line[4]), double.Parse(line[5]), bool.Parse(line[6]));
                TwoComponentLayerTypes.Add(twoComponentLayertype);
                LayerTypeNameIds.Add(layertypeName.Name, indexcounter);
                indexcounter++;
            }
        }

        public List<TwoComponentUncertainty> CreateTwoComponentUncertainties(string path)
        {
            var lines = CSVHandler.ReadFromCSV(path, true, Encoding.GetEncoding("ISO-8859-1"));
            var createdRanges = new List<TwoComponentUncertainty>();
            foreach (var line in lines)
            {
                var tcu = new TwoComponentUncertainty()
                {
                    KG3xxNameId = int.Parse(line[0]),
                    LayerTypeNameId = LayerTypeNameIds[line[1]],
                    ThicknessMin = Double.Parse(line[2]),
                    ThicknessAverage = Double.Parse(line[3]),
                    ThicknessMax = Double.Parse(line[4]),
                    Component2PercentageMin = Double.Parse(line[5]),
                    Component2PercentageAverage = Double.Parse(line[6]),
                    Component2PercentageMax = Double.Parse(line[7])
                };
                createdRanges.Add(tcu);
            }
            return createdRanges;
        }
    }
}
