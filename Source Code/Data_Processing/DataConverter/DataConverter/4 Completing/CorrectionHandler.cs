using System;
using System.Collections.Generic;
using System.Text;

namespace DataConverter
{
    public class CorrectionHandler
    {
        /***
        Corrects the entries with a reference value != 1 to 1
        :param indicesKG: list of indices for relevant entries in list of singleModEntries
        :param entries: list of all singleModEntries
        :param referenceUnitPosition: position of reference unit in general information of SingleModEntry
        :param referenceValuePosition: position of reference value in general information of SingleModEntry
        :param conversionToKgPosition: position of conversion to kg in general information of SingleModEntry
        ***/
        public static void CorrectReferenceValue(List<int> indicesKG, List<SingleModEntry> entries, int referenceUnitPosition, int referenceValuePosition, int conversionToKgPosition)
        {
            //get all entries that don't have 1 as reference value
            for (int i = 0; i < indicesKG.Count; i++)
            {
                var current = entries[indicesKG[i]];
                var currentValue = current.GeneralInformation[referenceValuePosition];
                if (currentValue != "1")
                {
                    var currentUnit = current.GeneralInformation[referenceUnitPosition];
                    //if kg, set conversionToKg to 1, else set it to empty
                    if (currentUnit == "kg")
                    {
                        double conversionFactor = (1 / ConversionHandler.ConvertStringToDouble(currentValue));
                        current.UpdateIndicators(referenceUnitPosition, referenceValuePosition, conversionToKgPosition, currentUnit, "1", "1", conversionFactor);
                    }
                    else if (currentUnit != "null")
                    {
                        double conversionFactor = (1 / ConversionHandler.ConvertStringToDouble(currentValue));
                        current.UpdateIndicators(referenceUnitPosition, referenceValuePosition, conversionToKgPosition, currentUnit, "1", "", conversionFactor);
                    }
                    current.ChangesToEntry += "Corrected reference value from " + currentValue + currentUnit + " to " + current.GeneralInformation[referenceValuePosition] + currentUnit +" (including indicators), ";
                }
            }
        }

        /***
        Corrects all entries where unit != desired if conversion to correct unit through extra info in oekobaudat possible
        :param indicesKG: list of indices for relevant entries in list of singleModEntries
        :param entries: list of all singleModEntries
        :param categoryPosition: position of category in general information of SingleModEntry
        :param referenceUnitPosition: position of reference unit in general information of SingleModEntry
        :param referenceValuePosition: position of reference value in general information of SingleModEntry
        :param conversionToKgPosition: position of conversion to kg in general information of SingleModEntry
        :param densityPos: position of density in general information of SingleModEntry
        :param thicknessPos: position of thickness in general information of SingleModEntry
        :param areaWeightPos: position of area weight in general information of SingleModEntry
        :param bulkDensityPos: position of bulk density in general information of SingleModEntry
        :param lengthMassPos: position of length mass in general information of SingleModEntry
         ***/
        public static void CorrectUnitThroughConversion(
            List<int> indicesKG, 
            List<SingleModEntry> entries, 
            int categoryPosition, 
            int referenceUnitPosition, 
            int referenceValuePosition, 
            int conversionToKgPosition, 
            int densityPos, 
            int thicknessPos, 
            int areaWeightPos, 
            int bulkDensityPos, 
            int lengthMassPosition)
        {
            //get all entries that don't have a correct unit
            var indicesWrongUnit = Order.GetEntriesWrongUnit(entries, indicesKG, referenceUnitPosition, referenceValuePosition, categoryPosition);
            //for each entry with wrong unit, try to correct it
            for (int i = 0; i < indicesWrongUnit.Count; i++)
            {
                var current = entries[indicesWrongUnit[i]];
                var currentUnit = current.GeneralInformation[referenceUnitPosition];
                var currentValue = current.GeneralInformation[referenceValuePosition];
                
                if (currentValue == "1" && currentUnit != "null")
                {
                    //check for desired unit
                    var desiredUnit = CorrectUnitHandler.GetDesiredUnit(current.GeneralInformation[categoryPosition]);
                    CorrectUnitThroughConversionOne(current, currentUnit, desiredUnit, referenceUnitPosition, referenceValuePosition, conversionToKgPosition, densityPos, thicknessPos, areaWeightPos, bulkDensityPos, lengthMassPosition);
                }
            }
        }

        /***
        Helper to CorrectUnitThroughConversion that corrects one entry where unit != (m3 or null) if conversion to m3 through extra info in oekobaudat possible
        ***/
        public static void CorrectUnitThroughConversionOne(SingleModEntry current, string currentUnit, DesiredUnit desiredUnit, int referenceUnitPosition, int referenceValuePosition, int conversionToKgPosition, int densityPos, int thicknessPos, int areaWeightPos, int bulkDensityPos, int lengthMassPosition)
        {
            double conversionFactor = 0.0;
            string conversionUnit = "";

            if (desiredUnit == DesiredUnit.qm)
            {
                conversionUnit = "qm";
                conversionFactor = ConvertToQm(currentUnit, current, areaWeightPos, lengthMassPosition, conversionToKgPosition, densityPos, thicknessPos);
            }
            else if(desiredUnit == DesiredUnit.m3_or_qm)
            {
                //try converting to qm first and only if that doesn't work, try m3
                conversionUnit = "qm";
                conversionFactor = ConvertToQm(currentUnit, current, areaWeightPos, lengthMassPosition, conversionToKgPosition, densityPos, thicknessPos);
                
                //corresponds to conversionFactor == 0
                if(Math.Abs(conversionFactor) < Double.Epsilon)
                {
                    //try converting to qm first and only if that doesn't work, try m3
                    conversionUnit = "m3";
                    conversionFactor = ConvertToM3(currentUnit, current, areaWeightPos, lengthMassPosition, conversionToKgPosition, densityPos, thicknessPos, bulkDensityPos);
                }
            
            }
            else if (desiredUnit == DesiredUnit.m3)
            {
                conversionUnit = "m3";
                conversionFactor = ConvertToM3(currentUnit, current, areaWeightPos, lengthMassPosition, conversionToKgPosition, densityPos, thicknessPos, bulkDensityPos);
            }

            //if conversion Factor > 0.0
            if (Math.Abs(conversionFactor) > Double.Epsilon)
            {
                current.UpdateIndicators(referenceUnitPosition, referenceValuePosition, conversionToKgPosition, conversionUnit, "1", "", conversionFactor);
                current.ChangesToEntry += "Conversion of unit from " + currentUnit + " to " + conversionUnit + ", ";
            }
        }//CorrectUnitThroughConversionOne

