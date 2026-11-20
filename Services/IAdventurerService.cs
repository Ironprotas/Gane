using GaNe.Models.Dto;
using GaNe.Repositories;

namespace GaNe.Services;

public interface IAdventurerService
{
    Task<Guid> CreateAdventurer(AdventurerDto adventurer, CancellationToken cancellationToken = default);
    Task<List<AdventurerDto>> GetAdventurers(CancellationToken cancellationToken = default);
    
    Task DeadAdventurer(Guid adventurerId, CancellationToken cancellationToken = default);
}

public class AdventurerService(IAdventurerRepository adventurerRepository) : IAdventurerService
{
    public async Task<Guid> CreateAdventurer(AdventurerDto adventurer, CancellationToken cancellationToken = default)
    {
        var res = await adventurerRepository.CreateAdventurer(adventurer, cancellationToken);
        return res;
    }

    public async Task<List<AdventurerDto>> GetAdventurers(CancellationToken cancellationToken = default)
    {
        return await adventurerRepository.GetAdventurers(cancellationToken);
    }

    public Task DeadAdventurer(Guid adventurerId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }


}