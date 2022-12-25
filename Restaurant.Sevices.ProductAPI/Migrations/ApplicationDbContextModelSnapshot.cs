﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Restaurant.Services.ProductAPI.DbContexts;

#nullable disable

namespace Restaurant.Services.ProductAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Restaurant.Sevices.ProductAPI.Models.Dto.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"), 1L, 1);

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("ProductDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ProductId = 1,
                            CategoryName = "Appetizer",
                            ImageURL = "https://vitaliikutsan.blob.core.windows.net/palyanuchka/Images/kanapka.jpg",
                            Price = 15.0,
                            ProductDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque eleifend dui nunc, vel aliquet diam viverra vel. ",
                            ProductName = "Kanapka"
                        },
                        new
                        {
                            ProductId = 2,
                            CategoryName = "Appetizer",
                            ImageURL = "https://vitaliikutsan.blob.core.windows.net/palyanuchka/Images/palyanychka.jpg",
                            Price = 18.0,
                            ProductDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque eleifend dui nunc, vel aliquet diam viverra vel. ",
                            ProductName = "Palyanuchka"
                        },
                        new
                        {
                            ProductId = 3,
                            CategoryName = "Sushi",
                            ImageURL = "https://vitaliikutsan.blob.core.windows.net/palyanuchka/Images/sushiki.jpg",
                            Price = 18.0,
                            ProductDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque eleifend dui nunc, vel aliquet diam viverra vel. ",
                            ProductName = "Sushiky"
                        },
                        new
                        {
                            ProductId = 4,
                            CategoryName = "Dish",
                            ImageURL = "https://vitaliikutsan.blob.core.windows.net/palyanuchka/Images/varenyky.jpg",
                            Price = 18.0,
                            ProductDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque eleifend dui nunc, vel aliquet diam viverra vel. ",
                            ProductName = "Varenyky"
                        },
                        new
                        {
                            ProductId = 5,
                            CategoryName = "Dish",
                            ImageURL = "https://vitaliikutsan.blob.core.windows.net/palyanuchka/Images/wings.jpg",
                            Price = 18.0,
                            ProductDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque eleifend dui nunc, vel aliquet diam viverra vel. ",
                            ProductName = "Wings"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
