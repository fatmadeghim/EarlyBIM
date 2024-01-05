using NUnit.Framework;
using System.Collections.Generic;
using DataConverter;

namespace DataConverterTests
{
    public class SingleModEntryTests
    {
        SingleModEntry entry;
        SingleModEntry expected;
        StructureOekobaudat structure;
        int UUIDPos;
        int referenceFluxValuePos;
        int referenceFluxUnitPos;
        int conversionToKgPos;
        int densityPos;
        int thicknessPos;
        int areaWeightPos;
        int catPos;
        int modPos;

        [SetUp]
        public void Setup()
        {
            structure = new StructureOekobaudat(new List<string>()
            {
                "UUID", //0
                "Version",//1 
                "Kategorie", //2
                "Typ", //3
                "Bezugsgroesse", //4
                "Bezugseinheit", //5
                "Referenzfluss-UUID", //6
                "Referenzfluss-Name", //7
                "Flaechengewicht (kg/m2)", //8
                "Rohdichte (kg/m3)", //9
                "Schichtdicke (m)", //10
                "Ergiebigkeit (m2)", //11
                "Laengengewicht (kg/m)", //12
                "Umrechungsfaktor auf 1kg", //13
                "Modul", //14
                "GWP", //15
                "ODP", //15
                "PENRE" //16
            });


            UUIDPos = structure.UUIDPos;
            referenceFluxValuePos = structure.ReferenceValuePos;
            referenceFluxUnitPos = structure.ReferenceUnitPos;
            conversionToKgPos = structure.FindIndex("Umrechungsfaktor auf 1kg");
            densityPos = structure.FindIndex("Rohdichte (kg/m3)");
            thicknessPos = structure.FindIndex("Schichtdicke (m)");
            areaWeightPos = structure.FindIndex("Flaechengewicht (kg/m2)");
            catPos = structure.CategoryPos;
            modPos = structure.ModulePos;

            var generalInformation1 = new List<string>()
            {
                "d601d54e-a2eb-42bb-b32b-c59d1b2332a9", //0
                "00.04.000",//1 
                "Dämmstoffe / Holzfasern / Holzfaserdämmplatte", //2
                "average dataset", //3
                "1", //4
                "m3", //5
                "c452761e-9dfe-257a-9b97-1d707e76c11f", //6
                "1 m³ Holzfaserdämmstoff", //7
                "", //8
                "200", //9
                "0.01", //10
                "", //11
                "", //12
                "0,00635", //13
                "A3" //14

            };
            var indicators1 = new List<double>()
            {
                1,
                1000,
                0
            };

            var generalInformation2 = new List<string>()
            {
                "d601d54e-a2eb-42bb-b32b-c59d1b2332a9", //0
                "00.04.000",//1 
                "Dämmstoffe / Holzfasern / Holzfaserdämmplatte", //2
                "average dataset", //3
                "1", //4
                "m3", //5
                "c452761e-9dfe-257a-9b97-1d707e76c11f", //6
                "1 m³ Holzfaserdämmstoff", //7
                "", //8
                "200", //9
                "0.01", //10
                "", //11
                "", //12
                "0,00635", //13
                "A3" //14

            };
            var indicators2 = new List<double>()
            {
                1,
                1000,
                0
            };

            entry = new SingleModEntry(generalInformation1, indicators1);
            expected = new SingleModEntry(generalInformation2, indicators2);
        }

        [Test]
        public void TestFilterEmptyEntries()
        {
            //setup
            var entry2 = new SingleModEntry(entry)
            {
                Indicators = new List<double>() { 0, 0, 0 }
            };
            var entry3 = new SingleModEntry(entry) 
            { 
                Indicators = new List<double>() { 0, 0, 0.00000001 } 
            };
            var entries = new List<SingleModEntry>(){ entry, entry, entry2, entry3};
            var expected = new List<SingleModEntry>() { entry, entry, entry3 };

            //run
            FilterHandler.FilterEmptyEntries(ref entries);

            //assert
            Assert.AreEqual(expected, entries);
        }

        [Test]
        public void TestSortKG300()
        {
            //setup
            var entry2 = new SingleModEntry(entry) { };
            entry2.GeneralInformation[catPos] = "Sonstige / Energieträger - Bereitstellung frei Verbraucher / Strom";
            var entry3 = new SingleModEntry(entry) { };
            entry3.GeneralInformation[modPos] = "B6";
            var entry4 = new SingleModEntry(entry) { };
            entry4.GeneralInformation[modPos] = "B7";
            var entry5 = new SingleModEntry(entry) { };
            entry5.GeneralInformation[catPos] = "Gussteile/test/test";

            var entries = new List<SingleModEntry>() { entry, entry, entry2, entry3 , entry4, entry5};
            var expected = new List<int>() { 0, 1};

            //run
            var positionList = SortingHandler.SortKG300(entries, modPos, 7, catPos,structure);

            //assert
            Assert.AreEqual(expected, positionList);
        }

