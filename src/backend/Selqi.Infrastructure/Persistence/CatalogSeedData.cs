using Selqi.Domain.Catalog;

namespace Selqi.Infrastructure.Persistence;

internal static class CatalogSeedData
{
    private static readonly DateTime SeededAtUtc = new(2026, 05, 10, 0, 0, 0, DateTimeKind.Utc);

    internal static readonly Category[] Categories =
    [
        // ── Top-level categories (1–12) ──────────────────────────────────────────
        new() { Id = Id(1),  Name = "Technology", Slug = "technology", Description = "Computing, phones, gaming, and smart home essentials.", ImageUrl = $"{Cdn}/photo-1498049794561-7780e7231661{CatParams}", DisplayOrder = 1,  CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(2),  Name = "Home",        Slug = "home",        Description = "Furniture, appliances, storage, and home improvement.",  ImageUrl = $"{Cdn}/photo-1555041469-a586c61ea9bc{CatParams}", DisplayOrder = 2,  CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(3),  Name = "Toys",        Slug = "toys",        Description = "Toys, games, learning, and outdoor play.",               ImageUrl = $"{Cdn}/photo-1587654780291-39c9404d746b{CatParams}", DisplayOrder = 3,  CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(4),  Name = "Garden",      Slug = "garden",      Description = "Garden furniture, tools, BBQs, and seasonal picks.",     ImageUrl = $"{Cdn}/photo-1416879595882-3373a0480b5b{CatParams}", DisplayOrder = 4,  CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(5),  Name = "Appliances",  Slug = "appliances",  Description = "Kitchen, laundry, cleaning, and everyday appliances.",   ImageUrl = $"{Cdn}/photo-1556909114-f6e7ad7d3136{CatParams}", DisplayOrder = 5,  CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(6),  Name = "Sports",      Slug = "sports",      Description = "Fitness equipment, bikes, and active lifestyle products.", ImageUrl = $"{Cdn}/photo-1517836357463-d25dfeac3438{CatParams}", DisplayOrder = 6,  CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(7),  Name = "Beauty",      Slug = "beauty",      Description = "Hair care, grooming, wellness, and personal care.",       ImageUrl = $"{Cdn}/photo-1596462502278-27bfdc403348{CatParams}", DisplayOrder = 7,  CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(8),  Name = "Baby",        Slug = "baby",        Description = "Nursery, travel, feeding, and baby care essentials.",     ImageUrl = $"{Cdn}/photo-1515488042361-ee00e0ddd4e4{CatParams}", DisplayOrder = 8,  CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(9),  Name = "Health",      Slug = "health",      Description = "Supplements, vitamins, wellbeing, and pharmacy essentials.", ImageUrl = $"{Cdn}/photo-1505576399279-565b52d4ac71{CatParams}", DisplayOrder = 9,  CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(10), Name = "Clothing",    Slug = "clothing",    Description = "Men's, women's, and children's fashion and accessories.", ImageUrl = $"{Cdn}/photo-1441986300917-64674bd600d8{CatParams}", DisplayOrder = 10, CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(11), Name = "Jewellery",   Slug = "jewellery",   Description = "Rings, necklaces, bracelets, and fine accessories.",      ImageUrl = $"{Cdn}/photo-1515562141207-7a88fb7ce338{CatParams}", DisplayOrder = 11, CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(12), Name = "Gifts",       Slug = "gifts",       Description = "Seasonal gifts, hampers, and special occasion ideas.",    ImageUrl = $"{Cdn}/photo-1549465220-1a8b9238cd48{CatParams}", DisplayOrder = 12, CreatedAtUtc = SeededAtUtc },

        // ── Sub-categories: Technology (parent 1) ────────────────────────────────
        new() { Id = Id(13), Name = "Laptops & Desktops",  Slug = "laptops-desktops",  Description = "Laptops, ultrabooks, and desktop computers.", ImageUrl = $"{Cdn}/photo-1496181133206-80ce9b88a853{CatParams}", ParentCategoryId = Id(1), DisplayOrder = 1, CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(14), Name = "Phones & Wearables",  Slug = "phones-wearables",  Description = "Smartphones, smartwatches, and earbuds.",      ImageUrl = $"{Cdn}/photo-1523275335684-37898b6baf30{CatParams}", ParentCategoryId = Id(1), DisplayOrder = 2, CreatedAtUtc = SeededAtUtc },

        // ── Sub-categories: Home (parent 2) ──────────────────────────────────────
        new() { Id = Id(15), Name = "Sofas & Chairs",  Slug = "sofas-chairs",  Description = "Sofas, armchairs, and accent seating.",      ImageUrl = $"{Cdn}/photo-1555041469-a586c61ea9bc{CatParams}", ParentCategoryId = Id(2), DisplayOrder = 1, CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(16), Name = "Lighting & Lamps", Slug = "lighting-lamps", Description = "Floor lamps, desk lamps, and pendant lights.", ImageUrl = $"{Cdn}/photo-1534575950704-07c5a26b1e00{CatParams}", ParentCategoryId = Id(2), DisplayOrder = 2, CreatedAtUtc = SeededAtUtc },

        // ── Sub-categories: Toys (parent 3) ──────────────────────────────────────
        new() { Id = Id(17), Name = "Kids' Toys",   Slug = "kids-toys",   Description = "RC cars, action figures, and outdoor play toys.", ImageUrl = $"{Cdn}/photo-1587654780291-39c9404d746b{CatParams}", ParentCategoryId = Id(3), DisplayOrder = 1, CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(18), Name = "Board Games",  Slug = "board-games", Description = "Family board games, card games, and puzzles.",    ImageUrl = $"{Cdn}/photo-1566576912321-d58ddd7a6088{CatParams}", ParentCategoryId = Id(3), DisplayOrder = 2, CreatedAtUtc = SeededAtUtc },

        // ── Sub-categories: Garden (parent 4) ────────────────────────────────────
        new() { Id = Id(19), Name = "Garden Furniture", Slug = "garden-furniture", Description = "Dining sets, loungers, and parasols.",    ImageUrl = $"{Cdn}/photo-1600210492493-0946911123ea{CatParams}", ParentCategoryId = Id(4), DisplayOrder = 1, CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(20), Name = "BBQs & Grills",    Slug = "bbqs-grills",      Description = "Charcoal, gas, and electric barbecues.", ImageUrl = $"{Cdn}/photo-1416879595882-3373a0480b5b{CatParams}", ParentCategoryId = Id(4), DisplayOrder = 2, CreatedAtUtc = SeededAtUtc },

        // ── Sub-categories: Appliances (parent 5) ────────────────────────────────
        new() { Id = Id(21), Name = "Kitchen Appliances", Slug = "kitchen-appliances", Description = "Coffee machines, air fryers, blenders, and more.", ImageUrl = $"{Cdn}/photo-1556909114-f6e7ad7d3136{CatParams}", ParentCategoryId = Id(5), DisplayOrder = 1, CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(22), Name = "Laundry & Cleaning", Slug = "laundry-cleaning",   Description = "Washing machines, vacuums, and steam mops.",        ImageUrl = $"{Cdn}/photo-1556909114-f6e7ad7d3136{CatParams}", ParentCategoryId = Id(5), DisplayOrder = 2, CreatedAtUtc = SeededAtUtc },

        // ── Sub-categories: Sports (parent 6) ────────────────────────────────────
        new() { Id = Id(23), Name = "Fitness Equipment", Slug = "fitness-equipment", Description = "Weights, dumbbells, yoga mats, and home gym gear.", ImageUrl = $"{Cdn}/photo-1517836357463-d25dfeac3438{CatParams}", ParentCategoryId = Id(6), DisplayOrder = 1, CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(24), Name = "Outdoor Sports",    Slug = "outdoor-sports",    Description = "Bikes, tents, and outdoor adventure equipment.",     ImageUrl = $"{Cdn}/photo-1571019613454-1cb2f99b2d8b{CatParams}", ParentCategoryId = Id(6), DisplayOrder = 2, CreatedAtUtc = SeededAtUtc },

        // ── Sub-categories: Beauty (parent 7) ────────────────────────────────────
        new() { Id = Id(25), Name = "Hair & Styling", Slug = "hair-styling", Description = "Straighteners, curlers, dryers, and hair tools.",  ImageUrl = $"{Cdn}/photo-1596462502278-27bfdc403348{CatParams}", ParentCategoryId = Id(7), DisplayOrder = 1, CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(26), Name = "Skin Care",      Slug = "skin-care",    Description = "Moisturisers, serums, cleansers, and skin kits.", ImageUrl = $"{Cdn}/photo-1596462502278-27bfdc403348{CatParams}", ParentCategoryId = Id(7), DisplayOrder = 2, CreatedAtUtc = SeededAtUtc },

        // ── Sub-categories: Baby (parent 8) ──────────────────────────────────────
        new() { Id = Id(27), Name = "Nursery Furniture", Slug = "nursery-furniture", Description = "Cots, cot beds, wardrobes, and nursery sets.",  ImageUrl = $"{Cdn}/photo-1515488042361-ee00e0ddd4e4{CatParams}", ParentCategoryId = Id(8), DisplayOrder = 1, CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(28), Name = "Feeding & Healthcare", Slug = "feeding-healthcare", Description = "Bottle warmers, monitors, and baby healthcare.", ImageUrl = $"{Cdn}/photo-1515488042361-ee00e0ddd4e4{CatParams}", ParentCategoryId = Id(8), DisplayOrder = 2, CreatedAtUtc = SeededAtUtc },

        // ── Sub-categories: Health (parent 9) ────────────────────────────────────
        new() { Id = Id(29), Name = "Supplements",     Slug = "supplements",    Description = "Vitamins, minerals, and nutritional supplements.", ImageUrl = $"{Cdn}/photo-1505576399279-565b52d4ac71{CatParams}", ParentCategoryId = Id(9), DisplayOrder = 1, CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(30), Name = "Wellness Devices", Slug = "wellness-devices", Description = "Scales, blood pressure monitors, and trackers.", ImageUrl = $"{Cdn}/photo-1505576399279-565b52d4ac71{CatParams}", ParentCategoryId = Id(9), DisplayOrder = 2, CreatedAtUtc = SeededAtUtc },

        // ── Sub-categories: Clothing (parent 10) ─────────────────────────────────
        new() { Id = Id(31), Name = "Men's Fashion",   Slug = "mens-fashion",   Description = "Men's tops, bottoms, jackets, and accessories.", ImageUrl = $"{Cdn}/photo-1441986300917-64674bd600d8{CatParams}", ParentCategoryId = Id(10), DisplayOrder = 1, CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(32), Name = "Women's Fashion", Slug = "womens-fashion", Description = "Women's dresses, tops, and seasonal fashion.",   ImageUrl = $"{Cdn}/photo-1441986300917-64674bd600d8{CatParams}", ParentCategoryId = Id(10), DisplayOrder = 2, CreatedAtUtc = SeededAtUtc },

        // ── Sub-categories: Jewellery (parent 11) ────────────────────────────────
        new() { Id = Id(33), Name = "Rings & Necklaces",    Slug = "rings-necklaces",    Description = "Gold, silver, and gemstone rings and necklaces.",  ImageUrl = $"{Cdn}/photo-1515562141207-7a88fb7ce338{CatParams}", ParentCategoryId = Id(11), DisplayOrder = 1, CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(34), Name = "Bracelets & Earrings", Slug = "bracelets-earrings", Description = "Charm bracelets, cuffs, studs, and drop earrings.", ImageUrl = $"{Cdn}/photo-1515562141207-7a88fb7ce338{CatParams}", ParentCategoryId = Id(11), DisplayOrder = 2, CreatedAtUtc = SeededAtUtc },

        // ── Sub-categories: Gifts (parent 12) ────────────────────────────────────
        new() { Id = Id(35), Name = "Gift Sets",      Slug = "gift-sets",      Description = "Curated pamper sets, food hampers, and gift bundles.",   ImageUrl = $"{Cdn}/photo-1549465220-1a8b9238cd48{CatParams}", ParentCategoryId = Id(12), DisplayOrder = 1, CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(36), Name = "Seasonal Gifts", Slug = "seasonal-gifts", Description = "Christmas, birthday, and special occasion gift ideas.", ImageUrl = $"{Cdn}/photo-1549465220-1a8b9238cd48{CatParams}", ParentCategoryId = Id(12), DisplayOrder = 2, CreatedAtUtc = SeededAtUtc },
    ];

