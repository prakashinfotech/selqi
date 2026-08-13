using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Selqi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000401"),
                column: "Url",
                value: "https://images.unsplash.com/photo-1496181133206-80ce9b88a853?auto=format&fit=crop&w=600&h=500");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000402"),
                column: "Url",
                value: "https://images.unsplash.com/photo-1523275335684-37898b6baf30?auto=format&fit=crop&w=600&h=500");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000403"),
                column: "Url",
                value: "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?auto=format&fit=crop&w=600&h=500");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000404"),
                column: "Url",
                value: "https://images.unsplash.com/photo-1493663284031-b7e3aefcae8e?auto=format&fit=crop&w=600&h=500");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000405"),
                column: "Url",
                value: "https://images.unsplash.com/photo-1534575950704-07c5a26b1e00?auto=format&fit=crop&w=600&h=500");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000406"),
                column: "Url",
                value: "https://images.unsplash.com/photo-1587654780291-39c9404d746b?auto=format&fit=crop&w=600&h=500");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000407"),
                column: "Url",
                value: "https://images.unsplash.com/photo-1600210492493-0946911123ea?auto=format&fit=crop&w=600&h=500");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000408"),
                column: "Url",
                value: "https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?auto=format&fit=crop&w=600&h=500");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000409"),
                column: "Url",
                value: "https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b?auto=format&fit=crop&w=600&h=500");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000410"),
                column: "Url",
                value: "https://images.unsplash.com/photo-1522338242992-e1a54906a8da?auto=format&fit=crop&w=600&h=500");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000411"),
                column: "Url",
                value: "https://images.unsplash.com/photo-1515488042361-ee00e0ddd4e4?auto=format&fit=crop&w=600&h=500");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000412"),
                column: "Url",
                value: "https://images.unsplash.com/photo-1544244015-0df4b3ffc6b0?auto=format&fit=crop&w=600&h=500");

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "AltText", "DisplayOrder", "ProductId", "Url" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000413"), "Astra 15 Laptop on desk", 2, new Guid("00000000-0000-0000-0000-000000000301"), "https://images.unsplash.com/photo-1525547719571-a2d4ac8945e2?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000414"), "Northline Smart Watch on wrist", 2, new Guid("00000000-0000-0000-0000-000000000302"), "https://images.unsplash.com/photo-1546868871-7041f2a55e12?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000415"), "Kestral Wireless Headphones flat lay", 2, new Guid("00000000-0000-0000-0000-000000000303"), "https://images.unsplash.com/photo-1484704849700-f032a568e944?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000416"), "Homekind Storage Cabinet in room", 2, new Guid("00000000-0000-0000-0000-000000000304"), "https://images.unsplash.com/photo-1555041469-a586c61ea9bc?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000417"), "Homekind Desk Lamp lit", 2, new Guid("00000000-0000-0000-0000-000000000305"), "https://images.unsplash.com/photo-1513506003901-1e6a35c98fe8?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000418"), "Playwell Builder Set pieces", 2, new Guid("00000000-0000-0000-0000-000000000306"), "https://images.unsplash.com/photo-1566576912321-d58ddd7a6088?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000419"), "Northline Patio Chair Pair outdoor setting", 2, new Guid("00000000-0000-0000-0000-000000000307"), "https://images.unsplash.com/photo-1561702771-eb3cf35eedd2?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000420"), "Kestral Air Fryer cooking food", 2, new Guid("00000000-0000-0000-0000-000000000308"), "https://images.unsplash.com/photo-1604503468506-a8da13d82791?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000421"), "VeloFit Exercise Bike in use", 2, new Guid("00000000-0000-0000-0000-000000000309"), "https://images.unsplash.com/photo-1534438327276-14e5300c3a48?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000422"), "Astra Hair Dryer in use", 2, new Guid("00000000-0000-0000-0000-000000000310"), "https://images.unsplash.com/photo-1527799820374-dcf8d9d4a388?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000423"), "Homekind Travel Cot assembled", 2, new Guid("00000000-0000-0000-0000-000000000311"), "https://images.unsplash.com/photo-1491013516836-7db643ee125a?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000424"), "Astra Tablet 10 in use", 2, new Guid("00000000-0000-0000-0000-000000000312"), "https://images.unsplash.com/photo-1512499617640-c74ae3a79d37?auto=format&fit=crop&w=600&h=500" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000413"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000414"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000415"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000416"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000417"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000418"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000419"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000420"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000421"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000422"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000423"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000424"));

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000401"),
                column: "Url",
                value: "/assets/products/astra-15-laptop.jpg");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000402"),
                column: "Url",
                value: "/assets/products/northline-smart-watch.jpg");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000403"),
                column: "Url",
                value: "/assets/products/kestral-wireless-headphones.jpg");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000404"),
                column: "Url",
                value: "/assets/products/homekind-storage-cabinet.jpg");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000405"),
                column: "Url",
                value: "/assets/products/homekind-desk-lamp.jpg");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000406"),
                column: "Url",
                value: "/assets/products/playwell-builder-set.jpg");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000407"),
                column: "Url",
                value: "/assets/products/northline-patio-chair-pair.jpg");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000408"),
                column: "Url",
                value: "/assets/products/kestral-air-fryer-6l.jpg");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000409"),
                column: "Url",
                value: "/assets/products/velofit-exercise-bike.jpg");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000410"),
                column: "Url",
                value: "/assets/products/astra-hair-dryer.jpg");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000411"),
                column: "Url",
                value: "/assets/products/homekind-travel-cot.jpg");

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000412"),
                column: "Url",
                value: "/assets/products/astra-tablet-10.jpg");
        }
    }
}