        [Test]
        public void TestFindMultiplesKeyValue()
        {
            //setup
            var entry1 = new DataConverter.SingleModEntry(new List<string>() { "material1", "test", "A1-3" }, 2);
            var entry2 = new DataConverter.SingleModEntry(new List<string>() { "material1", "test", "C3" }, 2);
            var entry3 = new DataConverter.SingleModEntry(new List<string>() { "material2", "test", "A1-3" }, 2);
            var entry4 = new DataConverter.SingleModEntry(new List<string>() { "material3", "test", "A1-3" }, 2);
            var entry5 = new DataConverter.SingleModEntry(new List<string>() { "material3", "test", "C4" }, 2);
            var entry6 = new DataConverter.SingleModEntry(new List<string>() { "material4", "test", "C4" }, 2);
            var list = new List<DataConverter.SingleModEntry>() { entry1, entry2, entry3, entry4, entry5 };
            var list2 = new List<SingleModEntry>() { entry1, entry2, entry3, entry4, entry5, entry6 };
            var expected = new List<KeyValuePair<int, List<int>>>();
            var listMaterial1 = new KeyValuePair<int, List<int>>(0, new List<int>() { 1 });
            var listMaterial3 = new KeyValuePair<int, List<int>>(3, new List<int>() { 4 });
            expected.Add(listMaterial1);
            expected.Add(listMaterial3);
            //run
            var actual = DataConverter.MultipleHandler.FindMultiples(new List<int>() { 0, 1, 2, 3, 4 }, 0, list);
            var actual2 = DataConverter.MultipleHandler.FindMultiples(new List<int>() { 0, 2, 3 }, 0, list);
            var actual3 = MultipleHandler.FindMultiples(new List<int>() { 0, 1, 2, 3, 4 }, 0, list2);
            //assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(new List<KeyValuePair<int, List<int>>>(), actual2);
            Assert.AreEqual(expected, actual3);
        }

        [Test]
        public void TestUpdateIndicator()
        {
            //Structure of entries:
            // UUID, reference value, reference unit, conversion to kg + 3 random indicator values
            var entry1 = new DataConverter.SingleModEntry(new List<string>() { "eaec8e5d-f70a-4993-b3a6-814607f5e7b3", "1", "t", "0.001" }, new List<double>() { 1000, 2000, 3000 });
            var expected = new DataConverter.SingleModEntry(new List<string>() { "eaec8e5d-f70a-4993-b3a6-814607f5e7b3", "1", "kg" , "1"}, new List<double>() { 1, 2, 3});
            entry1.UpdateIndicators(2, 1, 3,"kg", "1", "1", 0.001);
            Assert.AreEqual(expected.ToCSVString(), entry1.ToCSVString());
        }

        [Test]
        public void TestCorrectReferenceValue()
        {
            entry.GeneralInformation[referenceFluxValuePos] = "1000";
            var entryExpected = expected;
            entryExpected.GeneralInformation[conversionToKgPos] = "";
            entryExpected.Indicators = new List<double>() { 0.001, 1, 0 };
            DataConverter.CorrectionHandler.CorrectReferenceValue(new List<int>() { 0 }, new List<DataConverter.SingleModEntry>() { entry }, referenceFluxUnitPos, referenceFluxValuePos, conversionToKgPos);
            Assert.AreEqual(entryExpected.ToCSVString(), entry.ToCSVString());
        }

        [Test]
        public void TestCorrectUnitThroughConversionKg()
        {
             
            //for kg
            entry.GeneralInformation[referenceFluxUnitPos] = "kg";
            var entryExpected = expected;
            entryExpected.GeneralInformation[referenceFluxUnitPos] = "m3";
            entryExpected.GeneralInformation[referenceFluxValuePos] = "1";
            entryExpected.GeneralInformation[conversionToKgPos] = "";
            entryExpected.Indicators = new List<double>() { 200, 200000, 0 };
            //DataConverter.CorrectionHandler.CorrectUnitThroughConversion(new List<int>() { 0 }, new List<DataConverter.SingleModEntry>() { entry },categoryPosition, referenceFluxUnitPos, referenceFluxValuePos, conversionToKgPos, densityPos, thicknessPos, areaWeightPos,bulkDensityPos,lengthMassPosition);

            Assert.AreEqual(entryExpected.ToCSVString(), entry.ToCSVString());
            
        }

