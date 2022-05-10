﻿// <auto-generated />
using System;
using HoangNV.HotelBooking.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HoangNV.HotelBooking.Web.Migrations
{
    [DbContext(typeof(BookingContext))]
    [Migration("20220415082255_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.Beds", b =>
                {
                    b.Property<int>("BedId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BedType")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("BedId");

                    b.ToTable("Beds");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.Booking", b =>
                {
                    b.Property<string>("BookingId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("BookingStatus")
                        .HasColumnType("int");

                    b.Property<DateTime>("CheckInTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CheckOutTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<decimal>("SumCost")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("BookingId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.BookingDetails", b =>
                {
                    b.Property<string>("BookingDetailId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BookingId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("CostNow")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.HasKey("BookingDetailId");

                    b.HasIndex("BookingId");

                    b.HasIndex("RoomId");

                    b.ToTable("BookingDetails");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.ConvenientTypes", b =>
                {
                    b.Property<int>("ConvenientTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConvenientTypeName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("ConvenientTypeId");

                    b.ToTable("ConvenientTypes");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.Convenients", b =>
                {
                    b.Property<int>("ConvenientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConvenientName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("ConvenientTypeId")
                        .HasColumnType("int");

                    b.HasKey("ConvenientId");

                    b.HasIndex("ConvenientTypeId");

                    b.ToTable("Convenients");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.Customers", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CheckInPersonCode")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CheckInPersonName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("CustomerName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("CustomerId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.HotelBranchs", b =>
                {
                    b.Property<int>("HotelBranchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HotelBranchCode")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("HotelBranchName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.HasKey("HotelBranchId");

                    b.ToTable("HotelBranchs");

                    b.HasData(
                        new
                        {
                            HotelBranchId = 1,
                            Address = "Hà Nội",
                            Description = "None",
                            HotelBranchCode = "HNVHotel",
                            HotelBranchName = "Hotel",
                            Status = 1
                        });
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.Images", b =>
                {
                    b.Property<int>("ImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("HotelBranchId")
                        .HasColumnType("int");

                    b.Property<string>("ImageLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RoomTypeId")
                        .HasColumnType("int");

                    b.HasKey("ImageId");

                    b.HasIndex("HotelBranchId");

                    b.HasIndex("RoomTypeId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.Roles", b =>
                {
                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            RoleId = "6ea013dd-40ea-4a0e-9d0e-a675c0e9498a",
                            RoleName = "Admin"
                        },
                        new
                        {
                            RoleId = "6ea013dd-40ea-4a0e-9d0e-a675c0e9498e",
                            RoleName = "Employee"
                        },
                        new
                        {
                            RoleId = "6ea013dd-40ea-4a0e-9d0e-a675c0e9498f",
                            RoleName = "Client"
                        });
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.RoomBeds", b =>
                {
                    b.Property<int>("RoomBedId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BedId")
                        .HasColumnType("int");

                    b.Property<int>("NumOfBed")
                        .HasColumnType("int");

                    b.Property<int>("RoomTypeId")
                        .HasColumnType("int");

                    b.HasKey("RoomBedId");

                    b.HasIndex("BedId");

                    b.HasIndex("RoomTypeId");

                    b.ToTable("RoomBeds");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.RoomConvenients", b =>
                {
                    b.Property<int>("RoomConvenientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ConvenientId")
                        .HasColumnType("int");

                    b.Property<int>("RoomTypeId")
                        .HasColumnType("int");

                    b.HasKey("RoomConvenientId");

                    b.HasIndex("ConvenientId");

                    b.HasIndex("RoomTypeId");

                    b.ToTable("RoomConvenients");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.RoomTypes", b =>
                {
                    b.Property<int>("RoomTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("Area")
                        .HasColumnType("real");

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HotelBranchId")
                        .HasColumnType("int");

                    b.Property<int>("NumOfPer")
                        .HasColumnType("int");

                    b.Property<string>("RoomTypeCode")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("RoomTypeName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("RoomTypeId");

                    b.HasIndex("HotelBranchId");

                    b.ToTable("RoomTypes");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.Rooms", b =>
                {
                    b.Property<int>("RoomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RoomCode")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("RoomTypeId")
                        .HasColumnType("int");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.HasKey("RoomId");

                    b.HasIndex("RoomTypeId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.Users", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Active")
                        .HasColumnType("int");

                    b.Property<int?>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PassWord")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = "6ea013dd-40ea-4a0e-9d0e-a675c0e9498b",
                            Active = 1,
                            FullName = "Admin",
                            PassWord = "AQAAAAEAACcQAAAAEEs7Zo3FMgFwsGOsNmwqm/eFFcqfJIPOFblxUPAoTLW0qbyGgqRZ0sXTcNpyf9b7SQ==",
                            RoleId = "6ea013dd-40ea-4a0e-9d0e-a675c0e9498a",
                            UserName = "Admin"
                        });
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.Booking", b =>
                {
                    b.HasOne("HoangNV.HotelBooking.Entities.Entities.Customers", "Customer")
                        .WithMany("Bookings")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.BookingDetails", b =>
                {
                    b.HasOne("HoangNV.HotelBooking.Entities.Entities.Booking", "Booking")
                        .WithMany("BookingDetails")
                        .HasForeignKey("BookingId");

                    b.HasOne("HoangNV.HotelBooking.Entities.Entities.Rooms", "Rooms")
                        .WithMany("BookingDetails")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.Convenients", b =>
                {
                    b.HasOne("HoangNV.HotelBooking.Entities.Entities.ConvenientTypes", "ConvenientType")
                        .WithMany("Convenients")
                        .HasForeignKey("ConvenientTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ConvenientType");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.Images", b =>
                {
                    b.HasOne("HoangNV.HotelBooking.Entities.Entities.HotelBranchs", "HotelBranch")
                        .WithMany("Images")
                        .HasForeignKey("HotelBranchId");

                    b.HasOne("HoangNV.HotelBooking.Entities.Entities.RoomTypes", "RoomType")
                        .WithMany("Images")
                        .HasForeignKey("RoomTypeId");

                    b.Navigation("HotelBranch");

                    b.Navigation("RoomType");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.RoomBeds", b =>
                {
                    b.HasOne("HoangNV.HotelBooking.Entities.Entities.Beds", "Bed")
                        .WithMany("RoomBeds")
                        .HasForeignKey("BedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HoangNV.HotelBooking.Entities.Entities.RoomTypes", "RoomType")
                        .WithMany("RoomBeds")
                        .HasForeignKey("RoomTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bed");

                    b.Navigation("RoomType");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.RoomConvenients", b =>
                {
                    b.HasOne("HoangNV.HotelBooking.Entities.Entities.Convenients", "Convenient")
                        .WithMany("RoomConvenients")
                        .HasForeignKey("ConvenientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HoangNV.HotelBooking.Entities.Entities.RoomTypes", "RoomType")
                        .WithMany("RoomConvenients")
                        .HasForeignKey("RoomTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Convenient");

                    b.Navigation("RoomType");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.RoomTypes", b =>
                {
                    b.HasOne("HoangNV.HotelBooking.Entities.Entities.HotelBranchs", "HotelBranch")
                        .WithMany("RoomTypes")
                        .HasForeignKey("HotelBranchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HotelBranch");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.Rooms", b =>
                {
                    b.HasOne("HoangNV.HotelBooking.Entities.Entities.RoomTypes", "RoomType")
                        .WithMany("Rooms")
                        .HasForeignKey("RoomTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RoomType");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.Users", b =>
                {
                    b.HasOne("HoangNV.HotelBooking.Entities.Entities.Customers", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");

                    b.HasOne("HoangNV.HotelBooking.Entities.Entities.Roles", "Roles")
                        .WithMany("Users")
                        .HasForeignKey("RoleId");

                    b.Navigation("Customer");

                    b.Navigation("Roles");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.Beds", b =>
                {
                    b.Navigation("RoomBeds");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.Booking", b =>
                {
                    b.Navigation("BookingDetails");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.ConvenientTypes", b =>
                {
                    b.Navigation("Convenients");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.Convenients", b =>
                {
                    b.Navigation("RoomConvenients");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.Customers", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.HotelBranchs", b =>
                {
                    b.Navigation("Images");

                    b.Navigation("RoomTypes");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.Roles", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.RoomTypes", b =>
                {
                    b.Navigation("Images");

                    b.Navigation("RoomBeds");

                    b.Navigation("RoomConvenients");

                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("HoangNV.HotelBooking.Entities.Entities.Rooms", b =>
                {
                    b.Navigation("BookingDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
