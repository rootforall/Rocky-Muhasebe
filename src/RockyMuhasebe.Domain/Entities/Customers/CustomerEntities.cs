using RockyMuhasebe.Domain.Enums;

namespace RockyMuhasebe.Domain.Entities.Customers;

/// <summary>
/// Cari Hesap - Müşteri ve Tedarikçi bilgileri
/// </summary>
public class Customer : BaseEntity
{
    /// <summary>Cari kodu</summary>
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>Cari unvanı / İsim</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Cari türü (Müşteri / Tedarikçi / Her İkisi)</summary>
    public CustomerType CustomerType { get; set; } = CustomerType.Customer;

    /// <summary>Vergi numarası</summary>
    public string? TaxNumber { get; set; }

    /// <summary>TC Kimlik Numarası</summary>
    public string? IdentityNumber { get; set; }

    /// <summary>Vergi dairesi</summary>
    public string? TaxOffice { get; set; }

    /// <summary>Telefon</summary>
    public string? Phone { get; set; }

    /// <summary>Fax</summary>
    public string? Fax { get; set; }

    /// <summary>E-posta</summary>
    public string? Email { get; set; }

    /// <summary>Web sitesi</summary>
    public string? Website { get; set; }

    /// <summary>Adres</summary>
    public string? Address { get; set; }

    /// <summary>İl</summary>
    public string? City { get; set; }

    /// <summary>İlçe</summary>
    public string? District { get; set; }

    /// <summary>Posta kodu</summary>
    public string? PostalCode { get; set; }

    /// <summary>Ülke</summary>
    public string Country { get; set; } = "Türkiye";

    /// <summary>Yetkili kişi adı</summary>
    public string? ContactPerson { get; set; }

    /// <summary>Yetkili kişi telefonu</summary>
    public string? ContactPhone { get; set; }

    /// <summary>Borç bakiyesi</summary>
    public decimal DebitBalance { get; set; } = 0;

    /// <summary>Alacak bakiyesi</summary>
    public decimal CreditBalance { get; set; } = 0;

    /// <summary>Kredi limiti</summary>
    public decimal CreditLimit { get; set; } = 0;

    /// <summary>Risk puanı (1-100)</summary>
    public int RiskScore { get; set; } = 50;

    /// <summary>Varsayılan KDV oranı</summary>
    public decimal DefaultVatRate { get; set; } = 20;

    /// <summary>Varsayılan para birimi</summary>
    public CurrencyCode DefaultCurrency { get; set; } = CurrencyCode.TRY;

    /// <summary>Aktif mi?</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Notlar</summary>
    public string? Notes { get; set; }

    /// <summary>Şirket ID</summary>
    public int CompanyId { get; set; }

    /// <summary>E-Fatura mükellefi mi?</summary>
    public bool IsEInvoiceUser { get; set; } = false;

    /// <summary>İlişkili muhasebe hesabı ID</summary>
    public int? LedgerAccountId { get; set; }

    /// <summary>Faturalar</summary>
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    /// <summary>Ödemeler</summary>
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

/// <summary>
/// Fatura
/// </summary>
public class Invoice : BaseEntity
{
    /// <summary>Fatura numarası</summary>
    public string InvoiceNumber { get; set; } = string.Empty;

    /// <summary>Fatura seri</summary>
    public string? InvoiceSeries { get; set; }

    /// <summary>Fatura tarihi</summary>
    public DateTime InvoiceDate { get; set; } = DateTime.Today;

    /// <summary>Vade tarihi</summary>
    public DateTime? DueDate { get; set; }

    /// <summary>Fatura türü</summary>
    public InvoiceType InvoiceType { get; set; }

    /// <summary>Fatura durumu</summary>
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;

    /// <summary>Cari hesap ID</summary>
    public int CustomerId { get; set; }

    /// <summary>Cari hesap navigasyon</summary>
    public Customer Customer { get; set; } = null!;

    /// <summary>Ara toplam (KDV hariç)</summary>
    public decimal SubTotal { get; set; }

    /// <summary>KDV tutarı</summary>
    public decimal VatAmount { get; set; }

    /// <summary>İndirim tutarı</summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>Genel toplam</summary>
    public decimal TotalAmount { get; set; }

    /// <summary>Ödenen tutar</summary>
    public decimal PaidAmount { get; set; }

