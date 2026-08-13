using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Selqi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStoreEnrichmentAndInventoryThresholds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── Store enrichment ───────────────────────────────────────────────────
            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                table: "Stores",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "Stores",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Stores",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoreType",
                table: "Stores",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "Standalone");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Stores",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Stores",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Stores",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "Stores",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSUTCDATETIME()");

            // ── Inventory enrichment ───────────────────────────────────────────────
            migrationBuilder.AddColumn<int>(
                name: "LowStockThreshold",
                table: "Inventory",
                type: "int",
                nullable: false,
                defaultValue: 5);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastRestockedAtUtc",
                table: "Inventory",
                type: "datetime2",
                nullable: true);

            // ── Update seed data for existing 5 stores ─────────────────────────────
            var seededAt = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc);

            var columns = new[] { "AddressLine1", "Phone", "StoreType", "Latitude", "Longitude", "IsActive", "UpdatedAtUtc" };
            var columnTypes = new[] { "nvarchar(250)", "nvarchar(20)", "nvarchar(30)", "float", "float", "bit", "datetime2" };

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumns: new[] { "Id" },
                keyColumnTypes: new[] { "uniqueidentifier" },
                keyValues: new object[] { new Guid("00000000-0000-0000-0000-000000000201") },
                columns: columns,
                columnTypes: columnTypes,
                values: new object[] { "Unit 5, Central Retail Park", "01212 345678", "Standalone", 52.4862, -1.8904, true, seededAt });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumns: new[] { "Id" },
                keyColumnTypes: new[] { "uniqueidentifier" },
                keyValues: new object[] { new Guid("00000000-0000-0000-0000-000000000202") },
                columns: columns,
                columnTypes: columnTypes,
                values: new object[] { "Sainsbury's, Riverside Way", "01132 456789", "Concession", 53.7997, -1.5492, true, seededAt });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumns: new[] { "Id" },
                keyColumnTypes: new[] { "uniqueidentifier" },
                keyValues: new object[] { new Guid("00000000-0000-0000-0000-000000000203") },
                columns: columns,
                columnTypes: columnTypes,
                values: new object[] { "42 Market Street", "01612 567890", "Standalone", 53.4808, -2.2426, true, seededAt });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumns: new[] { "Id" },
                keyColumnTypes: new[] { "uniqueidentifier" },
                keyValues: new object[] { new Guid("00000000-0000-0000-0000-000000000204") },
                columns: columns,
                columnTypes: columnTypes,
                values: new object[] { "Sainsbury's, Harbourside", "01172 678901", "Concession", 51.4545, -2.5879, true, seededAt });

            migrationBuilder.UpdateData(
                table: "Stores",
                keyColumns: new[] { "Id" },
                keyColumnTypes: new[] { "uniqueidentifier" },
                keyValues: new object[] { new Guid("00000000-0000-0000-0000-000000000205") },
                columns: columns,
                columnTypes: columnTypes,
                values: new object[] { "Sainsbury's Local, Station Square", "02071 234567", "CollectionPoint", 51.5074, -0.0878, true, seededAt });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "AddressLine1", table: "Stores");
            migrationBuilder.DropColumn(name: "AddressLine2", table: "Stores");
            migrationBuilder.DropColumn(name: "Phone", table: "Stores");
            migrationBuilder.DropColumn(name: "StoreType", table: "Stores");
            migrationBuilder.DropColumn(name: "Latitude", table: "Stores");
            migrationBuilder.DropColumn(name: "Longitude", table: "Stores");
            migrationBuilder.DropColumn(name: "IsActive", table: "Stores");
            migrationBuilder.DropColumn(name: "UpdatedAtUtc", table: "Stores");
            migrationBuilder.DropColumn(name: "LowStockThreshold", table: "Inventory");
            migrationBuilder.DropColumn(name: "LastRestockedAtUtc", table: "Inventory");
        }
    }
}
