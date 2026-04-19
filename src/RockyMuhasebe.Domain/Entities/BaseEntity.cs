using RockyMuhasebe.Domain.Interfaces;

namespace RockyMuhasebe.Domain.Entities;

/// <summary>
/// Tüm entity'ler için temel sınıf - Ortak alanları içerir
/// </summary>
public abstract class BaseEntity : IEntity, IAuditable
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
}
