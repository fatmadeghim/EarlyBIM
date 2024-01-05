using System;
using System.Collections.Generic;
using System.Text;
using KnowledgeDB;

namespace DataConverter
{
    public class DBWritingHandler
    {
        public static void WritingTables(
            StructureOekobaudat structure,
            List<OekobaudatEntry> oekobaudatEntries,
            List<KG3xxName> kG3xxNames,
            //List<Unit> units,
            List<StandardLayerType> standardLayerTypes,
            string filepathDB)
        {
            using (var context = new KnowledgeContext(filepathDB))
            {
                //save non default thickness ranges
                var nonDefaultThicknessRanges = context.StoreNonDefaultThicknessRanges();

                //Clear relevant tables
                context.OekobaudatDatas.Clear();
                context.Units.Clear();
                context.SaveChanges();

                //Fill Units
                var units = GenerateUnits.GenUnits();
                units.ForEach(n => context.Units.Add(n));

                //Fill OekobaudatData
                var data = GenerateOekobaudatData.GenOekobaudat(units, structure, oekobaudatEntries);
                data.ForEach(n => context.OekobaudatDatas.Add(n));

                //Save changes
                context.SaveChanges();

                //Fill Layers
                var layers = GenerateLayers.LayersPlain(oekobaudatEntries, data, structure.NamePos, structure.CategoryPos, structure.UUIDPos, structure.ReferenceUnitPos, structure.ThicknessPos);
                layers.ForEach(n => context.Layers.Add(n));

                //Save changes
                context.SaveChanges();

                //add links
                GenerateLayers.LinksLayers(oekobaudatEntries, layers, structure, standardLayerTypes, kG3xxNames);

                //Save changes
                context.SaveChanges();

                //restore thicknesses
                context.RestoreThicknessRanges();
                context.RestoreNonDefaultThicknessRanges(nonDefaultThicknessRanges);
            }
        }
    }
}
