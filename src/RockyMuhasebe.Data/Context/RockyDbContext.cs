using Microsoft.EntityFrameworkCore;
using RockyMuhasebe.Domain.Entities;
using RockyMuhasebe.Domain.Entities.Accounting;
using RockyMuhasebe.Domain.Entities.Admin;
using RockyMuhasebe.Domain.Entities.Customers;
using RockyMuhasebe.Domain.Entities.Warehouse;
using RockyMuhasebe.Domain.Interfaces;

namespace RockyMuhasebe.Data.Context;

/// <summary>
/// Rocky Muhasebe Ana Veritabanı Context'i
/// Tüm entity'lerin veritabanı yapılandırmasını ve DbSet'lerini içerir
/// </summary>
public class RockyDbContext : DbContext
{
    public RockyDbContext(DbContextOptions<RockyDbContext> options) : base(options)
    {
    }

    // Muhasebe
    public DbSet<GeneralLedgerAccount> GeneralLedgerAccounts => Set<GeneralLedgerAccount>();
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
    public DbSet<JournalLine> JournalLines => Set<JournalLine>();

    // Cari Hesaplar
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceLine> InvoiceLines => Set<InvoiceLine>();
    public DbSet<Payment> Payments => Set<Payment>();

    // Depo & Stok
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();
    public DbSet<WarehouseDefinition> Warehouses => Set<WarehouseDefinition>();

