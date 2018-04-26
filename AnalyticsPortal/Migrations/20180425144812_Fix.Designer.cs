﻿// <auto-generated />
using AnalyticsPortal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace AnalyticsPortal.Migrations
{
    [DbContext(typeof(OrdersContext))]
    [Migration("20180425144812_Fix")]
    partial class Fix
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AnalyticsPortal.Models.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ClosedDate");

                    b.Property<DateTime>("OpendDate");

                    b.Property<int>("OrderFor");

                    b.Property<int>("State");

                    b.Property<double>("TipsAmount");

                    b.Property<double>("TotalPrice");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("AnalyticsPortal.Models.OrderItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Media");

                    b.Property<string>("Name");

                    b.Property<double>("Price");

                    b.HasKey("Id");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("AnalyticsPortal.Models.OrderItemOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("OrderId");

                    b.Property<Guid>("OrderItemId");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("OrderItemId");

                    b.ToTable("OrderItemOrders");
                });

            modelBuilder.Entity("AnalyticsPortal.Models.OrderItemOrder", b =>
                {
                    b.HasOne("AnalyticsPortal.Models.Order", "Order")
                        .WithMany("OrderItemOrders")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AnalyticsPortal.Models.OrderItem", "OrderItem")
                        .WithMany("OrderItemOrders")
                        .HasForeignKey("OrderItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
