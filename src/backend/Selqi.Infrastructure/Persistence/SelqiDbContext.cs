using Selqi.Domain.Catalog;
using Selqi.Domain.Identity;
using Selqi.Domain.Operations;
using Selqi.Domain.Orders;
using Selqi.Domain.Shopping;
using Microsoft.EntityFrameworkCore;

namespace Selqi.Infrastructure.Persistence;

public sealed class SelqiDbContext(DbContextOptions<SelqiDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<ProductSpecification> ProductSpecifications => Set<ProductSpecification>();
    public DbSet<Store> Stores => Set<Store>();
    public DbSet<Inventory> Inventory => Set<Inventory>();
    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Basket> Baskets => Set<Basket>();
    public DbSet<BasketItem> BasketItems => Set<BasketItem>();
    public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<OrderStatusHistory> OrderStatusHistories => Set<OrderStatusHistory>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();
    public DbSet<SavedCard> SavedCards => Set<SavedCard>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(category => category.Id);
            entity.Property(category => category.Name).HasMaxLength(120).IsRequired();
            entity.Property(category => category.Slug).HasMaxLength(140).IsRequired();
            entity.HasIndex(category => category.Slug).IsUnique();
            entity.Property(category => category.Description).HasMaxLength(500);
            entity.Property(category => category.CreatedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasOne(category => category.ParentCategory)
                .WithMany(category => category.SubCategories)
                .HasForeignKey(category => category.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
            entity.HasData(CatalogSeedData.Categories.Select(category => new
            {
                category.Id, category.Name, category.Slug, category.Description,
                category.ImageUrl, category.ParentCategoryId,
                category.DisplayOrder, category.IsDeleted, category.CreatedAtUtc, category.UpdatedAtUtc
            }));
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.ToTable("Brands");
            entity.HasKey(brand => brand.Id);
            entity.Property(brand => brand.Name).HasMaxLength(120).IsRequired();
            entity.Property(brand => brand.Slug).HasMaxLength(140).IsRequired();
            entity.HasIndex(brand => brand.Slug).IsUnique();
            entity.Property(brand => brand.CreatedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasData(CatalogSeedData.Brands.Select(brand => new
            {
                brand.Id, brand.Name, brand.Slug, brand.CreatedAtUtc
            }));
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(product => product.Id);
            entity.Property(product => product.Name).HasMaxLength(220).IsRequired();
            entity.Property(product => product.Slug).HasMaxLength(240).IsRequired();
            entity.Property(product => product.ShortDescription).HasMaxLength(500).IsRequired();
            entity.Property(product => product.FullDescription).HasMaxLength(2000);
            entity.Property(product => product.Sku).HasMaxLength(80).IsRequired();
            entity.Property(product => product.Price).HasPrecision(18, 2);
            entity.Property(product => product.WasPrice).HasPrecision(18, 2);
            entity.Property(product => product.Rating).HasPrecision(3, 2);
            entity.Property(product => product.CreatedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasIndex(product => product.Slug).IsUnique();
            entity.HasIndex(product => product.Sku).IsUnique();
            entity.HasOne(product => product.Category)
                .WithMany(category => category.Products)
                .HasForeignKey(product => product.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(product => product.Brand)
                .WithMany(brand => brand.Products)
                .HasForeignKey(product => product.BrandId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasData(CatalogSeedData.Products.Select(product => new
            {
                product.Id, product.CategoryId, product.BrandId, product.Name, product.Slug,
                product.ShortDescription, product.FullDescription, product.Sku, product.Price,
                product.WasPrice, product.Rating, product.ReviewCount, product.IsFeatured,
                product.IsDeleted, product.CreatedAtUtc, product.UpdatedAtUtc
            }));
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.ToTable("ProductImages");
            entity.HasKey(image => image.Id);
            entity.Property(image => image.Url).HasMaxLength(500).IsRequired();
            entity.Property(image => image.AltText).HasMaxLength(220).IsRequired();
            entity.HasOne(image => image.Product)
                .WithMany(product => product.Images)
                .HasForeignKey(image => image.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasData(CatalogSeedData.ProductImages.Select(image => new
            {
                image.Id, image.ProductId, image.Url, image.AltText, image.DisplayOrder
            }));
        });

        modelBuilder.Entity<ProductSpecification>(entity =>
        {
            entity.ToTable("ProductSpecifications");
            entity.HasKey(specification => specification.Id);
            entity.Property(specification => specification.Name).HasMaxLength(120).IsRequired();
            entity.Property(specification => specification.Value).HasMaxLength(500).IsRequired();
            entity.HasOne(specification => specification.Product)
                .WithMany(product => product.Specifications)
                .HasForeignKey(specification => specification.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasData(CatalogSeedData.ProductSpecifications.Select(specification => new
            {
                specification.Id, specification.ProductId, specification.Name,
                specification.Value, specification.DisplayOrder
            }));
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.ToTable("Stores");
            entity.HasKey(store => store.Id);
            entity.Property(store => store.Name).HasMaxLength(160).IsRequired();
            entity.Property(store => store.AddressLine1).HasMaxLength(250).IsRequired();
            entity.Property(store => store.AddressLine2).HasMaxLength(250);
            entity.Property(store => store.Town).HasMaxLength(120).IsRequired();
            entity.Property(store => store.Postcode).HasMaxLength(20).IsRequired();
            entity.Property(store => store.Phone).HasMaxLength(20);
            entity.Property(store => store.StoreType).HasConversion<string>().HasMaxLength(30);
            entity.Property(store => store.CreatedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.Property(store => store.UpdatedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasData(CatalogSeedData.Stores.Select(store => new
            {
                store.Id, store.Name, store.AddressLine1, store.AddressLine2,
                store.Town, store.Postcode, store.Phone, store.StoreType,
                store.Latitude, store.Longitude, store.IsActive,
                store.CreatedAtUtc, store.UpdatedAtUtc
            }));
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.ToTable("Inventory");
            entity.HasKey(inventory => inventory.Id);
            entity.HasIndex(inventory => new { inventory.ProductId, inventory.StoreId }).IsUnique();
            entity.Property(inventory => inventory.UpdatedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasOne(inventory => inventory.Product)
                .WithMany(product => product.Inventory)
                .HasForeignKey(inventory => inventory.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(inventory => inventory.Store)
                .WithMany(store => store.Inventory)
                .HasForeignKey(inventory => inventory.StoreId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasData(CatalogSeedData.InventoryRecords.Select(inventory => new
            {
                inventory.Id, inventory.ProductId, inventory.StoreId,
                inventory.QuantityAvailable, inventory.LowStockThreshold,
                inventory.LastRestockedAtUtc, inventory.UpdatedAtUtc
            }));
        });

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).HasMaxLength(256).IsRequired();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.PasswordHash).HasMaxLength(512).IsRequired();
            entity.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(u => u.LastName).HasMaxLength(100).IsRequired();
            entity.Property(u => u.Role).HasMaxLength(50).IsRequired().HasDefaultValue(UserRoles.Customer);
            entity.Property(u => u.CreatedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
        });

        modelBuilder.Entity<SavedCard>(entity =>
        {
            entity.ToTable("SavedCards");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.CardholderName).HasMaxLength(100).IsRequired();
            entity.Property(c => c.Last4).HasMaxLength(4).IsRequired();
            entity.Property(c => c.Brand).HasMaxLength(20).IsRequired();
            entity.Property(c => c.ExpiryMonth).HasMaxLength(2).IsRequired();
            entity.Property(c => c.ExpiryYear).HasMaxLength(2).IsRequired();
            entity.Property(c => c.Token).HasMaxLength(64).IsRequired();
            entity.Property(c => c.CreatedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasOne(c => c.User)
                .WithMany(u => u.SavedCards)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("Addresses");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Label).HasMaxLength(100).IsRequired();
            entity.Property(a => a.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(a => a.LastName).HasMaxLength(100).IsRequired();
            entity.Property(a => a.Line1).HasMaxLength(250).IsRequired();
            entity.Property(a => a.Line2).HasMaxLength(250);
            entity.Property(a => a.Town).HasMaxLength(120).IsRequired();
            entity.Property(a => a.County).HasMaxLength(120);
            entity.Property(a => a.Postcode).HasMaxLength(20).IsRequired();
            entity.Property(a => a.Country).HasMaxLength(100).HasDefaultValue("United Kingdom");
            entity.Property(a => a.Phone).HasMaxLength(20);
            entity.Property(a => a.CreatedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Basket>(entity =>
        {
            entity.ToTable("Baskets");
            entity.HasKey(b => b.Id);
            entity.HasIndex(b => b.UserId).IsUnique();
            entity.Property(b => b.CreatedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.Property(b => b.UpdatedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasOne(b => b.User)
                .WithOne(u => u.Basket)
                .HasForeignKey<Basket>(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BasketItem>(entity =>
        {
            entity.ToTable("BasketItems");
            entity.HasKey(i => i.Id);
            entity.Property(i => i.UnitPrice).HasPrecision(18, 2);
            entity.Property(i => i.AddedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasOne(i => i.Basket)
                .WithMany(b => b.Items)
                .HasForeignKey(i => i.BasketId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<WishlistItem>(entity =>
        {
            entity.ToTable("WishlistItems");
            entity.HasKey(w => w.Id);
            entity.HasIndex(w => new { w.UserId, w.ProductId }).IsUnique();
            entity.Property(w => w.AddedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasOne(w => w.User)
                .WithMany(u => u.WishlistItems)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(w => w.Product)
                .WithMany()
                .HasForeignKey(w => w.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");
            entity.HasKey(o => o.Id);
            entity.Property(o => o.OrderNumber).HasMaxLength(50).IsRequired();
            entity.HasIndex(o => o.OrderNumber).IsUnique();
            entity.Property(o => o.SubTotal).HasPrecision(18, 2);
            entity.Property(o => o.DeliveryCharge).HasPrecision(18, 2);
            entity.Property(o => o.Total).HasPrecision(18, 2);
            entity.Property(o => o.DeliveryFirstName).HasMaxLength(100).IsRequired();
            entity.Property(o => o.DeliveryLastName).HasMaxLength(100).IsRequired();
            entity.Property(o => o.DeliveryLine1).HasMaxLength(250).IsRequired();
            entity.Property(o => o.DeliveryLine2).HasMaxLength(250);
            entity.Property(o => o.DeliveryTown).HasMaxLength(120).IsRequired();
            entity.Property(o => o.DeliveryCounty).HasMaxLength(120);
            entity.Property(o => o.DeliveryPostcode).HasMaxLength(20).IsRequired();
            entity.Property(o => o.DeliveryCountry).HasMaxLength(100).HasDefaultValue("United Kingdom");
            entity.Property(o => o.DeliveryPhone).HasMaxLength(20);
            entity.Property(o => o.PaymentMethod).HasMaxLength(20).IsRequired().HasDefaultValue("Card");
            entity.Property(o => o.PaymentReference).HasMaxLength(150).IsRequired();
            entity.Property(o => o.PaymentStatus).HasMaxLength(50).IsRequired();
            entity.Property(o => o.PlacedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.Property(o => o.UpdatedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItems");
            entity.HasKey(i => i.Id);
            entity.Property(i => i.ProductName).HasMaxLength(220).IsRequired();
            entity.Property(i => i.ProductSku).HasMaxLength(80).IsRequired();
            entity.Property(i => i.ProductImageUrl).HasMaxLength(500);
            entity.Property(i => i.UnitPrice).HasPrecision(18, 2);
            entity.Property(i => i.LineTotal).HasPrecision(18, 2);
            entity.HasOne(i => i.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderStatusHistory>(entity =>
        {
            entity.ToTable("OrderStatusHistories");
            entity.HasKey(h => h.Id);
            entity.Property(h => h.Note).HasMaxLength(500);
            entity.Property(h => h.ChangedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasOne(h => h.Order)
                .WithMany(o => o.StatusHistory)
                .HasForeignKey(h => h.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.ToTable("AuditLogs");
            entity.HasKey(log => log.Id);
            entity.Property(log => log.ActorEmail).HasMaxLength(256).IsRequired();
            entity.Property(log => log.Action).HasMaxLength(80).IsRequired();
            entity.Property(log => log.EntityType).HasMaxLength(80).IsRequired();
            entity.Property(log => log.EntityId).HasMaxLength(80).IsRequired();
            entity.Property(log => log.Summary).HasMaxLength(500).IsRequired();
            entity.Property(log => log.CreatedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasIndex(log => log.CreatedAtUtc);
            entity.HasIndex(log => new { log.EntityType, log.EntityId });
        });

        modelBuilder.Entity<PasswordResetToken>(entity =>
        {
            entity.ToTable("PasswordResetTokens");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Token).HasMaxLength(64).IsRequired();
            entity.HasIndex(t => t.Token).IsUnique();
            entity.Property(t => t.CreatedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
