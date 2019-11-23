﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuaggBotCS2;

namespace QuaggBotCS2.Migrations
{
    [DbContext(typeof(BotContext))]
    [Migration("20191122142005_AddMutes")]
    partial class AddMutes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("QuaggBotCS2.Server", b =>
                {
                    b.Property<int>("ServerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ServerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SettingsJson")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("__ServerSnow")
                        .HasColumnType("bigint");

                    b.HasKey("ServerID");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("QuaggBotCS2.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GuildServerID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Strikes")
                        .HasColumnType("int");

                    b.Property<int>("TotalMutes")
                        .HasColumnType("int");

                    b.Property<long>("__UserSnow")
                        .HasColumnType("bigint");

                    b.HasKey("UserID");

                    b.HasIndex("GuildServerID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("QuaggBotCS2.User", b =>
                {
                    b.HasOne("QuaggBotCS2.Server", "Guild")
                        .WithMany("Users")
                        .HasForeignKey("GuildServerID");
                });
#pragma warning restore 612, 618
        }
    }
}
