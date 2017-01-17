using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ModelsLibrary.Models;

namespace BusinessCalendar.Migrations
{
    [DbContext(typeof(BusinessCalendarContext))]
    [Migration("20161222074247_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("ModelsLibrary.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AdressId");

                    b.Property<DateTimeOffset>("Date");

                    b.Property<string>("Description");

                    b.Property<TimeSpan>("FinishDate");

                    b.Property<int>("PersonId");

                    b.Property<int>("RemindingId");

                    b.Property<TimeSpan>("StartDate");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("AdressId");

                    b.HasIndex("PersonId");

                    b.HasIndex("RemindingId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("ModelsLibrary.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Adress");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("ModelsLibrary.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Birthday");

                    b.Property<string>("Name");

                    b.Property<string>("Phone");

                    b.Property<byte[]>("Photo");

                    b.Property<string>("Surname");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("ModelsLibrary.Models.Reminding", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AudioPath");

                    b.Property<string>("Composition");

                    b.Property<TimeSpan>("Delay");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Remindings");
                });

            modelBuilder.Entity("ModelsLibrary.Models.Event", b =>
                {
                    b.HasOne("ModelsLibrary.Models.Location", "Adress")
                        .WithMany("Events")
                        .HasForeignKey("AdressId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ModelsLibrary.Models.Person", "Person")
                        .WithMany("Events")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ModelsLibrary.Models.Reminding", "Reminding")
                        .WithMany("Events")
                        .HasForeignKey("RemindingId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