    internal static readonly Brand[] Brands =
    [
        new() { Id = Id(101), Name = "Astra", Slug = "astra", CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(102), Name = "Northline", Slug = "northline", CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(103), Name = "Kestral", Slug = "kestral", CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(104), Name = "Homekind", Slug = "homekind", CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(105), Name = "Playwell", Slug = "playwell", CreatedAtUtc = SeededAtUtc },
        new() { Id = Id(106), Name = "VeloFit", Slug = "velofit", CreatedAtUtc = SeededAtUtc }
    ];

    internal static readonly Store[] Stores =
    [
        new() { Id = Id(201), Name = "Central Retail Park", AddressLine1 = "Unit 5, Central Retail Park", Town = "Birmingham", Postcode = "B1 1AA", Phone = "01212 345678", StoreType = StoreType.Standalone, Latitude = 52.4862, Longitude = -1.8904, IsActive = true, CreatedAtUtc = SeededAtUtc, UpdatedAtUtc = SeededAtUtc },
        new() { Id = Id(202), Name = "Riverside Collection", AddressLine1 = "Sainsbury's, Riverside Way", Town = "Leeds", Postcode = "LS1 4PR", Phone = "01132 456789", StoreType = StoreType.Concession, Latitude = 53.7997, Longitude = -1.5492, IsActive = true, CreatedAtUtc = SeededAtUtc, UpdatedAtUtc = SeededAtUtc },
        new() { Id = Id(203), Name = "Metro High Street", AddressLine1 = "42 Market Street", Town = "Manchester", Postcode = "M1 2WD", Phone = "01612 567890", StoreType = StoreType.Standalone, Latitude = 53.4808, Longitude = -2.2426, IsActive = true, CreatedAtUtc = SeededAtUtc, UpdatedAtUtc = SeededAtUtc },
        new() { Id = Id(204), Name = "Harbour Point", AddressLine1 = "Sainsbury's, Harbourside", Town = "Bristol", Postcode = "BS1 6QF", Phone = "01172 678901", StoreType = StoreType.Concession, Latitude = 51.4545, Longitude = -2.5879, IsActive = true, CreatedAtUtc = SeededAtUtc, UpdatedAtUtc = SeededAtUtc },
        new() { Id = Id(205), Name = "Station Square", AddressLine1 = "Sainsbury's Local, Station Square", Town = "London", Postcode = "E1 8QS", Phone = "02071 234567", StoreType = StoreType.CollectionPoint, Latitude = 51.5074, Longitude = -0.0878, IsActive = true, CreatedAtUtc = SeededAtUtc, UpdatedAtUtc = SeededAtUtc }
    ];