        [Test]
        public void TestCorrectUnitThroughConversionQm()
        {
            //for qm
            entry.GeneralInformation[referenceFluxUnitPos] = "qm";
            var entryExpected = expected;
            entryExpected.GeneralInformation[referenceFluxUnitPos] = "m3";
            entryExpected.GeneralInformation[referenceFluxValuePos] = "1";
            entryExpected.GeneralInformation[conversionToKgPos] = "";
            entryExpected.Indicators = new List<double>() { 100, 100000, 0 };
            //DataConverter.CorrectionHandler.CorrectUnitThroughConversion(new List<int>() { 0 }, new List<DataConverter.SingleModEntry>() { entry }, referenceFluxUnitPos, referenceFluxValuePos, conversionToKgPos, densityPos, thicknessPos, areaWeightPos);
            Assert.AreEqual(entryExpected.ToCSVString(), entry.ToCSVString());
        }

        [Test]
        public void TestFilterUnapplicable()
        {
            //setup
            var map1 = new CategoryMap("Dämmstoffe / Holzfasern / Holzfaserdämmplatte", false, false);
            var map2 = new CategoryMap("Dämmstoffe / Holzfasern / Holzfaserdämmplatte", false, true);
            var map3 = new CategoryMap("Dämmstoffe / Holzfasern / Holzfaserdämmplatte", true, true);

            var positions1 = new List<int>() { 0 };
            var positions2 = new List<int>() { 0 };
            var positions3 = new List<int>() { 0 };

            var expected1 = new List<int>();
            var expected23 = new List<int>() { 0 };

            //run
            //FilterHandler.FilterUnapplicableEntries(new List<CategoryMap>() { map1 }, ref positions1, new List<SingleModEntry>() { entry }, catPos, uuidPosition, new StructureOekobaudat(new List<string>()));
            //FilterHandler.FilterUnapplicableEntries(new List<CategoryMap>() { map2 }, ref positions2, new List<SingleModEntry>() { entry }, catPos, new StructureOekobaudat(new List<string>()));
            //FilterHandler.FilterUnapplicableEntries(new List<CategoryMap>() { map3 }, ref positions3, new List<SingleModEntry>() { entry }, catPos, new StructureOekobaudat(new List<string>()));

            //assert
            Assert.AreEqual(expected1, positions1);
            Assert.AreEqual(expected23, positions2);
            Assert.AreEqual(expected23, positions3);
        }

        [Test]
        public void TestSingleModEntryCopyConstructor()
        {
            //run
            var actual = new SingleModEntry(entry);
            var actual2 = new SingleModEntry(entry);
            actual2.GeneralInformation[modPos] = "Test";

            //assert
            Assert.AreEqual(entry.ToCSVString(), actual.ToCSVString());
            Assert.AreNotEqual(entry.ToCSVString(), actual2.ToCSVString());
            Assert.AreNotSame(entry, actual);
        }

