IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    CREATE TABLE [Brands] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(120) NOT NULL,
        [Slug] nvarchar(140) NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_Brands] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    CREATE TABLE [Categories] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(120) NOT NULL,
        [Slug] nvarchar(140) NOT NULL,
        [Description] nvarchar(500) NULL,
        [DisplayOrder] int NOT NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL DEFAULT (SYSUTCDATETIME()),
        [UpdatedAtUtc] datetime2 NULL,
        CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    CREATE TABLE [Stores] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(160) NOT NULL,
        [Town] nvarchar(120) NOT NULL,
        [Postcode] nvarchar(20) NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_Stores] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    CREATE TABLE [Products] (
        [Id] uniqueidentifier NOT NULL,
        [CategoryId] uniqueidentifier NOT NULL,
        [BrandId] uniqueidentifier NOT NULL,
        [Name] nvarchar(220) NOT NULL,
        [Slug] nvarchar(240) NOT NULL,
        [ShortDescription] nvarchar(500) NOT NULL,
        [FullDescription] nvarchar(2000) NULL,
        [Sku] nvarchar(80) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [WasPrice] decimal(18,2) NULL,
        [Rating] decimal(3,2) NOT NULL,
        [ReviewCount] int NOT NULL,
        [IsFeatured] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL DEFAULT (SYSUTCDATETIME()),
        [UpdatedAtUtc] datetime2 NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Products_Brands_BrandId] FOREIGN KEY ([BrandId]) REFERENCES [Brands] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Products_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    CREATE TABLE [Inventory] (
        [Id] uniqueidentifier NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [StoreId] uniqueidentifier NOT NULL,
        [QuantityAvailable] int NOT NULL,
        [UpdatedAtUtc] datetime2 NOT NULL DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_Inventory] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Inventory_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Inventory_Stores_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [Stores] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    CREATE TABLE [ProductImages] (
        [Id] uniqueidentifier NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [Url] nvarchar(500) NOT NULL,
        [AltText] nvarchar(220) NOT NULL,
        [DisplayOrder] int NOT NULL,
        CONSTRAINT [PK_ProductImages] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ProductImages_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    CREATE TABLE [ProductSpecifications] (
        [Id] uniqueidentifier NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [Name] nvarchar(120) NOT NULL,
        [Value] nvarchar(500) NOT NULL,
        [DisplayOrder] int NOT NULL,
        CONSTRAINT [PK_ProductSpecifications] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ProductSpecifications_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAtUtc', N'Name', N'Slug') AND [object_id] = OBJECT_ID(N'[Brands]'))
        SET IDENTITY_INSERT [Brands] ON;
    EXEC(N'INSERT INTO [Brands] ([Id], [CreatedAtUtc], [Name], [Slug])
    VALUES (''00000000-0000-0000-0000-000000000101'', ''2026-05-10T00:00:00.0000000Z'', N''Astra'', N''astra''),
    (''00000000-0000-0000-0000-000000000102'', ''2026-05-10T00:00:00.0000000Z'', N''Northline'', N''northline''),
    (''00000000-0000-0000-0000-000000000103'', ''2026-05-10T00:00:00.0000000Z'', N''Kestral'', N''kestral''),
    (''00000000-0000-0000-0000-000000000104'', ''2026-05-10T00:00:00.0000000Z'', N''Homekind'', N''homekind''),
    (''00000000-0000-0000-0000-000000000105'', ''2026-05-10T00:00:00.0000000Z'', N''Playwell'', N''playwell''),
    (''00000000-0000-0000-0000-000000000106'', ''2026-05-10T00:00:00.0000000Z'', N''VeloFit'', N''velofit'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAtUtc', N'Name', N'Slug') AND [object_id] = OBJECT_ID(N'[Brands]'))
        SET IDENTITY_INSERT [Brands] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAtUtc', N'Description', N'DisplayOrder', N'IsDeleted', N'Name', N'Slug', N'UpdatedAtUtc') AND [object_id] = OBJECT_ID(N'[Categories]'))
        SET IDENTITY_INSERT [Categories] ON;
    EXEC(N'INSERT INTO [Categories] ([Id], [CreatedAtUtc], [Description], [DisplayOrder], [IsDeleted], [Name], [Slug], [UpdatedAtUtc])
    VALUES (''00000000-0000-0000-0000-000000000001'', ''2026-05-10T00:00:00.0000000Z'', N''Computing, phones, gaming, and smart home essentials.'', 1, CAST(0 AS bit), N''Technology'', N''technology'', NULL),
    (''00000000-0000-0000-0000-000000000002'', ''2026-05-10T00:00:00.0000000Z'', N''Furniture, appliances, storage, and home improvement.'', 2, CAST(0 AS bit), N''Home'', N''home'', NULL),
    (''00000000-0000-0000-0000-000000000003'', ''2026-05-10T00:00:00.0000000Z'', N''Toys, games, learning, and outdoor play.'', 3, CAST(0 AS bit), N''Toys'', N''toys'', NULL),
    (''00000000-0000-0000-0000-000000000004'', ''2026-05-10T00:00:00.0000000Z'', N''Garden furniture, tools, BBQs, and seasonal picks.'', 4, CAST(0 AS bit), N''Garden'', N''garden'', NULL),
    (''00000000-0000-0000-0000-000000000005'', ''2026-05-10T00:00:00.0000000Z'', N''Kitchen, laundry, cleaning, and everyday appliances.'', 5, CAST(0 AS bit), N''Appliances'', N''appliances'', NULL),
    (''00000000-0000-0000-0000-000000000006'', ''2026-05-10T00:00:00.0000000Z'', N''Fitness equipment, bikes, and active lifestyle products.'', 6, CAST(0 AS bit), N''Sports'', N''sports'', NULL),
    (''00000000-0000-0000-0000-000000000007'', ''2026-05-10T00:00:00.0000000Z'', N''Hair care, grooming, wellness, and personal care.'', 7, CAST(0 AS bit), N''Beauty'', N''beauty'', NULL),
    (''00000000-0000-0000-0000-000000000008'', ''2026-05-10T00:00:00.0000000Z'', N''Nursery, travel, feeding, and baby care essentials.'', 8, CAST(0 AS bit), N''Baby'', N''baby'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAtUtc', N'Description', N'DisplayOrder', N'IsDeleted', N'Name', N'Slug', N'UpdatedAtUtc') AND [object_id] = OBJECT_ID(N'[Categories]'))
        SET IDENTITY_INSERT [Categories] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAtUtc', N'Name', N'Postcode', N'Town') AND [object_id] = OBJECT_ID(N'[Stores]'))
        SET IDENTITY_INSERT [Stores] ON;
    EXEC(N'INSERT INTO [Stores] ([Id], [CreatedAtUtc], [Name], [Postcode], [Town])
    VALUES (''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z'', N''Central Retail Park'', N''B1 1AA'', N''Birmingham''),
    (''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z'', N''Riverside Collection'', N''LS1 4PR'', N''Leeds''),
    (''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z'', N''Metro High Street'', N''M1 2WD'', N''Manchester''),
    (''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z'', N''Harbour Point'', N''BS1 6QF'', N''Bristol''),
    (''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z'', N''Station Square'', N''E1 8QS'', N''London'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAtUtc', N'Name', N'Postcode', N'Town') AND [object_id] = OBJECT_ID(N'[Stores]'))
        SET IDENTITY_INSERT [Stores] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'BrandId', N'CategoryId', N'CreatedAtUtc', N'FullDescription', N'IsDeleted', N'IsFeatured', N'Name', N'Price', N'Rating', N'ReviewCount', N'ShortDescription', N'Sku', N'Slug', N'UpdatedAtUtc', N'WasPrice') AND [object_id] = OBJECT_ID(N'[Products]'))
        SET IDENTITY_INSERT [Products] ON;
    EXEC(N'INSERT INTO [Products] ([Id], [BrandId], [CategoryId], [CreatedAtUtc], [FullDescription], [IsDeleted], [IsFeatured], [Name], [Price], [Rating], [ReviewCount], [ShortDescription], [Sku], [Slug], [UpdatedAtUtc], [WasPrice])
    VALUES (''00000000-0000-0000-0000-000000000301'', ''00000000-0000-0000-0000-000000000101'', ''00000000-0000-0000-0000-000000000001'', ''2026-05-10T00:00:00.0000000Z'', N''A fast everyday laptop with a bright 15 inch display. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(1 AS bit), N''Astra 15 Laptop'', 699.99, 4.6, 184, N''A fast everyday laptop with a bright 15 inch display.'', N''SKU-AST-150'', N''astra-15-laptop'', NULL, 749.99),
    (''00000000-0000-0000-0000-000000000302'', ''00000000-0000-0000-0000-000000000102'', ''00000000-0000-0000-0000-000000000001'', ''2026-05-10T00:00:00.0000000Z'', N''Fitness tracking, notifications, and week-long battery life. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(1 AS bit), N''Northline Smart Watch'', 129.99, 4.4, 89, N''Fitness tracking, notifications, and week-long battery life.'', N''SKU-NOR-W10'', N''northline-smart-watch'', NULL, NULL),
    (''00000000-0000-0000-0000-000000000303'', ''00000000-0000-0000-0000-000000000103'', ''00000000-0000-0000-0000-000000000001'', ''2026-05-10T00:00:00.0000000Z'', N''Noise reducing headphones for work, travel, and music. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(0 AS bit), N''Kestral Wireless Headphones'', 89.99, 4.3, 241, N''Noise reducing headphones for work, travel, and music.'', N''SKU-KES-H70'', N''kestral-wireless-headphones'', NULL, 109.99),
    (''00000000-0000-0000-0000-000000000304'', ''00000000-0000-0000-0000-000000000104'', ''00000000-0000-0000-0000-000000000002'', ''2026-05-10T00:00:00.0000000Z'', N''Compact two-door storage for hallway, office, or utility rooms. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(1 AS bit), N''Homekind Storage Cabinet'', 149.99, 4.2, 56, N''Compact two-door storage for hallway, office, or utility rooms.'', N''SKU-HOM-C22'', N''homekind-storage-cabinet'', NULL, NULL),
    (''00000000-0000-0000-0000-000000000305'', ''00000000-0000-0000-0000-000000000104'', ''00000000-0000-0000-0000-000000000002'', ''2026-05-10T00:00:00.0000000Z'', N''Adjustable LED desk lamp with touch brightness controls. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(0 AS bit), N''Homekind Desk Lamp'', 34.99, 4.5, 132, N''Adjustable LED desk lamp with touch brightness controls.'', N''SKU-HOM-L08'', N''homekind-desk-lamp'', NULL, 44.99),
    (''00000000-0000-0000-0000-000000000306'', ''00000000-0000-0000-0000-000000000105'', ''00000000-0000-0000-0000-000000000003'', ''2026-05-10T00:00:00.0000000Z'', N''Creative construction set with 650 colorful pieces. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(1 AS bit), N''Playwell Builder Set'', 39.99, 4.8, 312, N''Creative construction set with 650 colorful pieces.'', N''SKU-PLA-B65'', N''playwell-builder-set'', NULL, NULL),
    (''00000000-0000-0000-0000-000000000307'', ''00000000-0000-0000-0000-000000000102'', ''00000000-0000-0000-0000-000000000004'', ''2026-05-10T00:00:00.0000000Z'', N''Weather-resistant garden chairs for compact outdoor spaces. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(0 AS bit), N''Northline Patio Chair Pair'', 119.99, 4.1, 47, N''Weather-resistant garden chairs for compact outdoor spaces.'', N''SKU-NOR-G02'', N''northline-patio-chair-pair'', NULL, 139.99),
    (''00000000-0000-0000-0000-000000000308'', ''00000000-0000-0000-0000-000000000103'', ''00000000-0000-0000-0000-000000000005'', ''2026-05-10T00:00:00.0000000Z'', N''Large capacity air fryer with digital presets. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(1 AS bit), N''Kestral Air Fryer 6L'', 79.99, 4.7, 420, N''Large capacity air fryer with digital presets.'', N''SKU-KES-AF6'', N''kestral-air-fryer-6l'', NULL, 99.99),
    (''00000000-0000-0000-0000-000000000309'', ''00000000-0000-0000-0000-000000000106'', ''00000000-0000-0000-0000-000000000006'', ''2026-05-10T00:00:00.0000000Z'', N''Quiet indoor exercise bike with adjustable resistance. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(1 AS bit), N''VeloFit Exercise Bike'', 249.99, 4.4, 75, N''Quiet indoor exercise bike with adjustable resistance.'', N''SKU-VEL-B11'', N''velofit-exercise-bike'', NULL, 299.99),
    (''00000000-0000-0000-0000-000000000310'', ''00000000-0000-0000-0000-000000000101'', ''00000000-0000-0000-0000-000000000007'', ''2026-05-10T00:00:00.0000000Z'', N''Lightweight ionic hair dryer with diffuser attachment. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(0 AS bit), N''Astra Hair Dryer'', 49.99, 4.2, 118, N''Lightweight ionic hair dryer with diffuser attachment.'', N''SKU-AST-HD2'', N''astra-hair-dryer'', NULL, NULL),
    (''00000000-0000-0000-0000-000000000311'', ''00000000-0000-0000-0000-000000000104'', ''00000000-0000-0000-0000-000000000008'', ''2026-05-10T00:00:00.0000000Z'', N''Foldable travel cot with carry bag and padded base. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(0 AS bit), N''Homekind Travel Cot'', 69.99, 4.5, 63, N''Foldable travel cot with carry bag and padded base.'', N''SKU-HOM-TC4'', N''homekind-travel-cot'', NULL, 84.99),
    (''00000000-0000-0000-0000-000000000312'', ''00000000-0000-0000-0000-000000000101'', ''00000000-0000-0000-0000-000000000001'', ''2026-05-10T00:00:00.0000000Z'', N''Portable 10 inch tablet for streaming, reading, and browsing. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(1 AS bit), N''Astra Tablet 10'', 179.99, 4.1, 98, N''Portable 10 inch tablet for streaming, reading, and browsing.'', N''SKU-AST-T10'', N''astra-tablet-10'', NULL, NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'BrandId', N'CategoryId', N'CreatedAtUtc', N'FullDescription', N'IsDeleted', N'IsFeatured', N'Name', N'Price', N'Rating', N'ReviewCount', N'ShortDescription', N'Sku', N'Slug', N'UpdatedAtUtc', N'WasPrice') AND [object_id] = OBJECT_ID(N'[Products]'))
        SET IDENTITY_INSERT [Products] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ProductId', N'QuantityAvailable', N'StoreId', N'UpdatedAtUtc') AND [object_id] = OBJECT_ID(N'[Inventory]'))
        SET IDENTITY_INSERT [Inventory] ON;
    EXEC(N'INSERT INTO [Inventory] ([Id], [ProductId], [QuantityAvailable], [StoreId], [UpdatedAtUtc])
    VALUES (''00000000-0000-0000-0000-000000000701'', ''00000000-0000-0000-0000-000000000301'', 0, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000702'', ''00000000-0000-0000-0000-000000000301'', 5, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000703'', ''00000000-0000-0000-0000-000000000301'', 6, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000704'', ''00000000-0000-0000-0000-000000000301'', 7, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000705'', ''00000000-0000-0000-0000-000000000301'', 0, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000706'', ''00000000-0000-0000-0000-000000000302'', 5, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000707'', ''00000000-0000-0000-0000-000000000302'', 6, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000708'', ''00000000-0000-0000-0000-000000000302'', 7, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000709'', ''00000000-0000-0000-0000-000000000302'', 0, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000710'', ''00000000-0000-0000-0000-000000000302'', 9, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000711'', ''00000000-0000-0000-0000-000000000303'', 6, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000712'', ''00000000-0000-0000-0000-000000000303'', 7, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000713'', ''00000000-0000-0000-0000-000000000303'', 0, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000714'', ''00000000-0000-0000-0000-000000000303'', 9, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000715'', ''00000000-0000-0000-0000-000000000303'', 10, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000716'', ''00000000-0000-0000-0000-000000000304'', 7, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000717'', ''00000000-0000-0000-0000-000000000304'', 0, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000718'', ''00000000-0000-0000-0000-000000000304'', 9, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000719'', ''00000000-0000-0000-0000-000000000304'', 10, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000720'', ''00000000-0000-0000-0000-000000000304'', 11, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000721'', ''00000000-0000-0000-0000-000000000305'', 0, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000722'', ''00000000-0000-0000-0000-000000000305'', 9, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000723'', ''00000000-0000-0000-0000-000000000305'', 10, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000724'', ''00000000-0000-0000-0000-000000000305'', 11, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000725'', ''00000000-0000-0000-0000-000000000305'', 0, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000726'', ''00000000-0000-0000-0000-000000000306'', 9, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000727'', ''00000000-0000-0000-0000-000000000306'', 10, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000728'', ''00000000-0000-0000-0000-000000000306'', 11, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000729'', ''00000000-0000-0000-0000-000000000306'', 0, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000730'', ''00000000-0000-0000-0000-000000000306'', 13, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000731'', ''00000000-0000-0000-0000-000000000307'', 10, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000732'', ''00000000-0000-0000-0000-000000000307'', 11, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000733'', ''00000000-0000-0000-0000-000000000307'', 0, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000734'', ''00000000-0000-0000-0000-000000000307'', 13, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000735'', ''00000000-0000-0000-0000-000000000307'', 14, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000736'', ''00000000-0000-0000-0000-000000000308'', 11, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000737'', ''00000000-0000-0000-0000-000000000308'', 0, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000738'', ''00000000-0000-0000-0000-000000000308'', 13, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000739'', ''00000000-0000-0000-0000-000000000308'', 14, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000740'', ''00000000-0000-0000-0000-000000000308'', 15, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000741'', ''00000000-0000-0000-0000-000000000309'', 0, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000742'', ''00000000-0000-0000-0000-000000000309'', 13, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z'');
    INSERT INTO [Inventory] ([Id], [ProductId], [QuantityAvailable], [StoreId], [UpdatedAtUtc])
    VALUES (''00000000-0000-0000-0000-000000000743'', ''00000000-0000-0000-0000-000000000309'', 14, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000744'', ''00000000-0000-0000-0000-000000000309'', 15, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000745'', ''00000000-0000-0000-0000-000000000309'', 0, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000746'', ''00000000-0000-0000-0000-000000000310'', 13, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000747'', ''00000000-0000-0000-0000-000000000310'', 14, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000748'', ''00000000-0000-0000-0000-000000000310'', 15, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000749'', ''00000000-0000-0000-0000-000000000310'', 0, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000750'', ''00000000-0000-0000-0000-000000000310'', 17, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000751'', ''00000000-0000-0000-0000-000000000311'', 14, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000752'', ''00000000-0000-0000-0000-000000000311'', 15, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000753'', ''00000000-0000-0000-0000-000000000311'', 0, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000754'', ''00000000-0000-0000-0000-000000000311'', 17, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000755'', ''00000000-0000-0000-0000-000000000311'', 18, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000756'', ''00000000-0000-0000-0000-000000000312'', 15, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000757'', ''00000000-0000-0000-0000-000000000312'', 0, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000758'', ''00000000-0000-0000-0000-000000000312'', 17, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000759'', ''00000000-0000-0000-0000-000000000312'', 18, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000760'', ''00000000-0000-0000-0000-000000000312'', 19, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ProductId', N'QuantityAvailable', N'StoreId', N'UpdatedAtUtc') AND [object_id] = OBJECT_ID(N'[Inventory]'))
        SET IDENTITY_INSERT [Inventory] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AltText', N'DisplayOrder', N'ProductId', N'Url') AND [object_id] = OBJECT_ID(N'[ProductImages]'))
        SET IDENTITY_INSERT [ProductImages] ON;
    EXEC(N'INSERT INTO [ProductImages] ([Id], [AltText], [DisplayOrder], [ProductId], [Url])
    VALUES (''00000000-0000-0000-0000-000000000401'', N''Astra 15 Laptop'', 1, ''00000000-0000-0000-0000-000000000301'', N''/assets/products/astra-15-laptop.jpg''),
    (''00000000-0000-0000-0000-000000000402'', N''Northline Smart Watch'', 1, ''00000000-0000-0000-0000-000000000302'', N''/assets/products/northline-smart-watch.jpg''),
    (''00000000-0000-0000-0000-000000000403'', N''Kestral Wireless Headphones'', 1, ''00000000-0000-0000-0000-000000000303'', N''/assets/products/kestral-wireless-headphones.jpg''),
    (''00000000-0000-0000-0000-000000000404'', N''Homekind Storage Cabinet'', 1, ''00000000-0000-0000-0000-000000000304'', N''/assets/products/homekind-storage-cabinet.jpg''),
    (''00000000-0000-0000-0000-000000000405'', N''Homekind Desk Lamp'', 1, ''00000000-0000-0000-0000-000000000305'', N''/assets/products/homekind-desk-lamp.jpg''),
    (''00000000-0000-0000-0000-000000000406'', N''Playwell Builder Set'', 1, ''00000000-0000-0000-0000-000000000306'', N''/assets/products/playwell-builder-set.jpg''),
    (''00000000-0000-0000-0000-000000000407'', N''Northline Patio Chair Pair'', 1, ''00000000-0000-0000-0000-000000000307'', N''/assets/products/northline-patio-chair-pair.jpg''),
    (''00000000-0000-0000-0000-000000000408'', N''Kestral Air Fryer 6L'', 1, ''00000000-0000-0000-0000-000000000308'', N''/assets/products/kestral-air-fryer-6l.jpg''),
    (''00000000-0000-0000-0000-000000000409'', N''VeloFit Exercise Bike'', 1, ''00000000-0000-0000-0000-000000000309'', N''/assets/products/velofit-exercise-bike.jpg''),
    (''00000000-0000-0000-0000-000000000410'', N''Astra Hair Dryer'', 1, ''00000000-0000-0000-0000-000000000310'', N''/assets/products/astra-hair-dryer.jpg''),
    (''00000000-0000-0000-0000-000000000411'', N''Homekind Travel Cot'', 1, ''00000000-0000-0000-0000-000000000311'', N''/assets/products/homekind-travel-cot.jpg''),
    (''00000000-0000-0000-0000-000000000412'', N''Astra Tablet 10'', 1, ''00000000-0000-0000-0000-000000000312'', N''/assets/products/astra-tablet-10.jpg'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AltText', N'DisplayOrder', N'ProductId', N'Url') AND [object_id] = OBJECT_ID(N'[ProductImages]'))
        SET IDENTITY_INSERT [ProductImages] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DisplayOrder', N'Name', N'ProductId', N'Value') AND [object_id] = OBJECT_ID(N'[ProductSpecifications]'))
        SET IDENTITY_INSERT [ProductSpecifications] ON;
    EXEC(N'INSERT INTO [ProductSpecifications] ([Id], [DisplayOrder], [Name], [ProductId], [Value])
    VALUES (''00000000-0000-0000-0000-000000000501'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000301'', N''SKU-AST-150''),
    (''00000000-0000-0000-0000-000000000502'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000301'', N''1 year''),
    (''00000000-0000-0000-0000-000000000503'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000302'', N''SKU-NOR-W10''),
    (''00000000-0000-0000-0000-000000000504'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000302'', N''1 year''),
    (''00000000-0000-0000-0000-000000000505'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000303'', N''SKU-KES-H70''),
    (''00000000-0000-0000-0000-000000000506'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000303'', N''1 year''),
    (''00000000-0000-0000-0000-000000000507'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000304'', N''SKU-HOM-C22''),
    (''00000000-0000-0000-0000-000000000508'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000304'', N''1 year''),
    (''00000000-0000-0000-0000-000000000509'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000305'', N''SKU-HOM-L08''),
    (''00000000-0000-0000-0000-000000000510'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000305'', N''1 year''),
    (''00000000-0000-0000-0000-000000000511'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000306'', N''SKU-PLA-B65''),
    (''00000000-0000-0000-0000-000000000512'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000306'', N''1 year''),
    (''00000000-0000-0000-0000-000000000513'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000307'', N''SKU-NOR-G02''),
    (''00000000-0000-0000-0000-000000000514'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000307'', N''1 year''),
    (''00000000-0000-0000-0000-000000000515'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000308'', N''SKU-KES-AF6''),
    (''00000000-0000-0000-0000-000000000516'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000308'', N''1 year''),
    (''00000000-0000-0000-0000-000000000517'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000309'', N''SKU-VEL-B11''),
    (''00000000-0000-0000-0000-000000000518'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000309'', N''1 year''),
    (''00000000-0000-0000-0000-000000000519'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000310'', N''SKU-AST-HD2''),
    (''00000000-0000-0000-0000-000000000520'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000310'', N''1 year''),
    (''00000000-0000-0000-0000-000000000521'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000311'', N''SKU-HOM-TC4''),
    (''00000000-0000-0000-0000-000000000522'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000311'', N''1 year''),
    (''00000000-0000-0000-0000-000000000523'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000312'', N''SKU-AST-T10''),
    (''00000000-0000-0000-0000-000000000524'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000312'', N''1 year'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DisplayOrder', N'Name', N'ProductId', N'Value') AND [object_id] = OBJECT_ID(N'[ProductSpecifications]'))
        SET IDENTITY_INSERT [ProductSpecifications] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Brands_Slug] ON [Brands] ([Slug]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Categories_Slug] ON [Categories] ([Slug]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Inventory_ProductId_StoreId] ON [Inventory] ([ProductId], [StoreId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    CREATE INDEX [IX_Inventory_StoreId] ON [Inventory] ([StoreId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    CREATE INDEX [IX_ProductImages_ProductId] ON [ProductImages] ([ProductId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    CREATE INDEX [IX_Products_BrandId] ON [Products] ([BrandId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    CREATE INDEX [IX_Products_CategoryId] ON [Products] ([CategoryId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Products_Sku] ON [Products] ([Sku]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Products_Slug] ON [Products] ([Slug]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    CREATE INDEX [IX_ProductSpecifications_ProductId] ON [ProductSpecifications] ([ProductId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260509185113_InitialCatalog'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260509185113_InitialCatalog', N'10.0.11');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511091131_AddAuthAndShopping'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] uniqueidentifier NOT NULL,
        [Email] nvarchar(256) NOT NULL,
        [PasswordHash] nvarchar(512) NOT NULL,
        [FirstName] nvarchar(100) NOT NULL,
        [LastName] nvarchar(100) NOT NULL,
        [Role] nvarchar(50) NOT NULL DEFAULT N'Customer',
        [IsEmailVerified] bit NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL DEFAULT (SYSUTCDATETIME()),
        [LastLoginAtUtc] datetime2 NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511091131_AddAuthAndShopping'
)
BEGIN
    CREATE TABLE [Addresses] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [Label] nvarchar(100) NOT NULL,
        [FirstName] nvarchar(100) NOT NULL,
        [LastName] nvarchar(100) NOT NULL,
        [Line1] nvarchar(250) NOT NULL,
        [Line2] nvarchar(250) NULL,
        [Town] nvarchar(120) NOT NULL,
        [County] nvarchar(120) NOT NULL,
        [Postcode] nvarchar(20) NOT NULL,
        [Country] nvarchar(100) NOT NULL DEFAULT N'United Kingdom',
        [Phone] nvarchar(20) NOT NULL,
        [IsDefault] bit NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_Addresses] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Addresses_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511091131_AddAuthAndShopping'
)
BEGIN
    CREATE TABLE [Baskets] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL DEFAULT (SYSUTCDATETIME()),
        [UpdatedAtUtc] datetime2 NOT NULL DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_Baskets] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Baskets_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511091131_AddAuthAndShopping'
)
BEGIN
    CREATE TABLE [Orders] (
        [Id] uniqueidentifier NOT NULL,
        [OrderNumber] nvarchar(50) NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [Status] int NOT NULL,
        [SubTotal] decimal(18,2) NOT NULL,
        [DeliveryCharge] decimal(18,2) NOT NULL,
        [Total] decimal(18,2) NOT NULL,
        [DeliveryFirstName] nvarchar(100) NOT NULL,
        [DeliveryLastName] nvarchar(100) NOT NULL,
        [DeliveryLine1] nvarchar(250) NOT NULL,
        [DeliveryLine2] nvarchar(250) NULL,
        [DeliveryTown] nvarchar(120) NOT NULL,
        [DeliveryCounty] nvarchar(120) NOT NULL,
        [DeliveryPostcode] nvarchar(20) NOT NULL,
        [DeliveryCountry] nvarchar(100) NOT NULL DEFAULT N'United Kingdom',
        [DeliveryPhone] nvarchar(20) NOT NULL,
        [PaymentReference] nvarchar(100) NOT NULL,
        [PaymentStatus] nvarchar(50) NOT NULL,
        [PlacedAtUtc] datetime2 NOT NULL DEFAULT (SYSUTCDATETIME()),
        [UpdatedAtUtc] datetime2 NOT NULL DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Orders_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511091131_AddAuthAndShopping'
)
BEGIN
    CREATE TABLE [WishlistItems] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [AddedAtUtc] datetime2 NOT NULL DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_WishlistItems] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_WishlistItems_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_WishlistItems_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511091131_AddAuthAndShopping'
)
BEGIN
    CREATE TABLE [BasketItems] (
        [Id] uniqueidentifier NOT NULL,
        [BasketId] uniqueidentifier NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [Quantity] int NOT NULL,
        [UnitPrice] decimal(18,2) NOT NULL,
        [AddedAtUtc] datetime2 NOT NULL DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_BasketItems] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BasketItems_Baskets_BasketId] FOREIGN KEY ([BasketId]) REFERENCES [Baskets] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_BasketItems_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511091131_AddAuthAndShopping'
)
BEGIN
    CREATE TABLE [OrderItems] (
        [Id] uniqueidentifier NOT NULL,
        [OrderId] uniqueidentifier NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [ProductName] nvarchar(220) NOT NULL,
        [ProductSku] nvarchar(80) NOT NULL,
        [ProductImageUrl] nvarchar(500) NULL,
        [Quantity] int NOT NULL,
        [UnitPrice] decimal(18,2) NOT NULL,
        [LineTotal] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_OrderItems] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_OrderItems_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511091131_AddAuthAndShopping'
)
BEGIN
    CREATE INDEX [IX_Addresses_UserId] ON [Addresses] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511091131_AddAuthAndShopping'
)
BEGIN
    CREATE INDEX [IX_BasketItems_BasketId] ON [BasketItems] ([BasketId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511091131_AddAuthAndShopping'
)
BEGIN
    CREATE INDEX [IX_BasketItems_ProductId] ON [BasketItems] ([ProductId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511091131_AddAuthAndShopping'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Baskets_UserId] ON [Baskets] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511091131_AddAuthAndShopping'
)
BEGIN
    CREATE INDEX [IX_OrderItems_OrderId] ON [OrderItems] ([OrderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511091131_AddAuthAndShopping'
)
BEGIN
    CREATE INDEX [IX_OrderItems_ProductId] ON [OrderItems] ([ProductId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511091131_AddAuthAndShopping'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Orders_OrderNumber] ON [Orders] ([OrderNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511091131_AddAuthAndShopping'
)
BEGIN
    CREATE INDEX [IX_Orders_UserId] ON [Orders] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511091131_AddAuthAndShopping'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511091131_AddAuthAndShopping'
)
BEGIN
    CREATE INDEX [IX_WishlistItems_ProductId] ON [WishlistItems] ([ProductId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511091131_AddAuthAndShopping'
)
BEGIN
    CREATE UNIQUE INDEX [IX_WishlistItems_UserId_ProductId] ON [WishlistItems] ([UserId], [ProductId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511091131_AddAuthAndShopping'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260511091131_AddAuthAndShopping', N'10.0.11');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511114557_UpdateProductImages'
)
BEGIN
    EXEC(N'UPDATE [ProductImages] SET [Url] = N''https://images.unsplash.com/photo-1496181133206-80ce9b88a853?auto=format&fit=crop&w=600&h=500''
    WHERE [Id] = ''00000000-0000-0000-0000-000000000401'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511114557_UpdateProductImages'
)
BEGIN
    EXEC(N'UPDATE [ProductImages] SET [Url] = N''https://images.unsplash.com/photo-1523275335684-37898b6baf30?auto=format&fit=crop&w=600&h=500''
    WHERE [Id] = ''00000000-0000-0000-0000-000000000402'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511114557_UpdateProductImages'
)
BEGIN
    EXEC(N'UPDATE [ProductImages] SET [Url] = N''https://images.unsplash.com/photo-1505740420928-5e560c06d30e?auto=format&fit=crop&w=600&h=500''
    WHERE [Id] = ''00000000-0000-0000-0000-000000000403'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511114557_UpdateProductImages'
)
BEGIN
    EXEC(N'UPDATE [ProductImages] SET [Url] = N''https://images.unsplash.com/photo-1493663284031-b7e3aefcae8e?auto=format&fit=crop&w=600&h=500''
    WHERE [Id] = ''00000000-0000-0000-0000-000000000404'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511114557_UpdateProductImages'
)
BEGIN
    EXEC(N'UPDATE [ProductImages] SET [Url] = N''https://images.unsplash.com/photo-1534575950704-07c5a26b1e00?auto=format&fit=crop&w=600&h=500''
    WHERE [Id] = ''00000000-0000-0000-0000-000000000405'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511114557_UpdateProductImages'
)
BEGIN
    EXEC(N'UPDATE [ProductImages] SET [Url] = N''https://images.unsplash.com/photo-1587654780291-39c9404d746b?auto=format&fit=crop&w=600&h=500''
    WHERE [Id] = ''00000000-0000-0000-0000-000000000406'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511114557_UpdateProductImages'
)
BEGIN
    EXEC(N'UPDATE [ProductImages] SET [Url] = N''https://images.unsplash.com/photo-1600210492493-0946911123ea?auto=format&fit=crop&w=600&h=500''
    WHERE [Id] = ''00000000-0000-0000-0000-000000000407'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511114557_UpdateProductImages'
)
BEGIN
    EXEC(N'UPDATE [ProductImages] SET [Url] = N''https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?auto=format&fit=crop&w=600&h=500''
    WHERE [Id] = ''00000000-0000-0000-0000-000000000408'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511114557_UpdateProductImages'
)
BEGIN
    EXEC(N'UPDATE [ProductImages] SET [Url] = N''https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b?auto=format&fit=crop&w=600&h=500''
    WHERE [Id] = ''00000000-0000-0000-0000-000000000409'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511114557_UpdateProductImages'
)
BEGIN
    EXEC(N'UPDATE [ProductImages] SET [Url] = N''https://images.unsplash.com/photo-1522338242992-e1a54906a8da?auto=format&fit=crop&w=600&h=500''
    WHERE [Id] = ''00000000-0000-0000-0000-000000000410'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511114557_UpdateProductImages'
)
BEGIN
    EXEC(N'UPDATE [ProductImages] SET [Url] = N''https://images.unsplash.com/photo-1515488042361-ee00e0ddd4e4?auto=format&fit=crop&w=600&h=500''
    WHERE [Id] = ''00000000-0000-0000-0000-000000000411'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511114557_UpdateProductImages'
)
BEGIN
    EXEC(N'UPDATE [ProductImages] SET [Url] = N''https://images.unsplash.com/photo-1544244015-0df4b3ffc6b0?auto=format&fit=crop&w=600&h=500''
    WHERE [Id] = ''00000000-0000-0000-0000-000000000412'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511114557_UpdateProductImages'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AltText', N'DisplayOrder', N'ProductId', N'Url') AND [object_id] = OBJECT_ID(N'[ProductImages]'))
        SET IDENTITY_INSERT [ProductImages] ON;
    EXEC(N'INSERT INTO [ProductImages] ([Id], [AltText], [DisplayOrder], [ProductId], [Url])
    VALUES (''00000000-0000-0000-0000-000000000413'', N''Astra 15 Laptop on desk'', 2, ''00000000-0000-0000-0000-000000000301'', N''https://images.unsplash.com/photo-1525547719571-a2d4ac8945e2?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000414'', N''Northline Smart Watch on wrist'', 2, ''00000000-0000-0000-0000-000000000302'', N''https://images.unsplash.com/photo-1546868871-7041f2a55e12?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000415'', N''Kestral Wireless Headphones flat lay'', 2, ''00000000-0000-0000-0000-000000000303'', N''https://images.unsplash.com/photo-1484704849700-f032a568e944?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000416'', N''Homekind Storage Cabinet in room'', 2, ''00000000-0000-0000-0000-000000000304'', N''https://images.unsplash.com/photo-1555041469-a586c61ea9bc?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000417'', N''Homekind Desk Lamp lit'', 2, ''00000000-0000-0000-0000-000000000305'', N''https://images.unsplash.com/photo-1513506003901-1e6a35c98fe8?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000418'', N''Playwell Builder Set pieces'', 2, ''00000000-0000-0000-0000-000000000306'', N''https://images.unsplash.com/photo-1566576912321-d58ddd7a6088?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000419'', N''Northline Patio Chair Pair outdoor setting'', 2, ''00000000-0000-0000-0000-000000000307'', N''https://images.unsplash.com/photo-1561702771-eb3cf35eedd2?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000420'', N''Kestral Air Fryer cooking food'', 2, ''00000000-0000-0000-0000-000000000308'', N''https://images.unsplash.com/photo-1604503468506-a8da13d82791?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000421'', N''VeloFit Exercise Bike in use'', 2, ''00000000-0000-0000-0000-000000000309'', N''https://images.unsplash.com/photo-1534438327276-14e5300c3a48?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000422'', N''Astra Hair Dryer in use'', 2, ''00000000-0000-0000-0000-000000000310'', N''https://images.unsplash.com/photo-1527799820374-dcf8d9d4a388?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000423'', N''Homekind Travel Cot assembled'', 2, ''00000000-0000-0000-0000-000000000311'', N''https://images.unsplash.com/photo-1491013516836-7db643ee125a?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000424'', N''Astra Tablet 10 in use'', 2, ''00000000-0000-0000-0000-000000000312'', N''https://images.unsplash.com/photo-1512499617640-c74ae3a79d37?auto=format&fit=crop&w=600&h=500'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AltText', N'DisplayOrder', N'ProductId', N'Url') AND [object_id] = OBJECT_ID(N'[ProductImages]'))
        SET IDENTITY_INSERT [ProductImages] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260511114557_UpdateProductImages'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260511114557_UpdateProductImages', N'10.0.11');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512125347_AddAdminOperationsAudit'
)
BEGIN
    CREATE TABLE [AuditLogs] (
        [Id] uniqueidentifier NOT NULL,
        [ActorUserId] uniqueidentifier NOT NULL,
        [ActorEmail] nvarchar(256) NOT NULL,
        [Action] nvarchar(80) NOT NULL,
        [EntityType] nvarchar(80) NOT NULL,
        [EntityId] nvarchar(80) NOT NULL,
        [Summary] nvarchar(500) NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_AuditLogs] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512125347_AddAdminOperationsAudit'
)
BEGIN
    CREATE INDEX [IX_AuditLogs_CreatedAtUtc] ON [AuditLogs] ([CreatedAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512125347_AddAdminOperationsAudit'
)
BEGIN
    CREATE INDEX [IX_AuditLogs_EntityType_EntityId] ON [AuditLogs] ([EntityType], [EntityId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512125347_AddAdminOperationsAudit'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260512125347_AddAdminOperationsAudit', N'10.0.11');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512142826_AddPasswordResetTokens'
)
BEGIN
    CREATE TABLE [PasswordResetTokens] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [Token] nvarchar(64) NOT NULL,
        [ExpiresAtUtc] datetime2 NOT NULL,
        [UsedAtUtc] datetime2 NULL,
        [CreatedAtUtc] datetime2 NOT NULL DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_PasswordResetTokens] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PasswordResetTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512142826_AddPasswordResetTokens'
)
BEGIN
    CREATE UNIQUE INDEX [IX_PasswordResetTokens_Token] ON [PasswordResetTokens] ([Token]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512142826_AddPasswordResetTokens'
)
BEGIN
    CREATE INDEX [IX_PasswordResetTokens_UserId] ON [PasswordResetTokens] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260512142826_AddPasswordResetTokens'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260512142826_AddPasswordResetTokens', N'10.0.11');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513090203_AddCategoryImageUrlAndNewCategories'
)
BEGIN
    ALTER TABLE [Categories] ADD [ImageUrl] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513090203_AddCategoryImageUrlAndNewCategories'
)
BEGIN
    EXEC(N'UPDATE [Categories] SET [ImageUrl] = NULL
    WHERE [Id] = ''00000000-0000-0000-0000-000000000001'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513090203_AddCategoryImageUrlAndNewCategories'
)
BEGIN
    EXEC(N'UPDATE [Categories] SET [ImageUrl] = NULL
    WHERE [Id] = ''00000000-0000-0000-0000-000000000002'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513090203_AddCategoryImageUrlAndNewCategories'
)
BEGIN
    EXEC(N'UPDATE [Categories] SET [ImageUrl] = NULL
    WHERE [Id] = ''00000000-0000-0000-0000-000000000003'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513090203_AddCategoryImageUrlAndNewCategories'
)
BEGIN
    EXEC(N'UPDATE [Categories] SET [ImageUrl] = NULL
    WHERE [Id] = ''00000000-0000-0000-0000-000000000004'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513090203_AddCategoryImageUrlAndNewCategories'
)
BEGIN
    EXEC(N'UPDATE [Categories] SET [ImageUrl] = NULL
    WHERE [Id] = ''00000000-0000-0000-0000-000000000005'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513090203_AddCategoryImageUrlAndNewCategories'
)
BEGIN
    EXEC(N'UPDATE [Categories] SET [ImageUrl] = NULL
    WHERE [Id] = ''00000000-0000-0000-0000-000000000006'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513090203_AddCategoryImageUrlAndNewCategories'
)
BEGIN
    EXEC(N'UPDATE [Categories] SET [ImageUrl] = NULL
    WHERE [Id] = ''00000000-0000-0000-0000-000000000007'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513090203_AddCategoryImageUrlAndNewCategories'
)
BEGIN
    EXEC(N'UPDATE [Categories] SET [ImageUrl] = NULL
    WHERE [Id] = ''00000000-0000-0000-0000-000000000008'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513090203_AddCategoryImageUrlAndNewCategories'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAtUtc', N'Description', N'DisplayOrder', N'ImageUrl', N'IsDeleted', N'Name', N'Slug', N'UpdatedAtUtc') AND [object_id] = OBJECT_ID(N'[Categories]'))
        SET IDENTITY_INSERT [Categories] ON;
    EXEC(N'INSERT INTO [Categories] ([Id], [CreatedAtUtc], [Description], [DisplayOrder], [ImageUrl], [IsDeleted], [Name], [Slug], [UpdatedAtUtc])
    VALUES (''00000000-0000-0000-0000-000000000009'', ''2026-05-10T00:00:00.0000000Z'', N''Supplements, vitamins, wellbeing, and pharmacy essentials.'', 9, NULL, CAST(0 AS bit), N''Health'', N''health'', NULL),
    (''00000000-0000-0000-0000-000000000010'', ''2026-05-10T00:00:00.0000000Z'', N''Men''''s, women''''s, and children''''s fashion and accessories.'', 10, NULL, CAST(0 AS bit), N''Clothing'', N''clothing'', NULL),
    (''00000000-0000-0000-0000-000000000011'', ''2026-05-10T00:00:00.0000000Z'', N''Rings, necklaces, bracelets, and fine accessories.'', 11, NULL, CAST(0 AS bit), N''Jewellery'', N''jewellery'', NULL),
    (''00000000-0000-0000-0000-000000000012'', ''2026-05-10T00:00:00.0000000Z'', N''Seasonal gifts, hampers, and special occasion ideas.'', 12, NULL, CAST(0 AS bit), N''Gifts'', N''gifts'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAtUtc', N'Description', N'DisplayOrder', N'ImageUrl', N'IsDeleted', N'Name', N'Slug', N'UpdatedAtUtc') AND [object_id] = OBJECT_ID(N'[Categories]'))
        SET IDENTITY_INSERT [Categories] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513090203_AddCategoryImageUrlAndNewCategories'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260513090203_AddCategoryImageUrlAndNewCategories', N'10.0.11');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513100328_AddSubCategoriesAndProducts'
)
BEGIN
    ALTER TABLE [Categories] ADD [ParentCategoryId] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513100328_AddSubCategoriesAndProducts'
)
BEGIN
    EXEC(N'UPDATE [Categories] SET [ImageUrl] = N''https://images.unsplash.com/photo-1498049794561-7780e7231661?auto=format&fit=crop&w=120&h=120'', [ParentCategoryId] = NULL
    WHERE [Id] = ''00000000-0000-0000-0000-000000000001'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513100328_AddSubCategoriesAndProducts'
)
BEGIN
    EXEC(N'UPDATE [Categories] SET [ImageUrl] = N''https://images.unsplash.com/photo-1555041469-a586c61ea9bc?auto=format&fit=crop&w=120&h=120'', [ParentCategoryId] = NULL
    WHERE [Id] = ''00000000-0000-0000-0000-000000000002'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513100328_AddSubCategoriesAndProducts'
)
BEGIN
    EXEC(N'UPDATE [Categories] SET [ImageUrl] = N''https://images.unsplash.com/photo-1587654780291-39c9404d746b?auto=format&fit=crop&w=120&h=120'', [ParentCategoryId] = NULL
    WHERE [Id] = ''00000000-0000-0000-0000-000000000003'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513100328_AddSubCategoriesAndProducts'
)
BEGIN
    EXEC(N'UPDATE [Categories] SET [ImageUrl] = N''https://images.unsplash.com/photo-1416879595882-3373a0480b5b?auto=format&fit=crop&w=120&h=120'', [ParentCategoryId] = NULL
    WHERE [Id] = ''00000000-0000-0000-0000-000000000004'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513100328_AddSubCategoriesAndProducts'
)
BEGIN
    EXEC(N'UPDATE [Categories] SET [ImageUrl] = N''https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?auto=format&fit=crop&w=120&h=120'', [ParentCategoryId] = NULL
    WHERE [Id] = ''00000000-0000-0000-0000-000000000005'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513100328_AddSubCategoriesAndProducts'
)
BEGIN
    EXEC(N'UPDATE [Categories] SET [ImageUrl] = N''https://images.unsplash.com/photo-1517836357463-d25dfeac3438?auto=format&fit=crop&w=120&h=120'', [ParentCategoryId] = NULL
    WHERE [Id] = ''00000000-0000-0000-0000-000000000006'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513100328_AddSubCategoriesAndProducts'
)
BEGIN
    EXEC(N'UPDATE [Categories] SET [ImageUrl] = N''https://images.unsplash.com/photo-1596462502278-27bfdc403348?auto=format&fit=crop&w=120&h=120'', [ParentCategoryId] = NULL
    WHERE [Id] = ''00000000-0000-0000-0000-000000000007'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513100328_AddSubCategoriesAndProducts'
)
BEGIN
    EXEC(N'UPDATE [Categories] SET [ImageUrl] = N''https://images.unsplash.com/photo-1515488042361-ee00e0ddd4e4?auto=format&fit=crop&w=120&h=120'', [ParentCategoryId] = NULL
    WHERE [Id] = ''00000000-0000-0000-0000-000000000008'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513100328_AddSubCategoriesAndProducts'
)
BEGIN
    EXEC(N'UPDATE [Categories] SET [ImageUrl] = N''https://images.unsplash.com/photo-1505576399279-565b52d4ac71?auto=format&fit=crop&w=120&h=120'', [ParentCategoryId] = NULL
    WHERE [Id] = ''00000000-0000-0000-0000-000000000009'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513100328_AddSubCategoriesAndProducts'
)
BEGIN
    EXEC(N'UPDATE [Categories] SET [ImageUrl] = N''https://images.unsplash.com/photo-1441986300917-64674bd600d8?auto=format&fit=crop&w=120&h=120'', [ParentCategoryId] = NULL
    WHERE [Id] = ''00000000-0000-0000-0000-000000000010'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513100328_AddSubCategoriesAndProducts'
)
BEGIN
    EXEC(N'UPDATE [Categories] SET [ImageUrl] = N''https://images.unsplash.com/photo-1515562141207-7a88fb7ce338?auto=format&fit=crop&w=120&h=120'', [ParentCategoryId] = NULL
    WHERE [Id] = ''00000000-0000-0000-0000-000000000011'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513100328_AddSubCategoriesAndProducts'
)
BEGIN
    EXEC(N'UPDATE [Categories] SET [ImageUrl] = N''https://images.unsplash.com/photo-1549465220-1a8b9238cd48?auto=format&fit=crop&w=120&h=120'', [ParentCategoryId] = NULL
    WHERE [Id] = ''00000000-0000-0000-0000-000000000012'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513100328_AddSubCategoriesAndProducts'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAtUtc', N'Description', N'DisplayOrder', N'ImageUrl', N'IsDeleted', N'Name', N'ParentCategoryId', N'Slug', N'UpdatedAtUtc') AND [object_id] = OBJECT_ID(N'[Categories]'))
        SET IDENTITY_INSERT [Categories] ON;
    EXEC(N'INSERT INTO [Categories] ([Id], [CreatedAtUtc], [Description], [DisplayOrder], [ImageUrl], [IsDeleted], [Name], [ParentCategoryId], [Slug], [UpdatedAtUtc])
    VALUES (''00000000-0000-0000-0000-000000000013'', ''2026-05-10T00:00:00.0000000Z'', N''Laptops, ultrabooks, and desktop computers.'', 1, N''https://images.unsplash.com/photo-1496181133206-80ce9b88a853?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''Laptops & Desktops'', ''00000000-0000-0000-0000-000000000001'', N''laptops-desktops'', NULL),
    (''00000000-0000-0000-0000-000000000014'', ''2026-05-10T00:00:00.0000000Z'', N''Smartphones, smartwatches, and earbuds.'', 2, N''https://images.unsplash.com/photo-1523275335684-37898b6baf30?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''Phones & Wearables'', ''00000000-0000-0000-0000-000000000001'', N''phones-wearables'', NULL),
    (''00000000-0000-0000-0000-000000000015'', ''2026-05-10T00:00:00.0000000Z'', N''Sofas, armchairs, and accent seating.'', 1, N''https://images.unsplash.com/photo-1555041469-a586c61ea9bc?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''Sofas & Chairs'', ''00000000-0000-0000-0000-000000000002'', N''sofas-chairs'', NULL),
    (''00000000-0000-0000-0000-000000000016'', ''2026-05-10T00:00:00.0000000Z'', N''Floor lamps, desk lamps, and pendant lights.'', 2, N''https://images.unsplash.com/photo-1534575950704-07c5a26b1e00?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''Lighting & Lamps'', ''00000000-0000-0000-0000-000000000002'', N''lighting-lamps'', NULL),
    (''00000000-0000-0000-0000-000000000017'', ''2026-05-10T00:00:00.0000000Z'', N''RC cars, action figures, and outdoor play toys.'', 1, N''https://images.unsplash.com/photo-1587654780291-39c9404d746b?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''Kids'''' Toys'', ''00000000-0000-0000-0000-000000000003'', N''kids-toys'', NULL),
    (''00000000-0000-0000-0000-000000000018'', ''2026-05-10T00:00:00.0000000Z'', N''Family board games, card games, and puzzles.'', 2, N''https://images.unsplash.com/photo-1566576912321-d58ddd7a6088?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''Board Games'', ''00000000-0000-0000-0000-000000000003'', N''board-games'', NULL),
    (''00000000-0000-0000-0000-000000000019'', ''2026-05-10T00:00:00.0000000Z'', N''Dining sets, loungers, and parasols.'', 1, N''https://images.unsplash.com/photo-1600210492493-0946911123ea?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''Garden Furniture'', ''00000000-0000-0000-0000-000000000004'', N''garden-furniture'', NULL),
    (''00000000-0000-0000-0000-000000000020'', ''2026-05-10T00:00:00.0000000Z'', N''Charcoal, gas, and electric barbecues.'', 2, N''https://images.unsplash.com/photo-1416879595882-3373a0480b5b?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''BBQs & Grills'', ''00000000-0000-0000-0000-000000000004'', N''bbqs-grills'', NULL),
    (''00000000-0000-0000-0000-000000000021'', ''2026-05-10T00:00:00.0000000Z'', N''Coffee machines, air fryers, blenders, and more.'', 1, N''https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''Kitchen Appliances'', ''00000000-0000-0000-0000-000000000005'', N''kitchen-appliances'', NULL),
    (''00000000-0000-0000-0000-000000000022'', ''2026-05-10T00:00:00.0000000Z'', N''Washing machines, vacuums, and steam mops.'', 2, N''https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''Laundry & Cleaning'', ''00000000-0000-0000-0000-000000000005'', N''laundry-cleaning'', NULL),
    (''00000000-0000-0000-0000-000000000023'', ''2026-05-10T00:00:00.0000000Z'', N''Weights, dumbbells, yoga mats, and home gym gear.'', 1, N''https://images.unsplash.com/photo-1517836357463-d25dfeac3438?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''Fitness Equipment'', ''00000000-0000-0000-0000-000000000006'', N''fitness-equipment'', NULL),
    (''00000000-0000-0000-0000-000000000024'', ''2026-05-10T00:00:00.0000000Z'', N''Bikes, tents, and outdoor adventure equipment.'', 2, N''https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''Outdoor Sports'', ''00000000-0000-0000-0000-000000000006'', N''outdoor-sports'', NULL),
    (''00000000-0000-0000-0000-000000000025'', ''2026-05-10T00:00:00.0000000Z'', N''Straighteners, curlers, dryers, and hair tools.'', 1, N''https://images.unsplash.com/photo-1596462502278-27bfdc403348?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''Hair & Styling'', ''00000000-0000-0000-0000-000000000007'', N''hair-styling'', NULL),
    (''00000000-0000-0000-0000-000000000026'', ''2026-05-10T00:00:00.0000000Z'', N''Moisturisers, serums, cleansers, and skin kits.'', 2, N''https://images.unsplash.com/photo-1596462502278-27bfdc403348?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''Skin Care'', ''00000000-0000-0000-0000-000000000007'', N''skin-care'', NULL),
    (''00000000-0000-0000-0000-000000000027'', ''2026-05-10T00:00:00.0000000Z'', N''Cots, cot beds, wardrobes, and nursery sets.'', 1, N''https://images.unsplash.com/photo-1515488042361-ee00e0ddd4e4?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''Nursery Furniture'', ''00000000-0000-0000-0000-000000000008'', N''nursery-furniture'', NULL),
    (''00000000-0000-0000-0000-000000000028'', ''2026-05-10T00:00:00.0000000Z'', N''Bottle warmers, monitors, and baby healthcare.'', 2, N''https://images.unsplash.com/photo-1515488042361-ee00e0ddd4e4?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''Feeding & Healthcare'', ''00000000-0000-0000-0000-000000000008'', N''feeding-healthcare'', NULL),
    (''00000000-0000-0000-0000-000000000029'', ''2026-05-10T00:00:00.0000000Z'', N''Vitamins, minerals, and nutritional supplements.'', 1, N''https://images.unsplash.com/photo-1505576399279-565b52d4ac71?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''Supplements'', ''00000000-0000-0000-0000-000000000009'', N''supplements'', NULL),
    (''00000000-0000-0000-0000-000000000030'', ''2026-05-10T00:00:00.0000000Z'', N''Scales, blood pressure monitors, and trackers.'', 2, N''https://images.unsplash.com/photo-1505576399279-565b52d4ac71?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''Wellness Devices'', ''00000000-0000-0000-0000-000000000009'', N''wellness-devices'', NULL),
    (''00000000-0000-0000-0000-000000000031'', ''2026-05-10T00:00:00.0000000Z'', N''Men''''s tops, bottoms, jackets, and accessories.'', 1, N''https://images.unsplash.com/photo-1441986300917-64674bd600d8?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''Men''''s Fashion'', ''00000000-0000-0000-0000-000000000010'', N''mens-fashion'', NULL),
    (''00000000-0000-0000-0000-000000000032'', ''2026-05-10T00:00:00.0000000Z'', N''Women''''s dresses, tops, and seasonal fashion.'', 2, N''https://images.unsplash.com/photo-1441986300917-64674bd600d8?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''Women''''s Fashion'', ''00000000-0000-0000-0000-000000000010'', N''womens-fashion'', NULL),
    (''00000000-0000-0000-0000-000000000033'', ''2026-05-10T00:00:00.0000000Z'', N''Gold, silver, and gemstone rings and necklaces.'', 1, N''https://images.unsplash.com/photo-1515562141207-7a88fb7ce338?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''Rings & Necklaces'', ''00000000-0000-0000-0000-000000000011'', N''rings-necklaces'', NULL),
    (''00000000-0000-0000-0000-000000000034'', ''2026-05-10T00:00:00.0000000Z'', N''Charm bracelets, cuffs, studs, and drop earrings.'', 2, N''https://images.unsplash.com/photo-1515562141207-7a88fb7ce338?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''Bracelets & Earrings'', ''00000000-0000-0000-0000-000000000011'', N''bracelets-earrings'', NULL),
    (''00000000-0000-0000-0000-000000000035'', ''2026-05-10T00:00:00.0000000Z'', N''Curated pamper sets, food hampers, and gift bundles.'', 1, N''https://images.unsplash.com/photo-1549465220-1a8b9238cd48?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''Gift Sets'', ''00000000-0000-0000-0000-000000000012'', N''gift-sets'', NULL),
    (''00000000-0000-0000-0000-000000000036'', ''2026-05-10T00:00:00.0000000Z'', N''Christmas, birthday, and special occasion gift ideas.'', 2, N''https://images.unsplash.com/photo-1549465220-1a8b9238cd48?auto=format&fit=crop&w=120&h=120'', CAST(0 AS bit), N''Seasonal Gifts'', ''00000000-0000-0000-0000-000000000012'', N''seasonal-gifts'', NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAtUtc', N'Description', N'DisplayOrder', N'ImageUrl', N'IsDeleted', N'Name', N'ParentCategoryId', N'Slug', N'UpdatedAtUtc') AND [object_id] = OBJECT_ID(N'[Categories]'))
        SET IDENTITY_INSERT [Categories] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513100328_AddSubCategoriesAndProducts'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'BrandId', N'CategoryId', N'CreatedAtUtc', N'FullDescription', N'IsDeleted', N'IsFeatured', N'Name', N'Price', N'Rating', N'ReviewCount', N'ShortDescription', N'Sku', N'Slug', N'UpdatedAtUtc', N'WasPrice') AND [object_id] = OBJECT_ID(N'[Products]'))
        SET IDENTITY_INSERT [Products] ON;
    EXEC(N'INSERT INTO [Products] ([Id], [BrandId], [CategoryId], [CreatedAtUtc], [FullDescription], [IsDeleted], [IsFeatured], [Name], [Price], [Rating], [ReviewCount], [ShortDescription], [Sku], [Slug], [UpdatedAtUtc], [WasPrice])
    VALUES (''00000000-0000-0000-0000-000000000313'', ''00000000-0000-0000-0000-000000000101'', ''00000000-0000-0000-0000-000000000013'', ''2026-05-10T00:00:00.0000000Z'', N''Ultra-thin 13 inch laptop with all-day battery and fast SSD. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(1 AS bit), N''Astra 13 Ultrabook'', 499.99, 4.5, 72, N''Ultra-thin 13 inch laptop with all-day battery and fast SSD.'', N''SKU-AST-130'', N''astra-13-ultrabook'', NULL, 599.99),
    (''00000000-0000-0000-0000-000000000314'', ''00000000-0000-0000-0000-000000000102'', ''00000000-0000-0000-0000-000000000014'', ''2026-05-10T00:00:00.0000000Z'', N''Compact earbuds with active noise cancellation and deep bass. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(1 AS bit), N''Northline True Wireless Earbuds'', 69.99, 4.4, 213, N''Compact earbuds with active noise cancellation and deep bass.'', N''SKU-NOR-E20'', N''northline-true-wireless-earbuds'', NULL, 89.99),
    (''00000000-0000-0000-0000-000000000315'', ''00000000-0000-0000-0000-000000000105'', ''00000000-0000-0000-0000-000000000017'', ''2026-05-10T00:00:00.0000000Z'', N''High-speed remote control racing car for kids aged 6 and up. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(1 AS bit), N''Playwell RC Racing Car'', 44.99, 4.6, 87, N''High-speed remote control racing car for kids aged 6 and up.'', N''SKU-PLA-RC1'', N''playwell-rc-racing-car'', NULL, NULL),
    (''00000000-0000-0000-0000-000000000316'', ''00000000-0000-0000-0000-000000000105'', ''00000000-0000-0000-0000-000000000018'', ''2026-05-10T00:00:00.0000000Z'', N''Classic strategy board game for 2–6 players, ages 8 and up. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(0 AS bit), N''Playwell Family Board Game'', 24.99, 4.8, 156, N''Classic strategy board game for 2–6 players, ages 8 and up.'', N''SKU-PLA-BG2'', N''playwell-family-board-game'', NULL, NULL),
    (''00000000-0000-0000-0000-000000000317'', ''00000000-0000-0000-0000-000000000102'', ''00000000-0000-0000-0000-000000000019'', ''2026-05-10T00:00:00.0000000Z'', N''Four-seater rattan dining table and chair set with cushions. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(1 AS bit), N''Northline Garden Dining Set'', 299.99, 4.2, 34, N''Four-seater rattan dining table and chair set with cushions.'', N''SKU-NOR-GD4'', N''northline-garden-dining-set'', NULL, 349.99),
    (''00000000-0000-0000-0000-000000000318'', ''00000000-0000-0000-0000-000000000103'', ''00000000-0000-0000-0000-000000000021'', ''2026-05-10T00:00:00.0000000Z'', N''Compact bean-to-cup espresso machine with built-in grinder. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(1 AS bit), N''Kestral Espresso Machine'', 119.99, 4.6, 198, N''Compact bean-to-cup espresso machine with built-in grinder.'', N''SKU-KES-EM3'', N''kestral-espresso-machine'', NULL, 149.99),
    (''00000000-0000-0000-0000-000000000319'', ''00000000-0000-0000-0000-000000000106'', ''00000000-0000-0000-0000-000000000023'', ''2026-05-10T00:00:00.0000000Z'', N''Adjustable 2–24 kg dumbbell set with quick-lock dial system. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(1 AS bit), N''VeloFit Adjustable Dumbbell Set'', 129.99, 4.5, 91, N''Adjustable 2–24 kg dumbbell set with quick-lock dial system.'', N''SKU-VEL-DB5'', N''velofit-adjustable-dumbbell-set'', NULL, NULL),
    (''00000000-0000-0000-0000-000000000320'', ''00000000-0000-0000-0000-000000000101'', ''00000000-0000-0000-0000-000000000025'', ''2026-05-10T00:00:00.0000000Z'', N''Ceramic floating plates with ionic technology for frizz-free styling. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(0 AS bit), N''Astra Pro Hair Straightener'', 59.99, 4.3, 143, N''Ceramic floating plates with ionic technology for frizz-free styling.'', N''SKU-AST-HS9'', N''astra-pro-hair-straightener'', NULL, 79.99),
    (''00000000-0000-0000-0000-000000000321'', ''00000000-0000-0000-0000-000000000104'', ''00000000-0000-0000-0000-000000000027'', ''2026-05-10T00:00:00.0000000Z'', N''Converts from cot to toddler bed, with adjustable base heights. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(1 AS bit), N''Homekind Convertible Cot Bed'', 189.99, 4.7, 62, N''Converts from cot to toddler bed, with adjustable base heights.'', N''SKU-HOM-CB8'', N''homekind-convertible-cot-bed'', NULL, 229.99),
    (''00000000-0000-0000-0000-000000000322'', ''00000000-0000-0000-0000-000000000102'', ''00000000-0000-0000-0000-000000000029'', ''2026-05-10T00:00:00.0000000Z'', N''Daily multivitamin complex with vitamin D, zinc, and magnesium. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(0 AS bit), N''Northline Vitamin Complex 90'', 19.99, 4.4, 289, N''Daily multivitamin complex with vitamin D, zinc, and magnesium.'', N''SKU-NOR-VC9'', N''northline-vitamin-complex-90'', NULL, NULL),
    (''00000000-0000-0000-0000-000000000323'', ''00000000-0000-0000-0000-000000000103'', ''00000000-0000-0000-0000-000000000030'', ''2026-05-10T00:00:00.0000000Z'', N''Bluetooth body composition scale tracking weight, BMI, and fat. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(1 AS bit), N''Kestral Smart Body Scale'', 34.99, 4.3, 177, N''Bluetooth body composition scale tracking weight, BMI, and fat.'', N''SKU-KES-SC2'', N''kestral-smart-body-scale'', NULL, 44.99),
    (''00000000-0000-0000-0000-000000000324'', ''00000000-0000-0000-0000-000000000101'', ''00000000-0000-0000-0000-000000000031'', ''2026-05-10T00:00:00.0000000Z'', N''Heavyweight cotton-blend zip hoodie with kangaroo pocket. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(0 AS bit), N''Astra Premium Zip Hoodie'', 39.99, 4.2, 95, N''Heavyweight cotton-blend zip hoodie with kangaroo pocket.'', N''SKU-AST-ZH4'', N''astra-premium-zip-hoodie'', NULL, 49.99),
    (''00000000-0000-0000-0000-000000000325'', ''00000000-0000-0000-0000-000000000101'', ''00000000-0000-0000-0000-000000000033'', ''2026-05-10T00:00:00.0000000Z'', N''Delicate sterling silver heart pendant on a fine trace chain. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(0 AS bit), N''Astra Sterling Silver Pendant'', 29.99, 4.5, 63, N''Delicate sterling silver heart pendant on a fine trace chain.'', N''SKU-AST-SP1'', N''astra-sterling-silver-pendant'', NULL, NULL),
    (''00000000-0000-0000-0000-000000000326'', ''00000000-0000-0000-0000-000000000104'', ''00000000-0000-0000-0000-000000000035'', ''2026-05-10T00:00:00.0000000Z'', N''Indulgent bath and body pamper set in a keepsake gift box. Built for the Selqi demo catalog with deterministic seed data.'', CAST(0 AS bit), CAST(1 AS bit), N''Homekind Luxury Spa Gift Set'', 49.99, 4.8, 112, N''Indulgent bath and body pamper set in a keepsake gift box.'', N''SKU-HOM-GS6'', N''homekind-luxury-spa-gift-set'', NULL, 64.99)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'BrandId', N'CategoryId', N'CreatedAtUtc', N'FullDescription', N'IsDeleted', N'IsFeatured', N'Name', N'Price', N'Rating', N'ReviewCount', N'ShortDescription', N'Sku', N'Slug', N'UpdatedAtUtc', N'WasPrice') AND [object_id] = OBJECT_ID(N'[Products]'))
        SET IDENTITY_INSERT [Products] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513100328_AddSubCategoriesAndProducts'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ProductId', N'QuantityAvailable', N'StoreId', N'UpdatedAtUtc') AND [object_id] = OBJECT_ID(N'[Inventory]'))
        SET IDENTITY_INSERT [Inventory] ON;
    EXEC(N'INSERT INTO [Inventory] ([Id], [ProductId], [QuantityAvailable], [StoreId], [UpdatedAtUtc])
    VALUES (''00000000-0000-0000-0000-000000000761'', ''00000000-0000-0000-0000-000000000313'', 0, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000762'', ''00000000-0000-0000-0000-000000000313'', 17, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000763'', ''00000000-0000-0000-0000-000000000313'', 18, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000764'', ''00000000-0000-0000-0000-000000000313'', 19, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000765'', ''00000000-0000-0000-0000-000000000313'', 0, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000766'', ''00000000-0000-0000-0000-000000000314'', 17, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000767'', ''00000000-0000-0000-0000-000000000314'', 18, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000768'', ''00000000-0000-0000-0000-000000000314'', 19, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000769'', ''00000000-0000-0000-0000-000000000314'', 0, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000770'', ''00000000-0000-0000-0000-000000000314'', 21, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000771'', ''00000000-0000-0000-0000-000000000315'', 18, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000772'', ''00000000-0000-0000-0000-000000000315'', 19, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000773'', ''00000000-0000-0000-0000-000000000315'', 0, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000774'', ''00000000-0000-0000-0000-000000000315'', 21, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000775'', ''00000000-0000-0000-0000-000000000315'', 22, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000776'', ''00000000-0000-0000-0000-000000000316'', 19, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000777'', ''00000000-0000-0000-0000-000000000316'', 0, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000778'', ''00000000-0000-0000-0000-000000000316'', 21, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000779'', ''00000000-0000-0000-0000-000000000316'', 22, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000780'', ''00000000-0000-0000-0000-000000000316'', 23, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000781'', ''00000000-0000-0000-0000-000000000317'', 0, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000782'', ''00000000-0000-0000-0000-000000000317'', 21, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000783'', ''00000000-0000-0000-0000-000000000317'', 22, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000784'', ''00000000-0000-0000-0000-000000000317'', 23, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000785'', ''00000000-0000-0000-0000-000000000317'', 0, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000786'', ''00000000-0000-0000-0000-000000000318'', 21, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000787'', ''00000000-0000-0000-0000-000000000318'', 22, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000788'', ''00000000-0000-0000-0000-000000000318'', 23, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000789'', ''00000000-0000-0000-0000-000000000318'', 0, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000790'', ''00000000-0000-0000-0000-000000000318'', 25, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000791'', ''00000000-0000-0000-0000-000000000319'', 22, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000792'', ''00000000-0000-0000-0000-000000000319'', 23, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000793'', ''00000000-0000-0000-0000-000000000319'', 0, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000794'', ''00000000-0000-0000-0000-000000000319'', 25, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000795'', ''00000000-0000-0000-0000-000000000319'', 26, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000796'', ''00000000-0000-0000-0000-000000000320'', 23, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000797'', ''00000000-0000-0000-0000-000000000320'', 0, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000798'', ''00000000-0000-0000-0000-000000000320'', 25, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000799'', ''00000000-0000-0000-0000-000000000320'', 26, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000800'', ''00000000-0000-0000-0000-000000000320'', 27, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000801'', ''00000000-0000-0000-0000-000000000321'', 0, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000802'', ''00000000-0000-0000-0000-000000000321'', 25, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z'');
    INSERT INTO [Inventory] ([Id], [ProductId], [QuantityAvailable], [StoreId], [UpdatedAtUtc])
    VALUES (''00000000-0000-0000-0000-000000000803'', ''00000000-0000-0000-0000-000000000321'', 26, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000804'', ''00000000-0000-0000-0000-000000000321'', 27, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000805'', ''00000000-0000-0000-0000-000000000321'', 0, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000806'', ''00000000-0000-0000-0000-000000000322'', 25, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000807'', ''00000000-0000-0000-0000-000000000322'', 26, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000808'', ''00000000-0000-0000-0000-000000000322'', 27, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000809'', ''00000000-0000-0000-0000-000000000322'', 0, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000810'', ''00000000-0000-0000-0000-000000000322'', 29, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000811'', ''00000000-0000-0000-0000-000000000323'', 26, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000812'', ''00000000-0000-0000-0000-000000000323'', 27, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000813'', ''00000000-0000-0000-0000-000000000323'', 0, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000814'', ''00000000-0000-0000-0000-000000000323'', 29, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000815'', ''00000000-0000-0000-0000-000000000323'', 30, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000816'', ''00000000-0000-0000-0000-000000000324'', 27, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000817'', ''00000000-0000-0000-0000-000000000324'', 0, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000818'', ''00000000-0000-0000-0000-000000000324'', 29, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000819'', ''00000000-0000-0000-0000-000000000324'', 30, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000820'', ''00000000-0000-0000-0000-000000000324'', 31, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000821'', ''00000000-0000-0000-0000-000000000325'', 0, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000822'', ''00000000-0000-0000-0000-000000000325'', 29, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000823'', ''00000000-0000-0000-0000-000000000325'', 30, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000824'', ''00000000-0000-0000-0000-000000000325'', 31, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000825'', ''00000000-0000-0000-0000-000000000325'', 0, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000826'', ''00000000-0000-0000-0000-000000000326'', 29, ''00000000-0000-0000-0000-000000000201'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000827'', ''00000000-0000-0000-0000-000000000326'', 30, ''00000000-0000-0000-0000-000000000202'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000828'', ''00000000-0000-0000-0000-000000000326'', 31, ''00000000-0000-0000-0000-000000000203'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000829'', ''00000000-0000-0000-0000-000000000326'', 0, ''00000000-0000-0000-0000-000000000204'', ''2026-05-10T00:00:00.0000000Z''),
    (''00000000-0000-0000-0000-000000000830'', ''00000000-0000-0000-0000-000000000326'', 33, ''00000000-0000-0000-0000-000000000205'', ''2026-05-10T00:00:00.0000000Z'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ProductId', N'QuantityAvailable', N'StoreId', N'UpdatedAtUtc') AND [object_id] = OBJECT_ID(N'[Inventory]'))
        SET IDENTITY_INSERT [Inventory] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513100328_AddSubCategoriesAndProducts'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AltText', N'DisplayOrder', N'ProductId', N'Url') AND [object_id] = OBJECT_ID(N'[ProductImages]'))
        SET IDENTITY_INSERT [ProductImages] ON;
    EXEC(N'INSERT INTO [ProductImages] ([Id], [AltText], [DisplayOrder], [ProductId], [Url])
    VALUES (''00000000-0000-0000-0000-000000000425'', N''Astra 13 Ultrabook'', 1, ''00000000-0000-0000-0000-000000000313'', N''https://images.unsplash.com/photo-1531297484001-80022131f5a1?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000426'', N''Astra 13 Ultrabook on desk'', 2, ''00000000-0000-0000-0000-000000000313'', N''https://images.unsplash.com/photo-1496181133206-80ce9b88a853?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000427'', N''Northline True Wireless Earbuds'', 1, ''00000000-0000-0000-0000-000000000314'', N''https://images.unsplash.com/photo-1590658268037-6bf12165a8df?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000428'', N''Northline Earbuds case open'', 2, ''00000000-0000-0000-0000-000000000314'', N''https://images.unsplash.com/photo-1505740420928-5e560c06d30e?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000429'', N''Playwell RC Racing Car'', 1, ''00000000-0000-0000-0000-000000000315'', N''https://images.unsplash.com/photo-1558618666-fcd25c85cd64?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000430'', N''Playwell RC Racing Car in action'', 2, ''00000000-0000-0000-0000-000000000315'', N''https://images.unsplash.com/photo-1587654780291-39c9404d746b?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000431'', N''Playwell Family Board Game'', 1, ''00000000-0000-0000-0000-000000000316'', N''https://images.unsplash.com/photo-1550459045-66bab08aab22?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000432'', N''Playwell Board Game pieces'', 2, ''00000000-0000-0000-0000-000000000316'', N''https://images.unsplash.com/photo-1566576912321-d58ddd7a6088?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000433'', N''Northline Garden Dining Set'', 1, ''00000000-0000-0000-0000-000000000317'', N''https://images.unsplash.com/photo-1600210492493-0946911123ea?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000434'', N''Northline Garden Dining Set styled'', 2, ''00000000-0000-0000-0000-000000000317'', N''https://images.unsplash.com/photo-1561702771-eb3cf35eedd2?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000435'', N''Kestral Espresso Machine'', 1, ''00000000-0000-0000-0000-000000000318'', N''https://images.unsplash.com/photo-1495474472287-4d71bcdd2085?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000436'', N''Kestral Espresso Machine brewing'', 2, ''00000000-0000-0000-0000-000000000318'', N''https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000437'', N''VeloFit Adjustable Dumbbell Set'', 1, ''00000000-0000-0000-0000-000000000319'', N''https://images.unsplash.com/photo-1583454110551-21f2fa2afe61?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000438'', N''VeloFit Dumbbells in use'', 2, ''00000000-0000-0000-0000-000000000319'', N''https://images.unsplash.com/photo-1534438327276-14e5300c3a48?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000439'', N''Astra Pro Hair Straightener'', 1, ''00000000-0000-0000-0000-000000000320'', N''https://images.unsplash.com/photo-1522338242992-e1a54906a8da?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000440'', N''Astra Hair Straightener styling'', 2, ''00000000-0000-0000-0000-000000000320'', N''https://images.unsplash.com/photo-1527799820374-dcf8d9d4a388?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000441'', N''Homekind Convertible Cot Bed'', 1, ''00000000-0000-0000-0000-000000000321'', N''https://images.unsplash.com/photo-1515488042361-ee00e0ddd4e4?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000442'', N''Homekind Cot Bed as toddler bed'', 2, ''00000000-0000-0000-0000-000000000321'', N''https://images.unsplash.com/photo-1491013516836-7db643ee125a?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000443'', N''Northline Vitamin Complex 90'', 1, ''00000000-0000-0000-0000-000000000322'', N''https://images.unsplash.com/photo-1505576399279-565b52d4ac71?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000444'', N''Northline Vitamin Complex capsules'', 2, ''00000000-0000-0000-0000-000000000322'', N''https://images.unsplash.com/photo-1512207736890-6ffed8a84e8d?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000445'', N''Kestral Smart Body Scale'', 1, ''00000000-0000-0000-0000-000000000323'', N''https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000446'', N''Kestral Scale in bathroom'', 2, ''00000000-0000-0000-0000-000000000323'', N''https://images.unsplash.com/photo-1517836357463-d25dfeac3438?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000447'', N''Astra Premium Zip Hoodie'', 1, ''00000000-0000-0000-0000-000000000324'', N''https://images.unsplash.com/photo-1620799140408-edc6dcb6d633?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000448'', N''Astra Zip Hoodie styled'', 2, ''00000000-0000-0000-0000-000000000324'', N''https://images.unsplash.com/photo-1441986300917-64674bd600d8?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000449'', N''Astra Sterling Silver Pendant'', 1, ''00000000-0000-0000-0000-000000000325'', N''https://images.unsplash.com/photo-1515562141207-7a88fb7ce338?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000450'', N''Astra Silver Pendant on model'', 2, ''00000000-0000-0000-0000-000000000325'', N''https://images.unsplash.com/photo-1611652022419-a9419f74343d?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000451'', N''Homekind Luxury Spa Gift Set'', 1, ''00000000-0000-0000-0000-000000000326'', N''https://images.unsplash.com/photo-1549465220-1a8b9238cd48?auto=format&fit=crop&w=600&h=500''),
    (''00000000-0000-0000-0000-000000000452'', N''Homekind Spa Gift Set contents'', 2, ''00000000-0000-0000-0000-000000000326'', N''https://images.unsplash.com/photo-1596462502278-27bfdc403348?auto=format&fit=crop&w=600&h=500'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AltText', N'DisplayOrder', N'ProductId', N'Url') AND [object_id] = OBJECT_ID(N'[ProductImages]'))
        SET IDENTITY_INSERT [ProductImages] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513100328_AddSubCategoriesAndProducts'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DisplayOrder', N'Name', N'ProductId', N'Value') AND [object_id] = OBJECT_ID(N'[ProductSpecifications]'))
        SET IDENTITY_INSERT [ProductSpecifications] ON;
    EXEC(N'INSERT INTO [ProductSpecifications] ([Id], [DisplayOrder], [Name], [ProductId], [Value])
    VALUES (''00000000-0000-0000-0000-000000000525'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000313'', N''SKU-AST-130''),
    (''00000000-0000-0000-0000-000000000526'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000313'', N''1 year''),
    (''00000000-0000-0000-0000-000000000527'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000314'', N''SKU-NOR-E20''),
    (''00000000-0000-0000-0000-000000000528'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000314'', N''1 year''),
    (''00000000-0000-0000-0000-000000000529'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000315'', N''SKU-PLA-RC1''),
    (''00000000-0000-0000-0000-000000000530'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000315'', N''1 year''),
    (''00000000-0000-0000-0000-000000000531'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000316'', N''SKU-PLA-BG2''),
    (''00000000-0000-0000-0000-000000000532'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000316'', N''1 year''),
    (''00000000-0000-0000-0000-000000000533'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000317'', N''SKU-NOR-GD4''),
    (''00000000-0000-0000-0000-000000000534'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000317'', N''1 year''),
    (''00000000-0000-0000-0000-000000000535'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000318'', N''SKU-KES-EM3''),
    (''00000000-0000-0000-0000-000000000536'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000318'', N''1 year''),
    (''00000000-0000-0000-0000-000000000537'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000319'', N''SKU-VEL-DB5''),
    (''00000000-0000-0000-0000-000000000538'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000319'', N''1 year''),
    (''00000000-0000-0000-0000-000000000539'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000320'', N''SKU-AST-HS9''),
    (''00000000-0000-0000-0000-000000000540'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000320'', N''1 year''),
    (''00000000-0000-0000-0000-000000000541'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000321'', N''SKU-HOM-CB8''),
    (''00000000-0000-0000-0000-000000000542'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000321'', N''1 year''),
    (''00000000-0000-0000-0000-000000000543'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000322'', N''SKU-NOR-VC9''),
    (''00000000-0000-0000-0000-000000000544'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000322'', N''1 year''),
    (''00000000-0000-0000-0000-000000000545'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000323'', N''SKU-KES-SC2''),
    (''00000000-0000-0000-0000-000000000546'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000323'', N''1 year''),
    (''00000000-0000-0000-0000-000000000547'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000324'', N''SKU-AST-ZH4''),
    (''00000000-0000-0000-0000-000000000548'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000324'', N''1 year''),
    (''00000000-0000-0000-0000-000000000549'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000325'', N''SKU-AST-SP1''),
    (''00000000-0000-0000-0000-000000000550'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000325'', N''1 year''),
    (''00000000-0000-0000-0000-000000000551'', 1, N''SKU'', ''00000000-0000-0000-0000-000000000326'', N''SKU-HOM-GS6''),
    (''00000000-0000-0000-0000-000000000552'', 2, N''Warranty'', ''00000000-0000-0000-0000-000000000326'', N''1 year'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DisplayOrder', N'Name', N'ProductId', N'Value') AND [object_id] = OBJECT_ID(N'[ProductSpecifications]'))
        SET IDENTITY_INSERT [ProductSpecifications] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513100328_AddSubCategoriesAndProducts'
)
BEGIN
    CREATE INDEX [IX_Categories_ParentCategoryId] ON [Categories] ([ParentCategoryId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513100328_AddSubCategoriesAndProducts'
)
BEGIN
    ALTER TABLE [Categories] ADD CONSTRAINT [FK_Categories_Categories_ParentCategoryId] FOREIGN KEY ([ParentCategoryId]) REFERENCES [Categories] ([Id]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513100328_AddSubCategoriesAndProducts'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260513100328_AddSubCategoriesAndProducts', N'10.0.11');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513110853_AddStoreEnrichmentAndInventoryThresholds'
)
BEGIN
    ALTER TABLE [Stores] ADD [AddressLine1] nvarchar(250) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513110853_AddStoreEnrichmentAndInventoryThresholds'
)
BEGIN
    ALTER TABLE [Stores] ADD [AddressLine2] nvarchar(250) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513110853_AddStoreEnrichmentAndInventoryThresholds'
)
BEGIN
    ALTER TABLE [Stores] ADD [Phone] nvarchar(20) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513110853_AddStoreEnrichmentAndInventoryThresholds'
)
BEGIN
    ALTER TABLE [Stores] ADD [StoreType] nvarchar(30) NOT NULL DEFAULT N'Standalone';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513110853_AddStoreEnrichmentAndInventoryThresholds'
)
BEGIN
    ALTER TABLE [Stores] ADD [Latitude] float NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513110853_AddStoreEnrichmentAndInventoryThresholds'
)
BEGIN
    ALTER TABLE [Stores] ADD [Longitude] float NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513110853_AddStoreEnrichmentAndInventoryThresholds'
)
BEGIN
    ALTER TABLE [Stores] ADD [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513110853_AddStoreEnrichmentAndInventoryThresholds'
)
BEGIN
    ALTER TABLE [Stores] ADD [UpdatedAtUtc] datetime2 NOT NULL DEFAULT (SYSUTCDATETIME());
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513110853_AddStoreEnrichmentAndInventoryThresholds'
)
BEGIN
    ALTER TABLE [Inventory] ADD [LowStockThreshold] int NOT NULL DEFAULT 5;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513110853_AddStoreEnrichmentAndInventoryThresholds'
)
BEGIN
    ALTER TABLE [Inventory] ADD [LastRestockedAtUtc] datetime2 NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513110853_AddStoreEnrichmentAndInventoryThresholds'
)
BEGIN
    EXEC(N'UPDATE [Stores] SET [AddressLine1] = N''Unit 5, Central Retail Park'', [Phone] = N''01212 345678'', [StoreType] = N''Standalone'', [Latitude] = 52.486199999999997E0, [Longitude] = -1.8904000000000001E0, [IsActive] = CAST(1 AS bit), [UpdatedAtUtc] = ''2024-01-15T00:00:00.0000000Z''
    WHERE [Id] = ''00000000-0000-0000-0000-000000000201'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513110853_AddStoreEnrichmentAndInventoryThresholds'
)
BEGIN
    EXEC(N'UPDATE [Stores] SET [AddressLine1] = N''Sainsbury''''s, Riverside Way'', [Phone] = N''01132 456789'', [StoreType] = N''Concession'', [Latitude] = 53.799700000000001E0, [Longitude] = -1.5491999999999999E0, [IsActive] = CAST(1 AS bit), [UpdatedAtUtc] = ''2024-01-15T00:00:00.0000000Z''
    WHERE [Id] = ''00000000-0000-0000-0000-000000000202'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513110853_AddStoreEnrichmentAndInventoryThresholds'
)
BEGIN
    EXEC(N'UPDATE [Stores] SET [AddressLine1] = N''42 Market Street'', [Phone] = N''01612 567890'', [StoreType] = N''Standalone'', [Latitude] = 53.480800000000002E0, [Longitude] = -2.2425999999999999E0, [IsActive] = CAST(1 AS bit), [UpdatedAtUtc] = ''2024-01-15T00:00:00.0000000Z''
    WHERE [Id] = ''00000000-0000-0000-0000-000000000203'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513110853_AddStoreEnrichmentAndInventoryThresholds'
)
BEGIN
    EXEC(N'UPDATE [Stores] SET [AddressLine1] = N''Sainsbury''''s, Harbourside'', [Phone] = N''01172 678901'', [StoreType] = N''Concession'', [Latitude] = 51.454500000000003E0, [Longitude] = -2.5878999999999999E0, [IsActive] = CAST(1 AS bit), [UpdatedAtUtc] = ''2024-01-15T00:00:00.0000000Z''
    WHERE [Id] = ''00000000-0000-0000-0000-000000000204'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513110853_AddStoreEnrichmentAndInventoryThresholds'
)
BEGIN
    EXEC(N'UPDATE [Stores] SET [AddressLine1] = N''Sainsbury''''s Local, Station Square'', [Phone] = N''02071 234567'', [StoreType] = N''CollectionPoint'', [Latitude] = 51.507399999999997E0, [Longitude] = -0.087800000000000003E0, [IsActive] = CAST(1 AS bit), [UpdatedAtUtc] = ''2024-01-15T00:00:00.0000000Z''
    WHERE [Id] = ''00000000-0000-0000-0000-000000000205'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513110853_AddStoreEnrichmentAndInventoryThresholds'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260513110853_AddStoreEnrichmentAndInventoryThresholds', N'10.0.11');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513150145_AddSavedCards'
)
BEGIN
    CREATE TABLE [SavedCards] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [CardholderName] nvarchar(100) NOT NULL,
        [Last4] nvarchar(4) NOT NULL,
        [Brand] nvarchar(20) NOT NULL,
        [ExpiryMonth] nvarchar(2) NOT NULL,
        [ExpiryYear] nvarchar(2) NOT NULL,
        [Token] nvarchar(64) NOT NULL,
        [IsDefault] bit NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_SavedCards] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SavedCards_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513150145_AddSavedCards'
)
BEGIN
    CREATE INDEX [IX_SavedCards_UserId] ON [SavedCards] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513150145_AddSavedCards'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260513150145_AddSavedCards', N'10.0.11');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513152022_AddOrderStatusHistory'
)
BEGIN
    CREATE TABLE [OrderStatusHistories] (
        [Id] uniqueidentifier NOT NULL,
        [OrderId] uniqueidentifier NOT NULL,
        [Status] int NOT NULL,
        [Note] nvarchar(500) NULL,
        [ChangedAtUtc] datetime2 NOT NULL DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_OrderStatusHistories] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrderStatusHistories_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513152022_AddOrderStatusHistory'
)
BEGIN
    CREATE INDEX [IX_OrderStatusHistories_OrderId] ON [OrderStatusHistories] ([OrderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513152022_AddOrderStatusHistory'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260513152022_AddOrderStatusHistory', N'10.0.11');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515101011_AddPaymentMethodToOrders'
)
BEGIN
    DECLARE @var nvarchar(max);
    SELECT @var = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Orders]') AND [c].[name] = N'PaymentReference');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [Orders] DROP CONSTRAINT ' + @var + ';');
    ALTER TABLE [Orders] ALTER COLUMN [PaymentReference] nvarchar(150) NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515101011_AddPaymentMethodToOrders'
)
BEGIN
    ALTER TABLE [Orders] ADD [PaymentMethod] nvarchar(20) NOT NULL DEFAULT N'Card';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515101011_AddPaymentMethodToOrders'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260515101011_AddPaymentMethodToOrders', N'10.0.11');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515105245_FixBrokenBoardGameImageUrl'
)
BEGIN
    EXEC(N'UPDATE [ProductImages] SET [Url] = N''https://images.unsplash.com/VJVsEnR_vNE?auto=format&fit=crop&w=600&h=500''
    WHERE [Id] = ''00000000-0000-0000-0000-000000000431'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515105245_FixBrokenBoardGameImageUrl'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260515105245_FixBrokenBoardGameImageUrl', N'10.0.11');
END;

COMMIT;
GO

