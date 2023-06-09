﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TomatesBuenos.Data;

#nullable disable

namespace TomatesBuenos.Migrations
{
    [DbContext(typeof(TomatesBuenosContext))]
    [Migration("20230524235828_Series-topseries")]
    partial class Seriestopseries
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TomatesBuenos.Models.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AudienceComments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AudienceRating")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AvaliablePlatfomrms")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Clasification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CriticsComments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CriticsRating")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DirectionTeam")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Duration")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Genre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MainActors")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReleaseDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Synopsis")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Movie");
                });

            modelBuilder.Entity("TomatesBuenos.Models.TVshow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AudienceRating")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AvaliablePlatforms")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Creators")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CriticsRating")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Genre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Premiere")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Starring")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Synopsis")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TVshow");
                });
#pragma warning restore 612, 618
        }
    }
}
