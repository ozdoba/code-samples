﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Payroll.Infrastructure.Persistence.Paycycles;

namespace Payroll.Infrastructure.Persistence.Paycycles.Migrations
{
    [DbContext(typeof(PaycyclesContext))]
    [Migration("20210527143613_PayeePaymentOptions")]
    partial class PayeePaymentOptions
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Payroll.Domain.Paycycles.Paycycle", b =>
                {
                    b.Property<Guid>("PaycycleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Payday")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("PaycycleId");

                    b.ToTable("Paycycles");
                });

            modelBuilder.Entity("Payroll.Domain.Paycycles.Payee", b =>
                {
                    b.Property<Guid>("PayeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PaycycleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PayeeId");

                    b.HasIndex("PaycycleId");

                    b.ToTable("Payees");
                });

            modelBuilder.Entity("Payroll.Domain.Paycycles.PaymentOptions", b =>
                {
                    b.Property<Guid>("PaymentOptionsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AccountHolder")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AccountNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BranchCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IsoCountryCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PayeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SwiftCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PaymentOptionsId");

                    b.HasIndex("PayeeId")
                        .IsUnique();

                    b.ToTable("PaymentOptions");
                });

            modelBuilder.Entity("Payroll.Domain.Paycycles.Payee", b =>
                {
                    b.HasOne("Payroll.Domain.Paycycles.Paycycle", "Paycycle")
                        .WithMany()
                        .HasForeignKey("PaycycleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Paycycle");
                });

            modelBuilder.Entity("Payroll.Domain.Paycycles.PaymentOptions", b =>
                {
                    b.HasOne("Payroll.Domain.Paycycles.Payee", "Payee")
                        .WithOne("PaymentOptions")
                        .HasForeignKey("Payroll.Domain.Paycycles.PaymentOptions", "PayeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Payroll.Domain.Paycycles.Address", "BranchAddress", b1 =>
                        {
                            b1.Property<Guid>("PaymentOptionsId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("BuildingNumber")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("City")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("CountryCode")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("PostalCode")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("State")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Street")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("PaymentOptionsId");

                            b1.ToTable("PaymentOptions");

                            b1.WithOwner()
                                .HasForeignKey("PaymentOptionsId");
                        });

                    b.Navigation("BranchAddress");

                    b.Navigation("Payee");
                });

            modelBuilder.Entity("Payroll.Domain.Paycycles.Payee", b =>
                {
                    b.Navigation("PaymentOptions");
                });
#pragma warning restore 612, 618
        }
    }
}
