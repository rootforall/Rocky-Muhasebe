using Microsoft.EntityFrameworkCore;
using RockyMuhasebe.Application.Services.Interfaces;
using RockyMuhasebe.Data.Context;
using RockyMuhasebe.Domain.Entities.Customers;
using RockyMuhasebe.Domain.Enums;
using RockyMuhasebe.Domain.Interfaces;

namespace RockyMuhasebe.Application.Services.Implementations;

/// <summary>
/// Cari hesap servisi implementasyonu
/// </summary>
public class CustomerService : ICustomerService
{
    private readonly RockyDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerService(RockyDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    #region Cari Hesap İşlemleri

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync(int companyId)
    {
        return await _context.Customers
            .Where(c => c.CompanyId == companyId)
            .OrderBy(c => c.CustomerCode)
            .ToListAsync();
    }

    public async Task<IEnumerable<Customer>> GetCustomersByTypeAsync(CustomerType type, int companyId)
    {
        return await _context.Customers
            .Where(c => c.CompanyId == companyId && c.CustomerType == type)
            .OrderBy(c => c.CustomerCode)
            .ToListAsync();
    }

    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        return await _context.Customers
            .Include(c => c.Invoices)
            .Include(c => c.Payments)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Customer?> GetCustomerByCodeAsync(string code, int companyId)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.CustomerCode == code && c.CompanyId == companyId);
    }

    public async Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm, int companyId)
    {
        return await _context.Customers
            .Where(c => c.CompanyId == companyId &&
                       (c.CustomerCode.Contains(searchTerm) ||
                        c.Name.Contains(searchTerm) ||
                        (c.TaxNumber != null && c.TaxNumber.Contains(searchTerm))))
            .OrderBy(c => c.Name)
            .Take(50)
            .ToListAsync();
    }

    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        if (string.IsNullOrEmpty(customer.CustomerCode))
            customer.CustomerCode = await GenerateNextCustomerCodeAsync(customer.CompanyId, customer.CustomerType);

        _context.Customers.Add(customer);
        await _unitOfWork.SaveChangesAsync();
        return customer;
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteCustomerAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) throw new InvalidOperationException("Cari hesap bulunamadı.");

        var hasInvoices = await _context.Invoices.AnyAsync(i => i.CustomerId == id);
        if (hasInvoices) throw new InvalidOperationException("Bu cari hesaba ait fatura bulunduğundan silinemez.");

        customer.IsDeleted = true;
        customer.DeletedAt = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<string> GenerateNextCustomerCodeAsync(int companyId, CustomerType type)
    {
        var prefix = type == CustomerType.Vendor ? "TDR" : "MUS";
        var lastCustomer = await _context.Customers
            .Where(c => c.CompanyId == companyId && c.CustomerCode.StartsWith(prefix))
            .OrderByDescending(c => c.CustomerCode)
            .FirstOrDefaultAsync();

        if (lastCustomer == null) return $"{prefix}-000001";

        var parts = lastCustomer.CustomerCode.Split('-');
        if (parts.Length >= 2 && int.TryParse(parts[1], out int num))
            return $"{prefix}-{(num + 1):D6}";

        return $"{prefix}-000001";
    }

    #endregion

    #region Fatura İşlemleri

    public async Task<IEnumerable<Invoice>> GetInvoicesAsync(int companyId, InvoiceType? type = null, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var query = _context.Invoices
            .Include(i => i.Customer)
            .Include(i => i.Lines)
            .Where(i => i.CompanyId == companyId);

        if (type.HasValue)
            query = query.Where(i => i.InvoiceType == type.Value);
        if (fromDate.HasValue)
            query = query.Where(i => i.InvoiceDate >= fromDate.Value);
        if (toDate.HasValue)
            query = query.Where(i => i.InvoiceDate <= toDate.Value);

        return await query.OrderByDescending(i => i.InvoiceDate).ToListAsync();
    }

    public async Task<IEnumerable<Invoice>> GetInvoicesByCustomerAsync(int customerId)
    {
        return await _context.Invoices
            .Include(i => i.Lines)
            .Where(i => i.CustomerId == customerId)
            .OrderByDescending(i => i.InvoiceDate)
            .ToListAsync();
    }

    public async Task<Invoice?> GetInvoiceByIdAsync(int id)
    {
        return await _context.Invoices
            .Include(i => i.Customer)
            .Include(i => i.Lines)
            .Include(i => i.Payments)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
    {
        if (string.IsNullOrEmpty(invoice.InvoiceNumber))
            invoice.InvoiceNumber = await GenerateNextInvoiceNumberAsync(invoice.CompanyId, invoice.InvoiceType);

        // Satır toplamlarını hesapla
        CalculateInvoiceTotals(invoice);

        _context.Invoices.Add(invoice);
        await _unitOfWork.SaveChangesAsync();

        // Cari hesap bakiyesini güncelle
        var customer = await _context.Customers.FindAsync(invoice.CustomerId);
        if (customer != null)
        {
            if (invoice.InvoiceType == InvoiceType.Sales || invoice.InvoiceType == InvoiceType.CashSales)
                customer.DebitBalance += invoice.TotalAmount;
            else
                customer.CreditBalance += invoice.TotalAmount;

            await _unitOfWork.SaveChangesAsync();
        }

        return invoice;
    }

    public async Task UpdateInvoiceAsync(Invoice invoice)
    {
        CalculateInvoiceTotals(invoice);
        _context.Invoices.Update(invoice);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteInvoiceAsync(int id)
    {
        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice == null) throw new InvalidOperationException("Fatura bulunamadı.");
        if (invoice.Status != InvoiceStatus.Draft)
            throw new InvalidOperationException("Sadece taslak durumundaki faturalar silinebilir.");

        invoice.IsDeleted = true;
        invoice.DeletedAt = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ApproveInvoiceAsync(int id)
    {
        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice == null) throw new InvalidOperationException("Fatura bulunamadı.");

        invoice.Status = InvoiceStatus.Approved;
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task CancelInvoiceAsync(int id)
    {
        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice == null) throw new InvalidOperationException("Fatura bulunamadı.");

        invoice.Status = InvoiceStatus.Cancelled;
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<string> GenerateNextInvoiceNumberAsync(int companyId, InvoiceType type)
    {
        var prefix = type switch
        {
            InvoiceType.Sales => "SFT",
            InvoiceType.Purchase => "AFT",
            InvoiceType.Proforma => "PFT",
            InvoiceType.Export => "IFT",
            InvoiceType.Return => "IFT",
            _ => "FTR"
        };

        var year = DateTime.Now.Year;
        var lastInvoice = await _context.Invoices
            .Where(i => i.CompanyId == companyId && i.InvoiceNumber.StartsWith($"{prefix}-{year}"))
            .OrderByDescending(i => i.InvoiceNumber)
            .FirstOrDefaultAsync();

        if (lastInvoice == null) return $"{prefix}-{year}-000001";

        var parts = lastInvoice.InvoiceNumber.Split('-');
        if (parts.Length >= 3 && int.TryParse(parts[2], out int num))
            return $"{prefix}-{year}-{(num + 1):D6}";

        return $"{prefix}-{year}-000001";
    }

    private static void CalculateInvoiceTotals(Invoice invoice)
    {
        decimal subTotal = 0, vatTotal = 0, discountTotal = 0;

        foreach (var line in invoice.Lines)
        {
            line.LineTotal = line.Quantity * line.UnitPrice;
            line.DiscountAmount = line.LineTotal * (line.DiscountRate / 100);
            var lineNet = line.LineTotal - line.DiscountAmount;
            line.VatAmount = lineNet * (line.VatRate / 100);
            line.LineTotalWithVat = lineNet + line.VatAmount;

            subTotal += lineNet;
            vatTotal += line.VatAmount;
            discountTotal += line.DiscountAmount;
        }

        invoice.SubTotal = subTotal;
        invoice.VatAmount = vatTotal;
        invoice.DiscountAmount = discountTotal;
        invoice.TotalAmount = subTotal + vatTotal;
    }

    #endregion

    #region Ödeme İşlemleri

    public async Task<IEnumerable<Payment>> GetPaymentsByCustomerAsync(int customerId)
    {
        return await _context.Payments
            .Where(p => p.CustomerId == customerId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<Payment> CreatePaymentAsync(Payment payment)
    {
        _context.Payments.Add(payment);
        await _unitOfWork.SaveChangesAsync();

        // Fatura ödeme tutarını güncelle
        if (payment.InvoiceId.HasValue)
        {
            var invoice = await _context.Invoices.FindAsync(payment.InvoiceId.Value);
            if (invoice != null)
            {
                invoice.PaidAmount += payment.Amount;
                if (invoice.PaidAmount >= invoice.TotalAmount)
                    invoice.Status = InvoiceStatus.Paid;
                else if (invoice.PaidAmount > 0)
                    invoice.Status = InvoiceStatus.PartiallyPaid;
            }
        }

        // Cari bakiyeyi güncelle
        var customer = await _context.Customers.FindAsync(payment.CustomerId);
        if (customer != null)
        {
            if (payment.IsIncoming)
                customer.CreditBalance += payment.Amount;
            else
                customer.DebitBalance += payment.Amount;
        }

        await _unitOfWork.SaveChangesAsync();
        return payment;
    }

    public async Task<decimal> GetCustomerBalanceAsync(int customerId)
    {
        var customer = await _context.Customers.FindAsync(customerId);
        if (customer == null) return 0;
        return customer.DebitBalance - customer.CreditBalance;
    }

    public async Task<IEnumerable<CustomerStatement>> GetCustomerStatementAsync(int customerId, DateTime fromDate, DateTime toDate)
    {
        var statements = new List<CustomerStatement>();
        decimal runningBalance = 0;

        // Faturalar
        var invoices = await _context.Invoices
            .Where(i => i.CustomerId == customerId && i.InvoiceDate >= fromDate && i.InvoiceDate <= toDate)
            .OrderBy(i => i.InvoiceDate)
            .ToListAsync();

        foreach (var inv in invoices)
        {
            bool isDebit = inv.InvoiceType == InvoiceType.Sales || inv.InvoiceType == InvoiceType.CashSales;
            runningBalance += isDebit ? inv.TotalAmount : -inv.TotalAmount;

            statements.Add(new CustomerStatement
            {
                Date = inv.InvoiceDate,
                DocumentNumber = inv.InvoiceNumber,
                Description = $"Fatura - {inv.InvoiceType}",
                DebitAmount = isDebit ? inv.TotalAmount : 0,
                CreditAmount = isDebit ? 0 : inv.TotalAmount,
                Balance = runningBalance
            });
        }

        // Ödemeler
        var payments = await _context.Payments
            .Where(p => p.CustomerId == customerId && p.PaymentDate >= fromDate && p.PaymentDate <= toDate)
            .OrderBy(p => p.PaymentDate)
            .ToListAsync();

        foreach (var pay in payments)
        {
            runningBalance += pay.IsIncoming ? -pay.Amount : pay.Amount;
            statements.Add(new CustomerStatement
            {
                Date = pay.PaymentDate,
                DocumentNumber = pay.PaymentNumber,
                Description = $"Ödeme - {pay.PaymentMethod}",
                DebitAmount = pay.IsIncoming ? 0 : pay.Amount,
                CreditAmount = pay.IsIncoming ? pay.Amount : 0,
                Balance = runningBalance
            });
        }

        return statements.OrderBy(s => s.Date).ThenBy(s => s.DocumentNumber);
    }

    #endregion
}
