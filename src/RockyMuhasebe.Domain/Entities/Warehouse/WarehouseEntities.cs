using RockyMuhasebe.Domain.Enums;

namespace RockyMuhasebe.Domain.Entities.Warehouse;

/// <summary>
/// Ürün / Stok kartı
/// </summary>
public class Product : BaseEntity
{
    /// <summary>Ürün kodu</summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>Ürün adı</summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>Barkod</summary>
    public string? Barcode { get; set; }

    /// <summary>Kategori ID</summary>
    public int? CategoryId { get; set; }

    /// <summary>Kategori navigasyon</summary>
    public ProductCategory? Category { get; set; }

    /// <summary>Birim (Adet, Kg, Lt, Mt vb.)</summary>
    public string Unit { get; set; } = "Adet";

    /// <summary>Alış fiyatı</summary>
    public decimal PurchasePrice { get; set; }

    /// <summary>Satış fiyatı</summary>
    public decimal SalesPrice { get; set; }

    /// <summary>KDV oranı (%)</summary>
    public decimal VatRate { get; set; } = 20;

    /// <summary>Mevcut stok miktarı</summary>
    public decimal StockQuantity { get; set; } = 0;

    /// <summary>Minimum stok seviyesi</summary>
    public decimal MinStockLevel { get; set; } = 0;

    /// <summary>Maksimum stok seviyesi</summary>
    public decimal MaxStockLevel { get; set; } = 0;

    /// <summary>Stok değerleme yöntemi</summary>
    public StockValuationMethod ValuationMethod { get; set; } = StockValuationMethod.WeightedAverage;

    /// <summary>Para birimi</summary>
    public CurrencyCode Currency { get; set; } = CurrencyCode.TRY;

    /// <summary>Ağırlık (kg)</summary>
    public decimal? Weight { get; set; }

    /// <summary>Hacim</summary>
    public string? Volume { get; set; }

    /// <summary>Açıklama</summary>
    public string? Description { get; set; }

    /// <summary>Aktif mi?</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Hizmet mi? (Stok takibi yapılmaz)</summary>
    public bool IsService { get; set; } = false;

    /// <summary>Şirket ID</summary>
    public int CompanyId { get; set; }

    /// <summary>Depo ID (Varsayılan)</summary>
    public int? DefaultWarehouseId { get; set; }

    /// <summary>Stok hareketleri</summary>
    public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}

/// <summary>
/// Ürün kategorisi
/// </summary>
public class ProductCategory : BaseEntity
{
    /// <summary>Kategori kodu</summary>
    public string CategoryCode { get; set; } = string.Empty;

    /// <summary>Kategori adı</summary>
    public string CategoryName { get; set; } = string.Empty;

    /// <summary>Üst kategori ID</summary>
    public int? ParentCategoryId { get; set; }

    /// <summary>Üst kategori navigasyon</summary>
    public ProductCategory? ParentCategory { get; set; }

    /// <summary>Alt kategoriler</summary>
    public ICollection<ProductCategory> SubCategories { get; set; } = new List<ProductCategory>();

    /// <summary>Bu kategorideki ürünler</summary>
    public ICollection<Product> Products { get; set; } = new List<Product>();

    /// <summary>Açıklama</summary>
    public string? Description { get; set; }

    /// <summary>Şirket ID</summary>
    public int CompanyId { get; set; }
}

/// <summary>
/// Stok hareketi
/// </summary>
public class StockMovement : BaseEntity
{
    /// <summary>Hareket numarası</summary>
    public string MovementNumber { get; set; } = string.Empty;

    /// <summary>Ürün ID</summary>
    public int ProductId { get; set; }

    /// <summary>Ürün navigasyon</summary>
    public Product Product { get; set; } = null!;

    /// <summary>Hareket türü</summary>
    public StockMovementType MovementType { get; set; }

    /// <summary>Miktar</summary>
    public decimal Quantity { get; set; }

    /// <summary>Birim fiyat</summary>
    public decimal UnitPrice { get; set; }

    /// <summary>Toplam tutar</summary>
    public decimal TotalAmount { get; set; }

    /// <summary>İşlem tarihi</summary>
    public DateTime MovementDate { get; set; } = DateTime.Today;

    /// <summary>Açıklama</summary>
    public string? Description { get; set; }

    /// <summary>Depo ID</summary>
    public int? WarehouseId { get; set; }

    /// <summary>Fatura ID (varsa)</summary>
    public int? InvoiceId { get; set; }

    /// <summary>Referans belge numarası</summary>
    public string? ReferenceDocument { get; set; }

    /// <summary>Şirket ID</summary>
    public int CompanyId { get; set; }
}

/// <summary>
/// Depo / Ambar tanımı
/// </summary>
public class WarehouseDefinition : BaseEntity
{
    /// <summary>Depo kodu</summary>
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>Depo adı</summary>
    public string WarehouseName { get; set; } = string.Empty;

    /// <summary>Adres</summary>
    public string? Address { get; set; }

    /// <summary>Sorumlu kişi</summary>
    public string? ResponsiblePerson { get; set; }

    /// <summary>Telefon</summary>
    public string? Phone { get; set; }

    /// <summary>Aktif mi?</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Şirket ID</summary>
    public int CompanyId { get; set; }
}
