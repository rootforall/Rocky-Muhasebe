using RockyMuhasebe.Domain.Entities.Accounting;
using RockyMuhasebe.Domain.Enums;

namespace RockyMuhasebe.Application.Services.Interfaces;

/// <summary>
/// Muhasebe servisi arayüzü
/// </summary>
public interface IAccountingService
{
    // Hesap Planı İşlemleri
    Task<IEnumerable<GeneralLedgerAccount>> GetAllAccountsAsync(int companyId);
    Task<GeneralLedgerAccount?> GetAccountByIdAsync(int id);
    Task<GeneralLedgerAccount?> GetAccountByCodeAsync(string code, int companyId);
    Task<IEnumerable<GeneralLedgerAccount>> GetAccountsByTypeAsync(AccountType type, int companyId);
    Task<IEnumerable<GeneralLedgerAccount>> SearchAccountsAsync(string searchTerm, int companyId);
    Task<GeneralLedgerAccount> CreateAccountAsync(GeneralLedgerAccount account);
    Task UpdateAccountAsync(GeneralLedgerAccount account);
    Task DeleteAccountAsync(int id);

    // Yevmiye İşlemleri
    Task<IEnumerable<JournalEntry>> GetJournalEntriesAsync(int companyId, DateTime? fromDate = null, DateTime? toDate = null);
    Task<JournalEntry?> GetJournalEntryByIdAsync(int id);
    Task<JournalEntry> CreateJournalEntryAsync(JournalEntry entry);
    Task UpdateJournalEntryAsync(JournalEntry entry);
    Task DeleteJournalEntryAsync(int id);
    Task ApproveJournalEntryAsync(int id, string approvedBy);
    Task<string> GenerateNextEntryNumberAsync(int companyId);

    // Mizan & Raporlar
    Task<IEnumerable<TrialBalanceItem>> GetTrialBalanceAsync(int companyId, DateTime? fromDate, DateTime? toDate);
    Task<decimal> GetAccountBalanceAsync(int accountId, DateTime? asOfDate = null);
}

/// <summary>
/// Mizan satır verisi
/// </summary>
public class TrialBalanceItem
{
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public AccountType AccountType { get; set; }
    public decimal DebitTotal { get; set; }
    public decimal CreditTotal { get; set; }
    public decimal DebitBalance { get; set; }
    public decimal CreditBalance { get; set; }
}
