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
    [Migration("20210606120439_PayCode")]
    partial class PayCode
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Payroll.Domain.Paycycles.PayCode", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("CustomerId", "Code");

                    b.ToTable("PayCodes");
                });

            modelBuilder.Entity("Payroll.Domain.Paycycles.PayInstruction", b =>
                {
                    b.Property<Guid>("InstructionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PayCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PayeeEmployeeNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal?>("UnitQuantity")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("InstructionId");

                    b.HasIndex("PayeeEmployeeNumber");

                    b.ToTable("PayInstruction");
                });

            modelBuilder.Entity("Payroll.Domain.Paycycles.Paycycle", b =>
                {
                    b.Property<Guid>("PaycycleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Payday")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PaycycleId");

                    b.ToTable("Paycycles");
                });

            modelBuilder.Entity("Payroll.Domain.Paycycles.Payee", b =>
                {
                    b.Property<string>("EmployeeNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("PaycycleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("EmployeeNumber");

                    b.HasIndex("PaycycleId");

                    b.ToTable("Payee");
                });

            modelBuilder.Entity("Payroll.Domain.Paycycles.PaymentOptions", b =>
                {
                    b.Property<string>("EmployeeNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AccountHolder")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BranchCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IsoCountryCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SwiftCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EmployeeNumber");

                    b.ToTable("PaymentOptions");
                });

            modelBuilder.Entity("Payroll.Domain.Paycycles.PayInstruction", b =>
                {
                    b.HasOne("Payroll.Domain.Paycycles.Payee", null)
                        .WithMany("PayInstructions")
                        .HasForeignKey("PayeeEmployeeNumber");

                    b.OwnsOne("Payroll.Domain.Paycycles.Money", "TotalAmount", b1 =>
                        {
                            b1.Property<Guid>("PayInstructionInstructionId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("decimal(18,2)");

                            b1.Property<string>("Currency")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("PayInstructionInstructionId");

                            b1.ToTable("PayInstruction");

                            b1.WithOwner()
                                .HasForeignKey("PayInstructionInstructionId");
                        });

                    b.OwnsOne("Payroll.Domain.Paycycles.Money", "UnitAmount", b1 =>
                        {
                            b1.Property<Guid>("PayInstructionInstructionId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("decimal(18,2)");

                            b1.Property<string>("Currency")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("PayInstructionInstructionId");

                            b1.ToTable("PayInstruction");

                            b1.WithOwner()
                                .HasForeignKey("PayInstructionInstructionId");
                        });

                    b.Navigation("TotalAmount");

                    b.Navigation("UnitAmount");
                });

            modelBuilder.Entity("Payroll.Domain.Paycycles.Payee", b =>
                {
                    b.HasOne("Payroll.Domain.Paycycles.Paycycle", null)
                        .WithMany("Payees")
                        .HasForeignKey("PaycycleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Payroll.Domain.Paycycles.PaymentOptions", b =>
                {
                    b.HasOne("Payroll.Domain.Paycycles.Payee", "Payee")
                        .WithOne("PaymentOptions")
                        .HasForeignKey("Payroll.Domain.Paycycles.PaymentOptions", "EmployeeNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Payroll.Domain.Paycycles.Address", "BranchAddress", b1 =>
                        {
                            b1.Property<string>("PaymentOptionsEmployeeNumber")
                                .HasColumnType("nvarchar(450)");

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

                            b1.HasKey("PaymentOptionsEmployeeNumber");

                            b1.ToTable("PaymentOptions");

                            b1.WithOwner()
                                .HasForeignKey("PaymentOptionsEmployeeNumber");
                        });

                    b.Navigation("BranchAddress");

                    b.Navigation("Payee");
                });

            modelBuilder.Entity("Payroll.Domain.Paycycles.Paycycle", b =>
                {
                    b.Navigation("Payees");
                });

            modelBuilder.Entity("Payroll.Domain.Paycycles.Payee", b =>
                {
                    b.Navigation("PayInstructions");

                    b.Navigation("PaymentOptions");
                });
#pragma warning restore 612, 618
        }
    }
}
