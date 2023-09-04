﻿// <auto-generated />
using System;
using Dashboard.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Dashboard.DataAccess.Migrations
{
    [DbContext(typeof(ProjectContext))]
    [Migration("20230831132110_addedBillingInfoTableandRelationsAccordingly")]
    partial class addedBillingInfoTableandRelationsAccordingly
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Dashboard.Models.Models.BillingInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<int?>("ClientFormId")
                        .HasColumnType("int");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("bit");

                    b.Property<string>("Month")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("Year")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("ClientFormId");

                    b.ToTable("BillingInfo");
                });

            modelBuilder.Entity("Dashboard.Models.Models.ClientForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("AproveDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("BlackBaudApiId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DiscontinueDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Logo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("OneTimeBill")
                        .HasColumnType("float");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SubDomain")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isActive")
                        .HasColumnType("bit");

                    b.Property<bool>("isAproved")
                        .HasColumnType("bit");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("ClientForm");
                });

            modelBuilder.Entity("Dashboard.Models.Models.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ClientFormId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateReolved")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Resolution")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ClientFormId");

                    b.ToTable("Ticket");
                });

            modelBuilder.Entity("Dashboard.Models.Models.BillingInfo", b =>
                {
                    b.HasOne("Dashboard.Models.Models.ClientForm", "ClientForm")
                        .WithMany("BillingInfos")
                        .HasForeignKey("ClientFormId");

                    b.Navigation("ClientForm");
                });

            modelBuilder.Entity("Dashboard.Models.Models.Ticket", b =>
                {
                    b.HasOne("Dashboard.Models.Models.ClientForm", "ClientForm")
                        .WithMany("Tickets")
                        .HasForeignKey("ClientFormId");

                    b.Navigation("ClientForm");
                });

            modelBuilder.Entity("Dashboard.Models.Models.ClientForm", b =>
                {
                    b.Navigation("BillingInfos");

                    b.Navigation("Tickets");
                });
#pragma warning restore 612, 618
        }
    }
}