        [Test]
        public void TestConstructorOekobaudatEntry()
        {
            //setup
            //setup
            var entry1_A1 = new SingleModEntry(entry);
            var entry1_A2 = new SingleModEntry(entry);
            var entry1_A3 = new SingleModEntry(entry);
            var entry1_D = new SingleModEntry(entry);
            var entry2_A13 = new SingleModEntry(entry);
            var entry2_C3 = new SingleModEntry(entry);
            var entry3_C4 = new SingleModEntry(entry);
            var entry4_A13 = new SingleModEntry(entry);

            entry1_A1.GeneralInformation[modPos] = "A1";
            entry1_A2.GeneralInformation[modPos] = "A2";
            entry1_A3.GeneralInformation[modPos] = "A3";
            entry1_D.GeneralInformation[modPos] = "D";
            entry2_A13.GeneralInformation[modPos] = "A1-A3";
            entry2_C3.GeneralInformation[modPos] = "C3";
            entry3_C4.GeneralInformation[modPos] = "C4";
            entry4_A13.GeneralInformation[modPos] = "A1-A3";

            var expected1 = new OekobaudatEntry
            {
                GeneralInformation = entry1_A1.GeneralInformation.GetRange(0, entry1_A1.GeneralInformation.Count - 1),
                IndicatorsD = entry1_D.Indicators,
                IndicatorsA1_A3 = new List<double>()
            };
            for (int i = 0; i < entry1_A1.Indicators.Count; i++)
            {
                expected1.IndicatorsA1_A3.Add(entry1_A1.Indicators[i] + entry1_A2.Indicators[i] + entry1_A3.Indicators[i]);
            }
            var expected2 = new OekobaudatEntry
            {
                GeneralInformation = entry2_A13.GeneralInformation.GetRange(0, entry2_A13.GeneralInformation.Count - 1),
                IndicatorsA1_A3 = entry2_A13.Indicators,
                IndicatorsC3 = entry2_C3.Indicators
            };
            var expected3 = new OekobaudatEntry
            {
                GeneralInformation = entry3_C4.GeneralInformation.GetRange(0, entry3_C4.GeneralInformation.Count - 1),
                IndicatorsC4 = entry3_C4.Indicators
            };
            var expected4 = new OekobaudatEntry
            {
                GeneralInformation = entry4_A13.GeneralInformation.GetRange(0, entry4_A13.GeneralInformation.Count - 1),
                IndicatorsA1_A3 = entry4_A13.Indicators
            };

            //run
            var actual1 = new OekobaudatEntry(new List<SingleModEntry>() { entry1_A1, entry1_A2, entry1_A3, entry1_D }, modPos, referenceFluxUnitPos, referenceFluxValuePos);
            var actual2 = new OekobaudatEntry(new List<SingleModEntry>() { entry2_A13, entry2_C3 }, modPos, referenceFluxUnitPos, referenceFluxValuePos);
            var actual3 = new OekobaudatEntry(new List<SingleModEntry>() { entry3_C4 }, modPos, referenceFluxUnitPos, referenceFluxValuePos);
            var actual4 = new OekobaudatEntry(new List<SingleModEntry>() { entry4_A13 }, modPos, referenceFluxUnitPos, referenceFluxValuePos);

            //assert
            Assert.AreEqual(expected1.ToCSVString(), actual1.ToCSVString());
            Assert.AreEqual(expected2.ToCSVString(), actual2.ToCSVString());
            Assert.AreEqual(expected3.ToCSVString(), actual3.ToCSVString());
            Assert.AreEqual(expected4.ToCSVString(), actual4.ToCSVString());
            Assert.AreEqual(3, actual1.IndicatorsA1_A3[0]);
            Assert.AreEqual(3000, actual1.IndicatorsA1_A3[1]);
            Assert.AreEqual(0, actual1.IndicatorsA1_A3[2]);
        }

        [Test]
        public void TestConversionSingleToOekobaudatEntry()
        {
            //setup
            var entry1_A1 = new SingleModEntry(entry);
            var entry1_A2 = new SingleModEntry(entry);
            var entry1_A3 = new SingleModEntry(entry);
            var entry1_D = new SingleModEntry(entry);
            var entry2_A13 = new SingleModEntry(entry);
            var entry2_C3 = new SingleModEntry(entry);
            var entry3_C4 = new SingleModEntry(entry);
            var entry4_A13 = new SingleModEntry(entry);

            entry2_A13.GeneralInformation[UUIDPos] = "entry2";
            entry2_C3.GeneralInformation[UUIDPos] = "entry2";
            entry3_C4.GeneralInformation[UUIDPos] = "entry3";
            entry4_A13.GeneralInformation[UUIDPos] = "entry4";


            entry1_A1.GeneralInformation[modPos] = "A1";
            entry1_A2.GeneralInformation[modPos] = "A2";
            entry1_A3.GeneralInformation[modPos] = "A3";
            entry1_D.GeneralInformation[modPos] = "D";
            entry2_A13.GeneralInformation[modPos] = "A1-A3";
            entry2_C3.GeneralInformation[modPos] = "C3";
            entry3_C4.GeneralInformation[modPos] = "C4";
            entry4_A13.GeneralInformation[modPos] = "A1-A3";


            var entries = new List<SingleModEntry>() { entry1_A1, entry1_A2, entry1_A3, entry1_D, entry2_A13, entry2_C3, entry3_C4, entry4_A13};
            var positions = new List<int>() { 0, 1, 2, 3,4,5,6,7 };
            var multiples = MultipleHandler.FindMultiples(positions, UUIDPos, entries);

            var expected1 = new OekobaudatEntry(new List<SingleModEntry>() { entry1_A1, entry1_A2, entry1_A3, entry1_D }, modPos, referenceFluxUnitPos, referenceFluxValuePos);
            var expected2 = new OekobaudatEntry(new List<SingleModEntry>() { entry2_A13, entry2_C3 }, modPos, referenceFluxUnitPos, referenceFluxValuePos);
            var expected3 = new OekobaudatEntry(new List<SingleModEntry>() { entry3_C4 }, modPos, referenceFluxUnitPos, referenceFluxValuePos);
            var expected4 = new OekobaudatEntry(new List<SingleModEntry>() { entry4_A13 }, modPos, referenceFluxUnitPos, referenceFluxValuePos);

            var expected = new List<OekobaudatEntry>() { expected1, expected2, expected3, expected4};

            //run
            var actual = ConversionHandler.ConvertSingleModToOekobaudatEntries(entries, multiples, positions, modPos, referenceFluxUnitPos, referenceFluxValuePos, UUIDPos, catPos,structure);

            //assert
            Assert.AreEqual(expected.Count, actual.Count);
            Assert.AreEqual(expected[0].IndicatorsA1_A3, actual[0].IndicatorsA1_A3);
            Assert.AreEqual(expected[0].ToCSVString(), actual[0].ToCSVString());
            Assert.AreEqual(expected[1].ToCSVString(), actual[1].ToCSVString());
            Assert.AreEqual(expected[2].ToCSVString(), actual[2].ToCSVString());
            Assert.AreEqual(expected[3].ToCSVString(), actual[3].ToCSVString());
        }

