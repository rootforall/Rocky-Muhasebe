# Rocky Muhasebe

Modern ve kullanıcı dostu bir masaüstü muhasebe yönetim sistemi. .NET 8 ve WPF teknolojileri kullanılarak geliştirilmiş, Türk muhasebe standartlarına uygun olarak tasarlanmıştır.

## 🌟 Özellikler

- **Genel Muhasebe**: Tekdüzen Hesap Planı'na uygun hesap yönetimi
- **Cari Hesap Yönetimi**: Müşteri ve tedarikçi takibi
- **Stok Yönetimi**: Depo ve envanter kontrolü
- **Yevmiye Kayıtları**: Detaylı muhasebe fişi girişi
- **Mizan Raporları**: Aylık ve yıllık mizan görüntüleme
- **Dashboard**: Anlık finansal göstergeler ve grafikler

## 🏗️ Proje Yapısı

Proje, Clean Architecture prensiplerine uygun olarak katmanlı bir yapıda geliştirilmiştir:

```
RockyMuhasebe/
├── src/
│   ├── RockyMuhasebe.Domain          # Varlık modelleri ve iş kuralları
│   ├── RockyMuhasebe.Application     # İş mantığı ve servis katmanı
│   ├── RockyMuhasebe.Data           # Veri erişim katmanı
│   ├── RockyMuhasebe.Infrastructure # Altyapı servisleri
│   ├── RockyMuhasebe.Integration    # Dış sistem entegrasyonları
│   └── RockyMuhasebe.Presentation   # WPF kullanıcı arayüzü
└── tests/
    └── RockyMuhasebe.Tests          # Birim testleri
```

## 🛠️ Teknolojiler

- **.NET 8** - Modern .NET platformu
- **WPF (Windows Presentation Foundation)** - Masaüstü UI framework
- **CommunityToolkit.Mvvm** - MVVM pattern desteği
- **Entity Framework Core** - ORM (veri erişim katmanı)
- **Dependency Injection** - Bağımlılık enjeksiyonu

## 📋 Gereksinimler

- Windows 10/11
- .NET 8.0 Runtime
- Visual Studio 2022 (geliştirme için)

## 🚀 Kurulum

### 1. Projeyi Klonlayın

```bash
git clone https://github.com/kullaniciadi/RockyMuhasebe.git
cd RockyMuhasebe
```

### 2. Bağımlılıkları Yükleyin

```bash
dotnet restore
```

### 3. Projeyi Derleyin

```bash
dotnet build
```

### 4. Uygulamayı Çalıştırın

```bash
dotnet run --project src/RockyMuhasebe.Presentation
```

## 📖 Kullanım

Uygulama başlatıldığında karşınıza gelen dashboard üzerinden:

1. **Hesap Planı**: Yeni hesap oluşturabilir veya mevcut hesapları düzenleyebilirsiniz
2. **Yevmiye Fişleri**: Borç/Alacak kayıtlarınızı girebilirsiniz
3. **Cari Hesaplar**: Müşteri ve tedarikçi bilgilerinizi yönetebilirsiniz
4. **Stok Takibi**: Ürün ve depo hareketlerinizi takip edebilirsiniz
5. **Raporlar**: Mizan, bilanço ve gelir tablosu gibi raporları görüntüleyebilirsiniz

## 🧪 Testler

Birim testlerini çalıştırmak için:

```bash
dotnet test
```

## 🤝 Katkıda Bulunma

Katkılarınızı bekliyoruz! Lütfen şu adımları izleyin:

1. Projeyi fork edin
2. Yeni bir branch oluşturun (`git checkout -b feature/yeniOzellik`)
3. Değişikliklerinizi commit edin (`git commit -am 'Yeni özellik eklendi'`)
4. Branch'inizi push edin (`git push origin feature/yeniOzellik`)
5. Pull Request oluşturun

## 📄 Lisans

Bu proje [MIT](LICENSE) lisansı altında lisanslanmıştır.

## 📞 İletişim

- **Proje Sahibi**: RockyMuhasebe Ekibi
- **E-posta**: info@rockymuhasebe.com
- **Website**: www.rockymuhasebe.com

## 🙏 Teşekkürler

- .NET ekibine modern araçlar sağladıkları için
- CommunityToolkit ekibine MVVM desteği için
- Tüm katkıda bulunanlara

---

**Rocky Muhasebe** - Güvenilir, hızlı ve kullanıcı dostu muhasebe çözümü.
