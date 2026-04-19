using System.Collections.ObjectModel;

namespace RockyMuhasebe.Presentation.ViewModels;

/// <summary>
/// Dashboard ViewModel - KPI kartları ve özet bilgiler
/// </summary>
public class DashboardViewModel : ViewModelBase
{
    // KPI Verileri
    public string TotalRevenue { get; set; } = "₺ 2,845,320";
    public string RevenueChange { get; set; } = "+12.5%";
    public string TotalExpense { get; set; } = "₺ 1,920,150";
    public string ExpenseChange { get; set; } = "+8.3%";
    public string NetProfit { get; set; } = "₺ 925,170";
    public string ProfitChange { get; set; } = "+18.7%";
    public string TotalCustomers { get; set; } = "342";
    public string CustomerChange { get; set; } = "+5";
    public string PendingInvoices { get; set; } = "28";
    public string OverdueInvoices { get; set; } = "7";
    public string TotalProducts { get; set; } = "1,245";
    public string LowStockProducts { get; set; } = "15";
    public string CashBalance { get; set; } = "₺ 456,780";
    public string BankBalance { get; set; } = "₺ 1,234,560";

    // Son işlemler
    public ObservableCollection<RecentTransactionItem> RecentTransactions { get; } = new();

    // Vadesi geçen faturalar
    public ObservableCollection<OverdueInvoiceItem> OverdueInvoicesList { get; } = new();

    public DashboardViewModel()
    {
        LoadDemoData();
    }

    private void LoadDemoData()
    {
        // Son işlemler
        RecentTransactions.Add(new RecentTransactionItem { Date = "18.04.2024", Type = "Satış Faturası", Document = "SFT-2024-000342", Customer = "ABC Ticaret Ltd.", Amount = "₺ 15,600.00", Status = "Onaylandı" });
        RecentTransactions.Add(new RecentTransactionItem { Date = "18.04.2024", Type = "Tahsilat", Document = "THK-2024-000189", Customer = "XYZ San. A.Ş.", Amount = "₺ 8,400.00", Status = "Tamamlandı" });
        RecentTransactions.Add(new RecentTransactionItem { Date = "17.04.2024", Type = "Alış Faturası", Document = "AFT-2024-000156", Customer = "Tedarik Plus", Amount = "₺ 23,100.00", Status = "Beklemede" });
        RecentTransactions.Add(new RecentTransactionItem { Date = "17.04.2024", Type = "Yevmiye", Document = "YMK-2024-000098", Customer = "-", Amount = "₺ 5,000.00", Status = "Onaylandı" });
        RecentTransactions.Add(new RecentTransactionItem { Date = "16.04.2024", Type = "Stok Girişi", Document = "SGR-2024-000045", Customer = "Ana Depo", Amount = "₺ 45,200.00", Status = "Tamamlandı" });

        // Vadesi geçen faturalar
        OverdueInvoicesList.Add(new OverdueInvoiceItem { InvoiceNo = "SFT-2024-000298", Customer = "DEF İnşaat A.Ş.", Amount = "₺ 34,500.00", DueDate = "05.04.2024", DaysOverdue = 13 });
        OverdueInvoicesList.Add(new OverdueInvoiceItem { InvoiceNo = "SFT-2024-000285", Customer = "GHI Elektronik", Amount = "₺ 12,800.00", DueDate = "01.04.2024", DaysOverdue = 17 });
        OverdueInvoicesList.Add(new OverdueInvoiceItem { InvoiceNo = "SFT-2024-000270", Customer = "JKL Gıda Ltd.", Amount = "₺ 8,950.00", DueDate = "25.03.2024", DaysOverdue = 24 });
    }
}

