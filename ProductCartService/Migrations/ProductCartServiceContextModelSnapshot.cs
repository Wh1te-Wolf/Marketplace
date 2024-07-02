﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ProductCartService.Repositories.EF;

#nullable disable

namespace ProductCartService.Migrations
{
    [DbContext(typeof(ProductCartServiceContext))]
    partial class ProductCartServiceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ProductCartService.Entities.Bill", b =>
                {
                    b.Property<Guid>("UUID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CustomerUUID")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("PaymentStatus")
                        .HasColumnType("integer");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.HasKey("UUID");

                    b.ToTable("Bills");
                });

            modelBuilder.Entity("ProductCartService.Entities.BillItem", b =>
                {
                    b.Property<Guid>("UUID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BillUUID")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<Guid>("ProductPriceUUID")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductUUID")
                        .HasColumnType("uuid");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("UUID");

                    b.HasIndex("BillUUID");

                    b.ToTable("BillItem");
                });

            modelBuilder.Entity("ProductCartService.Entities.ProductCart", b =>
                {
                    b.Property<Guid>("UUID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CustomerUUID")
                        .HasColumnType("uuid");

                    b.HasKey("UUID");

                    b.ToTable("ProductCarts");
                });

            modelBuilder.Entity("ProductCartService.Entities.ProductCartItem", b =>
                {
                    b.Property<Guid>("UUID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Included")
                        .HasColumnType("boolean");

                    b.Property<Guid>("ProductCartUUID")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductUUID")
                        .HasColumnType("uuid");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("UUID");

                    b.HasIndex("ProductCartUUID");

                    b.ToTable("ProductCartItem");
                });

            modelBuilder.Entity("ProductCartService.Entities.BillItem", b =>
                {
                    b.HasOne("ProductCartService.Entities.Bill", null)
                        .WithMany("Items")
                        .HasForeignKey("BillUUID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProductCartService.Entities.ProductCartItem", b =>
                {
                    b.HasOne("ProductCartService.Entities.ProductCart", null)
                        .WithMany("Items")
                        .HasForeignKey("ProductCartUUID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProductCartService.Entities.Bill", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("ProductCartService.Entities.ProductCart", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
