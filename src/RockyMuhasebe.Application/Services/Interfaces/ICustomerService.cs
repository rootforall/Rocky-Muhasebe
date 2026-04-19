using RockyMuhasebe.Domain.Entities.Customers;
using RockyMuhasebe.Domain.Enums;

namespace RockyMuhasebe.Application.Services.Interfaces;

/// <summary>
/// Cari Hesap servisi arayüzü
/// </summary>
public interface ICustomerService
{
    // Cari Hesap İşlemleri
    Task<IEnumerable<Customer>> GetAllCustomersAsync(int companyId);
    Task<IEnumerable<Customer>> GetCustomersByTypeAsync(CustomerType type, int companyId);
    Task<Customer?> GetCustomerByIdAsync(int id);
    Task<Customer?> GetCustomerByCodeAsync(string code, int companyId);
    Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm, int companyId);
    Task<Customer> CreateCustomerAsync(Customer customer);
    Task UpdateCustomerAsync(Customer customer);
    Task DeleteCustomerAsync(int id);
    Task<string> GenerateNextCustomerCodeAsync(int companyId, CustomerType type);

    // Fatura İşlemleri
    Task<IEnumerable<Invoice>> GetInvoicesAsync(int companyId, InvoiceType? type = null, DateTime? fromDate = null, DateTime? toDate = null);
    Task<IEnumerable<Invoice>> GetInvoicesByCustomerAsync(int customerId);
    Task<Invoice?> GetInvoiceByIdAsync(int id);
    Task<Invoice> CreateInvoiceAsync(Invoice invoice);
    Task UpdateInvoiceAsync(Invoice invoice);
    Task DeleteInvoiceAsync(int id);
    Task ApproveInvoiceAsync(int id);
    Task CancelInvoiceAsync(int id);
    Task<string> GenerateNextInvoiceNumberAsync(int companyId, InvoiceType type);

    // Ödeme İşlemleri
    Task<IEnumerable<Payment>> GetPaymentsByCustomerAsync(int customerId);
    Task<Payment> CreatePaymentAsync(Payment payment);
    Task<decimal> GetCustomerBalanceAsync(int customerId);
    Task<IEnumerable<CustomerStatement>> GetCustomerStatementAsync(int customerId, DateTime fromDate, DateTime toDate);
}

/// <summary>
/// Cari hesap ekstresi satırı
/// </summary>
public class CustomerStatement
{
    public DateTime Date { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public decimal Balance { get; set; }
}
