using System.Collections.ObjectModel;
using System.Windows;

namespace RockyMuhasebe.Presentation.ViewModels;

/// <summary>
/// Ana pencere ViewModel'i - Navigasyon ve genel uygulama durumu yönetimi
/// </summary>
public class MainWindowViewModel : ViewModelBase
{
    #region Properties

    private ViewModelBase? _currentView;
    public ViewModelBase? CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }

    private string _currentPageTitle = "Dashboard";
    public string CurrentPageTitle
    {
        get => _currentPageTitle;
        set => SetProperty(ref _currentPageTitle, value);
    }

    private string _currentUser = "Sistem Yöneticisi";
    public string CurrentUser
    {
        get => _currentUser;
        set => SetProperty(ref _currentUser, value);
    }

    private string _companyName = "Demo Şirket A.Ş.";
    public string CompanyName
    {
        get => _companyName;
        set => SetProperty(ref _companyName, value);
    }

    private string _currentDate = DateTime.Now.ToString("dd MMMM yyyy, dddd");
    public string CurrentDate
    {
        get => _currentDate;
        set => SetProperty(ref _currentDate, value);
    }

    private bool _isSidebarExpanded = true;
    public bool IsSidebarExpanded
    {
        get => _isSidebarExpanded;
        set => SetProperty(ref _isSidebarExpanded, value);
    }

    private string _selectedModule = "Dashboard";
    public string SelectedModule
    {
        get => _selectedModule;
        set => SetProperty(ref _selectedModule, value);
    }

    public ObservableCollection<NotificationItem> Notifications { get; } = new();

    #endregion

    #region Commands

    public RelayCommand NavigateCommand { get; }
    public RelayCommand ToggleSidebarCommand { get; }
    public RelayCommand LogoutCommand { get; }
    public RelayCommand ExitCommand { get; }

    #endregion

    // ViewModels
    private DashboardViewModel? _dashboardViewModel;
    private ChartOfAccountsViewModel? _chartOfAccountsViewModel;
    private JournalEntryViewModel? _journalEntryViewModel;
    private CustomersViewModel? _customersViewModel;
    private InvoicesViewModel? _invoicesViewModel;
    private ProductsViewModel? _productsViewModel;

    public MainWindowViewModel()
    {
        NavigateCommand = new RelayCommand(Navigate);
        ToggleSidebarCommand = new RelayCommand(_ => IsSidebarExpanded = !IsSidebarExpanded);
        LogoutCommand = new RelayCommand(_ => HandleLogout());
        ExitCommand = new RelayCommand(_ => global::System.Windows.Application.Current.Shutdown());

        // Varsayılan olarak Dashboard göster
        _dashboardViewModel = new DashboardViewModel();
        CurrentView = _dashboardViewModel;

        // Demo bildirimler
        Notifications.Add(new NotificationItem { Title = "Yeni Fatura", Message = "SFT-2024-000342 numaralı fatura oluşturuldu", Time = "10 dk önce", Type = "info" });
        Notifications.Add(new NotificationItem { Title = "Düşük Stok", Message = "3 üründe stok kritik seviyenin altında", Time = "1 saat önce", Type = "warning" });
        Notifications.Add(new NotificationItem { Title = "KDV Beyanname", Message = "Nisan 2024 KDV beyannamesi hazır", Time = "2 saat önce", Type = "success" });
    }

    private void Navigate(object? parameter)
    {
        if (parameter is string page)
        {
            SelectedModule = page;
            switch (page)
            {
                case "Dashboard":
                    CurrentPageTitle = "Dashboard";
                    _dashboardViewModel ??= new DashboardViewModel();
                    CurrentView = _dashboardViewModel;
                    break;
                case "HesapPlani":
                    CurrentPageTitle = "Hesap Planı";
                    _chartOfAccountsViewModel ??= new ChartOfAccountsViewModel();
                    CurrentView = _chartOfAccountsViewModel;
                    break;
                case "Yevmiye":
                    CurrentPageTitle = "Yevmiye Defteri";
                    _journalEntryViewModel ??= new JournalEntryViewModel();
                    CurrentView = _journalEntryViewModel;
                    break;
                case "CariHesaplar":
                    CurrentPageTitle = "Cari Hesaplar";
                    _customersViewModel ??= new CustomersViewModel();
                    CurrentView = _customersViewModel;
                    break;
                case "Faturalar":
                    CurrentPageTitle = "Fatura Yönetimi";
                    _invoicesViewModel ??= new InvoicesViewModel();
                    CurrentView = _invoicesViewModel;
                    break;
                case "Urunler":
                    CurrentPageTitle = "Ürün & Stok Yönetimi";
                    _productsViewModel ??= new ProductsViewModel();
                    CurrentView = _productsViewModel;
                    break;
                case "StokHareketleri":
                    CurrentPageTitle = "Stok Hareketleri";
                    CurrentView = new StockMovementsViewModel();
                    break;
                case "Mizan":
                    CurrentPageTitle = "Mizan Raporu";
                    CurrentView = new TrialBalanceViewModel();
                    break;
                case "Raporlar":
                    CurrentPageTitle = "Raporlar";
                    CurrentView = new ReportsViewModel();
                    break;
                case "Ayarlar":
                    CurrentPageTitle = "Sistem Ayarları";
                    CurrentView = new SettingsViewModel();
                    break;
                default:
                    CurrentPageTitle = page;
                    break;
            }
        }
    }

    private void HandleLogout()
    {
        // Çıkış işlemleri
        var result = MessageBox.Show("Oturumu kapatmak istediğinize emin misiniz?", 
            "Oturum Kapatma", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            global::System.Windows.Application.Current.Shutdown();
        }
    }
}

/// <summary>
/// Bildirim modeli
/// </summary>
public class NotificationItem
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Time { get; set; } = string.Empty;
    public string Type { get; set; } = "info";
}
