using RockyMuhasebe.Domain.Entities.Warehouse;
using RockyMuhasebe.Domain.Enums;

namespace RockyMuhasebe.Application.Services.Interfaces;

/// <summary>
/// Depo & Stok servisi arayüzü
/// </summary>
public interface IWarehouseService
{
    // Ürün İşlemleri
    Task<IEnumerable<Product>> GetAllProductsAsync(int companyId);
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product?> GetProductByCodeAsync(string code, int companyId);
    Task<Product?> GetProductByBarcodeAsync(string barcode);
    Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm, int companyId);
    Task<IEnumerable<Product>> GetLowStockProductsAsync(int companyId);
    Task<Product> CreateProductAsync(Product product);
    Task UpdateProductAsync(Product product);
    Task DeleteProductAsync(int id);
    Task<string> GenerateNextProductCodeAsync(int companyId);

    // Kategori İşlemleri
    Task<IEnumerable<ProductCategory>> GetAllCategoriesAsync(int companyId);
    Task<ProductCategory> CreateCategoryAsync(ProductCategory category);
    Task UpdateCategoryAsync(ProductCategory category);
    Task DeleteCategoryAsync(int id);

    // Stok Hareketi İşlemleri
    Task<IEnumerable<StockMovement>> GetStockMovementsAsync(int companyId, int? productId = null, DateTime? fromDate = null, DateTime? toDate = null);
    Task<StockMovement> CreateStockMovementAsync(StockMovement movement);
    Task<string> GenerateNextMovementNumberAsync(int companyId);

    // Depo İşlemleri
    Task<IEnumerable<WarehouseDefinition>> GetAllWarehousesAsync(int companyId);
    Task<WarehouseDefinition> CreateWarehouseAsync(WarehouseDefinition warehouse);
    Task UpdateWarehouseAsync(WarehouseDefinition warehouse);

    // Stok Değerleme
    Task<decimal> GetStockValueAsync(int companyId);
    Task<decimal> GetProductStockValueAsync(int productId);
}
