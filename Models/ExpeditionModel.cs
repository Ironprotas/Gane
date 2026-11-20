namespace GaNe.Models;

public class ExpeditionModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<AdventurerModel> Team { get; set; } = new();
    public int CompanyRating { get; set; }
}