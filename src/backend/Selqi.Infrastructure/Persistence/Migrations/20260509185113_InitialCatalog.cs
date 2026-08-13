using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Selqi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Town = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Postcode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(220)", maxLength: 220, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(240)", maxLength: 240, nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FullDescription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Sku = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    WasPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false),
                    ReviewCount = table.Column<int>(type: "int", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuantityAvailable = table.Column<int>(type: "int", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventory_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inventory_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AltText = table.Column<string>(type: "nvarchar(220)", maxLength: 220, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSpecifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSpecifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSpecifications_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "CreatedAtUtc", "Name", "Slug" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000101"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Astra", "astra" },
                    { new Guid("00000000-0000-0000-0000-000000000102"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Northline", "northline" },
                    { new Guid("00000000-0000-0000-0000-000000000103"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Kestral", "kestral" },
                    { new Guid("00000000-0000-0000-0000-000000000104"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Homekind", "homekind" },
                    { new Guid("00000000-0000-0000-0000-000000000105"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Playwell", "playwell" },
                    { new Guid("00000000-0000-0000-0000-000000000106"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "VeloFit", "velofit" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAtUtc", "Description", "DisplayOrder", "IsDeleted", "Name", "Slug", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Computing, phones, gaming, and smart home essentials.", 1, false, "Technology", "technology", null },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Furniture, appliances, storage, and home improvement.", 2, false, "Home", "home", null },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Toys, games, learning, and outdoor play.", 3, false, "Toys", "toys", null },
                    { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Garden furniture, tools, BBQs, and seasonal picks.", 4, false, "Garden", "garden", null },
                    { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Kitchen, laundry, cleaning, and everyday appliances.", 5, false, "Appliances", "appliances", null },
                    { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Fitness equipment, bikes, and active lifestyle products.", 6, false, "Sports", "sports", null },
                    { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Hair care, grooming, wellness, and personal care.", 7, false, "Beauty", "beauty", null },
                    { new Guid("00000000-0000-0000-0000-000000000008"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Nursery, travel, feeding, and baby care essentials.", 8, false, "Baby", "baby", null }
                });

            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "Id", "CreatedAtUtc", "Name", "Postcode", "Town" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Central Retail Park", "B1 1AA", "Birmingham" },
                    { new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Riverside Collection", "LS1 4PR", "Leeds" },
                    { new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Metro High Street", "M1 2WD", "Manchester" },
                    { new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Harbour Point", "BS1 6QF", "Bristol" },
                    { new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Station Square", "E1 8QS", "London" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "BrandId", "CategoryId", "CreatedAtUtc", "FullDescription", "IsDeleted", "IsFeatured", "Name", "Price", "Rating", "ReviewCount", "ShortDescription", "Sku", "Slug", "UpdatedAtUtc", "WasPrice" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000301"), new Guid("00000000-0000-0000-0000-000000000101"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "A fast everyday laptop with a bright 15 inch display. Built for the Selqi demo catalog with deterministic seed data.", false, true, "Astra 15 Laptop", 699.99m, 4.6m, 184, "A fast everyday laptop with a bright 15 inch display.", "SKU-AST-150", "astra-15-laptop", null, 749.99m },
                    { new Guid("00000000-0000-0000-0000-000000000302"), new Guid("00000000-0000-0000-0000-000000000102"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Fitness tracking, notifications, and week-long battery life. Built for the Selqi demo catalog with deterministic seed data.", false, true, "Northline Smart Watch", 129.99m, 4.4m, 89, "Fitness tracking, notifications, and week-long battery life.", "SKU-NOR-W10", "northline-smart-watch", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000303"), new Guid("00000000-0000-0000-0000-000000000103"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Noise reducing headphones for work, travel, and music. Built for the Selqi demo catalog with deterministic seed data.", false, false, "Kestral Wireless Headphones", 89.99m, 4.3m, 241, "Noise reducing headphones for work, travel, and music.", "SKU-KES-H70", "kestral-wireless-headphones", null, 109.99m },
                    { new Guid("00000000-0000-0000-0000-000000000304"), new Guid("00000000-0000-0000-0000-000000000104"), new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Compact two-door storage for hallway, office, or utility rooms. Built for the Selqi demo catalog with deterministic seed data.", false, true, "Homekind Storage Cabinet", 149.99m, 4.2m, 56, "Compact two-door storage for hallway, office, or utility rooms.", "SKU-HOM-C22", "homekind-storage-cabinet", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000305"), new Guid("00000000-0000-0000-0000-000000000104"), new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Adjustable LED desk lamp with touch brightness controls. Built for the Selqi demo catalog with deterministic seed data.", false, false, "Homekind Desk Lamp", 34.99m, 4.5m, 132, "Adjustable LED desk lamp with touch brightness controls.", "SKU-HOM-L08", "homekind-desk-lamp", null, 44.99m },
                    { new Guid("00000000-0000-0000-0000-000000000306"), new Guid("00000000-0000-0000-0000-000000000105"), new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Creative construction set with 650 colorful pieces. Built for the Selqi demo catalog with deterministic seed data.", false, true, "Playwell Builder Set", 39.99m, 4.8m, 312, "Creative construction set with 650 colorful pieces.", "SKU-PLA-B65", "playwell-builder-set", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000307"), new Guid("00000000-0000-0000-0000-000000000102"), new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Weather-resistant garden chairs for compact outdoor spaces. Built for the Selqi demo catalog with deterministic seed data.", false, false, "Northline Patio Chair Pair", 119.99m, 4.1m, 47, "Weather-resistant garden chairs for compact outdoor spaces.", "SKU-NOR-G02", "northline-patio-chair-pair", null, 139.99m },
                    { new Guid("00000000-0000-0000-0000-000000000308"), new Guid("00000000-0000-0000-0000-000000000103"), new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Large capacity air fryer with digital presets. Built for the Selqi demo catalog with deterministic seed data.", false, true, "Kestral Air Fryer 6L", 79.99m, 4.7m, 420, "Large capacity air fryer with digital presets.", "SKU-KES-AF6", "kestral-air-fryer-6l", null, 99.99m },
                    { new Guid("00000000-0000-0000-0000-000000000309"), new Guid("00000000-0000-0000-0000-000000000106"), new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Quiet indoor exercise bike with adjustable resistance. Built for the Selqi demo catalog with deterministic seed data.", false, true, "VeloFit Exercise Bike", 249.99m, 4.4m, 75, "Quiet indoor exercise bike with adjustable resistance.", "SKU-VEL-B11", "velofit-exercise-bike", null, 299.99m },
                    { new Guid("00000000-0000-0000-0000-000000000310"), new Guid("00000000-0000-0000-0000-000000000101"), new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Lightweight ionic hair dryer with diffuser attachment. Built for the Selqi demo catalog with deterministic seed data.", false, false, "Astra Hair Dryer", 49.99m, 4.2m, 118, "Lightweight ionic hair dryer with diffuser attachment.", "SKU-AST-HD2", "astra-hair-dryer", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000311"), new Guid("00000000-0000-0000-0000-000000000104"), new Guid("00000000-0000-0000-0000-000000000008"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Foldable travel cot with carry bag and padded base. Built for the Selqi demo catalog with deterministic seed data.", false, false, "Homekind Travel Cot", 69.99m, 4.5m, 63, "Foldable travel cot with carry bag and padded base.", "SKU-HOM-TC4", "homekind-travel-cot", null, 84.99m },
                    { new Guid("00000000-0000-0000-0000-000000000312"), new Guid("00000000-0000-0000-0000-000000000101"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Portable 10 inch tablet for streaming, reading, and browsing. Built for the Selqi demo catalog with deterministic seed data.", false, true, "Astra Tablet 10", 179.99m, 4.1m, 98, "Portable 10 inch tablet for streaming, reading, and browsing.", "SKU-AST-T10", "astra-tablet-10", null, null }
                });

            migrationBuilder.InsertData(
                table: "Inventory",
                columns: new[] { "Id", "ProductId", "QuantityAvailable", "StoreId", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000701"), new Guid("00000000-0000-0000-0000-000000000301"), 0, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000702"), new Guid("00000000-0000-0000-0000-000000000301"), 5, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000703"), new Guid("00000000-0000-0000-0000-000000000301"), 6, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000704"), new Guid("00000000-0000-0000-0000-000000000301"), 7, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000705"), new Guid("00000000-0000-0000-0000-000000000301"), 0, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000706"), new Guid("00000000-0000-0000-0000-000000000302"), 5, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000707"), new Guid("00000000-0000-0000-0000-000000000302"), 6, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000708"), new Guid("00000000-0000-0000-0000-000000000302"), 7, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000709"), new Guid("00000000-0000-0000-0000-000000000302"), 0, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000710"), new Guid("00000000-0000-0000-0000-000000000302"), 9, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000711"), new Guid("00000000-0000-0000-0000-000000000303"), 6, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000712"), new Guid("00000000-0000-0000-0000-000000000303"), 7, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000713"), new Guid("00000000-0000-0000-0000-000000000303"), 0, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000714"), new Guid("00000000-0000-0000-0000-000000000303"), 9, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000715"), new Guid("00000000-0000-0000-0000-000000000303"), 10, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000716"), new Guid("00000000-0000-0000-0000-000000000304"), 7, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000717"), new Guid("00000000-0000-0000-0000-000000000304"), 0, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000718"), new Guid("00000000-0000-0000-0000-000000000304"), 9, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000719"), new Guid("00000000-0000-0000-0000-000000000304"), 10, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000720"), new Guid("00000000-0000-0000-0000-000000000304"), 11, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000721"), new Guid("00000000-0000-0000-0000-000000000305"), 0, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000722"), new Guid("00000000-0000-0000-0000-000000000305"), 9, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000723"), new Guid("00000000-0000-0000-0000-000000000305"), 10, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000724"), new Guid("00000000-0000-0000-0000-000000000305"), 11, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000725"), new Guid("00000000-0000-0000-0000-000000000305"), 0, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000726"), new Guid("00000000-0000-0000-0000-000000000306"), 9, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000727"), new Guid("00000000-0000-0000-0000-000000000306"), 10, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000728"), new Guid("00000000-0000-0000-0000-000000000306"), 11, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000729"), new Guid("00000000-0000-0000-0000-000000000306"), 0, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000730"), new Guid("00000000-0000-0000-0000-000000000306"), 13, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000731"), new Guid("00000000-0000-0000-0000-000000000307"), 10, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000732"), new Guid("00000000-0000-0000-0000-000000000307"), 11, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000733"), new Guid("00000000-0000-0000-0000-000000000307"), 0, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000734"), new Guid("00000000-0000-0000-0000-000000000307"), 13, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000735"), new Guid("00000000-0000-0000-0000-000000000307"), 14, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000736"), new Guid("00000000-0000-0000-0000-000000000308"), 11, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000737"), new Guid("00000000-0000-0000-0000-000000000308"), 0, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000738"), new Guid("00000000-0000-0000-0000-000000000308"), 13, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000739"), new Guid("00000000-0000-0000-0000-000000000308"), 14, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000740"), new Guid("00000000-0000-0000-0000-000000000308"), 15, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000741"), new Guid("00000000-0000-0000-0000-000000000309"), 0, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000742"), new Guid("00000000-0000-0000-0000-000000000309"), 13, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000743"), new Guid("00000000-0000-0000-0000-000000000309"), 14, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000744"), new Guid("00000000-0000-0000-0000-000000000309"), 15, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000745"), new Guid("00000000-0000-0000-0000-000000000309"), 0, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000746"), new Guid("00000000-0000-0000-0000-000000000310"), 13, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000747"), new Guid("00000000-0000-0000-0000-000000000310"), 14, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000748"), new Guid("00000000-0000-0000-0000-000000000310"), 15, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000749"), new Guid("00000000-0000-0000-0000-000000000310"), 0, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000750"), new Guid("00000000-0000-0000-0000-000000000310"), 17, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000751"), new Guid("00000000-0000-0000-0000-000000000311"), 14, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000752"), new Guid("00000000-0000-0000-0000-000000000311"), 15, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000753"), new Guid("00000000-0000-0000-0000-000000000311"), 0, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000754"), new Guid("00000000-0000-0000-0000-000000000311"), 17, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000755"), new Guid("00000000-0000-0000-0000-000000000311"), 18, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000756"), new Guid("00000000-0000-0000-0000-000000000312"), 15, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000757"), new Guid("00000000-0000-0000-0000-000000000312"), 0, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000758"), new Guid("00000000-0000-0000-0000-000000000312"), 17, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000759"), new Guid("00000000-0000-0000-0000-000000000312"), 18, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000760"), new Guid("00000000-0000-0000-0000-000000000312"), 19, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "AltText", "DisplayOrder", "ProductId", "Url" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000401"), "Astra 15 Laptop", 1, new Guid("00000000-0000-0000-0000-000000000301"), "/assets/products/astra-15-laptop.jpg" },
                    { new Guid("00000000-0000-0000-0000-000000000402"), "Northline Smart Watch", 1, new Guid("00000000-0000-0000-0000-000000000302"), "/assets/products/northline-smart-watch.jpg" },
                    { new Guid("00000000-0000-0000-0000-000000000403"), "Kestral Wireless Headphones", 1, new Guid("00000000-0000-0000-0000-000000000303"), "/assets/products/kestral-wireless-headphones.jpg" },
                    { new Guid("00000000-0000-0000-0000-000000000404"), "Homekind Storage Cabinet", 1, new Guid("00000000-0000-0000-0000-000000000304"), "/assets/products/homekind-storage-cabinet.jpg" },
                    { new Guid("00000000-0000-0000-0000-000000000405"), "Homekind Desk Lamp", 1, new Guid("00000000-0000-0000-0000-000000000305"), "/assets/products/homekind-desk-lamp.jpg" },
                    { new Guid("00000000-0000-0000-0000-000000000406"), "Playwell Builder Set", 1, new Guid("00000000-0000-0000-0000-000000000306"), "/assets/products/playwell-builder-set.jpg" },
                    { new Guid("00000000-0000-0000-0000-000000000407"), "Northline Patio Chair Pair", 1, new Guid("00000000-0000-0000-0000-000000000307"), "/assets/products/northline-patio-chair-pair.jpg" },
                    { new Guid("00000000-0000-0000-0000-000000000408"), "Kestral Air Fryer 6L", 1, new Guid("00000000-0000-0000-0000-000000000308"), "/assets/products/kestral-air-fryer-6l.jpg" },
                    { new Guid("00000000-0000-0000-0000-000000000409"), "VeloFit Exercise Bike", 1, new Guid("00000000-0000-0000-0000-000000000309"), "/assets/products/velofit-exercise-bike.jpg" },
                    { new Guid("00000000-0000-0000-0000-000000000410"), "Astra Hair Dryer", 1, new Guid("00000000-0000-0000-0000-000000000310"), "/assets/products/astra-hair-dryer.jpg" },
                    { new Guid("00000000-0000-0000-0000-000000000411"), "Homekind Travel Cot", 1, new Guid("00000000-0000-0000-0000-000000000311"), "/assets/products/homekind-travel-cot.jpg" },
                    { new Guid("00000000-0000-0000-0000-000000000412"), "Astra Tablet 10", 1, new Guid("00000000-0000-0000-0000-000000000312"), "/assets/products/astra-tablet-10.jpg" }
                });

            migrationBuilder.InsertData(
                table: "ProductSpecifications",
                columns: new[] { "Id", "DisplayOrder", "Name", "ProductId", "Value" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000501"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000301"), "SKU-AST-150" },
                    { new Guid("00000000-0000-0000-0000-000000000502"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000301"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000503"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000302"), "SKU-NOR-W10" },
                    { new Guid("00000000-0000-0000-0000-000000000504"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000302"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000505"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000303"), "SKU-KES-H70" },
                    { new Guid("00000000-0000-0000-0000-000000000506"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000303"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000507"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000304"), "SKU-HOM-C22" },
                    { new Guid("00000000-0000-0000-0000-000000000508"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000304"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000509"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000305"), "SKU-HOM-L08" },
                    { new Guid("00000000-0000-0000-0000-000000000510"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000305"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000511"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000306"), "SKU-PLA-B65" },
                    { new Guid("00000000-0000-0000-0000-000000000512"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000306"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000513"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000307"), "SKU-NOR-G02" },
                    { new Guid("00000000-0000-0000-0000-000000000514"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000307"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000515"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000308"), "SKU-KES-AF6" },
                    { new Guid("00000000-0000-0000-0000-000000000516"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000308"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000517"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000309"), "SKU-VEL-B11" },
                    { new Guid("00000000-0000-0000-0000-000000000518"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000309"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000519"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000310"), "SKU-AST-HD2" },
                    { new Guid("00000000-0000-0000-0000-000000000520"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000310"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000521"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000311"), "SKU-HOM-TC4" },
                    { new Guid("00000000-0000-0000-0000-000000000522"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000311"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000523"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000312"), "SKU-AST-T10" },
                    { new Guid("00000000-0000-0000-0000-000000000524"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000312"), "1 year" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Brands_Slug",
                table: "Brands",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                table: "Categories",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_ProductId_StoreId",
                table: "Inventory",
                columns: new[] { "ProductId", "StoreId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_StoreId",
                table: "Inventory",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Sku",
                table: "Products",
                column: "Sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Slug",
                table: "Products",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecifications_ProductId",
                table: "ProductSpecifications",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductSpecifications");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