    // Yönetim
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<User> Users => Set<User>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Global soft-delete filtresi
        modelBuilder.Entity<GeneralLedgerAccount>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<JournalEntry>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Customer>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Invoice>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Product>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<StockMovement>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);

        // ========== MUHASEBE YAPILANDIRMASI ==========

        modelBuilder.Entity<GeneralLedgerAccount>(entity =>
        {
            entity.ToTable("GeneralLedgerAccounts");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AccountCode).HasMaxLength(50).IsRequired();
            entity.Property(e => e.AccountName).HasMaxLength(250).IsRequired();
            entity.Property(e => e.DebitBalance).HasColumnType("decimal(18,2)");
            entity.Property(e => e.CreditBalance).HasColumnType("decimal(18,2)");
            entity.HasIndex(e => new { e.CompanyId, e.AccountCode }).IsUnique();

            entity.HasOne(e => e.ParentAccount)
                  .WithMany(e => e.SubAccounts)
                  .HasForeignKey(e => e.ParentAccountId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<JournalEntry>(entity =>
        {
            entity.ToTable("JournalEntries");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EntryNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.TotalDebit).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalCredit).HasColumnType("decimal(18,2)");
            entity.HasIndex(e => new { e.CompanyId, e.EntryNumber }).IsUnique();
        });

        modelBuilder.Entity<JournalLine>(entity =>
        {
            entity.ToTable("JournalLines");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DebitAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.CreditAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ForeignAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ExchangeRate).HasColumnType("decimal(18,6)");

            entity.HasOne(e => e.JournalEntry)
                  .WithMany(e => e.Lines)
                  .HasForeignKey(e => e.JournalEntryId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Account)
                  .WithMany(e => e.JournalLines)
                  .HasForeignKey(e => e.AccountId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ========== CARİ HESAPLAR YAPILANDIRMASI ==========

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CustomerCode).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(250).IsRequired();
            entity.Property(e => e.TaxNumber).HasMaxLength(11);
            entity.Property(e => e.IdentityNumber).HasMaxLength(11);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.DebitBalance).HasColumnType("decimal(18,2)");
            entity.Property(e => e.CreditBalance).HasColumnType("decimal(18,2)");
            entity.Property(e => e.CreditLimit).HasColumnType("decimal(18,2)");
            entity.Property(e => e.DefaultVatRate).HasColumnType("decimal(5,2)");
            entity.HasIndex(e => new { e.CompanyId, e.CustomerCode }).IsUnique();
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.ToTable("Invoices");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.InvoiceNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18,2)");
            entity.Property(e => e.VatAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.PaidAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ExchangeRate).HasColumnType("decimal(18,6)");
            entity.Ignore(e => e.RemainingAmount);
            entity.HasIndex(e => new { e.CompanyId, e.InvoiceNumber }).IsUnique();

            entity.HasOne(e => e.Customer)
                  .WithMany(e => e.Invoices)
                  .HasForeignKey(e => e.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<InvoiceLine>(entity =>
        {
            entity.ToTable("InvoiceLines");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Quantity).HasColumnType("decimal(18,4)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,4)");
            entity.Property(e => e.DiscountRate).HasColumnType("decimal(5,2)");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.VatRate).HasColumnType("decimal(5,2)");
            entity.Property(e => e.VatAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.LineTotal).HasColumnType("decimal(18,2)");
            entity.Property(e => e.LineTotalWithVat).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Unit).HasMaxLength(20);

            entity.HasOne(e => e.Invoice)
                  .WithMany(e => e.Lines)
                  .HasForeignKey(e => e.InvoiceId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payments");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PaymentNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ExchangeRate).HasColumnType("decimal(18,6)");
            entity.HasIndex(e => new { e.CompanyId, e.PaymentNumber }).IsUnique();

            entity.HasOne(e => e.Customer)
                  .WithMany(e => e.Payments)
                  .HasForeignKey(e => e.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Invoice)
                  .WithMany(e => e.Payments)
                  .HasForeignKey(e => e.InvoiceId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ========== DEPO & STOK YAPILANDIRMASI ==========

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ProductCode).HasMaxLength(50).IsRequired();
            entity.Property(e => e.ProductName).HasMaxLength(250).IsRequired();
            entity.Property(e => e.Barcode).HasMaxLength(50);
            entity.Property(e => e.PurchasePrice).HasColumnType("decimal(18,4)");
            entity.Property(e => e.SalesPrice).HasColumnType("decimal(18,4)");
            entity.Property(e => e.VatRate).HasColumnType("decimal(5,2)");
            entity.Property(e => e.StockQuantity).HasColumnType("decimal(18,4)");
            entity.Property(e => e.MinStockLevel).HasColumnType("decimal(18,4)");
            entity.Property(e => e.MaxStockLevel).HasColumnType("decimal(18,4)");
            entity.Property(e => e.Unit).HasMaxLength(20);
            entity.HasIndex(e => new { e.CompanyId, e.ProductCode }).IsUnique();
            entity.HasIndex(e => e.Barcode);

            entity.HasOne(e => e.Category)
                  .WithMany(e => e.Products)
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.ToTable("ProductCategories");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CategoryCode).HasMaxLength(50).IsRequired();
            entity.Property(e => e.CategoryName).HasMaxLength(200).IsRequired();

            entity.HasOne(e => e.ParentCategory)
                  .WithMany(e => e.SubCategories)
                  .HasForeignKey(e => e.ParentCategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<StockMovement>(entity =>
        {
            entity.ToTable("StockMovements");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.MovementNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Quantity).HasColumnType("decimal(18,4)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,4)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");

            entity.HasOne(e => e.Product)
                  .WithMany(e => e.StockMovements)
                  .HasForeignKey(e => e.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<WarehouseDefinition>(entity =>
        {
            entity.ToTable("Warehouses");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.WarehouseCode).HasMaxLength(50).IsRequired();
            entity.Property(e => e.WarehouseName).HasMaxLength(200).IsRequired();
            entity.HasIndex(e => new { e.CompanyId, e.WarehouseCode }).IsUnique();
        });

        // ========== YÖNETİM YAPILANDIRMASI ==========

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Companies");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CompanyName).HasMaxLength(250).IsRequired();
            entity.Property(e => e.TaxNumber).HasMaxLength(11).IsRequired();
            entity.Property(e => e.DefaultVatRate).HasColumnType("decimal(5,2)");
            entity.HasIndex(e => e.TaxNumber).IsUnique();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).HasMaxLength(50).IsRequired();
            entity.Property(e => e.PasswordHash).HasMaxLength(256).IsRequired();
            entity.Property(e => e.PasswordSalt).HasMaxLength(256).IsRequired();
            entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
            entity.Ignore(e => e.FullName);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.ToTable("RolePermissions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ModuleName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.PermissionName).HasMaxLength(100).IsRequired();
            entity.HasIndex(e => new { e.Role, e.ModuleName, e.PermissionName }).IsUnique();
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.ToTable("AuditLogs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EntityName).HasMaxLength(200);
            entity.Property(e => e.Username).HasMaxLength(50);
            entity.HasIndex(e => e.ActionDate);
            entity.HasIndex(e => e.EntityName);
        });
    }

    /// <summary>
    /// SaveChanges sırasında otomatik audit alanlarını doldurur
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
