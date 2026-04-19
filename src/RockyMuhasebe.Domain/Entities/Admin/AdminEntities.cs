using RockyMuhasebe.Domain.Enums;

namespace RockyMuhasebe.Domain.Entities.Admin;

/// <summary>
/// Şirket bilgileri
/// </summary>
public class Company : BaseEntity
{
    /// <summary>Şirket adı</summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>Ticari unvanı</summary>
    public string? TradeName { get; set; }

    /// <summary>Vergi numarası</summary>
    public string TaxNumber { get; set; } = string.Empty;

    /// <summary>Vergi dairesi</summary>
    public string? TaxOffice { get; set; }

    /// <summary>Ticaret Sicil No</summary>
    public string? TradeRegistryNumber { get; set; }

    /// <summary>MERSIS No</summary>
    public string? MersisNumber { get; set; }

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

    /// <summary>Mali yıl başlangıç ayı</summary>
    public int FiscalYearStartMonth { get; set; } = 1;

    /// <summary>Varsayılan para birimi</summary>
    public CurrencyCode DefaultCurrency { get; set; } = CurrencyCode.TRY;

    /// <summary>Varsayılan KDV oranı</summary>
    public decimal DefaultVatRate { get; set; } = 20;

    /// <summary>Stok değerleme yöntemi</summary>
    public StockValuationMethod DefaultValuationMethod { get; set; } = StockValuationMethod.WeightedAverage;

    /// <summary>E-Fatura mükellefi mi?</summary>
    public bool IsEInvoiceEnabled { get; set; } = false;

    /// <summary>E-Defter mükellefi mi?</summary>
    public bool IsELedgerEnabled { get; set; } = false;

    /// <summary>Logo / Amblem (binary)</summary>
    public byte[]? Logo { get; set; }

    /// <summary>Aktif mi?</summary>
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// Kullanıcı
/// </summary>
public class User : BaseEntity
{
    /// <summary>Kullanıcı adı</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>Parola hash'i</summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>Parola salt'ı</summary>
    public string PasswordSalt { get; set; } = string.Empty;

    /// <summary>Ad</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Soyad</summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>Tam ad</summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>E-posta</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Telefon</summary>
    public string? Phone { get; set; }

    /// <summary>Rol</summary>
    public UserRole Role { get; set; } = UserRole.ReadOnly;

    /// <summary>Şirket ID</summary>
    public int CompanyId { get; set; }

    /// <summary>Aktif mi?</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Hesap kilitli mi?</summary>
    public bool IsLocked { get; set; } = false;

    /// <summary>Başarısız giriş denemesi sayısı</summary>
    public int FailedLoginAttempts { get; set; } = 0;

    /// <summary>Kilitlenme zamanı</summary>
    public DateTime? LockoutEndTime { get; set; }

    /// <summary>Son giriş zamanı</summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>Son giriş IP adresi</summary>
    public string? LastLoginIp { get; set; }

    /// <summary>Parola son değiştirilme tarihi</summary>
    public DateTime? PasswordChangedAt { get; set; }

    /// <summary>İlk giriş mi?</summary>
    public bool MustChangePassword { get; set; } = true;

    /// <summary>Fotoğraf</summary>
    public byte[]? ProfilePhoto { get; set; }
}

/// <summary>
/// Rol ve izin tanımı
/// </summary>
public class RolePermission : BaseEntity
{
    /// <summary>Rol adı</summary>
    public UserRole Role { get; set; }

    /// <summary>Modül adı (ör: Muhasebe, Fatura, Stok)</summary>
    public string ModuleName { get; set; } = string.Empty;

    /// <summary>İzin adı (ör: Create, Read, Update, Delete, Print, Export)</summary>
    public string PermissionName { get; set; } = string.Empty;

    /// <summary>İzin var mı?</summary>
    public bool IsGranted { get; set; } = false;
}

/// <summary>
/// Denetim günlüğü
/// </summary>
public class AuditLog : BaseEntity
{
    /// <summary>Kullanıcı ID</summary>
    public int? UserId { get; set; }

    /// <summary>Kullanıcı adı</summary>
    public string? Username { get; set; }

    /// <summary>İşlem türü</summary>
    public AuditActionType ActionType { get; set; }

    /// <summary>Tablo/Modül adı</summary>
    public string EntityName { get; set; } = string.Empty;

    /// <summary>Kayıt ID</summary>
    public string? EntityId { get; set; }

    /// <summary>Eski değerler (JSON)</summary>
    public string? OldValues { get; set; }

    /// <summary>Yeni değerler (JSON)</summary>
    public string? NewValues { get; set; }

    /// <summary>IP adresi</summary>
    public string? IpAddress { get; set; }

    /// <summary>İşlem zamanı</summary>
    public DateTime ActionDate { get; set; } = DateTime.UtcNow;

    /// <summary>Açıklama</summary>
    public string? Description { get; set; }

    /// <summary>Şirket ID</summary>
    public int CompanyId { get; set; }
}
