﻿// <auto-generated />
using System;
using DAL.Storage.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(FamilyTreeDbContext))]
    [Migration("20241208191338_ChangeUnion")]
    partial class ChangeUnion
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DAL.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("BirthYear")
                        .HasColumnType("integer");

                    b.Property<int?>("DeathYear")
                        .HasColumnType("integer");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("DAL.Entities.Union", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ChildId")
                        .HasColumnType("integer");

                    b.Property<int>("Partner1Id")
                        .HasColumnType("integer");

                    b.Property<int>("Partner2Id")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Partner1Id");

                    b.HasIndex("Partner2Id");

                    b.ToTable("Unions");
                });

            modelBuilder.Entity("DAL.Entities.Union", b =>
                {
                    b.HasOne("DAL.Entities.Person", null)
                        .WithMany()
                        .HasForeignKey("Partner1Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.Person", null)
                        .WithMany()
                        .HasForeignKey("Partner2Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
