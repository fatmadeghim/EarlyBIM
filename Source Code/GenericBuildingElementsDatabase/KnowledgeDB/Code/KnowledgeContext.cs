using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Diagnostics;

using SharedDBLibrary;

namespace KnowledgeDB
{
    public class KnowledgeContext : Context
    {
        public DbSet<KG3x0Option> KG3x0Options { get; set; }
        public DbSet<KG3xxOption> KG3xxOptions { get; set; }
        public DbSet<SurfaceR> SurfaceRs { get; set; }
        public DbSet<VariationParam> VariationParams { get; set; }
        public DbSet<VariationTarget> VariationTargets { get; set; }
        public DbSet<Layer> Layers { get; set; }
        public DbSet<OekobaudatData> OekobaudatDatas { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<DefaultThicknessRange> DefaultThicknessRanges { get; set; }
        public DbSet<ThicknessRange> ThicknessRanges { get; set; }
        public DbSet<TwoComponentUncertainty> TwoComponentUncertainties { get; set; }
        public DbSet<CostRange> CostRanges { get; set; }
        public DbSet<StandardLayerType> StandardLayerTypes { get; set; }
        public DbSet<TwoComponentLayerType> TwoComponentLayerTypes { get; set; }
                
        //Names
        public DbSet<KG3x0Name> KG3x0Names { get; set; }
        public DbSet<KG3xxName> KG3xxNames { get; set; }
        public DbSet<ConstructionTypeName> ConstructionTypeNames { get; set; }
        public DbSet<LayerTypeName> LayerTypeNames { get; set; }
        public DbSet<ReplacementOrder> ReplacementOrders { get; set; }
        public DbSet<EnergyStandardName> EnergyStandardNames { get; set; }

        public DbSet<KG3x0Option_KG3xxOption> KG3x0_KG3xxs { get; set; }
        public DbSet<KG3xxOption_LayerTypeName> KG3xx_LayerTypeNames { get; set; }
        public DbSet<KG3x0Name_KG3xxName> KG3x0Name_KG3xxNames {get; set;}
        public DbSet<Layer_StandardLayerType> Layer_StandardLayerTypes { get; set; }
        public DbSet<KG3xxName_Layer> KG3xxName_Layers { get; set; }
        public DbSet<KG3x0Name_EnergyStandardName> KG3x0Name_EnergyStandardNames { get; set; }
        public DbSet<EnergyStandardName_WindowLayerTypeName> EnergyStandardName_WindowLayerTypeNames { get; set; }

        public KnowledgeContext() //This constructor is used when adding a migration and creating the DB 
        {
            filepath = Path.GetFullPath("BuildingElementsKnowledge.db");
        }

        public KnowledgeContext(string filepath)
        {
            this.filepath = filepath;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //KG3x0_KG3xx
            modelBuilder.Entity<KG3x0Option_KG3xxOption>()
                .HasKey(t1t2 => new { t1t2.Id1, t1t2.Id2, t1t2.Position });

            modelBuilder.Entity<KG3x0Option_KG3xxOption>()
                .HasOne<KG3x0Option>(t1t2 => t1t2.Type1)
                .WithMany(kg => kg.KG3x0_KG3xxs)
                .HasForeignKey(t1t2 => t1t2.Id1)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<KG3x0Option_KG3xxOption>()
                .HasOne<KG3xxOption>(t1t2 => t1t2.Type2)
                .WithMany(lt => lt.KG3x0_KG3xxs)
                .HasForeignKey(t1t2 => t1t2.Id2)
                .OnDelete(DeleteBehavior.Cascade);


            //KG3xx_LayerTypeName
            modelBuilder.Entity<KG3xxOption_LayerTypeName>()
                .HasKey(t1t2 => t1t2.Id);

            modelBuilder.Entity<KG3xxOption_LayerTypeName>()
                .HasOne<KG3xxOption>(t1t2 => t1t2.Type1)
                .WithMany(kg => kg.KG3xx_LayerTypeNames)
                .HasForeignKey(t1t2 => t1t2.Id1)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<KG3xxOption_LayerTypeName>()
                .HasOne<LayerTypeName>(t1t2 => t1t2.Type2)
                .WithMany(lt => lt.KG3xx_LayerTypeNames)
                .HasForeignKey(t1t2 => t1t2.Id2)
                .OnDelete(DeleteBehavior.Cascade);


            //KG3x0Name_KG3xxName
            modelBuilder.Entity<KG3x0Name_KG3xxName>()
                .HasKey(t1t2 => new { t1t2.Id1, t1t2.Id2, t1t2.Position });

            modelBuilder.Entity<KG3x0Name_KG3xxName>()
                .HasOne<KG3x0Name>(t1t2 => t1t2.Type1)
                .WithMany(kg => kg.KG3x0Name_KG3xxNames)
                .HasForeignKey(t1t2 => t1t2.Id1)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<KG3x0Name_KG3xxName>()
                .HasOne<KG3xxName>(t1t2 => t1t2.Type2)
                .WithMany(lt => lt.KG3x0Name_KG3xxNames)
                .HasForeignKey(t1t2 => t1t2.Id2)
                .OnDelete(DeleteBehavior.Cascade);


            //Layer_StandardLayerTypeName
            modelBuilder.Entity<Layer_StandardLayerType>()
                .HasKey(t1t2 => new { t1t2.Id1, t1t2.Id2 });

            modelBuilder.Entity<Layer_StandardLayerType>()
                .HasOne<Layer>(t1t2 => t1t2.Type1)
                .WithMany(l => l.Layer_StandardLayerTypes)
                .HasForeignKey(t1t2 => t1t2.Id1)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Layer_StandardLayerType>()
                .HasOne<StandardLayerType>(t1t2 => t1t2.Type2)
                .WithMany(lt => lt.Layer_StandardLayerTypes)
                .HasForeignKey(t1t2 => t1t2.Id2)
                .OnDelete(DeleteBehavior.Cascade);


            //KG3xxName_Layer
            modelBuilder.Entity<KG3xxName_Layer>()
                .HasKey(t1t2 => new { t1t2.Id1, t1t2.Id2 });

            modelBuilder.Entity<KG3xxName_Layer>()
                .HasOne<KG3xxName>(t1t2 => t1t2.Type1)
                .WithMany(l => l.KG3xxName_Layers)
                .HasForeignKey(t1t2 => t1t2.Id1)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<KG3xxName_Layer>()
                .HasOne<Layer>(t1t2 => t1t2.Type2)
                .WithMany(lt => lt.KG3xxName_Layers)
                .HasForeignKey(t1t2 => t1t2.Id2)
                .OnDelete(DeleteBehavior.Cascade);


            //KG3x0Name_EnergyStandardName
            modelBuilder.Entity<KG3x0Name_EnergyStandardName>()
                .HasKey(t1t2 => new { t1t2.Id1, t1t2.Id2 });

            modelBuilder.Entity<KG3x0Name_EnergyStandardName>()
                .HasOne<KG3x0Name>(t1t2 => t1t2.Type1)
                .WithMany()
                .HasForeignKey(t1t2 => t1t2.Id1)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<KG3x0Name_EnergyStandardName>()
                .HasOne<EnergyStandardName>(t1t2 => t1t2.Type2)
                .WithMany()
                .HasForeignKey(t1t2 => t1t2.Id2)
                .OnDelete(DeleteBehavior.Cascade);


            //DefaultThicknessRange
            modelBuilder.Entity<DefaultThicknessRange>()
                .HasKey(tR => new { tR.KG3xxNameId, tR.LayerTypeNameId});

            modelBuilder.Entity<DefaultThicknessRange>()
                .HasOne<KG3xxName>(tR => tR.KG3xxName)
                .WithMany()
                .HasForeignKey(tR => tR.KG3xxNameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DefaultThicknessRange>()
                .HasOne<LayerTypeName>(tR => tR.LayerTypeName)
                .WithMany()
                .HasForeignKey(tR => tR.LayerTypeNameId)
                .OnDelete(DeleteBehavior.Cascade);


            //ThicknessRange
            modelBuilder.Entity<ThicknessRange>()
                .HasKey(tR => new { tR.KG3xxNameId, tR.LayerTypeNameId, tR.LayerId });

            modelBuilder.Entity<ThicknessRange>()
                .HasOne<KG3xxName>(tR => tR.KG3xxName)
                .WithMany()
                .HasForeignKey(tR => tR.KG3xxNameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ThicknessRange>()
                .HasOne<LayerTypeName>(tR => tR.LayerTypeName)
                .WithMany()
                .HasForeignKey(tR => tR.LayerTypeNameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ThicknessRange>()
                .HasOne<Layer>(tR => tR.Layer)
                .WithMany()
                .HasForeignKey(tR => tR.LayerId)
                .OnDelete(DeleteBehavior.Cascade);


            //TwoComponentUncertainty
            modelBuilder.Entity<TwoComponentUncertainty>()
                .HasKey(tR => new { tR.KG3xxNameId, tR.LayerTypeNameId });

            modelBuilder.Entity<TwoComponentUncertainty>()
                .HasOne<KG3xxName>(tR => tR.KG3xxName)
                .WithMany()
                .HasForeignKey(tR => tR.KG3xxNameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TwoComponentUncertainty>()
                .HasOne<LayerTypeName>(tR => tR.LayerTypeName)
                .WithMany()
                .HasForeignKey(tR => tR.LayerTypeNameId)
                .OnDelete(DeleteBehavior.Cascade);


            //CostRange
            modelBuilder.Entity<CostRange>()
                .HasKey(tR => new { tR.KG3xxNameId, tR.LayerTypeNameId, tR.LayerId });

            modelBuilder.Entity<CostRange>()
                .HasOne<KG3xxName>(tR => tR.KG3xxName)
                .WithMany()
                .HasForeignKey(tR => tR.KG3xxNameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CostRange>()
                .HasOne<LayerTypeName>(tR => tR.LayerTypeName)
                .WithMany()
                .HasForeignKey(tR => tR.LayerTypeNameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CostRange>()
                .HasOne<Layer>(tR => tR.Layer)
                .WithMany()
                .HasForeignKey(tR => tR.LayerId)
                .OnDelete(DeleteBehavior.Cascade);


            //EnergyStandardName_LayerTypeName (Windows)
            modelBuilder.Entity<EnergyStandardName_WindowLayerTypeName>()
                .HasKey(t1t2 => new { t1t2.Id1, t1t2.Id2 });

            modelBuilder.Entity<EnergyStandardName_WindowLayerTypeName>()
                .HasOne<EnergyStandardName>(t1t2 => t1t2.Type1)
                .WithMany()
                .HasForeignKey(t1t2 => t1t2.Id1)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EnergyStandardName_WindowLayerTypeName>()
                .HasOne<LayerTypeName>(t1t2 => t1t2.Type2)
                .WithMany()
                .HasForeignKey(t1t2 => t1t2.Id2)
                .OnDelete(DeleteBehavior.Cascade);


            //One to Many Relations
            //KG3x0Option -> KG3x0Name
            modelBuilder.Entity<KG3x0Option>()
                .HasOne<KG3x0Name>(kg => kg.Name)
                .WithMany()
                .HasForeignKey(kg => kg.NameId)
                .OnDelete(DeleteBehavior.SetNull);

            //KG3x0Option -> ConstructionTypeName
            modelBuilder.Entity<KG3x0Option>()
                .HasOne<ConstructionTypeName>(kg => kg.ConstructionTypeName)
                .WithMany()
                .HasForeignKey(kg => kg.ConstructionTypeNameId)
                .OnDelete(DeleteBehavior.SetNull);

            //VariationParam -> KG3x0Option
            modelBuilder.Entity<VariationParam>()
                .HasOne<KG3x0Option>(vp => vp.KG3X0Option)
                .WithMany()
                .HasForeignKey(vp => vp.KG3x0OptionId)
                .OnDelete(DeleteBehavior.Cascade);

            //VariationParam -> VariationTarget
            modelBuilder.Entity<VariationParam>()
                .HasOne<VariationTarget>(vp => vp.Name)
                .WithMany()
                .HasForeignKey(vp => vp.NameId)
                .OnDelete(DeleteBehavior.SetNull);

            //VariationParam -> KG3xxOption_LayerTypeName
            modelBuilder.Entity<VariationParam>()
                .HasOne<KG3xxOption_LayerTypeName>(vp => vp.KG3xxOption_LayerTypeName)
                .WithMany()
                .HasForeignKey(vp => vp.KG3xxOption_LayerTypeNameId)
                .OnDelete(DeleteBehavior.SetNull);

            //KG3xxOption -> KG3xxName
            modelBuilder.Entity<KG3xxOption>()
                .HasOne<KG3xxName>(kg => kg.Name)
                .WithMany()
                .HasForeignKey(Kg => Kg.NameId)
                .OnDelete(DeleteBehavior.SetNull);

            //KG3xxOption -> ReplacementOrder
            modelBuilder.Entity<KG3xxOption>()
                .HasOne<ReplacementOrder>(kg => kg.ReplacementOrder)
                .WithMany()
                .HasForeignKey(Kg => Kg.ReplacementOrderId)
                .OnDelete(DeleteBehavior.SetNull);

            //KG3x0Name -> SurfaceR
            modelBuilder.Entity<KG3x0Name>()
                .HasOne<SurfaceR>(kgn => kgn.SurfaceR)
                .WithMany()
                .HasForeignKey(kgn => kgn.SurfaceRId)
                .OnDelete(DeleteBehavior.SetNull);

            //OekobaudatData -> Unit
            modelBuilder.Entity<OekobaudatData>()
                .HasOne<Unit>(kgn => kgn.Unit)
                .WithMany()
                .HasForeignKey(kgn => kgn.UnitId)
                .OnDelete(DeleteBehavior.Cascade);
            
            //Layer -> OekobaudatData
            modelBuilder.Entity<Layer>()
               .HasOne<OekobaudatData>(o => o.OekobaudatData)
               .WithMany()
               .HasForeignKey(kgn => kgn.OekobaudatDataId)
               .OnDelete(DeleteBehavior.Cascade);

            //KG3xxName -> ReplacementOrder
            modelBuilder.Entity<KG3xxName>()
                .HasOne<ReplacementOrder>(kgn => kgn.ReplacementOrder)
                .WithMany()
                .HasForeignKey(kgn => kgn.ReplacementOrderId)
                .OnDelete(DeleteBehavior.SetNull);

            //TwoComponentLayerType -> StandardLayerType (Component1)
            modelBuilder.Entity<TwoComponentLayerType>()
                .HasOne<StandardLayerType>(kgn => kgn.Component1)
                .WithMany()
                .HasForeignKey(kgn => kgn.Component1Id)
                .OnDelete(DeleteBehavior.SetNull);

            //TwoComponentLayerType -> StandardLayerType (Component2)
            modelBuilder.Entity<TwoComponentLayerType>()
                .HasOne<StandardLayerType>(kgn => kgn.Component2)
                .WithMany()
                .HasForeignKey(kgn => kgn.Component2Id)
                .OnDelete(DeleteBehavior.SetNull);

            //TwoComponentLayerType -> LayerTypeName
            modelBuilder.Entity<TwoComponentLayerType>()
                .HasOne<LayerTypeName>(kgn => kgn.Name)
                .WithMany()
                .HasForeignKey(kgn => kgn.NameId)
                .OnDelete(DeleteBehavior.Cascade);

            //StandardLayerType -> LayerTypeName
            modelBuilder.Entity<StandardLayerType>()
                .HasOne<LayerTypeName>(kgn => kgn.Name)
                .WithMany()
                .HasForeignKey(kgn => kgn.NameId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class KnowledgeCreationContext : KnowledgeContext
    {
        /*
        This context is meant to be used when creating a new database. Use "add-migration <migrationName> -Context "KnowledgeCreationContext""
        and "Update-Database -Context "KnowledgeCreationContext""
        Separating this from KnowledgeContext is necessary because OnModelCreating (where the initialdata is created) is invoked every time a context
        is initialized.
        */
        protected void CreateInitialData(ModelBuilder modelBuilder)
        {
            {
                //Creates Initial Data when the Database is created
                
                var layerTypeCreator = new LayerTypeCreator("SetupCSV/StandardLayerTypes.csv", "SetupCSV/TwoComponentLayerTypes.csv");
                modelBuilder.Entity<LayerTypeName>().HasData(layerTypeCreator.LayerTypeNames);
                modelBuilder.Entity<StandardLayerType>().HasData(layerTypeCreator.StandardLayerTypes);
                modelBuilder.Entity<TwoComponentLayerType>().HasData(layerTypeCreator.TwoComponentLayerTypes);                
                
                modelBuilder.Entity<VariationTarget>().HasData(CSVHandler.GenerateEntriesStringConstructor<VariationTarget>
                    ("SetupCSV/VariationTargets.csv"));
                modelBuilder.Entity<ConstructionTypeName>().HasData(CSVHandler.GenerateEntriesStringConstructor<ConstructionTypeName>
                    ("SetupCSV/ConstructionTypeNames.csv"));
                modelBuilder.Entity<KG3x0Name>().HasData(CSVHandler.GenerateEntriesKg3x0Names<KG3x0Name>
                    ("SetupCSV/KG3x0Names.csv"));
                modelBuilder.Entity<KG3xxName>().HasData(CSVHandler.GenerateEntriesStringIntConstructor<KG3xxName>
                    ("SetupCSV/KG3xxNames.csv"));
                modelBuilder.Entity<SurfaceR>().HasData(CSVHandler.GenerateEntriesTwoDoubleConstructor<SurfaceR>
                    ("SetupCSV/SurfaceRs.csv"));
                modelBuilder.Entity<KG3x0Name_KG3xxName>().HasData(CSVHandler.GenerateEntriesThreeIntNoIdConstructor<KG3x0Name_KG3xxName>
                    ("SetupCSV/KG3x0Name_KG3xxNames.csv"));
                modelBuilder.Entity<ReplacementOrder>().HasData(CSVHandler.GenerateEntriesStringConstructor<ReplacementOrder>
                    ("SetupCSV/ReplacementOrders.csv"));
                modelBuilder.Entity<EnergyStandardName>().HasData(CSVHandler.GenerateEntriesStringConstructor<EnergyStandardName>
                    ("SetupCSV/EnergyStandardNames.csv"));
                modelBuilder.Entity<KG3x0Name_EnergyStandardName>().HasData(CSVHandler.GenerateEntriesTwoIntDoubleConstructor<KG3x0Name_EnergyStandardName>
                    ("SetupCSV/KG3x0Name_EnergyStandardNames.csv"));
                modelBuilder.Entity<TwoComponentUncertainty>().HasData(layerTypeCreator.CreateTwoComponentUncertainties
                    ("SetupCSV/TwoComponentUncertainties.csv"));
                modelBuilder.Entity<EnergyStandardName_WindowLayerTypeName>().HasData(CSVHandler.GenerateEnergyStandardWindowLayerTypeLinks
                    ("SetupCSV/EnergyStandardName_WindowLayerTypes.csv", layerTypeCreator.LayerTypeNameIds));            
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            CreateInitialData(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }

}
