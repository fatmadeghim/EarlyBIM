using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using SharedDBLibrary;

namespace KnowledgeDB
{
    public static class KnowledgeContextExtensions
    // Contains extension methods for KnowledgeContext (Predefined Queries)
    {
        public static List<KG3x0Name> FindKG3x0NamesWhereKg3xxNameIs(this KnowledgeContext context, string name)
        {
            return GenericQueries.FindT1sWhereT2IsName<KG3x0Name, KG3xxName, KG3x0Name_KG3xxName>(context.KG3x0Name_KG3xxNames,
                context.KG3x0Names, context.KG3xxNames, name).ToList();         
        }

        public static List<KG3xxName> FindKG3xxNamesWhereKg3x0NameIs(this KnowledgeContext context, string name)
        {
            return (from t2 in context.KG3xxNames
                    join mTm in context.KG3x0Name_KG3xxNames on t2.Id equals mTm.Id2
                    join t1 in context.KG3x0Names on mTm.Id1 equals t1.Id
                    where t1.Name.Equals(name)
                    orderby mTm.Position
                    select t2).ToList();
        }

        public static List<KG3xxOption> FindKg3xxWhereKg3x0Is(this KnowledgeContext context, KG3x0Option kg3x0)
        {
            {
                return (from mTm in context.KG3x0_KG3xxs
                        join t2 in context.KG3xxOptions on mTm.Id2 equals t2.Id
                        where mTm.Id1 == kg3x0.Id
                        orderby mTm.Position
                        select t2).ToList();
            }
        }

        public static List<LayerTypeName> FindLayerTypeNamesWhereKG3xxIs(this KnowledgeContext context, KG3xxOption kg3xx)
        {
            return (from mTm in context.KG3xx_LayerTypeNames
                    join t2 in context.LayerTypeNames on mTm.Id2 equals t2.Id
                    where mTm.Id1 == kg3xx.Id
                    orderby mTm.Position
                    select t2).ToList();
        }

        public static Unit FindUnitOfLayer(this KnowledgeContext context, Layer layer)
        {
            return (from u in context.Units
                    join obd in context.OekobaudatDatas on u.Id equals obd.UnitId
                    join lay in context.Layers on obd.Id equals lay.OekobaudatDataId
                    where lay.Id == layer.Id
                    select u).FirstOrDefault();
        }

        public static List<Layer> FindLayersOfLayerType(this KnowledgeContext context, StandardLayerType layerType, KG3xxName kg3xxName)
        {
            return (from lay in context.Layers
                    join lay_slt in context.Layer_StandardLayerTypes on lay.Id equals lay_slt.Id1
                    join kg_lay in context.KG3xxName_Layers on lay.Id equals kg_lay.Id2
                    where lay_slt.Id2 == layerType.Id &&
                          kg_lay.Id1 == kg3xxName.Id
                    select lay).ToList();
        }
        
        //Check if there is at least one layer for a given LayerTypeName - Layer combo
        public static bool LayersInLayerType(this KnowledgeContext context, string layerTypeNameString, string kG3xxNameString)
        {
            //Get LayerTypeNameObject
            var layerTypeNameObject = context.LayerTypeNames.Where(ltn => ltn.Name.Equals(layerTypeNameString)).FirstOrDefault();

            //Case 1: LayerType is StandardLayerType (1 Component)
            if (!layerTypeNameObject.Is2Component)
            {
                var layers = (from lay in context.Layers
                              join lay_stn in context.Layer_StandardLayerTypes on lay.Id equals lay_stn.Id1
                              join stn in context.StandardLayerTypes on lay_stn.Id2 equals stn.Id
                              join kgN_lay in context.KG3xxName_Layers on lay.Id equals kgN_lay.Id2
                              join kgN in context.KG3xxNames on kgN_lay.Id1 equals kgN.Id
                              where stn.NameId == layerTypeNameObject.Id &&
                                    kgN.Name.Equals(kG3xxNameString)
                              select lay).ToList();
                if (layers.Any())
                {
                    return true;
                }
                return false;
            }

            //Case 2: LayerType is TwoComponentLayerType
            var tclt = context.TwoComponentLayerTypes.Where(lt => lt.NameId == layerTypeNameObject.Id).FirstOrDefault();
            //Get standardlayertypenames of both components
            var ltn1 = (from ltn in context.LayerTypeNames
                        join slt in context.StandardLayerTypes on ltn.Id equals slt.NameId
                        where slt.Id == tclt.Component1Id
                        select ltn.Name).FirstOrDefault();
            var ltn2 = (from ltn in context.LayerTypeNames
                        join slt in context.StandardLayerTypes on ltn.Id equals slt.NameId
                        where slt.Id == tclt.Component2Id
                        select ltn.Name).FirstOrDefault();

            return (context.LayersInLayerType(ltn1, kG3xxNameString) && context.LayersInLayerType(ltn2, kG3xxNameString));
        }

        //Creates all necessary thickness Range entries and sets them to their default value (useful after updates)
        //Only works when Defaults are kept
        //Use CSV Export and Import of Thicknessranges for complete rebuilding of DB
        //Loses values of non-default thicknessranges!
        public static void RestoreThicknessRanges(this KnowledgeContext context)
        {
            //Clear out already existing thickness ranges
            context.ThicknessRanges.RemoveRange(context.ThicknessRanges);

            //Get all KG3xxOptions
            var kg3xxs = context.KG3xxOptions.ToList();

            foreach(var kg3xx in kg3xxs)
            {
                //Get all layertypenames in KG3xxOption
                var ltnames = (from ltn in context.LayerTypeNames
                               join kg3xx_ltn in context.KG3xx_LayerTypeNames on ltn.Id equals kg3xx_ltn.Id2
                               where kg3xx_ltn.Id1 == kg3xx.Id
                               select ltn).ToList();

                ThicknessRange.BuildThicknessRanges(context.KG3xxNames.Where(kgn => kgn.Id == kg3xx.NameId).FirstOrDefault(), ltnames, context);
            }
        }

        //Creates all necessary CostRange entries and sets them to 0
        //Use csv Import and Export to keep values
        public static void RestoreCostRanges(this KnowledgeContext context)
        {
            //Clear out already existing thickness ranges
            context.CostRanges.RemoveRange(context.CostRanges);

            //Get all KG3xxOptions
            var kg3xxs = context.KG3xxOptions.ToList();

            foreach (var kg3xx in kg3xxs)
            {
                //Get all layertypenames in KG3xxOption
                var ltPackage = (from ltn in context.LayerTypeNames
                               join kg3xx_ltn in context.KG3xx_LayerTypeNames on ltn.Id equals kg3xx_ltn.Id2
                               where kg3xx_ltn.Id1 == kg3xx.Id
                               select new { ltn, kg3xx_ltn.HasExposureQuality }).ToList();

                //Unpack values
                var ltnames = new List<LayerTypeName>();
                var hasExposureQualities = new List<bool>();
                foreach(var pack in ltPackage)
                {
                    ltnames.Add(pack.ltn);
                    hasExposureQualities.Add(pack.HasExposureQuality);
                }

                CostRange.BuildCostRanges(context.KG3xxNames.Where(kgn => kgn.Id == kg3xx.NameId).FirstOrDefault(), ltnames, hasExposureQualities, 
                    context);
            }
        }

        public static List<TempThicknessRangeStorage> StoreNonDefaultThicknessRanges(this KnowledgeContext context)
        {
            var tempThicknessRanges = new List<TempThicknessRangeStorage>();

            //Get Non-Default ThicknessRanges
            var tranges = context.ThicknessRanges.Where(tr => tr.IsDefault == false).ToList();

            foreach(var trange in tranges)
            {
                tempThicknessRanges.Add(new TempThicknessRangeStorage(trange, context));
            }

            return tempThicknessRanges;
        }

        public static void RestoreNonDefaultThicknessRanges(this KnowledgeContext context, List<TempThicknessRangeStorage> tempThicknessRanges)
        {
            foreach(var temptr in tempThicknessRanges)
            {
                temptr.RestoreToDB(context);
            }
        }

        //Export of ThicknessRanges to CSVs
        public static void ExportThicknessesToCSV(this KnowledgeContext context, string filepath)
        {
            File.WriteAllText(filepath + "/DefaultThicknessRanges.csv", context.DefaultThicknessRanges.ToList().getCSVExport(context), Encoding.GetEncoding("ISO-8859-1"));
            File.WriteAllText(filepath + "/ThicknessRanges.csv", context.ThicknessRanges.ToList().getCSVExport(context), Encoding.GetEncoding("ISO-8859-1"));
            File.WriteAllText(filepath + "/TwoComponentUncertainties.csv", context.TwoComponentUncertainties.ToList().getCSVExport(context), Encoding.GetEncoding("ISO-8859-1"));
        }

        public static void ExportCostRangesToCSV(this KnowledgeContext context, string filepath)
        {
            var a = context.CostRanges.ToList().getCSVExport(context);
            File.WriteAllText(filepath + "/CostRanges.csv", context.CostRanges.ToList().getCSVExport(context), Encoding.GetEncoding("ISO-8859-1"));
        }

        public static void ExportBuildingPartsToCSV(this KnowledgeContext context, string filepath)
        {
            File.WriteAllText(filepath + "/VariationParams.csv", context.VariationParams.ToList().getCSVExport(context), Encoding.GetEncoding("ISO-8859-1"));
            File.WriteAllText(filepath + "/KG3x0Options.csv", context.KG3x0Options.ToList().getCSVExport(context), Encoding.GetEncoding("ISO-8859-1"));
            File.WriteAllText(filepath + "/KG3x0_KG3xxs.csv", context.KG3x0_KG3xxs.ToList().getCSVExport(context), Encoding.GetEncoding("ISO-8859-1"));
            File.WriteAllText(filepath + "/KG3xxOptions.csv", context.KG3xxOptions.ToList().getCSVExport(context), Encoding.GetEncoding("ISO-8859-1"));
            File.WriteAllText(filepath + "/KG3xx_LayerTypeNames.csv", context.KG3xx_LayerTypeNames.ToList().getCSVExport(context), Encoding.GetEncoding("ISO-8859-1"));
        }

        public static void ImportThicknessesCSV(this KnowledgeContext context, string filepath)
        {
            //Clear old ThicknessRanges
            context.DefaultThicknessRanges.RemoveRange(context.DefaultThicknessRanges);
            context.ThicknessRanges.RemoveRange(context.ThicknessRanges);
            context.TwoComponentUncertainties.RemoveRange(context.TwoComponentUncertainties);

            //Import from csv
            context.DefaultThicknessRanges.AddRange(CSVHandler.ImportDefaultThicknessRangesCSV(filepath + "/DefaultThicknessRanges.csv", context));
            context.ThicknessRanges.AddRange(CSVHandler.ImportThicknessRangesCSV(filepath + "/ThicknessRanges.csv", context));
            /*
            //Uncomment for debugging
            var i = 0;
            foreach(var range in CSVHandler.ImportThicknessRangesCSV(filepath + "/ThicknessRanges.csv", context))
            {
                context.ThicknessRanges.Add(range);
                i++;
                context.SaveChanges();
            }
            */
            context.TwoComponentUncertainties.AddRange(CSVHandler.ImportTwoCompUncertaintiesCSV(filepath + "/TwoComponentUncertainties.csv", context));
            context.SaveChanges();
        }

        public static void ImportCostRangesCSV(this KnowledgeContext context, string folderpath)
        {
            //Clear old CostRanges
            context.CostRanges.RemoveRange(context.CostRanges);

            //Import CSV
            context.CostRanges.AddRange(CSVHandler.ImportCostRangesCSV(folderpath + "/CostRanges.csv", context));

            context.SaveChanges();
        }

        public static void ImportBuildingPartsCSV(this KnowledgeContext context, string folderpath)
        {
            //Clear old Building Parts
            context.VariationParams.RemoveRange(context.VariationParams);
            context.KG3x0Options.RemoveRange(context.KG3x0Options);
            context.KG3xxOptions.RemoveRange(context.KG3xxOptions);
            context.KG3xx_LayerTypeNames.RemoveRange(context.KG3xx_LayerTypeNames);
            context.KG3x0_KG3xxs.RemoveRange(context.KG3x0_KG3xxs);

            //Create objects with only Ids
            var variationParams = CSVHandler.ImportOnlyIDObjectCSV<VariationParam>(folderpath + "/VariationParams.csv");
            var kg3x0Options = CSVHandler.ImportOnlyIDObjectCSV<KG3x0Option>(folderpath + "/KG3x0Options.csv");
            var kg3xxOptions = CSVHandler.ImportOnlyIDObjectCSV<KG3xxOption>(folderpath + "/KG3xxOptions.csv");
            var kg3xx_LayerTypeNames = CSVHandler.ImportOnlyIDObjectCSV<KG3xxOption_LayerTypeName>(folderpath + "/KG3xx_LayerTypeNames.csv");

            //Restore DB Links
            CSVHandler.RestoreKG3x0OptionLinks(folderpath + "/KG3x0Options.csv", kg3x0Options, context);
            CSVHandler.RestoreKG3xxOptionsLinks(folderpath + "/KG3xxOptions.csv", kg3xxOptions, context);
            CSVHandler.RestoreKG3xxLayerTypeNameLinks(folderpath + "/KG3xx_LayerTypeNames.csv", kg3xx_LayerTypeNames, kg3xxOptions, context);
            CSVHandler.RestoreVariationParamLinks(folderpath + "/VariationParams.csv", variationParams, kg3x0Options, kg3xx_LayerTypeNames, context);
            var kg3x0_kg3xxs = CSVHandler.RestoreKG3x0_KG3xxs(folderpath + "/KG3x0_KG3xxs.csv", kg3x0Options, kg3xxOptions);

            //Add objects to the DB
            context.KG3x0Options.AddRange(kg3x0Options);
            context.KG3xxOptions.AddRange(kg3xxOptions);
            context.KG3x0_KG3xxs.AddRange(kg3x0_kg3xxs);
            context.KG3xx_LayerTypeNames.AddRange(kg3xx_LayerTypeNames);

            context.SaveChanges(); //This is necessary, do not delete!

            context.VariationParams.AddRange(variationParams);

            context.SaveChanges();
        }

        public static void DeleteUnneccessaryKG3xxs(this KnowledgeContext context)
        {
            //Deletes all KG3xxOptions that are not used in a KG3x0Option
            var unneccKG3xxs = (from kg3xx in context.KG3xxOptions
                                where
                                !(context.KG3x0_KG3xxs.Where(kg3x0_kg3xx => kg3x0_kg3xx.Id2 == kg3xx.Id)).Any()
                                select kg3xx).ToList();

            context.KG3xxOptions.RemoveRange(unneccKG3xxs);
            context.SaveChanges();
        }

        public static void DeleteUnneccessaryThicknessRanges(this KnowledgeContext context)
        {
            //Delete all ThicknessRanges / SDefaultthicknessranges / TwoComponentUncertainties that are not needed for a Building Part in the context
            //Does only delete thicknessRanges that are not used in an existing KG3xx -> KG3xx are not automatically deleted when they are unused
            //To fully delete every unneccessary thicknessrange, call DeleteUnneccessaryKG3xxs before calling this function

            //Find KG3xxNameId of KG334 (windows)
            var windowId = context.KG3xxNames.Where(kg3xxN => kg3xxN.Name.Equals("334")).Select(kg3xxN => kg3xxN.Id).FirstOrDefault();

            //ThicknessRanges
            var unneccTRs = (from tr in context.ThicknessRanges
                             where
                             !(from kg3xx in context.KG3xxOptions
                               join kg3xx_ltn in context.KG3xx_LayerTypeNames on kg3xx.Id equals kg3xx_ltn.Id1
                               where kg3xx.NameId == tr.KG3xxNameId && kg3xx_ltn.Id2 == tr.LayerTypeNameId
                               select kg3xx).Any()
                             select tr).ToList();

            //Keep windows
            unneccTRs.RemoveAll(tr => tr.KG3xxNameId == windowId);

            context.ThicknessRanges.RemoveRange(unneccTRs);


            //DefaultthicknessRanges
            var unneccDTRs = (from dtr in context.DefaultThicknessRanges
                              where
                              !(from kg3xx in context.KG3xxOptions
                                join kg3xx_ltn in context.KG3xx_LayerTypeNames on kg3xx.Id equals kg3xx_ltn.Id1
                                where kg3xx.NameId == dtr.KG3xxNameId && kg3xx_ltn.Id2 == dtr.LayerTypeNameId
                                select kg3xx).Any()
                              select dtr).ToList();

            //Keep Windows
            unneccDTRs.RemoveAll(dtr => dtr.KG3xxNameId == windowId);

            context.DefaultThicknessRanges.RemoveRange(unneccDTRs);

            //TwoComponentUncertainties
            var unneccTCUs = (from tcu in context.TwoComponentUncertainties
                             where
                             !(from kg3xx in context.KG3xxOptions
                               join kg3xx_ltn in context.KG3xx_LayerTypeNames on kg3xx.Id equals kg3xx_ltn.Id1
                               where kg3xx.NameId == tcu.KG3xxNameId && kg3xx_ltn.Id2 == tcu.LayerTypeNameId
                               select kg3xx).Any()
                              select tcu).ToList();

            //Keep Windows
            unneccTCUs.RemoveAll(tcu => tcu.KG3xxNameId == windowId);

            context.TwoComponentUncertainties.RemoveRange(unneccTCUs);

            //Save changes
            context.SaveChanges();
        }

    }   

}
