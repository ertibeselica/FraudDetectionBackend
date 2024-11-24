﻿// <auto-generated />
using System;
using FraudDetection.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FraudDetection.Data.Migrations
{
    [DbContext(typeof(FraudDetectionDbContext))]
    [Migration("20241124114552_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FraudDetection.Models.AnomalyLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Decision")
                        .HasColumnType("boolean");

                    b.Property<double>("Score")
                        .HasColumnType("double precision");

                    b.Property<int>("TransactionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TransactionId");

                    b.ToTable("AnomalyLogs");
                });

            modelBuilder.Entity("FraudDetection.Models.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<string>("Device")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool?>("IsFraud")
                        .HasColumnType("boolean");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("FraudDetection.Models.AnomalyLog", b =>
                {
                    b.HasOne("FraudDetection.Models.Transaction", "Transaction")
                        .WithMany("AnomalyLogs")
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("FraudDetection.Models.Transaction", b =>
                {
                    b.Navigation("AnomalyLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