public class RecentTransactionItem
{
    public string Date { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public string Customer { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class OverdueInvoiceItem
{
    public string InvoiceNo { get; set; } = string.Empty;
    public string Customer { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;
    public string DueDate { get; set; } = string.Empty;
    public int DaysOverdue { get; set; }
}

// ===== Placeholder ViewModel'ler =====

public class ChartOfAccountsViewModel : ViewModelBase
{
    public ObservableCollection<AccountItem> Accounts { get; } = new();
    public RelayCommand AddAccountCommand { get; }
    public RelayCommand RefreshCommand { get; }

    public ChartOfAccountsViewModel()
    {
        AddAccountCommand = new RelayCommand(_ => { });
        RefreshCommand = new RelayCommand(_ => LoadAccounts());
        LoadAccounts();
    }

    private void LoadAccounts()
    {
        Accounts.Clear();
        Accounts.Add(new AccountItem { Code = "100", Name = "Kasa", Type = "Aktif", Balance = "₺ 45,230.00" });
        Accounts.Add(new AccountItem { Code = "100.01", Name = "TL Kasası", Type = "Aktif", Balance = "₺ 42,130.00" });
        Accounts.Add(new AccountItem { Code = "100.02", Name = "Döviz Kasası", Type = "Aktif", Balance = "₺ 3,100.00" });
        Accounts.Add(new AccountItem { Code = "102", Name = "Bankalar", Type = "Aktif", Balance = "₺ 1,234,560.00" });
        Accounts.Add(new AccountItem { Code = "120", Name = "Alıcılar", Type = "Aktif", Balance = "₺ 567,890.00" });
        Accounts.Add(new AccountItem { Code = "153", Name = "Ticari Mallar", Type = "Aktif", Balance = "₺ 890,340.00" });
        Accounts.Add(new AccountItem { Code = "191", Name = "İndirilecek KDV", Type = "Aktif", Balance = "₺ 34,560.00" });
        Accounts.Add(new AccountItem { Code = "255", Name = "Demirbaşlar", Type = "Aktif", Balance = "₺ 123,450.00" });
        Accounts.Add(new AccountItem { Code = "320", Name = "Satıcılar", Type = "Pasif", Balance = "₺ 345,670.00" });
        Accounts.Add(new AccountItem { Code = "335", Name = "Personele Borçlar", Type = "Pasif", Balance = "₺ 78,900.00" });
        Accounts.Add(new AccountItem { Code = "360", Name = "Ödenecek Vergiler", Type = "Pasif", Balance = "₺ 56,780.00" });
        Accounts.Add(new AccountItem { Code = "391", Name = "Hesaplanan KDV", Type = "Pasif", Balance = "₺ 67,890.00" });
        Accounts.Add(new AccountItem { Code = "500", Name = "Sermaye", Type = "Öz Kaynak", Balance = "₺ 500,000.00" });
        Accounts.Add(new AccountItem { Code = "600", Name = "Yurtiçi Satışlar", Type = "Gelir", Balance = "₺ 2,845,320.00" });
        Accounts.Add(new AccountItem { Code = "621", Name = "Satılan Ticari Mal Maliyeti", Type = "Gider", Balance = "₺ 1,920,150.00" });
        Accounts.Add(new AccountItem { Code = "632", Name = "Genel Yönetim Giderleri", Type = "Gider", Balance = "₺ 234,560.00" });
    }
}

public class AccountItem
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Balance { get; set; } = string.Empty;
}

public class JournalEntryViewModel : ViewModelBase
{
    public ObservableCollection<JournalEntryItem> Entries { get; } = new();

    public JournalEntryViewModel()
    {
        Entries.Add(new JournalEntryItem { EntryNo = "YMK-2024-000098", Date = "18.04.2024", Description = "Satış faturası kaydı - ABC Ticaret", Debit = "₺ 18,720.00", Credit = "₺ 18,720.00", Status = "Onaylı" });
        Entries.Add(new JournalEntryItem { EntryNo = "YMK-2024-000097", Date = "17.04.2024", Description = "Maaş tahakkuku - Nisan 2024", Debit = "₺ 78,900.00", Credit = "₺ 78,900.00", Status = "Onaylı" });
        Entries.Add(new JournalEntryItem { EntryNo = "YMK-2024-000096", Date = "16.04.2024", Description = "Alış faturası kaydı - Tedarik Plus", Debit = "₺ 23,100.00", Credit = "₺ 23,100.00", Status = "Taslak" });
        Entries.Add(new JournalEntryItem { EntryNo = "YMK-2024-000095", Date = "15.04.2024", Description = "Banka masrafı kaydı", Debit = "₺ 350.00", Credit = "₺ 350.00", Status = "Onaylı" });
        Entries.Add(new JournalEntryItem { EntryNo = "YMK-2024-000094", Date = "15.04.2024", Description = "Kira ödemesi - Nisan 2024", Debit = "₺ 12,000.00", Credit = "₺ 12,000.00", Status = "Onaylı" });
    }
}

public class JournalEntryItem
{
    public string EntryNo { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Debit { get; set; } = string.Empty;
    public string Credit { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class CustomersViewModel : ViewModelBase
{
    public ObservableCollection<CustomerItem> Customers { get; } = new();
    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    public CustomersViewModel()
    {
        Customers.Add(new CustomerItem { Code = "MUS-000001", Name = "ABC Ticaret Ltd. Şti.", Type = "Müşteri", Phone = "0212 555 1234", Balance = "₺ 45,600.00", Status = "Aktif" });
        Customers.Add(new CustomerItem { Code = "MUS-000002", Name = "XYZ Sanayi A.Ş.", Type = "Müşteri", Phone = "0216 444 5678", Balance = "₺ 123,400.00", Status = "Aktif" });
        Customers.Add(new CustomerItem { Code = "TDR-000001", Name = "Tedarik Plus A.Ş.", Type = "Tedarikçi", Phone = "0312 333 9012", Balance = "-₺ 67,800.00", Status = "Aktif" });
        Customers.Add(new CustomerItem { Code = "MUS-000003", Name = "DEF İnşaat A.Ş.", Type = "Müşteri", Phone = "0232 222 3456", Balance = "₺ 34,500.00", Status = "Aktif" });
        Customers.Add(new CustomerItem { Code = "TDR-000002", Name = "Global Lojistik Ltd.", Type = "Tedarikçi", Phone = "0242 111 7890", Balance = "-₺ 23,100.00", Status = "Aktif" });
        Customers.Add(new CustomerItem { Code = "MUS-000004", Name = "GHI Elektronik San.", Type = "Müşteri", Phone = "0224 666 2345", Balance = "₺ 12,800.00", Status = "Aktif" });
    }
}

public class CustomerItem
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Balance { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class InvoicesViewModel : ViewModelBase
{
    public ObservableCollection<InvoiceItem> Invoices { get; } = new();

    public InvoicesViewModel()
    {
        Invoices.Add(new InvoiceItem { InvoiceNo = "SFT-2024-000342", Date = "18.04.2024", Customer = "ABC Ticaret Ltd.", Type = "Satış", Amount = "₺ 15,600.00", Vat = "₺ 3,120.00", Total = "₺ 18,720.00", Status = "Onaylandı" });
        Invoices.Add(new InvoiceItem { InvoiceNo = "AFT-2024-000156", Date = "17.04.2024", Customer = "Tedarik Plus A.Ş.", Type = "Alış", Amount = "₺ 19,250.00", Vat = "₺ 3,850.00", Total = "₺ 23,100.00", Status = "Beklemede" });
        Invoices.Add(new InvoiceItem { InvoiceNo = "SFT-2024-000341", Date = "16.04.2024", Customer = "XYZ Sanayi A.Ş.", Type = "Satış", Amount = "₺ 45,000.00", Vat = "₺ 9,000.00", Total = "₺ 54,000.00", Status = "Ödendi" });
        Invoices.Add(new InvoiceItem { InvoiceNo = "SFT-2024-000340", Date = "15.04.2024", Customer = "DEF İnşaat A.Ş.", Type = "Satış", Amount = "₺ 28,750.00", Vat = "₺ 5,750.00", Total = "₺ 34,500.00", Status = "Vadesi Geçmiş" });
    }
}

public class InvoiceItem
{
    public string InvoiceNo { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Customer { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;
    public string Vat { get; set; } = string.Empty;
    public string Total { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class ProductsViewModel : ViewModelBase
{
    public ObservableCollection<ProductItem> Products { get; } = new();

    public ProductsViewModel()
    {
        Products.Add(new ProductItem { Code = "URN-0001", Name = "A4 Fotokopi Kağıdı", Category = "Kırtasiye", Stock = "2,500", Unit = "Paket", BuyPrice = "₺ 85.00", SellPrice = "₺ 120.00", Status = "Normal" });
        Products.Add(new ProductItem { Code = "URN-0002", Name = "Lazer Yazıcı Toneri", Category = "Bilgisayar", Stock = "45", Unit = "Adet", BuyPrice = "₺ 450.00", SellPrice = "₺ 650.00", Status = "Normal" });
        Products.Add(new ProductItem { Code = "URN-0003", Name = "Ofis Koltuğu Ergonomik", Category = "Mobilya", Stock = "8", Unit = "Adet", BuyPrice = "₺ 3,200.00", SellPrice = "₺ 4,500.00", Status = "Düşük Stok" });
        Products.Add(new ProductItem { Code = "URN-0004", Name = "LED Monitor 27\"", Category = "Bilgisayar", Stock = "3", Unit = "Adet", BuyPrice = "₺ 5,800.00", SellPrice = "₺ 7,500.00", Status = "Kritik" });
        Products.Add(new ProductItem { Code = "URN-0005", Name = "Kablosuz Mouse", Category = "Bilgisayar", Stock = "120", Unit = "Adet", BuyPrice = "₺ 180.00", SellPrice = "₺ 280.00", Status = "Normal" });
    }
}

public class ProductItem
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Stock { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public string BuyPrice { get; set; } = string.Empty;
    public string SellPrice { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class StockMovementsViewModel : ViewModelBase { }
public class TrialBalanceViewModel : ViewModelBase { }
public class ReportsViewModel : ViewModelBase { }
public class SettingsViewModel : ViewModelBase { }