    internal static readonly Product[] Products =
    [
        // ── Original products ─────────────────────────────────────────────────────
        Product(301, 1,  101, "Astra 15 Laptop",             "astra-15-laptop",             "A fast everyday laptop with a bright 15 inch display.",           "SKU-AST-150", 699.99m, 749.99m, 4.6m, 184, true),
        Product(302, 1,  102, "Northline Smart Watch",        "northline-smart-watch",        "Fitness tracking, notifications, and week-long battery life.",    "SKU-NOR-W10", 129.99m, null,    4.4m,  89, true),
        Product(303, 1,  103, "Kestral Wireless Headphones",  "kestral-wireless-headphones",  "Noise reducing headphones for work, travel, and music.",          "SKU-KES-H70",  89.99m, 109.99m, 4.3m, 241, false),
        Product(304, 2,  104, "Homekind Storage Cabinet",     "homekind-storage-cabinet",     "Compact two-door storage for hallway, office, or utility rooms.", "SKU-HOM-C22", 149.99m, null,    4.2m,  56, true),
        Product(305, 2,  104, "Homekind Desk Lamp",           "homekind-desk-lamp",           "Adjustable LED desk lamp with touch brightness controls.",        "SKU-HOM-L08",  34.99m,  44.99m, 4.5m, 132, false),
        Product(306, 3,  105, "Playwell Builder Set",         "playwell-builder-set",         "Creative construction set with 650 colorful pieces.",             "SKU-PLA-B65",  39.99m, null,    4.8m, 312, true),
        Product(307, 4,  102, "Northline Patio Chair Pair",   "northline-patio-chair-pair",   "Weather-resistant garden chairs for compact outdoor spaces.",     "SKU-NOR-G02", 119.99m, 139.99m, 4.1m,  47, false),
        Product(308, 5,  103, "Kestral Air Fryer 6L",         "kestral-air-fryer-6l",         "Large capacity air fryer with digital presets.",                  "SKU-KES-AF6",  79.99m,  99.99m, 4.7m, 420, true),
        Product(309, 6,  106, "VeloFit Exercise Bike",        "velofit-exercise-bike",        "Quiet indoor exercise bike with adjustable resistance.",          "SKU-VEL-B11", 249.99m, 299.99m, 4.4m,  75, true),
        Product(310, 7,  101, "Astra Hair Dryer",             "astra-hair-dryer",             "Lightweight ionic hair dryer with diffuser attachment.",          "SKU-AST-HD2",  49.99m, null,    4.2m, 118, false),
        Product(311, 8,  104, "Homekind Travel Cot",          "homekind-travel-cot",          "Foldable travel cot with carry bag and padded base.",             "SKU-HOM-TC4",  69.99m,  84.99m, 4.5m,  63, false),
        Product(312, 1,  101, "Astra Tablet 10",              "astra-tablet-10",              "Portable 10 inch tablet for streaming, reading, and browsing.",   "SKU-AST-T10", 179.99m, null,    4.1m,  98, true),

        // ── Products in sub-categories ────────────────────────────────────────────
        Product(313, 13, 101, "Astra 13 Ultrabook",           "astra-13-ultrabook",           "Ultra-thin 13 inch laptop with all-day battery and fast SSD.",   "SKU-AST-130", 499.99m, 599.99m, 4.5m,  72, true),
        Product(314, 14, 102, "Northline True Wireless Earbuds","northline-true-wireless-earbuds","Compact earbuds with active noise cancellation and deep bass.", "SKU-NOR-E20",  69.99m,  89.99m, 4.4m, 213, true),
        Product(315, 17, 105, "Playwell RC Racing Car",        "playwell-rc-racing-car",       "High-speed remote control racing car for kids aged 6 and up.",   "SKU-PLA-RC1",  44.99m, null,    4.6m,  87, true),
        Product(316, 18, 105, "Playwell Family Board Game",    "playwell-family-board-game",   "Classic strategy board game for 2–6 players, ages 8 and up.",    "SKU-PLA-BG2",  24.99m, null,    4.8m, 156, false),
        Product(317, 19, 102, "Northline Garden Dining Set",   "northline-garden-dining-set",  "Four-seater rattan dining table and chair set with cushions.",    "SKU-NOR-GD4", 299.99m, 349.99m, 4.2m,  34, true),
        Product(318, 21, 103, "Kestral Espresso Machine",      "kestral-espresso-machine",     "Compact bean-to-cup espresso machine with built-in grinder.",    "SKU-KES-EM3", 119.99m, 149.99m, 4.6m, 198, true),
        Product(319, 23, 106, "VeloFit Adjustable Dumbbell Set","velofit-adjustable-dumbbell-set","Adjustable 2–24 kg dumbbell set with quick-lock dial system.", "SKU-VEL-DB5", 129.99m, null,    4.5m,  91, true),
        Product(320, 25, 101, "Astra Pro Hair Straightener",   "astra-pro-hair-straightener",  "Ceramic floating plates with ionic technology for frizz-free styling.", "SKU-AST-HS9",  59.99m,  79.99m, 4.3m, 143, false),
        Product(321, 27, 104, "Homekind Convertible Cot Bed",  "homekind-convertible-cot-bed", "Converts from cot to toddler bed, with adjustable base heights.", "SKU-HOM-CB8", 189.99m, 229.99m, 4.7m,  62, true),
        Product(322, 29, 102, "Northline Vitamin Complex 90",  "northline-vitamin-complex-90",  "Daily multivitamin complex with vitamin D, zinc, and magnesium.", "SKU-NOR-VC9",  19.99m, null,    4.4m, 289, false),
        Product(323, 30, 103, "Kestral Smart Body Scale",      "kestral-smart-body-scale",     "Bluetooth body composition scale tracking weight, BMI, and fat.", "SKU-KES-SC2",  34.99m,  44.99m, 4.3m, 177, true),
        Product(324, 31, 101, "Astra Premium Zip Hoodie",      "astra-premium-zip-hoodie",     "Heavyweight cotton-blend zip hoodie with kangaroo pocket.",       "SKU-AST-ZH4",  39.99m,  49.99m, 4.2m,  95, false),
        Product(325, 33, 101, "Astra Sterling Silver Pendant", "astra-sterling-silver-pendant","Delicate sterling silver heart pendant on a fine trace chain.",   "SKU-AST-SP1",  29.99m, null,    4.5m,  63, false),
        Product(326, 35, 104, "Homekind Luxury Spa Gift Set",  "homekind-luxury-spa-gift-set", "Indulgent bath and body pamper set in a keepsake gift box.",       "SKU-HOM-GS6",  49.99m,  64.99m, 4.8m, 112, true),
    ];