    /// <summary>Kalan bakiye</summary>
    public decimal RemainingAmount => TotalAmount - PaidAmount;

    /// <summary>Para birimi</summary>
    public CurrencyCode Currency { get; set; } = CurrencyCode.TRY;

    /// <summary>Döviz kuru</summary>
    public decimal ExchangeRate { get; set; } = 1;

    /// <summary>Açıklama</summary>
    public string? Description { get; set; }

    /// <summary>Sevkiyat adresi</summary>
    public string? ShippingAddress { get; set; }

    /// <summary>E-Fatura ETTN numarası</summary>
    public string? EInvoiceId { get; set; }

    /// <summary>E-Fatura mı?</summary>
    public bool IsEInvoice { get; set; } = false;

    /// <summary>Yevmiye kaydı oluşturuldu mu?</summary>
    public bool IsJournalCreated { get; set; } = false;

    /// <summary>İlişkili yevmiye ID</summary>
    public int? JournalEntryId { get; set; }

    /// <summary>Şirket ID</summary>
    public int CompanyId { get; set; }

    /// <summary>Fatura satırları</summary>
    public ICollection<InvoiceLine> Lines { get; set; } = new List<InvoiceLine>();

    /// <summary>Ödemeler</summary>
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

/// <summary>
/// Fatura satırı
/// </summary>
public class InvoiceLine : BaseEntity
{
    /// <summary>Fatura ID</summary>
    public int InvoiceId { get; set; }

    /// <summary>Fatura navigasyon</summary>
    public Invoice Invoice { get; set; } = null!;

    /// <summary>Ürün ID (varsa)</summary>
    public int? ProductId { get; set; }

    /// <summary>Satır açıklaması</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Miktar</summary>
    public decimal Quantity { get; set; }

    /// <summary>Birim fiyat</summary>
    public decimal UnitPrice { get; set; }

    /// <summary>İndirim oranı (%)</summary>
    public decimal DiscountRate { get; set; } = 0;

    /// <summary>İndirim tutarı</summary>
    public decimal DiscountAmount { get; set; } = 0;

    /// <summary>KDV oranı (%)</summary>
    public decimal VatRate { get; set; } = 20;

    /// <summary>KDV tutarı</summary>
    public decimal VatAmount { get; set; }

    /// <summary>Satır toplam (KDV hariç)</summary>
    public decimal LineTotal { get; set; }

    /// <summary>Satır toplam (KDV dahil)</summary>
    public decimal LineTotalWithVat { get; set; }

    /// <summary>Birim (Adet, Kg, Lt vb.)</summary>
    public string Unit { get; set; } = "Adet";

    /// <summary>Satır sırası</summary>
    public int LineOrder { get; set; }
}

/// <summary>
/// Ödeme kaydı
/// </summary>
public class Payment : BaseEntity
{
    /// <summary>Ödeme numarası</summary>
    public string PaymentNumber { get; set; } = string.Empty;

    /// <summary>Cari hesap ID</summary>
    public int CustomerId { get; set; }

    /// <summary>Cari hesap navigasyon</summary>
    public Customer Customer { get; set; } = null!;

    /// <summary>Fatura ID (varsa)</summary>
    public int? InvoiceId { get; set; }

    /// <summary>Fatura navigasyon</summary>
    public Invoice? Invoice { get; set; }

    /// <summary>Ödeme tarihi</summary>
    public DateTime PaymentDate { get; set; } = DateTime.Today;

    /// <summary>Ödeme tutarı</summary>
    public decimal Amount { get; set; }

    /// <summary>Ödeme yöntemi</summary>
    public PaymentMethod PaymentMethod { get; set; }

    /// <summary>Para birimi</summary>
    public CurrencyCode Currency { get; set; } = CurrencyCode.TRY;

    /// <summary>Döviz kuru</summary>
    public decimal ExchangeRate { get; set; } = 1;

    /// <summary>Açıklama</summary>
    public string? Description { get; set; }

    /// <summary>Banka hesap ID</summary>
    public int? BankAccountId { get; set; }

    /// <summary>Referans numarası</summary>
    public string? ReferenceNumber { get; set; }

    /// <summary>Tahsilat mı? (true) / Ödeme mi? (false)</summary>
    public bool IsIncoming { get; set; }

    /// <summary>Şirket ID</summary>
    public int CompanyId { get; set; }
}
