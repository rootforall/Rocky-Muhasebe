using RockyMuhasebe.Domain.Enums;

namespace RockyMuhasebe.Domain.Entities.Accounting;

/// <summary>
/// Muhasebe Hesap Planı - Genel Muhasebe Hesabı
/// Tekdüzen Hesap Planı'na uygun şekilde oluşturulur
/// </summary>
public class GeneralLedgerAccount : BaseEntity
{
    /// <summary>Hesap kodu (ör: 100, 100.01, 120.01.001)</summary>
    public string AccountCode { get; set; } = string.Empty;

    /// <summary>Hesap adı</summary>
    public string AccountName { get; set; } = string.Empty;

    /// <summary>Hesap türü (Aktif, Pasif, Gelir, Gider, Öz Kaynak)</summary>
    public AccountType AccountType { get; set; }

    /// <summary>Üst hesap ID - Hiyerarşik yapı için</summary>
    public int? ParentAccountId { get; set; }

    /// <summary>Üst hesap navigasyon özelliği</summary>
    public GeneralLedgerAccount? ParentAccount { get; set; }

    /// <summary>Alt hesaplar</summary>
    public ICollection<GeneralLedgerAccount> SubAccounts { get; set; } = new List<GeneralLedgerAccount>();

    /// <summary>Hesap seviyesi (1, 2, 3...)</summary>
    public int Level { get; set; } = 1;

    /// <summary>Bu hesaba kayıt yapılabilir mi? (Yaprak hesap)</summary>
    public bool IsPostable { get; set; } = true;

    /// <summary>Aktif mi?</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Para birimi</summary>
    public CurrencyCode Currency { get; set; } = CurrencyCode.TRY;

    /// <summary>Açıklama</summary>
    public string? Description { get; set; }

    /// <summary>Borç bakiye</summary>
    public decimal DebitBalance { get; set; } = 0;

    /// <summary>Alacak bakiye</summary>
    public decimal CreditBalance { get; set; } = 0;

    /// <summary>Şirket ID</summary>
    public int CompanyId { get; set; }

    /// <summary>İlişkili yevmiye satırları</summary>
    public ICollection<JournalLine> JournalLines { get; set; } = new List<JournalLine>();
}

/// <summary>
/// Yevmiye Kaydı (Journal Entry) - Muhasebe işleminin başlık bilgileri
/// </summary>
public class JournalEntry : BaseEntity
{
    /// <summary>Yevmiye numarası</summary>
    public string EntryNumber { get; set; } = string.Empty;

    /// <summary>İşlem tarihi</summary>
    public DateTime EntryDate { get; set; } = DateTime.Today;

    /// <summary>Açıklama</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Referans numarası (Fatura no, makbuz no vb.)</summary>
    public string? ReferenceNumber { get; set; }

    /// <summary>Dönem (ör: 2024-01)</summary>
    public string? Period { get; set; }

    /// <summary>Toplam borç tutarı</summary>
    public decimal TotalDebit { get; set; }

    /// <summary>Toplam alacak tutarı</summary>
    public decimal TotalCredit { get; set; }

    /// <summary>Onaylandı mı?</summary>
    public bool IsApproved { get; set; } = false;

    /// <summary>Onaylayan kullanıcı</summary>
    public string? ApprovedBy { get; set; }

    /// <summary>Onay tarihi</summary>
    public DateTime? ApprovedAt { get; set; }

    /// <summary>Şirket ID</summary>
    public int CompanyId { get; set; }

    /// <summary>Yevmiye satırları</summary>
    public ICollection<JournalLine> Lines { get; set; } = new List<JournalLine>();
}

/// <summary>
/// Yevmiye Satırı (Journal Line) - Her bir borç veya alacak kaydı
/// </summary>
public class JournalLine : BaseEntity
{
    /// <summary>Yevmiye kaydı ID</summary>
    public int JournalEntryId { get; set; }

    /// <summary>Yevmiye kaydı navigasyon</summary>
    public JournalEntry JournalEntry { get; set; } = null!;

    /// <summary>Hesap ID</summary>
    public int AccountId { get; set; }

    /// <summary>Hesap navigasyon</summary>
    public GeneralLedgerAccount Account { get; set; } = null!;

    /// <summary>Borç tutarı</summary>
    public decimal DebitAmount { get; set; }

    /// <summary>Alacak tutarı</summary>
    public decimal CreditAmount { get; set; }

    /// <summary>Satır açıklaması</summary>
    public string? Description { get; set; }

    /// <summary>Döviz tutarı (varsa)</summary>
    public decimal? ForeignAmount { get; set; }

    /// <summary>Döviz kuru</summary>
    public decimal? ExchangeRate { get; set; }

    /// <summary>Para birimi</summary>
    public CurrencyCode Currency { get; set; } = CurrencyCode.TRY;

    /// <summary>Satır sırası</summary>
    public int LineOrder { get; set; }
}
