using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DataConverter
{
    public class ConversionHandler
    {
        /***
            Converts input singleModEntries to oekobaudatEntries
            :param singleModEntries: List of all singleModEntries
            :param multiplesKG300: List of KeyValuePairs of Multiples
            :param positionKG300: positions of relevant singleModEntries in the List of all singleModEntries
            :param modulePosition: position of module in the general information of singleModEntries
            :param unitPosition: position of unit in the general information of singleModEntries
            :param valuePosition: position of value in the general information of singleModEntries
            :param UUIDPosition: position of UUID in the general information of singleModEntries
            :param structure: for exporting not ConvertedEntries
            :return: List of converted OekobaudatEntries
        ***/
        public static List<OekobaudatEntry> ConvertSingleModToOekobaudatEntries(List<SingleModEntry> singleModEntries, List<KeyValuePair<int,List<int>>> multiplesKG300, List<int> positionKG300, int modulePosition, int unitPosition, int valuePosition, int UUIDPosition, int categoryPosition, StructureOekobaudat structure)
        {
            var oekobaudatEntries = new List<OekobaudatEntry>();

            //find all indices of entries with correct unit and no duplicates in KG300
            var positionsCorrectUnit = Order.GetEntriesCorrectUnit(singleModEntries, positionKG300, unitPosition, valuePosition, categoryPosition);
            var positionsNoDuplicates = Order.EliminateDuplicates(singleModEntries, positionsCorrectUnit, UUIDPosition);

            //not converted entries
            var positionsNotConverted = positionKG300.Except(positionsCorrectUnit).ToList();
            CsvExportHandler.ExportPositionsMult("NotConvertedEntries", positionsNotConverted, singleModEntries, structure);

            //go through every position and take procedure depending on if multiple or not
            foreach (var pos in positionsNoDuplicates)
            {
                OekobaudatEntry current;
                //adding for multiples
                var mult = multiplesKG300.FirstOrDefault(pair => pair.Key == pos);
                if (mult.Value != null)
                {
                    var correspondingEntries = new List<SingleModEntry>() { singleModEntries[mult.Key] };
                    foreach(var n in mult.Value)
                    {
                        if (positionsCorrectUnit.Contains(n))
                        {
                            correspondingEntries.Add(singleModEntries[n]);
                        }
                        else
                        {
                            //unclear what to do here --> throw error
                            throw new NotImplementedException("This is not implemented");
                        }
                    }
                    current = new OekobaudatEntry(correspondingEntries, modulePosition, unitPosition, valuePosition);
                }

                //adding non-multiples
                else
                {
                    current = new OekobaudatEntry(new List<SingleModEntry>() { singleModEntries[pos] }, modulePosition, unitPosition, valuePosition);
                }
                oekobaudatEntries.Add(current);
            }
            return oekobaudatEntries;
        }

        /***
            Converts the input string into a double
            :param value: string that is to be converted to a double
            :param decimalSeparator: string with decimal separator
            :return: double value
        ***/
        public static double ConvertStringToDouble(string value)
        {
            string decimalSeparator;
            bool containsComma = value.Contains(',');
            bool containsPoint = value.Contains('.');
            if(containsComma && containsPoint)
            {
                throw new FormatException("Value contains Comma and Point");
            }
            else if (containsComma)
            {
                decimalSeparator = ",";
            }
            else if (containsPoint)
            {
                decimalSeparator = ".";
            }
            else //value has neither comma nor point, then it doesn't matter which separator is used
            {
                decimalSeparator = ".";
            }
            NumberFormatInfo provider = new NumberFormatInfo
            {
                NumberDecimalSeparator = decimalSeparator
            };

            if(value != string.Empty)
            {
                return Convert.ToDouble(value, provider);
            }
            else
            {
                return 0.0;
            }
        }
    }
}
