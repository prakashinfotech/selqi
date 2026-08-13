using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Selqi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixBrokenBoardGameImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000431"),
                column: "Url",
                value: "https://images.unsplash.com/VJVsEnR_vNE?auto=format&fit=crop&w=600&h=500");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000431"),
                column: "Url",
                value: "https://images.unsplash.com/photo-1550459045-66bab08aab22?auto=format&fit=crop&w=600&h=500");
        }
    }
}
