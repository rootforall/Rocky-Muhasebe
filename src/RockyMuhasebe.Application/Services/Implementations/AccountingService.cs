using Microsoft.EntityFrameworkCore;
using RockyMuhasebe.Application.Services.Interfaces;
using RockyMuhasebe.Data.Context;
using RockyMuhasebe.Domain.Entities.Accounting;
using RockyMuhasebe.Domain.Enums;
using RockyMuhasebe.Domain.Interfaces;

namespace RockyMuhasebe.Application.Services.Implementations;

/// <summary>
/// Muhasebe servisi implementasyonu
/// </summary>
public class AccountingService : IAccountingService
{
    private readonly RockyDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public AccountingService(RockyDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    #region Hesap Planı İşlemleri

    public async Task<IEnumerable<GeneralLedgerAccount>> GetAllAccountsAsync(int companyId)
    {
        return await _context.GeneralLedgerAccounts
            .Where(a => a.CompanyId == companyId)
            .OrderBy(a => a.AccountCode)
            .ToListAsync();
    }

    public async Task<GeneralLedgerAccount?> GetAccountByIdAsync(int id)
    {
        return await _context.GeneralLedgerAccounts
            .Include(a => a.SubAccounts)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<GeneralLedgerAccount?> GetAccountByCodeAsync(string code, int companyId)
    {
        return await _context.GeneralLedgerAccounts
            .FirstOrDefaultAsync(a => a.AccountCode == code && a.CompanyId == companyId);
    }

    public async Task<IEnumerable<GeneralLedgerAccount>> GetAccountsByTypeAsync(AccountType type, int companyId)
    {
        return await _context.GeneralLedgerAccounts
            .Where(a => a.AccountType == type && a.CompanyId == companyId)
            .OrderBy(a => a.AccountCode)
            .ToListAsync();
    }

    public async Task<IEnumerable<GeneralLedgerAccount>> SearchAccountsAsync(string searchTerm, int companyId)
    {
        return await _context.GeneralLedgerAccounts
            .Where(a => a.CompanyId == companyId &&
                       (a.AccountCode.Contains(searchTerm) || a.AccountName.Contains(searchTerm)))
            .OrderBy(a => a.AccountCode)
            .Take(50)
            .ToListAsync();
    }

    public async Task<GeneralLedgerAccount> CreateAccountAsync(GeneralLedgerAccount account)
    {
        // Hesap kodu benzersiz olmalı
        var exists = await _context.GeneralLedgerAccounts
            .AnyAsync(a => a.AccountCode == account.AccountCode && a.CompanyId == account.CompanyId);

        if (exists)
            throw new InvalidOperationException($"'{account.AccountCode}' hesap kodu zaten mevcut.");

        _context.GeneralLedgerAccounts.Add(account);
        await _unitOfWork.SaveChangesAsync();
        return account;
    }

    public async Task UpdateAccountAsync(GeneralLedgerAccount account)
    {
        _context.GeneralLedgerAccounts.Update(account);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAccountAsync(int id)
    {
        var account = await _context.GeneralLedgerAccounts.FindAsync(id);
        if (account == null)
            throw new InvalidOperationException("Hesap bulunamadı.");

        // Yevmiye kaydı varsa silinemez
        var hasJournalLines = await _context.JournalLines.AnyAsync(j => j.AccountId == id);
        if (hasJournalLines)
            throw new InvalidOperationException("Bu hesapta yevmiye kaydı bulunduğundan silinemez.");

        account.IsDeleted = true;
        account.DeletedAt = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
    }

    #endregion

    #region Yevmiye İşlemleri

    public async Task<IEnumerable<JournalEntry>> GetJournalEntriesAsync(int companyId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var query = _context.JournalEntries
            .Include(j => j.Lines)
                .ThenInclude(l => l.Account)
            .Where(j => j.CompanyId == companyId);

        if (fromDate.HasValue)
            query = query.Where(j => j.EntryDate >= fromDate.Value);
        if (toDate.HasValue)
            query = query.Where(j => j.EntryDate <= toDate.Value);

        return await query.OrderByDescending(j => j.EntryDate)
                          .ThenByDescending(j => j.EntryNumber)
                          .ToListAsync();
    }

    public async Task<JournalEntry?> GetJournalEntryByIdAsync(int id)
    {
        return await _context.JournalEntries
            .Include(j => j.Lines)
                .ThenInclude(l => l.Account)
            .FirstOrDefaultAsync(j => j.Id == id);
    }

    public async Task<JournalEntry> CreateJournalEntryAsync(JournalEntry entry)
    {
        // Borç-Alacak dengesi kontrolü
        var totalDebit = entry.Lines.Sum(l => l.DebitAmount);
        var totalCredit = entry.Lines.Sum(l => l.CreditAmount);

        if (Math.Abs(totalDebit - totalCredit) > 0.01m)
            throw new InvalidOperationException(
                $"Borç ({totalDebit:N2}) ve Alacak ({totalCredit:N2}) tutarları eşit olmalıdır.");

        entry.TotalDebit = totalDebit;
        entry.TotalCredit = totalCredit;

        if (string.IsNullOrEmpty(entry.EntryNumber))
            entry.EntryNumber = await GenerateNextEntryNumberAsync(entry.CompanyId);

        _context.JournalEntries.Add(entry);
        await _unitOfWork.SaveChangesAsync();

        // Hesap bakiyelerini güncelle
        foreach (var line in entry.Lines)
        {
            var account = await _context.GeneralLedgerAccounts.FindAsync(line.AccountId);
            if (account != null)
            {
                account.DebitBalance += line.DebitAmount;
                account.CreditBalance += line.CreditAmount;
            }
        }
        await _unitOfWork.SaveChangesAsync();

        return entry;
    }

    public async Task UpdateJournalEntryAsync(JournalEntry entry)
    {
        var totalDebit = entry.Lines.Sum(l => l.DebitAmount);
        var totalCredit = entry.Lines.Sum(l => l.CreditAmount);

        if (Math.Abs(totalDebit - totalCredit) > 0.01m)
            throw new InvalidOperationException("Borç ve Alacak tutarları eşit olmalıdır.");

        entry.TotalDebit = totalDebit;
        entry.TotalCredit = totalCredit;

        _context.JournalEntries.Update(entry);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteJournalEntryAsync(int id)
    {
        var entry = await _context.JournalEntries.Include(j => j.Lines).FirstOrDefaultAsync(j => j.Id == id);
        if (entry == null) throw new InvalidOperationException("Yevmiye kaydı bulunamadı.");
        if (entry.IsApproved) throw new InvalidOperationException("Onaylanmış yevmiye kaydı silinemez.");

        entry.IsDeleted = true;
        entry.DeletedAt = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ApproveJournalEntryAsync(int id, string approvedBy)
    {
        var entry = await _context.JournalEntries.FindAsync(id);
        if (entry == null) throw new InvalidOperationException("Yevmiye kaydı bulunamadı.");

        entry.IsApproved = true;
        entry.ApprovedBy = approvedBy;
        entry.ApprovedAt = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<string> GenerateNextEntryNumberAsync(int companyId)
    {
        var year = DateTime.Now.Year;
        var lastEntry = await _context.JournalEntries
            .Where(j => j.CompanyId == companyId && j.EntryDate.Year == year)
            .OrderByDescending(j => j.EntryNumber)
            .FirstOrDefaultAsync();

        if (lastEntry == null)
            return $"YMK-{year}-000001";

        var parts = lastEntry.EntryNumber.Split('-');
        if (parts.Length >= 3 && int.TryParse(parts[2], out int num))
            return $"YMK-{year}-{(num + 1):D6}";

        return $"YMK-{year}-000001";
    }

    #endregion

    #region Mizan & Raporlar

    public async Task<IEnumerable<TrialBalanceItem>> GetTrialBalanceAsync(int companyId, DateTime? fromDate, DateTime? toDate)
    {
        var query = _context.JournalLines
            .Include(l => l.Account)
            .Include(l => l.JournalEntry)
            .Where(l => l.JournalEntry.CompanyId == companyId && l.JournalEntry.IsApproved);

        if (fromDate.HasValue)
            query = query.Where(l => l.JournalEntry.EntryDate >= fromDate.Value);
        if (toDate.HasValue)
            query = query.Where(l => l.JournalEntry.EntryDate <= toDate.Value);

        var result = await query
            .GroupBy(l => new { l.Account.AccountCode, l.Account.AccountName, l.Account.AccountType })
            .Select(g => new TrialBalanceItem
            {
                AccountCode = g.Key.AccountCode,
                AccountName = g.Key.AccountName,
                AccountType = g.Key.AccountType,
                DebitTotal = g.Sum(l => l.DebitAmount),
                CreditTotal = g.Sum(l => l.CreditAmount),
                DebitBalance = g.Sum(l => l.DebitAmount) > g.Sum(l => l.CreditAmount)
                    ? g.Sum(l => l.DebitAmount) - g.Sum(l => l.CreditAmount) : 0,
                CreditBalance = g.Sum(l => l.CreditAmount) > g.Sum(l => l.DebitAmount)
                    ? g.Sum(l => l.CreditAmount) - g.Sum(l => l.DebitAmount) : 0
            })
            .OrderBy(t => t.AccountCode)
            .ToListAsync();

        return result;
    }

    public async Task<decimal> GetAccountBalanceAsync(int accountId, DateTime? asOfDate = null)
    {
        var query = _context.JournalLines
            .Include(l => l.JournalEntry)
            .Where(l => l.AccountId == accountId && l.JournalEntry.IsApproved);

        if (asOfDate.HasValue)
            query = query.Where(l => l.JournalEntry.EntryDate <= asOfDate.Value);

        var debit = await query.SumAsync(l => l.DebitAmount);
        var credit = await query.SumAsync(l => l.CreditAmount);

        return debit - credit;
    }

    #endregion
}
