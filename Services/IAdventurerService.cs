using GaNe.Models.Dto;
using GaNe.Repositories;

namespace GaNe.Services;

public interface IAdventurerService
{
    Task<Guid> CreateAdventurer(AdventurerDto adventurer, CancellationToken cancellationToken = default);
    Task<List<AdventurerDto>> GetAdventurers(CancellationToken cancellationToken = default);
    Task<AdventurerDto?> GetAdventurer(Guid adventurerId, CancellationToken cancellationToken = default);
    Task DeadAdventurer(Guid adventurerId, CancellationToken cancellationToken = default);
}

public class AdventurerService(IAdventurerRepository adventurerRepository) : IAdventurerService
{
    public Task<Guid> CreateAdventurer(AdventurerDto adventurer, CancellationToken cancellationToken = default)
        => adventurerRepository.CreateAdventurer(adventurer, cancellationToken);

    public Task<List<AdventurerDto>> GetAdventurers(CancellationToken cancellationToken = default)
        => adventurerRepository.GetAdventurers(cancellationToken);

    public Task<AdventurerDto?> GetAdventurer(Guid adventurerId, CancellationToken cancellationToken = default)
        => adventurerRepository.GetAdventurer(adventurerId, cancellationToken);

    public Task DeadAdventurer(Guid adventurerId, CancellationToken cancellationToken = default)
        => adventurerRepository.DeadAdventurer(adventurerId, cancellationToken);
}
