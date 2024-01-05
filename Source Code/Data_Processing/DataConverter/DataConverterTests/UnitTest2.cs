using NUnit.Framework;
using System.Collections.Generic;
using DataConverter;

namespace DataConverterTests
{
    class OekobaudatEntryTests
    {
        OekobaudatEntry entry1;
        OekobaudatEntry entry2;
        List<string> generalInformation1;
        List<string> generalInformation2;
        StructureOekobaudat structure;
        int UUIDPos;
        int referenceFluxValuePos;
        int referenceFluxUnitPos;
        int conversionToKgPos;
        int densityPos;
        int thicknessPos;
        int areaWeightPos;
        int catPos;
        string pathMappingCategories;


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
                "A1-A3 existing", //15
                "C3 existing", //16
                "C4 existing", //17
                "D existing" //18
            });

            UUIDPos = structure.UUIDPos;
            referenceFluxValuePos = structure.ReferenceValuePos;
            referenceFluxUnitPos = structure.ReferenceUnitPos;
            conversionToKgPos = structure.FindIndex("Umrechungsfaktor auf 1kg");
            densityPos = structure.FindIndex("Rohdichte (kg/m3)");
            thicknessPos = structure.FindIndex("Schichtdicke (m)");
            areaWeightPos = structure.FindIndex("Flaechengewicht (kg/m2)");
            catPos = structure.CategoryPos;
            

            generalInformation1 = new List<string>()
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
            };

            generalInformation2 = new List<string>()
            {
                "9f6fd0a4-491d-4a39-874f-6f78cae6444c", //0
                "00.04.000",//1 
                "Komponenten von Fenstern und Vorhangfassaden / Füllungen / Transparente Füllungen", //2
                "average dataset", //3
                "1", //4
                "m3", //5
                "c452761e-9dfe-257a-9b97-1d707e76c11f", //6
                "Transparente Bauelemente - Rodeca GmbH - Lichtbauelement 30 mm", //7
                "", //8
                "200", //9
                "0.01", //10
                "", //11
                "", //12
                "0,00635", //13
            };

            entry1 = new OekobaudatEntry()
            {
                IndicatorsA1_A3 = new List<double>() { 0, 1, 1000 },
                GeneralInformation = generalInformation1
            };

            entry2 = new OekobaudatEntry()
            {
                IndicatorsA1_A3 = new List<double>() { 2, 500, 100 },
                GeneralInformation = generalInformation2
            };
            string pathMappingCategories = "E:/Tanja/Documents/Uni/Master Bauingenieurwesen/Hiwi/dfg-projekt-git/C# Data Tanja/CSV files/MappingKategorien-LayerType20210311.csv";

        }


        [Test]
        public void TestMapOekobaudatCategory()
        {
            //setup
            var generalInformation3 = new List<string>()
            {
                "9f6fd0a4-491d-4a39-874f-6f78cae6444c", //0
                "00.04.000",//1 
                "Holz / Holzwerkstoffe / Holzfaserplatten", //2
                "average dataset", //3
                "1", //4
                "m3", //5
                "c452761e-9dfe-257a-9b97-1d707e76c11f", //6
                "Transparente Bauelemente - Rodeca GmbH - Lichtbauelement 30 mm", //7
                "", //8
                "200", //9
                "0.01", //10
                "", //11
                "", //12
                "0,00635", //13
            };
            var entry3 = new OekobaudatEntry()
            {
                GeneralInformation = generalInformation3,
                IndicatorsA1_A3 = new List<double>() { 3, 300, 1 }
            };

            var oekobaudatEntries = new List<OekobaudatEntry>() { entry1, entry2, entry3 };
            var categoryMaps = CsvImportHandler.ReadCategories(pathMappingCategories);

            var expected1 = new OekobaudatEntry()
            {
                IndicatorsA1_A3 = new List<double>() { 0, 1, 1000 },
                GeneralInformation = generalInformation1,
                LayerTypes = new List<string>() {"insulation hard" },
                KGs = new List<string>() { "324", "335B", "336", "345", "353", "354", "363", "364" }
            };

            var expected2 = new OekobaudatEntry()
            {
                IndicatorsA1_A3 = new List<double>() { 2, 500, 100 },
                GeneralInformation = generalInformation2,
                LayerTypes = new List<string>() { "transparent"},
                KGs = new List<string>() { "334", "344", "362" }
            };

            //run
            MappingHandler.MapOekobaudatEntryToCategoryMap(ref oekobaudatEntries, categoryMaps, catPos);

            //assert
            Assert.IsNotNull(entry1.LayerTypes);
            Assert.AreEqual(expected1.LayerTypes, entry1.LayerTypes);
            Assert.AreEqual(expected1.KGs, entry1.KGs);
            Assert.AreEqual(expected2.LayerTypes, entry2.LayerTypes);
            Assert.AreEqual(expected2.KGs, entry2.KGs);
            Assert.IsNull(entry3.KGs);
            Assert.IsNull(entry3.LayerTypes);
        }

        [Test]
        public void TestCompleting() 
        {
            //setup
            var generalInformationTest1 = new List<string>()
            {
                "c8d19b6f-e0e7-422d-b704-ff44770df984", //0
                "Großformatige Elemente aus Leichtbeton",//1 
                "Mineralische Baustoffe / Steine und Elemente / Leichtbeton", //2
                "test", //3
                "1", //4
                "m3", //5
                "test", //6
                "Großformatige Elemente aus Leichtbeton", //7
                "", //8
                "200", //9
                "0.01", //10
                "", //11
                "", //12
                "0,00635", //13
                "A1-A3" //Modul = 14
            };

            var generalInformationTest2 = new List<string>()
            {
                "b7cacb37-7945-4518-be5a-bf7df7edf5c2", //0
                "Bauschutt",//1 
                "End of Life / Generisch / Bauschutt", //2
                "test", //3
                "1", //4
                "kg", //5
                "test", //6
                "Bauschutt", //7
                "", //8
                "200", //9
                "0.01", //10
                "", //11
                "", //12
                "0,00635", //13
                "C4" //Modul = 14
            };
            
            string pathBaustoffkonfiguration = "E:/Tanja/Documents/Uni/Master Bauingenieurwesen/Hiwi/dfg-projekt-git/C# Data Tanja/CSV files/Baustoffkonfigurationen_OBD_2020_II.csv";
            var singleEntry1 = new SingleModEntry(generalInformationTest1, new List<double>() { 1 });
            var singleEntry2 = new SingleModEntry(generalInformationTest2, new List<double>() { 1 });
            var singleEntry2m3 = new SingleModEntry(singleEntry2);
            singleEntry2m3.GeneralInformation[referenceFluxUnitPos] = "m3";
            var allSingleModEntries = new List<SingleModEntry>() { singleEntry1, singleEntry2 };
            var oekobaudatentry = new OekobaudatEntry(new List<SingleModEntry>() { singleEntry1 }, 14, referenceFluxUnitPos, referenceFluxValuePos);

            var expected = new OekobaudatEntry(new List<SingleModEntry>() { singleEntry1, singleEntry2m3 }, 14, referenceFluxUnitPos, referenceFluxValuePos);
            //run --> todo: not correct as no general area weight position
            CompletingHandlerOekobaudatEntry.CompleteOekobaudatEntries(pathBaustoffkonfiguration, new List<OekobaudatEntry>() { oekobaudatentry }, UUIDPos, allSingleModEntries, new List<int>() { 0, 1 }, referenceFluxUnitPos, densityPos, thicknessPos,-1);

            //assert
            Assert.AreEqual(expected.IndicatorsC4, oekobaudatentry.IndicatorsC4);
        }

    }
}