        /***
        Helper Function for conversion to qm
        ***/
        private static double ConvertToQm(string currentUnit, SingleModEntry current, int areaWeightPos, int lengthMassPosition, int conversionToKgPosition, int densityPos, int thicknessPos)
        {
            double conversionFactor = 0.0;

            //double values
            double conversionToKg = ConversionHandler.ConvertStringToDouble(current.GeneralInformation[conversionToKgPosition]);
            double density = ConversionHandler.ConvertStringToDouble(current.GeneralInformation[densityPos]);
            double thickness = ConversionHandler.ConvertStringToDouble(current.GeneralInformation[thicknessPos]);
            double areaWeight = ConversionHandler.ConvertStringToDouble(current.GeneralInformation[areaWeightPos]);
            double lengthMass = ConversionHandler.ConvertStringToDouble(current.GeneralInformation[lengthMassPosition]);

            if (currentUnit == "m")
            {
                //check if area weight and length mass or conversion to kg != empty
                if (current.GeneralInformation[areaWeightPos] != String.Empty && current.GeneralInformation[lengthMassPosition] != String.Empty)
                {
                    conversionFactor = areaWeight / lengthMass;
                }
                else if (current.GeneralInformation[areaWeightPos] != String.Empty && current.GeneralInformation[conversionToKgPosition] != String.Empty)
                {
                    conversionFactor = areaWeight * conversionToKg;
                }
                //lengthMass, density and thickness != empty
                else if (current.GeneralInformation[lengthMassPosition] != String.Empty && current.GeneralInformation[densityPos] != String.Empty && current.GeneralInformation[thicknessPos] != String.Empty)
                {
                    conversionFactor = density * thickness / lengthMass;
                }
            }
            else if (currentUnit == "kg")
            {
                //check if areaWeight != empty
                if (current.GeneralInformation[areaWeightPos] != String.Empty)
                {
                    conversionFactor = areaWeight;
                }
                //if thickness and density != empty
                else if(current.GeneralInformation[thicknessPos] != String.Empty && current.GeneralInformation[densityPos] != String.Empty)
                {
                    conversionFactor = density * thickness;
                }
            }
            else
            {
                throw new NotImplementedException("This is not implemented");
            }
            return conversionFactor;
        }

        /***
        Helper Function for conversion to m3
        ***/
        private static double ConvertToM3(string currentUnit, SingleModEntry current, int areaWeightPos, int lengthMassPosition, int conversionToKgPosition, int densityPos, int thicknessPos, int bulkDensityPos)
        {
            double conversionFactor = 0.0;

            //double values
            double conversionToKg = ConversionHandler.ConvertStringToDouble(current.GeneralInformation[conversionToKgPosition]);
            double density = ConversionHandler.ConvertStringToDouble(current.GeneralInformation[densityPos]);
            double thickness = ConversionHandler.ConvertStringToDouble(current.GeneralInformation[thicknessPos]);
            double areaWeight = ConversionHandler.ConvertStringToDouble(current.GeneralInformation[areaWeightPos]);
            double bulkDensity = ConversionHandler.ConvertStringToDouble(current.GeneralInformation[bulkDensityPos]);
            double lengthMass = ConversionHandler.ConvertStringToDouble(current.GeneralInformation[lengthMassPosition]);

            if (currentUnit == "kg")
            {
                //check if bulk density != empty
                if (current.GeneralInformation[bulkDensityPos] != String.Empty)
                {
                    conversionFactor = bulkDensity;
                }
                //check if density != empty
                else if (current.GeneralInformation[densityPos] != String.Empty)
                {
                    conversionFactor = density;
                }
            }
            else if (currentUnit == "qm")
            {
                //check if thickness != empty
                if (current.GeneralInformation[thicknessPos] != String.Empty)
                {

                    conversionFactor = (1 / thickness);
                }
                else if (current.GeneralInformation[densityPos] != String.Empty && current.GeneralInformation[conversionToKgPosition] != String.Empty)
                {
                    conversionFactor = conversionToKg * density;
                }
                else if (current.GeneralInformation[densityPos] != String.Empty && current.GeneralInformation[areaWeightPos] != String.Empty)
                {
                    conversionFactor = density / areaWeight;
                }
            }
            else if (currentUnit == "m")
            {
                //check if density and length mass or conversion to kg != empty
                if (current.GeneralInformation[densityPos] != String.Empty && current.GeneralInformation[lengthMassPosition] != String.Empty)
                {
                    conversionFactor = density / lengthMass;
                }
                else if (current.GeneralInformation[densityPos] != String.Empty && current.GeneralInformation[conversionToKgPosition] != String.Empty)
                {
                    conversionFactor = density * conversionToKg;
                }
            }
            else
            {
                //no other actions yet
                throw new NotImplementedException("This is not implemented");

            }
            return conversionFactor;
        }
    }
}
