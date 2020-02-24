﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PQ_TiedonLaatuService.Data;

namespace PQ_TiedonLaatuService.Migrations
{
    [DbContext(typeof(PrimusAlertContext))]
    partial class PrimusAlertContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PQ_TiedonLaatuService.Models.Database.AlertReceiver", b =>
                {
                    b.Property<int>("AlertReceiverId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CardNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AlertReceiverId");

                    b.ToTable("AlertReceiver");
                });

            modelBuilder.Entity("PQ_TiedonLaatuService.Models.Database.AlertType", b =>
                {
                    b.Property<int>("AlertTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AlertMsgSubject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AlertMsgText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CardNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsInUse")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QueryString")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AlertTypeId");

                    b.ToTable("AlertType");
                });

            modelBuilder.Entity("PQ_TiedonLaatuService.Models.Database.PrimusAlert", b =>
                {
                    b.Property<int>("PrimusAlertId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AlertReceiverId")
                        .HasColumnType("int");

                    b.Property<int?>("AlertTypeId")
                        .HasColumnType("int");

                    b.Property<string>("CardNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReceiverCardNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SentDate")
                        .HasColumnType("datetime2");

                    b.HasKey("PrimusAlertId");

                    b.HasIndex("AlertReceiverId");

                    b.HasIndex("AlertTypeId");

                    b.ToTable("PrimusAlert");
                });

            modelBuilder.Entity("PQ_TiedonLaatuService.Models.Database.PrimusAlert", b =>
                {
                    b.HasOne("PQ_TiedonLaatuService.Models.Database.AlertReceiver", "AlertReceiver")
                        .WithMany("PrimusAlerts")
                        .HasForeignKey("AlertReceiverId");

                    b.HasOne("PQ_TiedonLaatuService.Models.Database.AlertType", "AlertType")
                        .WithMany("PrimusAlerts")
                        .HasForeignKey("AlertTypeId");
                });
#pragma warning restore 612, 618
        }
    }
}