    private const string Cdn = "https://images.unsplash.com";
    private const string Params = "?auto=format&fit=crop&w=600&h=500";
    private const string CatParams = "?auto=format&fit=crop&w=120&h=120";

    internal static readonly ProductImage[] ProductImages =
    [
        // Astra 15 Laptop (301)
        new() { Id = Id(401), ProductId = Id(301), Url = $"{Cdn}/photo-1496181133206-80ce9b88a853{Params}", AltText = "Astra 15 Laptop", DisplayOrder = 1 },
        new() { Id = Id(413), ProductId = Id(301), Url = $"{Cdn}/photo-1525547719571-a2d4ac8945e2{Params}", AltText = "Astra 15 Laptop on desk", DisplayOrder = 2 },
        // Northline Smart Watch (302)
        new() { Id = Id(402), ProductId = Id(302), Url = $"{Cdn}/photo-1523275335684-37898b6baf30{Params}", AltText = "Northline Smart Watch", DisplayOrder = 1 },
        new() { Id = Id(414), ProductId = Id(302), Url = $"{Cdn}/photo-1546868871-7041f2a55e12{Params}", AltText = "Northline Smart Watch on wrist", DisplayOrder = 2 },
        // Kestral Wireless Headphones (303)
        new() { Id = Id(403), ProductId = Id(303), Url = $"{Cdn}/photo-1505740420928-5e560c06d30e{Params}", AltText = "Kestral Wireless Headphones", DisplayOrder = 1 },
        new() { Id = Id(415), ProductId = Id(303), Url = $"{Cdn}/photo-1484704849700-f032a568e944{Params}", AltText = "Kestral Wireless Headphones flat lay", DisplayOrder = 2 },
        // Homekind Storage Cabinet (304)
        new() { Id = Id(404), ProductId = Id(304), Url = $"{Cdn}/photo-1493663284031-b7e3aefcae8e{Params}", AltText = "Homekind Storage Cabinet", DisplayOrder = 1 },
        new() { Id = Id(416), ProductId = Id(304), Url = $"{Cdn}/photo-1555041469-a586c61ea9bc{Params}", AltText = "Homekind Storage Cabinet in room", DisplayOrder = 2 },
        // Homekind Desk Lamp (305)
        new() { Id = Id(405), ProductId = Id(305), Url = $"{Cdn}/photo-1534575950704-07c5a26b1e00{Params}", AltText = "Homekind Desk Lamp", DisplayOrder = 1 },
        new() { Id = Id(417), ProductId = Id(305), Url = $"{Cdn}/photo-1513506003901-1e6a35c98fe8{Params}", AltText = "Homekind Desk Lamp lit", DisplayOrder = 2 },
        // Playwell Builder Set (306)
        new() { Id = Id(406), ProductId = Id(306), Url = $"{Cdn}/photo-1587654780291-39c9404d746b{Params}", AltText = "Playwell Builder Set", DisplayOrder = 1 },
        new() { Id = Id(418), ProductId = Id(306), Url = $"{Cdn}/photo-1566576912321-d58ddd7a6088{Params}", AltText = "Playwell Builder Set pieces", DisplayOrder = 2 },
        // Northline Patio Chair Pair (307)
        new() { Id = Id(407), ProductId = Id(307), Url = $"{Cdn}/photo-1600210492493-0946911123ea{Params}", AltText = "Northline Patio Chair Pair", DisplayOrder = 1 },
        new() { Id = Id(419), ProductId = Id(307), Url = $"{Cdn}/photo-1561702771-eb3cf35eedd2{Params}", AltText = "Northline Patio Chair Pair outdoor setting", DisplayOrder = 2 },
        // Kestral Air Fryer 6L (308)
        new() { Id = Id(408), ProductId = Id(308), Url = $"{Cdn}/photo-1556909114-f6e7ad7d3136{Params}", AltText = "Kestral Air Fryer 6L", DisplayOrder = 1 },
        new() { Id = Id(420), ProductId = Id(308), Url = $"{Cdn}/photo-1604503468506-a8da13d82791{Params}", AltText = "Kestral Air Fryer cooking food", DisplayOrder = 2 },
        // VeloFit Exercise Bike (309)
        new() { Id = Id(409), ProductId = Id(309), Url = $"{Cdn}/photo-1571019613454-1cb2f99b2d8b{Params}", AltText = "VeloFit Exercise Bike", DisplayOrder = 1 },
        new() { Id = Id(421), ProductId = Id(309), Url = $"{Cdn}/photo-1534438327276-14e5300c3a48{Params}", AltText = "VeloFit Exercise Bike in use", DisplayOrder = 2 },
        // Astra Hair Dryer (310)
        new() { Id = Id(410), ProductId = Id(310), Url = $"{Cdn}/photo-1522338242992-e1a54906a8da{Params}", AltText = "Astra Hair Dryer", DisplayOrder = 1 },
        new() { Id = Id(422), ProductId = Id(310), Url = $"{Cdn}/photo-1527799820374-dcf8d9d4a388{Params}", AltText = "Astra Hair Dryer in use", DisplayOrder = 2 },
        // Homekind Travel Cot (311)
        new() { Id = Id(411), ProductId = Id(311), Url = $"{Cdn}/photo-1515488042361-ee00e0ddd4e4{Params}", AltText = "Homekind Travel Cot", DisplayOrder = 1 },
        new() { Id = Id(423), ProductId = Id(311), Url = $"{Cdn}/photo-1491013516836-7db643ee125a{Params}", AltText = "Homekind Travel Cot assembled", DisplayOrder = 2 },
        // Astra Tablet 10 (312)
        new() { Id = Id(412), ProductId = Id(312), Url = $"{Cdn}/photo-1544244015-0df4b3ffc6b0{Params}", AltText = "Astra Tablet 10", DisplayOrder = 1 },
        new() { Id = Id(424), ProductId = Id(312), Url = $"{Cdn}/photo-1512499617640-c74ae3a79d37{Params}", AltText = "Astra Tablet 10 in use", DisplayOrder = 2 },

        // Astra 13 Ultrabook (313)
        new() { Id = Id(425), ProductId = Id(313), Url = $"{Cdn}/photo-1531297484001-80022131f5a1{Params}", AltText = "Astra 13 Ultrabook", DisplayOrder = 1 },
        new() { Id = Id(426), ProductId = Id(313), Url = $"{Cdn}/photo-1496181133206-80ce9b88a853{Params}", AltText = "Astra 13 Ultrabook on desk", DisplayOrder = 2 },

        // Northline True Wireless Earbuds (314)
        new() { Id = Id(427), ProductId = Id(314), Url = $"{Cdn}/photo-1590658268037-6bf12165a8df{Params}", AltText = "Northline True Wireless Earbuds", DisplayOrder = 1 },
        new() { Id = Id(428), ProductId = Id(314), Url = $"{Cdn}/photo-1505740420928-5e560c06d30e{Params}", AltText = "Northline Earbuds case open", DisplayOrder = 2 },

        // Playwell RC Racing Car (315)
        new() { Id = Id(429), ProductId = Id(315), Url = $"{Cdn}/photo-1558618666-fcd25c85cd64{Params}", AltText = "Playwell RC Racing Car", DisplayOrder = 1 },
        new() { Id = Id(430), ProductId = Id(315), Url = $"{Cdn}/photo-1587654780291-39c9404d746b{Params}", AltText = "Playwell RC Racing Car in action", DisplayOrder = 2 },

        // Playwell Family Board Game (316)
        new() { Id = Id(431), ProductId = Id(316), Url = $"{Cdn}/VJVsEnR_vNE{Params}", AltText = "Playwell Family Board Game", DisplayOrder = 1 },
        new() { Id = Id(432), ProductId = Id(316), Url = $"{Cdn}/photo-1566576912321-d58ddd7a6088{Params}", AltText = "Playwell Board Game pieces", DisplayOrder = 2 },

        // Northline Garden Dining Set (317)
        new() { Id = Id(433), ProductId = Id(317), Url = $"{Cdn}/photo-1600210492493-0946911123ea{Params}", AltText = "Northline Garden Dining Set", DisplayOrder = 1 },
        new() { Id = Id(434), ProductId = Id(317), Url = $"{Cdn}/photo-1561702771-eb3cf35eedd2{Params}", AltText = "Northline Garden Dining Set styled", DisplayOrder = 2 },

        // Kestral Espresso Machine (318)
        new() { Id = Id(435), ProductId = Id(318), Url = $"{Cdn}/photo-1495474472287-4d71bcdd2085{Params}", AltText = "Kestral Espresso Machine", DisplayOrder = 1 },
        new() { Id = Id(436), ProductId = Id(318), Url = $"{Cdn}/photo-1556909114-f6e7ad7d3136{Params}", AltText = "Kestral Espresso Machine brewing", DisplayOrder = 2 },

        // VeloFit Adjustable Dumbbell Set (319)
        new() { Id = Id(437), ProductId = Id(319), Url = $"{Cdn}/photo-1583454110551-21f2fa2afe61{Params}", AltText = "VeloFit Adjustable Dumbbell Set", DisplayOrder = 1 },
        new() { Id = Id(438), ProductId = Id(319), Url = $"{Cdn}/photo-1534438327276-14e5300c3a48{Params}", AltText = "VeloFit Dumbbells in use", DisplayOrder = 2 },

        // Astra Pro Hair Straightener (320)
        new() { Id = Id(439), ProductId = Id(320), Url = $"{Cdn}/photo-1522338242992-e1a54906a8da{Params}", AltText = "Astra Pro Hair Straightener", DisplayOrder = 1 },
        new() { Id = Id(440), ProductId = Id(320), Url = $"{Cdn}/photo-1527799820374-dcf8d9d4a388{Params}", AltText = "Astra Hair Straightener styling", DisplayOrder = 2 },

        // Homekind Convertible Cot Bed (321)
        new() { Id = Id(441), ProductId = Id(321), Url = $"{Cdn}/photo-1515488042361-ee00e0ddd4e4{Params}", AltText = "Homekind Convertible Cot Bed", DisplayOrder = 1 },
        new() { Id = Id(442), ProductId = Id(321), Url = $"{Cdn}/photo-1491013516836-7db643ee125a{Params}", AltText = "Homekind Cot Bed as toddler bed", DisplayOrder = 2 },

        // Northline Vitamin Complex 90 (322)
        new() { Id = Id(443), ProductId = Id(322), Url = $"{Cdn}/photo-1505576399279-565b52d4ac71{Params}", AltText = "Northline Vitamin Complex 90", DisplayOrder = 1 },
        new() { Id = Id(444), ProductId = Id(322), Url = $"{Cdn}/photo-1512207736890-6ffed8a84e8d{Params}", AltText = "Northline Vitamin Complex capsules", DisplayOrder = 2 },

        // Kestral Smart Body Scale (323)
        new() { Id = Id(445), ProductId = Id(323), Url = $"{Cdn}/photo-1571019613454-1cb2f99b2d8b{Params}", AltText = "Kestral Smart Body Scale", DisplayOrder = 1 },
        new() { Id = Id(446), ProductId = Id(323), Url = $"{Cdn}/photo-1517836357463-d25dfeac3438{Params}", AltText = "Kestral Scale in bathroom", DisplayOrder = 2 },

        // Astra Premium Zip Hoodie (324)
        new() { Id = Id(447), ProductId = Id(324), Url = $"{Cdn}/photo-1620799140408-edc6dcb6d633{Params}", AltText = "Astra Premium Zip Hoodie", DisplayOrder = 1 },
        new() { Id = Id(448), ProductId = Id(324), Url = $"{Cdn}/photo-1441986300917-64674bd600d8{Params}", AltText = "Astra Zip Hoodie styled", DisplayOrder = 2 },

        // Astra Sterling Silver Pendant (325)
        new() { Id = Id(449), ProductId = Id(325), Url = $"{Cdn}/photo-1515562141207-7a88fb7ce338{Params}", AltText = "Astra Sterling Silver Pendant", DisplayOrder = 1 },
        new() { Id = Id(450), ProductId = Id(325), Url = $"{Cdn}/photo-1611652022419-a9419f74343d{Params}", AltText = "Astra Silver Pendant on model", DisplayOrder = 2 },

        // Homekind Luxury Spa Gift Set (326)
        new() { Id = Id(451), ProductId = Id(326), Url = $"{Cdn}/photo-1549465220-1a8b9238cd48{Params}", AltText = "Homekind Luxury Spa Gift Set", DisplayOrder = 1 },
        new() { Id = Id(452), ProductId = Id(326), Url = $"{Cdn}/photo-1596462502278-27bfdc403348{Params}", AltText = "Homekind Spa Gift Set contents", DisplayOrder = 2 },
    ];