        [Test]
        public void TestCSVExportMultNonConsec()
        {
            //setup
            var entry2 = new SingleModEntry(entry);
            var entry3 = new SingleModEntry(entry);
            entry3.GeneralInformation[UUIDPos] = "UUIDtest";
            entry3.Indicators = new List<double>() { 3, 1, 2 };
            entry3.GeneralInformation[1] = "AnotherVersion";

            var line1 = string.Join(Constants.strSeparator, entry.GeneralInformation) +";" + string.Join(Constants.strSeparator, entry.Indicators) + ";" + entry.GeneralInformation[modPos] + ";" + string.Join(Constants.strSeparator, entry2.Indicators);
            var line2 = string.Join(Constants.strSeparator, entry3.GeneralInformation) + ";" + string.Join(Constants.strSeparator, entry3.Indicators);
            var expected = new List<string>() { line1, line2 };
            var positionlist = new List<int>() { 0, 1, 2 };
            var entries = new List<SingleModEntry>() { entry, entry2, entry3 };
            var multKV = MultipleHandler.FindMultiples(positionlist, UUIDPos, entries);

            //run
            var actual3 = CsvExportHandler.ExportPositionsKVMult("Test", positionlist, entries, multKV, structure);

            //assert
            Assert.AreEqual(expected, actual3);
        }
        /*
        [Test]
        public void TestCSVExportGenMultNonConsec()
        {
            //setup
            var entry2 = new SingleModEntry(entry);
            var entry3 = new SingleModEntry(entry);
            entry3.GeneralInformation[UUIDPos] = "UUIDtest";
            entry3.Indicators = new List<double>() { 3, 1, 2 };
            entry3.GeneralInformation[1] = "AnotherVersion";

            var line1 = string.Join(Constants.strSeparator, entry.GeneralInformation.GetRange(0,entry.GeneralInformation.Count-1));
            var line2 = string.Join(Constants.strSeparator, entry3.GeneralInformation.GetRange(0, entry3.GeneralInformation.Count - 1));
            var expected = new List<string>() { line1, line2 };


            //run
            var actual = CsvExportHandler.ExportPositionGeneralInfoMult("test", new List<int>() { 0, 1, 2 }, new List<SingleModEntry>() { entry, entry2, entry3 }, structure);

            //assert
            Assert.AreEqual(expected, actual);
        }
        */
        [Test]
        public void TestEndOfLifePositions()
        {
            //setup
            var entry2 = new SingleModEntry(entry);
            entry2.GeneralInformation[catPos] = "End of Life/Generisch/Kunststoffe";
            var entry3 = new SingleModEntry(entry);
            entry3.GeneralInformation[catPos] = "Endoflife/test"; //should not be recognized as end of life
            var list1 = new List<int>() { 0, 1 };
            var list2 = new List<int>() { 0, 1 ,2};
            var list3 = new List<int>() { 0, 1, 2, 3, 4, 5 };
            var entriesAll = new List<SingleModEntry>() { entry, entry2 , entry3, entry, entry, entry};
            var expected = new List<int>() { 1 };
            var expectedlist1updated = new List<int>() { 0 };
            var expectedlist2updated = new List<int>() { 0,2 };
            var expectedlist3updated = new List<int>() { 0, 2, 3, 4, 5 };

            //run
            SortingHandler.RemoveEndOfLife(entriesAll, list1, catPos);
            SortingHandler.RemoveEndOfLife(entriesAll, list2, catPos);
            SortingHandler.RemoveEndOfLife(entriesAll, list3, catPos);

            //assert
            Assert.AreEqual(expectedlist1updated, list1);
            Assert.AreEqual(expectedlist2updated, list2);
            Assert.AreEqual(expectedlist3updated, list3);
        }
    }
}