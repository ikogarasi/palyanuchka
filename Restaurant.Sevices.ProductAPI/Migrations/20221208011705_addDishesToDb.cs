using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant.Sevices.ProductAPI.Migrations
{
    public partial class addDishesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryName", "ImageURL", "Price", "ProductDescription", "ProductName" },
                values: new object[,]
                {
                    { 1, "Appetizer", "https://vitaliikutsan.blob.core.windows.net/palyanuchka/Images/kanapka.jpg", 15.0, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque eleifend dui nunc, vel aliquet diam viverra vel. ", "Kanapka" },
                    { 2, "Appetizer", "https://vitaliikutsan.blob.core.windows.net/palyanuchka/Images/palyanychka.jpg", 18.0, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque eleifend dui nunc, vel aliquet diam viverra vel. ", "Palyanuchka" },
                    { 3, "Sushi", "https://vitaliikutsan.blob.core.windows.net/palyanuchka/Images/sushiki.jpg", 18.0, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque eleifend dui nunc, vel aliquet diam viverra vel. ", "Sushiky" },
                    { 4, "Dish", "https://vitaliikutsan.blob.core.windows.net/palyanuchka/Images/varenyky.jpg", 18.0, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque eleifend dui nunc, vel aliquet diam viverra vel. ", "Varenyky" },
                    { 5, "Dish", "https://vitaliikutsan.blob.core.windows.net/palyanuchka/Images/wings.jpg", 18.0, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque eleifend dui nunc, vel aliquet diam viverra vel. ", "Wings" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5);
        }
    }
}
