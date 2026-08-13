using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Selqi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSubCategoriesAndProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentCategoryId",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "ImageUrl", "ParentCategoryId" },
                values: new object[] { "https://images.unsplash.com/photo-1498049794561-7780e7231661?auto=format&fit=crop&w=120&h=120", null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                columns: new[] { "ImageUrl", "ParentCategoryId" },
                values: new object[] { "https://images.unsplash.com/photo-1555041469-a586c61ea9bc?auto=format&fit=crop&w=120&h=120", null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                columns: new[] { "ImageUrl", "ParentCategoryId" },
                values: new object[] { "https://images.unsplash.com/photo-1587654780291-39c9404d746b?auto=format&fit=crop&w=120&h=120", null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"),
                columns: new[] { "ImageUrl", "ParentCategoryId" },
                values: new object[] { "https://images.unsplash.com/photo-1416879595882-3373a0480b5b?auto=format&fit=crop&w=120&h=120", null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"),
                columns: new[] { "ImageUrl", "ParentCategoryId" },
                values: new object[] { "https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?auto=format&fit=crop&w=120&h=120", null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000006"),
                columns: new[] { "ImageUrl", "ParentCategoryId" },
                values: new object[] { "https://images.unsplash.com/photo-1517836357463-d25dfeac3438?auto=format&fit=crop&w=120&h=120", null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000007"),
                columns: new[] { "ImageUrl", "ParentCategoryId" },
                values: new object[] { "https://images.unsplash.com/photo-1596462502278-27bfdc403348?auto=format&fit=crop&w=120&h=120", null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000008"),
                columns: new[] { "ImageUrl", "ParentCategoryId" },
                values: new object[] { "https://images.unsplash.com/photo-1515488042361-ee00e0ddd4e4?auto=format&fit=crop&w=120&h=120", null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000009"),
                columns: new[] { "ImageUrl", "ParentCategoryId" },
                values: new object[] { "https://images.unsplash.com/photo-1505576399279-565b52d4ac71?auto=format&fit=crop&w=120&h=120", null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"),
                columns: new[] { "ImageUrl", "ParentCategoryId" },
                values: new object[] { "https://images.unsplash.com/photo-1441986300917-64674bd600d8?auto=format&fit=crop&w=120&h=120", null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"),
                columns: new[] { "ImageUrl", "ParentCategoryId" },
                values: new object[] { "https://images.unsplash.com/photo-1515562141207-7a88fb7ce338?auto=format&fit=crop&w=120&h=120", null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000012"),
                columns: new[] { "ImageUrl", "ParentCategoryId" },
                values: new object[] { "https://images.unsplash.com/photo-1549465220-1a8b9238cd48?auto=format&fit=crop&w=120&h=120", null });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAtUtc", "Description", "DisplayOrder", "ImageUrl", "IsDeleted", "Name", "ParentCategoryId", "Slug", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000013"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Laptops, ultrabooks, and desktop computers.", 1, "https://images.unsplash.com/photo-1496181133206-80ce9b88a853?auto=format&fit=crop&w=120&h=120", false, "Laptops & Desktops", new Guid("00000000-0000-0000-0000-000000000001"), "laptops-desktops", null },
                    { new Guid("00000000-0000-0000-0000-000000000014"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Smartphones, smartwatches, and earbuds.", 2, "https://images.unsplash.com/photo-1523275335684-37898b6baf30?auto=format&fit=crop&w=120&h=120", false, "Phones & Wearables", new Guid("00000000-0000-0000-0000-000000000001"), "phones-wearables", null },
                    { new Guid("00000000-0000-0000-0000-000000000015"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Sofas, armchairs, and accent seating.", 1, "https://images.unsplash.com/photo-1555041469-a586c61ea9bc?auto=format&fit=crop&w=120&h=120", false, "Sofas & Chairs", new Guid("00000000-0000-0000-0000-000000000002"), "sofas-chairs", null },
                    { new Guid("00000000-0000-0000-0000-000000000016"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Floor lamps, desk lamps, and pendant lights.", 2, "https://images.unsplash.com/photo-1534575950704-07c5a26b1e00?auto=format&fit=crop&w=120&h=120", false, "Lighting & Lamps", new Guid("00000000-0000-0000-0000-000000000002"), "lighting-lamps", null },
                    { new Guid("00000000-0000-0000-0000-000000000017"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "RC cars, action figures, and outdoor play toys.", 1, "https://images.unsplash.com/photo-1587654780291-39c9404d746b?auto=format&fit=crop&w=120&h=120", false, "Kids' Toys", new Guid("00000000-0000-0000-0000-000000000003"), "kids-toys", null },
                    { new Guid("00000000-0000-0000-0000-000000000018"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Family board games, card games, and puzzles.", 2, "https://images.unsplash.com/photo-1566576912321-d58ddd7a6088?auto=format&fit=crop&w=120&h=120", false, "Board Games", new Guid("00000000-0000-0000-0000-000000000003"), "board-games", null },
                    { new Guid("00000000-0000-0000-0000-000000000019"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Dining sets, loungers, and parasols.", 1, "https://images.unsplash.com/photo-1600210492493-0946911123ea?auto=format&fit=crop&w=120&h=120", false, "Garden Furniture", new Guid("00000000-0000-0000-0000-000000000004"), "garden-furniture", null },
                    { new Guid("00000000-0000-0000-0000-000000000020"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Charcoal, gas, and electric barbecues.", 2, "https://images.unsplash.com/photo-1416879595882-3373a0480b5b?auto=format&fit=crop&w=120&h=120", false, "BBQs & Grills", new Guid("00000000-0000-0000-0000-000000000004"), "bbqs-grills", null },
                    { new Guid("00000000-0000-0000-0000-000000000021"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Coffee machines, air fryers, blenders, and more.", 1, "https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?auto=format&fit=crop&w=120&h=120", false, "Kitchen Appliances", new Guid("00000000-0000-0000-0000-000000000005"), "kitchen-appliances", null },
                    { new Guid("00000000-0000-0000-0000-000000000022"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Washing machines, vacuums, and steam mops.", 2, "https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?auto=format&fit=crop&w=120&h=120", false, "Laundry & Cleaning", new Guid("00000000-0000-0000-0000-000000000005"), "laundry-cleaning", null },
                    { new Guid("00000000-0000-0000-0000-000000000023"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Weights, dumbbells, yoga mats, and home gym gear.", 1, "https://images.unsplash.com/photo-1517836357463-d25dfeac3438?auto=format&fit=crop&w=120&h=120", false, "Fitness Equipment", new Guid("00000000-0000-0000-0000-000000000006"), "fitness-equipment", null },
                    { new Guid("00000000-0000-0000-0000-000000000024"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Bikes, tents, and outdoor adventure equipment.", 2, "https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b?auto=format&fit=crop&w=120&h=120", false, "Outdoor Sports", new Guid("00000000-0000-0000-0000-000000000006"), "outdoor-sports", null },
                    { new Guid("00000000-0000-0000-0000-000000000025"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Straighteners, curlers, dryers, and hair tools.", 1, "https://images.unsplash.com/photo-1596462502278-27bfdc403348?auto=format&fit=crop&w=120&h=120", false, "Hair & Styling", new Guid("00000000-0000-0000-0000-000000000007"), "hair-styling", null },
                    { new Guid("00000000-0000-0000-0000-000000000026"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Moisturisers, serums, cleansers, and skin kits.", 2, "https://images.unsplash.com/photo-1596462502278-27bfdc403348?auto=format&fit=crop&w=120&h=120", false, "Skin Care", new Guid("00000000-0000-0000-0000-000000000007"), "skin-care", null },
                    { new Guid("00000000-0000-0000-0000-000000000027"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Cots, cot beds, wardrobes, and nursery sets.", 1, "https://images.unsplash.com/photo-1515488042361-ee00e0ddd4e4?auto=format&fit=crop&w=120&h=120", false, "Nursery Furniture", new Guid("00000000-0000-0000-0000-000000000008"), "nursery-furniture", null },
                    { new Guid("00000000-0000-0000-0000-000000000028"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Bottle warmers, monitors, and baby healthcare.", 2, "https://images.unsplash.com/photo-1515488042361-ee00e0ddd4e4?auto=format&fit=crop&w=120&h=120", false, "Feeding & Healthcare", new Guid("00000000-0000-0000-0000-000000000008"), "feeding-healthcare", null },
                    { new Guid("00000000-0000-0000-0000-000000000029"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Vitamins, minerals, and nutritional supplements.", 1, "https://images.unsplash.com/photo-1505576399279-565b52d4ac71?auto=format&fit=crop&w=120&h=120", false, "Supplements", new Guid("00000000-0000-0000-0000-000000000009"), "supplements", null },
                    { new Guid("00000000-0000-0000-0000-000000000030"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Scales, blood pressure monitors, and trackers.", 2, "https://images.unsplash.com/photo-1505576399279-565b52d4ac71?auto=format&fit=crop&w=120&h=120", false, "Wellness Devices", new Guid("00000000-0000-0000-0000-000000000009"), "wellness-devices", null },
                    { new Guid("00000000-0000-0000-0000-000000000031"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Men's tops, bottoms, jackets, and accessories.", 1, "https://images.unsplash.com/photo-1441986300917-64674bd600d8?auto=format&fit=crop&w=120&h=120", false, "Men's Fashion", new Guid("00000000-0000-0000-0000-000000000010"), "mens-fashion", null },
                    { new Guid("00000000-0000-0000-0000-000000000032"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Women's dresses, tops, and seasonal fashion.", 2, "https://images.unsplash.com/photo-1441986300917-64674bd600d8?auto=format&fit=crop&w=120&h=120", false, "Women's Fashion", new Guid("00000000-0000-0000-0000-000000000010"), "womens-fashion", null },
                    { new Guid("00000000-0000-0000-0000-000000000033"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Gold, silver, and gemstone rings and necklaces.", 1, "https://images.unsplash.com/photo-1515562141207-7a88fb7ce338?auto=format&fit=crop&w=120&h=120", false, "Rings & Necklaces", new Guid("00000000-0000-0000-0000-000000000011"), "rings-necklaces", null },
                    { new Guid("00000000-0000-0000-0000-000000000034"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Charm bracelets, cuffs, studs, and drop earrings.", 2, "https://images.unsplash.com/photo-1515562141207-7a88fb7ce338?auto=format&fit=crop&w=120&h=120", false, "Bracelets & Earrings", new Guid("00000000-0000-0000-0000-000000000011"), "bracelets-earrings", null },
                    { new Guid("00000000-0000-0000-0000-000000000035"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Curated pamper sets, food hampers, and gift bundles.", 1, "https://images.unsplash.com/photo-1549465220-1a8b9238cd48?auto=format&fit=crop&w=120&h=120", false, "Gift Sets", new Guid("00000000-0000-0000-0000-000000000012"), "gift-sets", null },
                    { new Guid("00000000-0000-0000-0000-000000000036"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Christmas, birthday, and special occasion gift ideas.", 2, "https://images.unsplash.com/photo-1549465220-1a8b9238cd48?auto=format&fit=crop&w=120&h=120", false, "Seasonal Gifts", new Guid("00000000-0000-0000-0000-000000000012"), "seasonal-gifts", null }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "BrandId", "CategoryId", "CreatedAtUtc", "FullDescription", "IsDeleted", "IsFeatured", "Name", "Price", "Rating", "ReviewCount", "ShortDescription", "Sku", "Slug", "UpdatedAtUtc", "WasPrice" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000313"), new Guid("00000000-0000-0000-0000-000000000101"), new Guid("00000000-0000-0000-0000-000000000013"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Ultra-thin 13 inch laptop with all-day battery and fast SSD. Built for the Selqi demo catalog with deterministic seed data.", false, true, "Astra 13 Ultrabook", 499.99m, 4.5m, 72, "Ultra-thin 13 inch laptop with all-day battery and fast SSD.", "SKU-AST-130", "astra-13-ultrabook", null, 599.99m },
                    { new Guid("00000000-0000-0000-0000-000000000314"), new Guid("00000000-0000-0000-0000-000000000102"), new Guid("00000000-0000-0000-0000-000000000014"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Compact earbuds with active noise cancellation and deep bass. Built for the Selqi demo catalog with deterministic seed data.", false, true, "Northline True Wireless Earbuds", 69.99m, 4.4m, 213, "Compact earbuds with active noise cancellation and deep bass.", "SKU-NOR-E20", "northline-true-wireless-earbuds", null, 89.99m },
                    { new Guid("00000000-0000-0000-0000-000000000315"), new Guid("00000000-0000-0000-0000-000000000105"), new Guid("00000000-0000-0000-0000-000000000017"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "High-speed remote control racing car for kids aged 6 and up. Built for the Selqi demo catalog with deterministic seed data.", false, true, "Playwell RC Racing Car", 44.99m, 4.6m, 87, "High-speed remote control racing car for kids aged 6 and up.", "SKU-PLA-RC1", "playwell-rc-racing-car", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000316"), new Guid("00000000-0000-0000-0000-000000000105"), new Guid("00000000-0000-0000-0000-000000000018"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Classic strategy board game for 2–6 players, ages 8 and up. Built for the Selqi demo catalog with deterministic seed data.", false, false, "Playwell Family Board Game", 24.99m, 4.8m, 156, "Classic strategy board game for 2–6 players, ages 8 and up.", "SKU-PLA-BG2", "playwell-family-board-game", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000317"), new Guid("00000000-0000-0000-0000-000000000102"), new Guid("00000000-0000-0000-0000-000000000019"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Four-seater rattan dining table and chair set with cushions. Built for the Selqi demo catalog with deterministic seed data.", false, true, "Northline Garden Dining Set", 299.99m, 4.2m, 34, "Four-seater rattan dining table and chair set with cushions.", "SKU-NOR-GD4", "northline-garden-dining-set", null, 349.99m },
                    { new Guid("00000000-0000-0000-0000-000000000318"), new Guid("00000000-0000-0000-0000-000000000103"), new Guid("00000000-0000-0000-0000-000000000021"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Compact bean-to-cup espresso machine with built-in grinder. Built for the Selqi demo catalog with deterministic seed data.", false, true, "Kestral Espresso Machine", 119.99m, 4.6m, 198, "Compact bean-to-cup espresso machine with built-in grinder.", "SKU-KES-EM3", "kestral-espresso-machine", null, 149.99m },
                    { new Guid("00000000-0000-0000-0000-000000000319"), new Guid("00000000-0000-0000-0000-000000000106"), new Guid("00000000-0000-0000-0000-000000000023"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Adjustable 2–24 kg dumbbell set with quick-lock dial system. Built for the Selqi demo catalog with deterministic seed data.", false, true, "VeloFit Adjustable Dumbbell Set", 129.99m, 4.5m, 91, "Adjustable 2–24 kg dumbbell set with quick-lock dial system.", "SKU-VEL-DB5", "velofit-adjustable-dumbbell-set", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000320"), new Guid("00000000-0000-0000-0000-000000000101"), new Guid("00000000-0000-0000-0000-000000000025"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Ceramic floating plates with ionic technology for frizz-free styling. Built for the Selqi demo catalog with deterministic seed data.", false, false, "Astra Pro Hair Straightener", 59.99m, 4.3m, 143, "Ceramic floating plates with ionic technology for frizz-free styling.", "SKU-AST-HS9", "astra-pro-hair-straightener", null, 79.99m },
                    { new Guid("00000000-0000-0000-0000-000000000321"), new Guid("00000000-0000-0000-0000-000000000104"), new Guid("00000000-0000-0000-0000-000000000027"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Converts from cot to toddler bed, with adjustable base heights. Built for the Selqi demo catalog with deterministic seed data.", false, true, "Homekind Convertible Cot Bed", 189.99m, 4.7m, 62, "Converts from cot to toddler bed, with adjustable base heights.", "SKU-HOM-CB8", "homekind-convertible-cot-bed", null, 229.99m },
                    { new Guid("00000000-0000-0000-0000-000000000322"), new Guid("00000000-0000-0000-0000-000000000102"), new Guid("00000000-0000-0000-0000-000000000029"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Daily multivitamin complex with vitamin D, zinc, and magnesium. Built for the Selqi demo catalog with deterministic seed data.", false, false, "Northline Vitamin Complex 90", 19.99m, 4.4m, 289, "Daily multivitamin complex with vitamin D, zinc, and magnesium.", "SKU-NOR-VC9", "northline-vitamin-complex-90", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000323"), new Guid("00000000-0000-0000-0000-000000000103"), new Guid("00000000-0000-0000-0000-000000000030"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Bluetooth body composition scale tracking weight, BMI, and fat. Built for the Selqi demo catalog with deterministic seed data.", false, true, "Kestral Smart Body Scale", 34.99m, 4.3m, 177, "Bluetooth body composition scale tracking weight, BMI, and fat.", "SKU-KES-SC2", "kestral-smart-body-scale", null, 44.99m },
                    { new Guid("00000000-0000-0000-0000-000000000324"), new Guid("00000000-0000-0000-0000-000000000101"), new Guid("00000000-0000-0000-0000-000000000031"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Heavyweight cotton-blend zip hoodie with kangaroo pocket. Built for the Selqi demo catalog with deterministic seed data.", false, false, "Astra Premium Zip Hoodie", 39.99m, 4.2m, 95, "Heavyweight cotton-blend zip hoodie with kangaroo pocket.", "SKU-AST-ZH4", "astra-premium-zip-hoodie", null, 49.99m },
                    { new Guid("00000000-0000-0000-0000-000000000325"), new Guid("00000000-0000-0000-0000-000000000101"), new Guid("00000000-0000-0000-0000-000000000033"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Delicate sterling silver heart pendant on a fine trace chain. Built for the Selqi demo catalog with deterministic seed data.", false, false, "Astra Sterling Silver Pendant", 29.99m, 4.5m, 63, "Delicate sterling silver heart pendant on a fine trace chain.", "SKU-AST-SP1", "astra-sterling-silver-pendant", null, null },
                    { new Guid("00000000-0000-0000-0000-000000000326"), new Guid("00000000-0000-0000-0000-000000000104"), new Guid("00000000-0000-0000-0000-000000000035"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Indulgent bath and body pamper set in a keepsake gift box. Built for the Selqi demo catalog with deterministic seed data.", false, true, "Homekind Luxury Spa Gift Set", 49.99m, 4.8m, 112, "Indulgent bath and body pamper set in a keepsake gift box.", "SKU-HOM-GS6", "homekind-luxury-spa-gift-set", null, 64.99m }
                });

            migrationBuilder.InsertData(
                table: "Inventory",
                columns: new[] { "Id", "ProductId", "QuantityAvailable", "StoreId", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000761"), new Guid("00000000-0000-0000-0000-000000000313"), 0, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000762"), new Guid("00000000-0000-0000-0000-000000000313"), 17, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000763"), new Guid("00000000-0000-0000-0000-000000000313"), 18, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000764"), new Guid("00000000-0000-0000-0000-000000000313"), 19, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000765"), new Guid("00000000-0000-0000-0000-000000000313"), 0, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000766"), new Guid("00000000-0000-0000-0000-000000000314"), 17, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000767"), new Guid("00000000-0000-0000-0000-000000000314"), 18, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000768"), new Guid("00000000-0000-0000-0000-000000000314"), 19, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000769"), new Guid("00000000-0000-0000-0000-000000000314"), 0, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000770"), new Guid("00000000-0000-0000-0000-000000000314"), 21, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000771"), new Guid("00000000-0000-0000-0000-000000000315"), 18, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000772"), new Guid("00000000-0000-0000-0000-000000000315"), 19, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000773"), new Guid("00000000-0000-0000-0000-000000000315"), 0, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000774"), new Guid("00000000-0000-0000-0000-000000000315"), 21, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000775"), new Guid("00000000-0000-0000-0000-000000000315"), 22, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000776"), new Guid("00000000-0000-0000-0000-000000000316"), 19, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000777"), new Guid("00000000-0000-0000-0000-000000000316"), 0, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000778"), new Guid("00000000-0000-0000-0000-000000000316"), 21, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000779"), new Guid("00000000-0000-0000-0000-000000000316"), 22, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000780"), new Guid("00000000-0000-0000-0000-000000000316"), 23, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000781"), new Guid("00000000-0000-0000-0000-000000000317"), 0, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000782"), new Guid("00000000-0000-0000-0000-000000000317"), 21, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000783"), new Guid("00000000-0000-0000-0000-000000000317"), 22, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000784"), new Guid("00000000-0000-0000-0000-000000000317"), 23, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000785"), new Guid("00000000-0000-0000-0000-000000000317"), 0, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000786"), new Guid("00000000-0000-0000-0000-000000000318"), 21, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000787"), new Guid("00000000-0000-0000-0000-000000000318"), 22, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000788"), new Guid("00000000-0000-0000-0000-000000000318"), 23, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000789"), new Guid("00000000-0000-0000-0000-000000000318"), 0, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000790"), new Guid("00000000-0000-0000-0000-000000000318"), 25, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000791"), new Guid("00000000-0000-0000-0000-000000000319"), 22, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000792"), new Guid("00000000-0000-0000-0000-000000000319"), 23, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000793"), new Guid("00000000-0000-0000-0000-000000000319"), 0, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000794"), new Guid("00000000-0000-0000-0000-000000000319"), 25, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000795"), new Guid("00000000-0000-0000-0000-000000000319"), 26, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000796"), new Guid("00000000-0000-0000-0000-000000000320"), 23, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000797"), new Guid("00000000-0000-0000-0000-000000000320"), 0, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000798"), new Guid("00000000-0000-0000-0000-000000000320"), 25, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000799"), new Guid("00000000-0000-0000-0000-000000000320"), 26, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000800"), new Guid("00000000-0000-0000-0000-000000000320"), 27, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000801"), new Guid("00000000-0000-0000-0000-000000000321"), 0, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000802"), new Guid("00000000-0000-0000-0000-000000000321"), 25, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000803"), new Guid("00000000-0000-0000-0000-000000000321"), 26, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000804"), new Guid("00000000-0000-0000-0000-000000000321"), 27, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000805"), new Guid("00000000-0000-0000-0000-000000000321"), 0, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000806"), new Guid("00000000-0000-0000-0000-000000000322"), 25, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000807"), new Guid("00000000-0000-0000-0000-000000000322"), 26, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000808"), new Guid("00000000-0000-0000-0000-000000000322"), 27, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000809"), new Guid("00000000-0000-0000-0000-000000000322"), 0, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000810"), new Guid("00000000-0000-0000-0000-000000000322"), 29, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000811"), new Guid("00000000-0000-0000-0000-000000000323"), 26, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000812"), new Guid("00000000-0000-0000-0000-000000000323"), 27, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000813"), new Guid("00000000-0000-0000-0000-000000000323"), 0, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000814"), new Guid("00000000-0000-0000-0000-000000000323"), 29, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000815"), new Guid("00000000-0000-0000-0000-000000000323"), 30, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000816"), new Guid("00000000-0000-0000-0000-000000000324"), 27, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000817"), new Guid("00000000-0000-0000-0000-000000000324"), 0, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000818"), new Guid("00000000-0000-0000-0000-000000000324"), 29, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000819"), new Guid("00000000-0000-0000-0000-000000000324"), 30, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000820"), new Guid("00000000-0000-0000-0000-000000000324"), 31, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000821"), new Guid("00000000-0000-0000-0000-000000000325"), 0, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000822"), new Guid("00000000-0000-0000-0000-000000000325"), 29, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000823"), new Guid("00000000-0000-0000-0000-000000000325"), 30, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000824"), new Guid("00000000-0000-0000-0000-000000000325"), 31, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000825"), new Guid("00000000-0000-0000-0000-000000000325"), 0, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000826"), new Guid("00000000-0000-0000-0000-000000000326"), 29, new Guid("00000000-0000-0000-0000-000000000201"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000827"), new Guid("00000000-0000-0000-0000-000000000326"), 30, new Guid("00000000-0000-0000-0000-000000000202"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000828"), new Guid("00000000-0000-0000-0000-000000000326"), 31, new Guid("00000000-0000-0000-0000-000000000203"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000829"), new Guid("00000000-0000-0000-0000-000000000326"), 0, new Guid("00000000-0000-0000-0000-000000000204"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000830"), new Guid("00000000-0000-0000-0000-000000000326"), 33, new Guid("00000000-0000-0000-0000-000000000205"), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "AltText", "DisplayOrder", "ProductId", "Url" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000425"), "Astra 13 Ultrabook", 1, new Guid("00000000-0000-0000-0000-000000000313"), "https://images.unsplash.com/photo-1531297484001-80022131f5a1?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000426"), "Astra 13 Ultrabook on desk", 2, new Guid("00000000-0000-0000-0000-000000000313"), "https://images.unsplash.com/photo-1496181133206-80ce9b88a853?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000427"), "Northline True Wireless Earbuds", 1, new Guid("00000000-0000-0000-0000-000000000314"), "https://images.unsplash.com/photo-1590658268037-6bf12165a8df?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000428"), "Northline Earbuds case open", 2, new Guid("00000000-0000-0000-0000-000000000314"), "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000429"), "Playwell RC Racing Car", 1, new Guid("00000000-0000-0000-0000-000000000315"), "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000430"), "Playwell RC Racing Car in action", 2, new Guid("00000000-0000-0000-0000-000000000315"), "https://images.unsplash.com/photo-1587654780291-39c9404d746b?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000431"), "Playwell Family Board Game", 1, new Guid("00000000-0000-0000-0000-000000000316"), "https://images.unsplash.com/photo-1550459045-66bab08aab22?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000432"), "Playwell Board Game pieces", 2, new Guid("00000000-0000-0000-0000-000000000316"), "https://images.unsplash.com/photo-1566576912321-d58ddd7a6088?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000433"), "Northline Garden Dining Set", 1, new Guid("00000000-0000-0000-0000-000000000317"), "https://images.unsplash.com/photo-1600210492493-0946911123ea?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000434"), "Northline Garden Dining Set styled", 2, new Guid("00000000-0000-0000-0000-000000000317"), "https://images.unsplash.com/photo-1561702771-eb3cf35eedd2?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000435"), "Kestral Espresso Machine", 1, new Guid("00000000-0000-0000-0000-000000000318"), "https://images.unsplash.com/photo-1495474472287-4d71bcdd2085?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000436"), "Kestral Espresso Machine brewing", 2, new Guid("00000000-0000-0000-0000-000000000318"), "https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000437"), "VeloFit Adjustable Dumbbell Set", 1, new Guid("00000000-0000-0000-0000-000000000319"), "https://images.unsplash.com/photo-1583454110551-21f2fa2afe61?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000438"), "VeloFit Dumbbells in use", 2, new Guid("00000000-0000-0000-0000-000000000319"), "https://images.unsplash.com/photo-1534438327276-14e5300c3a48?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000439"), "Astra Pro Hair Straightener", 1, new Guid("00000000-0000-0000-0000-000000000320"), "https://images.unsplash.com/photo-1522338242992-e1a54906a8da?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000440"), "Astra Hair Straightener styling", 2, new Guid("00000000-0000-0000-0000-000000000320"), "https://images.unsplash.com/photo-1527799820374-dcf8d9d4a388?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000441"), "Homekind Convertible Cot Bed", 1, new Guid("00000000-0000-0000-0000-000000000321"), "https://images.unsplash.com/photo-1515488042361-ee00e0ddd4e4?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000442"), "Homekind Cot Bed as toddler bed", 2, new Guid("00000000-0000-0000-0000-000000000321"), "https://images.unsplash.com/photo-1491013516836-7db643ee125a?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000443"), "Northline Vitamin Complex 90", 1, new Guid("00000000-0000-0000-0000-000000000322"), "https://images.unsplash.com/photo-1505576399279-565b52d4ac71?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000444"), "Northline Vitamin Complex capsules", 2, new Guid("00000000-0000-0000-0000-000000000322"), "https://images.unsplash.com/photo-1512207736890-6ffed8a84e8d?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000445"), "Kestral Smart Body Scale", 1, new Guid("00000000-0000-0000-0000-000000000323"), "https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000446"), "Kestral Scale in bathroom", 2, new Guid("00000000-0000-0000-0000-000000000323"), "https://images.unsplash.com/photo-1517836357463-d25dfeac3438?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000447"), "Astra Premium Zip Hoodie", 1, new Guid("00000000-0000-0000-0000-000000000324"), "https://images.unsplash.com/photo-1620799140408-edc6dcb6d633?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000448"), "Astra Zip Hoodie styled", 2, new Guid("00000000-0000-0000-0000-000000000324"), "https://images.unsplash.com/photo-1441986300917-64674bd600d8?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000449"), "Astra Sterling Silver Pendant", 1, new Guid("00000000-0000-0000-0000-000000000325"), "https://images.unsplash.com/photo-1515562141207-7a88fb7ce338?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000450"), "Astra Silver Pendant on model", 2, new Guid("00000000-0000-0000-0000-000000000325"), "https://images.unsplash.com/photo-1611652022419-a9419f74343d?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000451"), "Homekind Luxury Spa Gift Set", 1, new Guid("00000000-0000-0000-0000-000000000326"), "https://images.unsplash.com/photo-1549465220-1a8b9238cd48?auto=format&fit=crop&w=600&h=500" },
                    { new Guid("00000000-0000-0000-0000-000000000452"), "Homekind Spa Gift Set contents", 2, new Guid("00000000-0000-0000-0000-000000000326"), "https://images.unsplash.com/photo-1596462502278-27bfdc403348?auto=format&fit=crop&w=600&h=500" }
                });

            migrationBuilder.InsertData(
                table: "ProductSpecifications",
                columns: new[] { "Id", "DisplayOrder", "Name", "ProductId", "Value" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000525"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000313"), "SKU-AST-130" },
                    { new Guid("00000000-0000-0000-0000-000000000526"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000313"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000527"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000314"), "SKU-NOR-E20" },
                    { new Guid("00000000-0000-0000-0000-000000000528"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000314"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000529"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000315"), "SKU-PLA-RC1" },
                    { new Guid("00000000-0000-0000-0000-000000000530"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000315"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000531"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000316"), "SKU-PLA-BG2" },
                    { new Guid("00000000-0000-0000-0000-000000000532"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000316"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000533"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000317"), "SKU-NOR-GD4" },
                    { new Guid("00000000-0000-0000-0000-000000000534"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000317"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000535"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000318"), "SKU-KES-EM3" },
                    { new Guid("00000000-0000-0000-0000-000000000536"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000318"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000537"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000319"), "SKU-VEL-DB5" },
                    { new Guid("00000000-0000-0000-0000-000000000538"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000319"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000539"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000320"), "SKU-AST-HS9" },
                    { new Guid("00000000-0000-0000-0000-000000000540"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000320"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000541"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000321"), "SKU-HOM-CB8" },
                    { new Guid("00000000-0000-0000-0000-000000000542"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000321"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000543"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000322"), "SKU-NOR-VC9" },
                    { new Guid("00000000-0000-0000-0000-000000000544"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000322"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000545"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000323"), "SKU-KES-SC2" },
                    { new Guid("00000000-0000-0000-0000-000000000546"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000323"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000547"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000324"), "SKU-AST-ZH4" },
                    { new Guid("00000000-0000-0000-0000-000000000548"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000324"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000549"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000325"), "SKU-AST-SP1" },
                    { new Guid("00000000-0000-0000-0000-000000000550"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000325"), "1 year" },
                    { new Guid("00000000-0000-0000-0000-000000000551"), 1, "SKU", new Guid("00000000-0000-0000-0000-000000000326"), "SKU-HOM-GS6" },
                    { new Guid("00000000-0000-0000-0000-000000000552"), 2, "Warranty", new Guid("00000000-0000-0000-0000-000000000326"), "1 year" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Categories_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Categories_ParentCategoryId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ParentCategoryId",
                table: "Categories");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000024"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000026"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000028"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000032"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000034"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000036"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000761"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000762"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000763"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000764"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000765"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000766"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000767"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000768"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000769"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000770"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000771"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000772"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000773"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000774"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000775"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000776"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000777"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000778"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000779"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000780"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000781"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000782"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000783"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000784"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000785"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000786"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000787"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000788"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000789"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000790"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000791"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000792"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000793"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000794"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000795"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000796"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000797"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000798"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000799"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000800"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000801"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000802"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000803"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000804"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000805"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000806"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000807"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000808"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000809"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000810"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000811"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000812"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000813"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000814"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000815"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000816"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000817"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000818"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000819"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000820"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000821"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000822"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000823"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000824"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000825"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000826"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000827"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000828"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000829"));

            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000830"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000425"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000426"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000427"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000428"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000429"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000430"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000431"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000432"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000433"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000434"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000435"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000436"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000437"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000438"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000439"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000440"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000441"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000442"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000443"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000444"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000445"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000446"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000447"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000448"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000449"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000450"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000451"));

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000452"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000525"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000526"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000527"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000528"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000529"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000530"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000531"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000532"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000533"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000534"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000535"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000536"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000537"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000538"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000539"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000540"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000541"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000542"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000543"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000544"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000545"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000546"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000547"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000548"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000549"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000550"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000551"));

            migrationBuilder.DeleteData(
                table: "ProductSpecifications",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000552"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000313"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000314"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000315"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000316"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000317"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000318"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000319"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000320"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000321"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000322"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000323"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000324"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000325"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000326"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000021"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000023"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000025"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000027"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000029"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000030"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000031"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000033"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000035"));

            migrationBuilder.DropColumn(
                name: "ParentCategoryId",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"),
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"),
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000006"),
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000007"),
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000008"),
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000009"),
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"),
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"),
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000012"),
                column: "ImageUrl",
                value: null);
        }
    }
}
