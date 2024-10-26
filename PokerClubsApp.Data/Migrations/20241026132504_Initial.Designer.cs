﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PokerClubsApp.Data;

#nullable disable

namespace PokerClubsApp.Data.Migrations
{
    [DbContext(typeof(PokerClubsDbContext))]
    [Migration("20241026132504_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PokerClubsApp.Data.Models.Club", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<int>("UnionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UnionId");

                    b.ToTable("Clubs");
                });

            modelBuilder.Entity("PokerClubsApp.Data.Models.GameType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("GamesTypes");
                });

            modelBuilder.Entity("PokerClubsApp.Data.Models.PlayedGame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EndedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("GameTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameTypeId");

                    b.ToTable("PlayedGames");
                });

            modelBuilder.Entity("PokerClubsApp.Data.Models.Player", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountId"));

                    b.Property<int>("ClubId")
                        .HasColumnType("int");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("AccountId");

                    b.HasIndex("ClubId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("PokerClubsApp.Data.Models.PlayerGame", b =>
                {
                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<decimal>("Fee")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<decimal>("Result")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("PlayerId", "GameId");

                    b.HasIndex("GameId");

                    b.ToTable("PlayersGames");
                });

            modelBuilder.Entity("PokerClubsApp.Data.Models.Union", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Unions");
                });

            modelBuilder.Entity("PokerClubsApp.Data.Models.Club", b =>
                {
                    b.HasOne("PokerClubsApp.Data.Models.Union", "Union")
                        .WithMany()
                        .HasForeignKey("UnionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Union");
                });

            modelBuilder.Entity("PokerClubsApp.Data.Models.PlayedGame", b =>
                {
                    b.HasOne("PokerClubsApp.Data.Models.GameType", "GameType")
                        .WithMany()
                        .HasForeignKey("GameTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameType");
                });

            modelBuilder.Entity("PokerClubsApp.Data.Models.Player", b =>
                {
                    b.HasOne("PokerClubsApp.Data.Models.Club", "Club")
                        .WithMany()
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Club");
                });

            modelBuilder.Entity("PokerClubsApp.Data.Models.PlayerGame", b =>
                {
                    b.HasOne("PokerClubsApp.Data.Models.PlayedGame", "PlayedGame")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PokerClubsApp.Data.Models.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlayedGame");

                    b.Navigation("Player");
                });
#pragma warning restore 612, 618
        }
    }
}
