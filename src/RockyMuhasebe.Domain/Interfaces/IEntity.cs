namespace RockyMuhasebe.Domain.Interfaces;

/// <summary>
/// Tüm entity'ler için temel arayüz
/// </summary>
public interface IEntity
{
    int Id { get; set; }
}

/// <summary>
/// Denetlenebilir entity'ler için arayüz - Kim, ne zaman oluşturdu/güncelledi bilgisini tutar
/// </summary>
public interface IAuditable
{
    DateTime CreatedAt { get; set; }
    string? CreatedBy { get; set; }
    DateTime? UpdatedAt { get; set; }
    string? UpdatedBy { get; set; }
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }
    string? DeletedBy { get; set; }
}

/// <summary>
/// Soft-delete destekleyen entity'ler için arayüz
/// </summary>
public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }
    string? DeletedBy { get; set; }
}
