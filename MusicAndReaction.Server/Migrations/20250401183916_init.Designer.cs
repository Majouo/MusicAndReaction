﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicAndReaction.Server.Database;

#nullable disable

namespace MusicAndReaction.Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250401183916_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.3");

            modelBuilder.Entity("MusicAndReaction.Server.Database.UserReaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("AttemptDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("ReactionTime")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TrackId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("UserReactions");
                });
#pragma warning restore 612, 618
        }
    }
}
