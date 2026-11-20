using GaNe.Enums;

namespace GaNe.Models.Dto;

public sealed class AdventurerDto
{
    public Guid? ExpeditionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsLive { get; set; } = true;
    public AdventurerClassEnum  Archetype { get; set; }
    public SexEnum Sex  { get; set; }
    public int Rating { get; set; }
}