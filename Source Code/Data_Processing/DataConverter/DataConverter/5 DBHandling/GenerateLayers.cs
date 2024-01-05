using System;
using System.Collections.Generic;
using System.Text;
using KnowledgeDB;
using System.Linq;

namespace DataConverter
{
    public class GenerateLayers
    {
        /***
        This function creates a list of Layers from the given oekobaudatEntries with suiting unit, kg3xxNames and standardLayerTypes
        :param oekobaudatEntries: list of OekobaudatEntries
        :param data: List of all oekobaudatData generated before
        :param namePos: index of name in general information of oekobaudatEntry
        :param catPos: index of category in general information of oekobaudatEntry
        :param UUIDPos: index of UUID in general information of oekobaudatEntry
        :param unitPos: index of unit in general information of oekobaudatEntry
        :param thicknessPos: index of thickness in general information of oekobaudatEntry
        :return: list with all created layers
        ***/
        public static List<Layer> LayersPlain(
            List<OekobaudatEntry> oekobaudatEntries,
            List<OekobaudatData> data,
            int namePos,
            int catPos,
            int UUIDPos,
            int unitPos,
            int thicknessPos)
        {
            //generate Layers for each entry in oekobaudatEntries
            var layers = new List<Layer>();
            for (int i = 0; i < oekobaudatEntries.Count(); i++)
            {
                var current = oekobaudatEntries[i];
                if (current.KGs != null && current.LayerTypes != null && current.IndicatorsA1_A3 != null &&(current.IndicatorsC3 != null || current.IndicatorsC4 != null))
                {
                    var correspData = data.Find(n => n.UUID == current.GeneralInformation[UUIDPos]);
                    var name = current.GeneralInformation[namePos];
                    var UUID = current.GeneralInformation[UUIDPos];
                    var category = current.GeneralInformation[catPos];
                    //lambda is 1/R if unit qm
                    double lambda;
                    double estimatedThickness = 0.0 ;
                    if (current.GeneralInformation[unitPos] == "qm") {
                        lambda = current.ReverseOfR;
                        if(current.EstimatedThickness != null && current.EstimatedThickness != string.Empty)
                        {
                            estimatedThickness = ConversionHandler.ConvertStringToDouble(current.EstimatedThickness);
                        }
                        else if (current.GeneralInformation[thicknessPos] != null && current.GeneralInformation[thicknessPos] != string.Empty)
                        {
                            estimatedThickness = ConversionHandler.ConvertStringToDouble(current.GeneralInformation[thicknessPos]);
                        }
                        else
                        {
                            //throw error!
                        }
                    }
                    else 
                    {
                        lambda = current.ThermalConductivity;
                    }
                    
                    //create layer
                    var layer = new Layer(name, UUID, category, correspData, lambda, estimatedThickness);
                    layers.Add(layer);
                }
                else
                {
                    Console.WriteLine("Couldn't convert entry with UUID " + current.GeneralInformation[0] + " as KGs null or LayerTypes null or A1-A3 null or C3 and C4 null.");
                }
            }
            return layers;
        }//LayersPlain

        /***
        This function adds the links to kg3xxNames and standardLayerTypes for the given layers
        :param oekobaudatEntries: list of OekobaudatEntries
        :param layers: list of Layers
        :param structure: Sture of Oekobaudat (might be substituted with relevant positions)
        :allStandardLayerTypes: List of all StandardLayerTypes that are present in the db
        :allKG3xxNames: List of all KG3xxNames that are present in the db
        ***/
        public static void LinksLayers(
            List<OekobaudatEntry> oekobaudatEntries,
            List<Layer> layers,
            StructureOekobaudat structure,
            List<StandardLayerType> allStandardLayerTypes,
            List<KG3xxName> allKG3xxNames)
        {
            //get positions
            var namePos = structure.NamePos;
            var catPos = structure.CategoryPos;
            var UUIDPos = structure.UUIDPos;

            //add Links
            for (int i = 0; i < layers.Count(); i++)
            {
                var current = layers[i];
                //add link between KG3xx and Layer
                var correspOeEntry = oekobaudatEntries.Find(n => n.GeneralInformation[UUIDPos] == current.UUID);
                if (correspOeEntry != null && correspOeEntry.LayerTypes != null)
                {
                    current.KG3xxName_Layers = new List<KG3xxName_Layer>();
                    foreach (var kg3xx in correspOeEntry.KGs)
                    {
                        var correspondingDBEntry = allKG3xxNames.Find(n => n.Name == kg3xx);
                        var lifeSpan = correspOeEntry.ServiceLife;
                        if (correspondingDBEntry != null)
                        {
                            current.KG3xxName_Layers.Add(new KG3xxName_Layer(correspondingDBEntry,current, lifeSpan));
                        }
                        else
                        {
                            Console.WriteLine("Couldn't find kg " + kg3xx + " in db");
                        }
                    }

                    //add link between Layer and standardLayerType
                    current.Layer_StandardLayerTypes= new List<Layer_StandardLayerType>();
                    foreach (var layerType in correspOeEntry.LayerTypes)
                    {
                        var correspondingDBEntry = allStandardLayerTypes.Find(n => n.Name.Name == layerType);
                        if (correspondingDBEntry != null)
                        {
                            current.Layer_StandardLayerTypes.Add(new Layer_StandardLayerType(current, correspondingDBEntry));
                        }
                        else
                        {
                            Console.WriteLine("Couldn't find StandardLayerType " + layerType + " in db");
                        }
                    }//foreach
                }//if
            }//for
        }//LinksLayers
    }
}
