namespace RockyMuhasebe.Domain.Enums;

/// <summary>
/// Fatura durumu
/// </summary>
public enum InvoiceStatus
{
    /// <summary>Taslak</summary>
    Draft = 0,
    /// <summary>Onaylandı</summary>
    Approved = 1,
    /// <summary>Gönderildi</summary>
    Sent = 2,
    /// <summary>Kısmen Ödendi</summary>
    PartiallyPaid = 3,
    /// <summary>Ödendi</summary>
    Paid = 4,
    /// <summary>İptal Edildi</summary>
    Cancelled = 5,
    /// <summary>İade Edildi</summary>
    Returned = 6,
    /// <summary>Vadesi Geçmiş</summary>
    Overdue = 7
}

/// <summary>
/// Fatura türü
/// </summary>
public enum InvoiceType
{
    /// <summary>Satış Faturası</summary>
    Sales = 0,
    /// <summary>Alış Faturası</summary>
    Purchase = 1,
    /// <summary>Proforma Fatura</summary>
    Proforma = 2,
    /// <summary>İhracat Faturası</summary>
    Export = 3,
    /// <summary>İade Faturası</summary>
    Return = 4,
    /// <summary>Peşin Satış Faturası</summary>
    CashSales = 5
}

/// <summary>
/// Stok hareket türü
/// </summary>
public enum StockMovementType
{
    /// <summary>Giriş - Satın Alma</summary>
    PurchaseIn = 0,
    /// <summary>Giriş - Üretim</summary>
    ProductionIn = 1,
    /// <summary>Giriş - İade</summary>
    ReturnIn = 2,
    /// <summary>Giriş - Devir</summary>
    TransferIn = 3,
    /// <summary>Çıkış - Satış</summary>
    SalesOut = 10,
    /// <summary>Çıkış - İç Kullanım</summary>
    InternalUseOut = 11,
    /// <summary>Çıkış - İade</summary>
    ReturnOut = 12,
    /// <summary>Çıkış - Devir</summary>
    TransferOut = 13,
    /// <summary>Sayım Düzeltme - Artış</summary>
    AdjustmentIncrease = 20,
    /// <summary>Sayım Düzeltme - Azalış</summary>
    AdjustmentDecrease = 21,
    /// <summary>Fire/Zayi</summary>
    Wastage = 22
}

/// <summary>
/// Kullanıcı rolleri
/// </summary>
public enum UserRole
{
    /// <summary>Sistem Yöneticisi</summary>
    SystemAdmin = 0,
    /// <summary>Muhasebe Müdürü</summary>
    AccountingManager = 1,
    /// <summary>Muhasebeci</summary>
    Accountant = 2,
    /// <summary>Satış Sorumlusu</summary>
    SalesRepresentative = 3,
    /// <summary>Depo Sorumlusu</summary>
    WarehouseManager = 4,
    /// <summary>İnsan Kaynakları</summary>
    HumanResources = 5,
    /// <summary>Yönetici</summary>
    Manager = 6,
    /// <summary>Salt Okunur Kullanıcı</summary>
    ReadOnly = 7
}

/// <summary>
/// Vergi türleri
/// </summary>
public enum TaxType
{
    /// <summary>KDV</summary>
    VAT = 0,
    /// <summary>ÖTV</summary>
    SpecialConsumptionTax = 1,
    /// <summary>Stopaj</summary>
    WithholdingTax = 2,
    /// <summary>Damga Vergisi</summary>
    StampTax = 3,
    /// <summary>Kurumlar Vergisi</summary>
    CorporateTax = 4,
    /// <summary>Gelir Vergisi</summary>
    IncomeTax = 5
}

/// <summary>
/// Hesap türleri (Muhasebe)
/// </summary>
public enum AccountType
{
    /// <summary>Aktif - Varlık</summary>
    Asset = 0,
    /// <summary>Pasif - Yükümlülük</summary>
    Liability = 1,
    /// <summary>Öz Kaynak</summary>
    Equity = 2,
    /// <summary>Gelir</summary>
    Revenue = 3,
    /// <summary>Gider</summary>
    Expense = 4
}

/// <summary>
/// Ödeme yöntemi
/// </summary>
public enum PaymentMethod
{
    /// <summary>Nakit</summary>
    Cash = 0,
    /// <summary>Banka Havalesi</summary>
    BankTransfer = 1,
    /// <summary>Kredi Kartı</summary>
    CreditCard = 2,
    /// <summary>Çek</summary>
    Check = 3,
    /// <summary>Senet</summary>
    PromissoryNote = 4,
    /// <summary>EFT</summary>
    EFT = 5,
    /// <summary>Diğer</summary>
    Other = 99
}

/// <summary>
/// Para birimi
/// </summary>
public enum CurrencyCode
{
    TRY = 0,
    USD = 1,
    EUR = 2,
    GBP = 3
}

/// <summary>
/// Cari hesap türü
/// </summary>
public enum CustomerType
{
    /// <summary>Müşteri</summary>
    Customer = 0,
    /// <summary>Tedarikçi</summary>
    Vendor = 1,
    /// <summary>Hem Müşteri Hem Tedarikçi</summary>
    Both = 2
}

/// <summary>
/// Stok değerleme yöntemi
/// </summary>
public enum StockValuationMethod
{
    /// <summary>İlk Giren İlk Çıkar</summary>
    FIFO = 0,
    /// <summary>Son Giren İlk Çıkar</summary>
    LIFO = 1,
    /// <summary>Ağırlıklı Ortalama Maliyet</summary>
    WeightedAverage = 2,
    /// <summary>Standart Maliyet</summary>
    StandardCost = 3
}

/// <summary>
/// Denetim işlem türleri
/// </summary>
public enum AuditActionType
{
    Create = 0,
    Update = 1,
    Delete = 2,
    Login = 3,
    Logout = 4,
    Export = 5,
    Print = 6,
    Approve = 7,
    Reject = 8
}