    internal static readonly ProductSpecification[] ProductSpecifications =
        Products.SelectMany((product, index) => new[]
        {
            new ProductSpecification { Id = Id(501 + (index * 2)), ProductId = product.Id, Name = "SKU", Value = product.Sku, DisplayOrder = 1 },
            new ProductSpecification { Id = Id(502 + (index * 2)), ProductId = product.Id, Name = "Warranty", Value = "1 year", DisplayOrder = 2 }
        }).ToArray();

    internal static readonly Inventory[] InventoryRecords =
        Products.SelectMany((product, productIndex) => Stores.Select((store, storeIndex) => new Inventory
        {
            Id = Id(701 + (productIndex * Stores.Length) + storeIndex),
            ProductId = product.Id,
            StoreId = store.Id,
            QuantityAvailable = ((productIndex + storeIndex) % 4 == 0) ? 0 : 4 + productIndex + storeIndex,
            UpdatedAtUtc = SeededAtUtc
        })).ToArray();

    private static Product Product(
        int id,
        int categoryOrdinal,
        int brandOrdinal,
        string name,
        string slug,
        string shortDescription,
        string sku,
        decimal price,
        decimal? wasPrice,
        decimal rating,
        int reviewCount,
        bool isFeatured)
    {
        return new Product
        {
            Id = Id(id),
            CategoryId = Id(categoryOrdinal),
            BrandId = Id(brandOrdinal),
            Name = name,
            Slug = slug,
            ShortDescription = shortDescription,
            FullDescription = $"{shortDescription} Built for the Selqi demo catalog with deterministic seed data.",
            Sku = sku,
            Price = price,
            WasPrice = wasPrice,
            Rating = rating,
            ReviewCount = reviewCount,
            IsFeatured = isFeatured,
            CreatedAtUtc = SeededAtUtc
        };
    }

    private static Guid Id(int value)
    {
        return Guid.Parse($"00000000-0000-0000-0000-{value:000000000000}");
    }
}
