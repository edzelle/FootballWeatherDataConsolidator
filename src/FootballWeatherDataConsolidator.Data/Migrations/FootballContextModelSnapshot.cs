﻿// <auto-generated />
using System;
using FootballWeatherDataConsolidator.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FootballWeatherDataConsolidator.Data.Migrations
{
    [DbContext(typeof(FootballContext))]
    partial class FootballContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.5");

            modelBuilder.Entity("FootballWeatherDataConsolidator.Data.Entites.GameEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AwayTeam")
                        .HasColumnType("TEXT");

                    b.Property<int>("AwayTeamScore")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GMTOffset")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("GameDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("GameSite")
                        .HasColumnType("TEXT");

                    b.Property<string>("HomeTeam")
                        .HasColumnType("TEXT");

                    b.Property<int>("HomeTeamScore")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Season")
                        .HasColumnType("INTEGER");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("Week")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("GameEntities");
                });
#pragma warning restore 612, 618
        }
    }
}
