using RockyMuhasebe.Data.Context;
using RockyMuhasebe.Domain.Entities.Accounting;
using RockyMuhasebe.Domain.Entities.Admin;
using RockyMuhasebe.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace RockyMuhasebe.Data;

/// <summary>
/// Veritabanı ilk kurulum verileri - Tekdüzen Hesap Planı ve varsayılan veriler
/// </summary>
public static class DbInitializer
{
    public static async Task SeedAsync(RockyDbContext context)
    {
        // Veritabanını oluştur (Development modunda)
        await context.Database.EnsureCreatedAsync();

        // Şirket yoksa varsayılan şirket ekle
        if (!await context.Companies.AnyAsync())
        {
            var company = new Company
            {
                CompanyName = "Demo Şirket A.Ş.",
                TaxNumber = "1234567890",
                TaxOffice = "Başkent Vergi Dairesi",
                Phone = "0312 999 99 99",
                Email = "info@demosirket.com.tr",
                Address = "Atatürk Bulvarı No:100",
                City = "Ankara",
                Country = "Türkiye",
                DefaultCurrency = CurrencyCode.TRY,
                DefaultVatRate = 20,
                CreatedBy = "System"
            };
            context.Companies.Add(company);
            await context.SaveChangesAsync();
        }

        // Varsayılan admin kullanıcısı
        if (!await context.Users.AnyAsync())
        {
            var admin = new User
            {
                Username = "admin",
                PasswordHash = "AQAAAAIAAYagAAAAEKb+ePvpVJVxJHbEfv6k4RCq+CaJvOqWKJlS/wU2Z3aBs+ZP7k8Nb9FQnMCQz7ypA==", // "Admin123!"
                PasswordSalt = "rocky_salt_2024",
                FirstName = "Sistem",
                LastName = "Yöneticisi",
                Email = "admin@rockymuhasebe.com",
                Role = UserRole.SystemAdmin,
                CompanyId = 1,
                IsActive = true,
                MustChangePassword = false,
                CreatedBy = "System"
            };
            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }

        // Tekdüzen Hesap Planı yoksa oluştur
        if (!await context.GeneralLedgerAccounts.AnyAsync())
        {
            var accounts = GetDefaultChartOfAccounts();
            await context.GeneralLedgerAccounts.AddRangeAsync(accounts);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Türkiye Tekdüzen Hesap Planı - Ana hesap grupları
    /// </summary>
    private static List<GeneralLedgerAccount> GetDefaultChartOfAccounts()
    {
        var accounts = new List<GeneralLedgerAccount>();
        int companyId = 1;

        // ===== 1 - DÖNEN VARLIKLAR =====
        AddAccount(accounts, "100", "Kasa", AccountType.Asset, 1, companyId, false);
        AddAccount(accounts, "100.01", "TL Kasası", AccountType.Asset, 2, companyId, true);
        AddAccount(accounts, "100.02", "Döviz Kasası", AccountType.Asset, 2, companyId, true);
        AddAccount(accounts, "101", "Alınan Çekler", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "102", "Bankalar", AccountType.Asset, 1, companyId, false);
        AddAccount(accounts, "102.01", "Vadesiz Mevduat", AccountType.Asset, 2, companyId, true);
        AddAccount(accounts, "102.02", "Vadeli Mevduat", AccountType.Asset, 2, companyId, true);
        AddAccount(accounts, "103", "Verilen Çekler ve Ödeme Emirleri (-)", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "108", "Diğer Hazır Değerler", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "120", "Alıcılar", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "121", "Alacak Senetleri", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "126", "Verilen Depozito ve Teminatlar", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "127", "Diğer Ticari Alacaklar", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "131", "Ortaklardan Alacaklar", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "135", "Personelden Alacaklar", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "136", "Diğer Çeşitli Alacaklar", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "150", "İlk Madde ve Malzeme", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "152", "Mamuller", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "153", "Ticari Mallar", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "157", "Diğer Stoklar", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "159", "Verilen Sipariş Avansları", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "180", "Gelecek Aylara Ait Giderler", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "190", "Devreden KDV", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "191", "İndirilecek KDV", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "192", "Diğer KDV", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "193", "Peşin Ödenen Vergiler ve Fonlar", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "196", "Personel Avansları", AccountType.Asset, 1, companyId, true);

        // ===== 2 - DURAN VARLIKLAR =====
        AddAccount(accounts, "250", "Arazi ve Arsalar", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "252", "Binalar", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "253", "Tesis, Makine ve Cihazlar", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "254", "Taşıtlar", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "255", "Demirbaşlar", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "257", "Birikmiş Amortismanlar (-)", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "260", "Haklar", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "264", "Özel Maliyetler", AccountType.Asset, 1, companyId, true);
        AddAccount(accounts, "268", "Birikmiş Amortismanlar (-)", AccountType.Asset, 1, companyId, true);

        // ===== 3 - KISA VADELİ YABANCI KAYNAKLAR =====
        AddAccount(accounts, "300", "Banka Kredileri", AccountType.Liability, 1, companyId, true);
        AddAccount(accounts, "320", "Satıcılar", AccountType.Liability, 1, companyId, true);
        AddAccount(accounts, "321", "Borç Senetleri", AccountType.Liability, 1, companyId, true);
        AddAccount(accounts, "326", "Alınan Depozito ve Teminatlar", AccountType.Liability, 1, companyId, true);
        AddAccount(accounts, "335", "Personele Borçlar", AccountType.Liability, 1, companyId, true);
        AddAccount(accounts, "336", "Diğer Çeşitli Borçlar", AccountType.Liability, 1, companyId, true);
        AddAccount(accounts, "340", "Alınan Sipariş Avansları", AccountType.Liability, 1, companyId, true);
        AddAccount(accounts, "360", "Ödenecek Vergiler ve Fonlar", AccountType.Liability, 1, companyId, true);
        AddAccount(accounts, "361", "Ödenecek Sosyal Güvenlik Kesintileri", AccountType.Liability, 1, companyId, true);
        AddAccount(accounts, "368", "Vadesi Geçmiş Ertelenmiş Taksitlendirilmiş Borçlar", AccountType.Liability, 1, companyId, true);
        AddAccount(accounts, "370", "Dönem Kârı Vergi ve Yasal Yükümlülükleri", AccountType.Liability, 1, companyId, true);
        AddAccount(accounts, "371", "Dönem Kârının Peşin Ödenen Vergisi (-)", AccountType.Liability, 1, companyId, true);
        AddAccount(accounts, "380", "Gelecek Aylara Ait Gelirler", AccountType.Liability, 1, companyId, true);
        AddAccount(accounts, "391", "Hesaplanan KDV", AccountType.Liability, 1, companyId, true);

        // ===== 4 - UZUN VADELİ YABANCI KAYNAKLAR =====
        AddAccount(accounts, "400", "Banka Kredileri", AccountType.Liability, 1, companyId, true);
        AddAccount(accounts, "420", "Satıcılar", AccountType.Liability, 1, companyId, true);
        AddAccount(accounts, "421", "Borç Senetleri", AccountType.Liability, 1, companyId, true);

        // ===== 5 - ÖZ KAYNAKLAR =====
        AddAccount(accounts, "500", "Sermaye", AccountType.Equity, 1, companyId, true);
        AddAccount(accounts, "501", "Ödenmemiş Sermaye (-)", AccountType.Equity, 1, companyId, true);
        AddAccount(accounts, "520", "Hisse Senedi İhraç Primleri", AccountType.Equity, 1, companyId, true);
        AddAccount(accounts, "540", "Yasal Yedekler", AccountType.Equity, 1, companyId, true);
        AddAccount(accounts, "541", "Statü Yedekleri", AccountType.Equity, 1, companyId, true);
        AddAccount(accounts, "542", "Olağanüstü Yedekler", AccountType.Equity, 1, companyId, true);
        AddAccount(accounts, "549", "Özel Fonlar", AccountType.Equity, 1, companyId, true);
        AddAccount(accounts, "570", "Geçmiş Yıllar Kârları", AccountType.Equity, 1, companyId, true);
        AddAccount(accounts, "580", "Geçmiş Yıllar Zararları (-)", AccountType.Equity, 1, companyId, true);
        AddAccount(accounts, "590", "Dönem Net Kârı", AccountType.Equity, 1, companyId, true);
        AddAccount(accounts, "591", "Dönem Net Zararı (-)", AccountType.Equity, 1, companyId, true);

        // ===== 6 - GELİR TABLOSU HESAPLARI =====
        AddAccount(accounts, "600", "Yurtiçi Satışlar", AccountType.Revenue, 1, companyId, true);
        AddAccount(accounts, "601", "Yurtdışı Satışlar", AccountType.Revenue, 1, companyId, true);
        AddAccount(accounts, "602", "Diğer Gelirler", AccountType.Revenue, 1, companyId, true);
        AddAccount(accounts, "610", "Satıştan İadeler (-)", AccountType.Revenue, 1, companyId, true);
        AddAccount(accounts, "611", "Satış İskontoları (-)", AccountType.Revenue, 1, companyId, true);
        AddAccount(accounts, "620", "Satılan Mamuller Maliyeti (-)", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "621", "Satılan Ticari Mallar Maliyeti (-)", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "622", "Satılan Hizmet Maliyeti (-)", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "630", "Araştırma ve Geliştirme Giderleri (-)", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "631", "Pazarlama Satış Dağıtım Giderleri (-)", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "632", "Genel Yönetim Giderleri (-)", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "640", "İştiraklerden Temettü Gelirleri", AccountType.Revenue, 1, companyId, true);
        AddAccount(accounts, "642", "Faiz Gelirleri", AccountType.Revenue, 1, companyId, true);
        AddAccount(accounts, "644", "Konusu Kalmayan Karşılıklar", AccountType.Revenue, 1, companyId, true);
        AddAccount(accounts, "646", "Kambiyo Kârları", AccountType.Revenue, 1, companyId, true);
        AddAccount(accounts, "649", "Diğer Olağan Gelir ve Kârlar", AccountType.Revenue, 1, companyId, true);
        AddAccount(accounts, "653", "Komisyon Giderleri (-)", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "654", "Karşılık Giderleri (-)", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "656", "Kambiyo Zararları (-)", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "659", "Diğer Olağan Gider ve Zararlar (-)", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "660", "Kısa Vadeli Borçlanma Giderleri (-)", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "661", "Uzun Vadeli Borçlanma Giderleri (-)", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "671", "Önceki Dönem Gelir ve Kârları", AccountType.Revenue, 1, companyId, true);
        AddAccount(accounts, "680", "Çalışmayan Kısım Gider ve Zararları (-)", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "681", "Önceki Dönem Gider ve Zararları (-)", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "689", "Diğer Olağandışı Gider ve Zararlar (-)", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "690", "Dönem Kârı veya Zararı", AccountType.Equity, 1, companyId, true);
        AddAccount(accounts, "691", "Dönem Kârı Vergi ve Yasal Yükümlülük Karşılığı (-)", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "692", "Dönem Net Kârı veya Zararı", AccountType.Equity, 1, companyId, true);

        // ===== 7 - MALİYET HESAPLARI =====
        AddAccount(accounts, "700", "Maliyet Muhasebesi Bağlantı Hesabı", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "710", "Direkt İlk Madde ve Malzeme Giderleri", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "720", "Direkt İşçilik Giderleri", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "730", "Genel Üretim Giderleri", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "740", "Hizmet Üretim Maliyeti", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "750", "Araştırma ve Geliştirme Giderleri", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "760", "Pazarlama Satış ve Dağıtım Giderleri", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "770", "Genel Yönetim Giderleri", AccountType.Expense, 1, companyId, true);
        AddAccount(accounts, "780", "Finansman Giderleri", AccountType.Expense, 1, companyId, true);

        return accounts;
    }

    private static void AddAccount(List<GeneralLedgerAccount> accounts, string code, string name,
        AccountType type, int level, int companyId, bool isPostable)
    {
        accounts.Add(new GeneralLedgerAccount
        {
            AccountCode = code,
            AccountName = name,
            AccountType = type,
            Level = level,
            CompanyId = companyId,
            IsPostable = isPostable,
            IsActive = true,
            CreatedBy = "System"
        });
    }
}
