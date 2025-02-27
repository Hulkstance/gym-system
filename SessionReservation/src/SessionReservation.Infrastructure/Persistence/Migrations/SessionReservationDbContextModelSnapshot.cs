﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SessionReservation.Infrastructure.Persistence;

#nullable disable

namespace SessionReservation.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(SessionReservationDbContext))]
    partial class SessionReservationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.1");

            modelBuilder.Entity("SessionReservation.Domain.ParticipantAggregate.Participant", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("_sessionIds")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("SessionIds");

                    b.HasKey("Id");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("SessionReservation.Domain.RoomsAggregate.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("GymId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("_maxSessions")
                        .HasColumnType("INTEGER")
                        .HasColumnName("MaxSessions");

                    b.Property<string>("_sessionIds")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("SessionIds");

                    b.HasKey("Id");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("SessionReservation.Domain.SessionAggregate.Session", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("MaxParticipants")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TrainerId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("SessionReservation.Domain.TrainerAggregate.Trainer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("_sessionIds")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("SessionIds");

                    b.HasKey("Id");

                    b.ToTable("Trainers");
                });

            modelBuilder.Entity("SessionReservation.Domain.ParticipantAggregate.Participant", b =>
                {
                    b.OwnsOne("SessionReservation.Domain.Common.Entities.Schedule", "_schedule", b1 =>
                        {
                            b1.Property<Guid>("ParticipantId")
                                .HasColumnType("TEXT");

                            b1.Property<Guid>("Id")
                                .HasColumnType("TEXT")
                                .HasColumnName("ScheduleId");

                            b1.Property<string>("_calendar")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .HasColumnName("ScheduleCalendar");

                            b1.HasKey("ParticipantId");

                            b1.ToTable("Participants");

                            b1.WithOwner()
                                .HasForeignKey("ParticipantId");
                        });

                    b.Navigation("_schedule");
                });

            modelBuilder.Entity("SessionReservation.Domain.RoomsAggregate.Room", b =>
                {
                    b.OwnsOne("SessionReservation.Domain.Common.Entities.Schedule", "_schedule", b1 =>
                        {
                            b1.Property<Guid>("RoomId")
                                .HasColumnType("TEXT");

                            b1.Property<Guid>("Id")
                                .HasColumnType("TEXT")
                                .HasColumnName("ScheduleId");

                            b1.Property<string>("_calendar")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .HasColumnName("ScheduleCalendar");

                            b1.HasKey("RoomId");

                            b1.ToTable("Rooms");

                            b1.WithOwner()
                                .HasForeignKey("RoomId");
                        });

                    b.Navigation("_schedule");
                });

            modelBuilder.Entity("SessionReservation.Domain.SessionAggregate.Session", b =>
                {
                    b.OwnsOne("SessionReservation.Domain.Common.ValueObjects.TimeRange", "Time", b1 =>
                        {
                            b1.Property<Guid>("SessionId")
                                .HasColumnType("TEXT");

                            b1.Property<TimeOnly>("End")
                                .HasColumnType("TEXT");

                            b1.Property<TimeOnly>("Start")
                                .HasColumnType("TEXT");

                            b1.HasKey("SessionId");

                            b1.ToTable("Sessions");

                            b1.WithOwner()
                                .HasForeignKey("SessionId");
                        });

                    b.OwnsMany("SessionReservation.Domain.SessionAggregate.Reservation", "_reservations", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("TEXT");

                            b1.Property<Guid>("ParticipantId")
                                .HasColumnType("TEXT");

                            b1.Property<Guid>("SessionId")
                                .HasColumnType("TEXT");

                            b1.HasKey("Id");

                            b1.HasIndex("SessionId");

                            b1.ToTable("SessionReservations", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("SessionId");
                        });

                    b.OwnsMany("SessionReservation.Domain.SessionAggregate.SessionCategory", "Categories", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("INTEGER");

                            b1.Property<string>("Name")
                                .HasColumnType("TEXT");

                            b1.Property<Guid>("SessionId")
                                .HasColumnType("TEXT");

                            b1.Property<int>("Value")
                                .HasColumnType("INTEGER");

                            b1.HasKey("Id");

                            b1.HasIndex("SessionId");

                            b1.ToTable("SessionCategories", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("SessionId");
                        });

                    b.Navigation("Categories");

                    b.Navigation("Time")
                        .IsRequired();

                    b.Navigation("_reservations");
                });

            modelBuilder.Entity("SessionReservation.Domain.TrainerAggregate.Trainer", b =>
                {
                    b.OwnsOne("SessionReservation.Domain.Common.Entities.Schedule", "_schedule", b1 =>
                        {
                            b1.Property<Guid>("TrainerId")
                                .HasColumnType("TEXT");

                            b1.Property<Guid>("Id")
                                .HasColumnType("TEXT")
                                .HasColumnName("ScheduleId");

                            b1.Property<string>("_calendar")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .HasColumnName("ScheduleCalendar");

                            b1.HasKey("TrainerId");

                            b1.ToTable("Trainers");

                            b1.WithOwner()
                                .HasForeignKey("TrainerId");
                        });

                    b.Navigation("_schedule");
                });
#pragma warning restore 612, 618
        }
    }
}
