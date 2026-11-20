using GaNe.Enums;
using GaNe.Models.Dto;
using Npgsql;

namespace GaNe.Repositories;

public interface IAdventurerRepository
{
    Task<Guid> CreateAdventurer(AdventurerDto adventurer, CancellationToken cancellationToken = default);

    Task<List<AdventurerDto>> GetAdventurers(CancellationToken cancellationToken = default);
    
    Task DeadAdventurer(Guid adventurerId, CancellationToken cancellationToken = default);
    
    Task<AdventurerDto?> GetAdventurer(Guid adventurerId, CancellationToken cancellationToken = default);
}

public class AdventurerRepository(NpgsqlDataSource dataSource) : IAdventurerRepository
{
    public async Task<Guid> CreateAdventurer(AdventurerDto adventurer, CancellationToken cancellationToken = default)
    {
        var id = Guid.NewGuid();
        await using var connect = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = connect.CreateCommand();

        command.CommandText = """
                              INSERT  INTO adventurers (id, expedition_id, name, archetype, sex, is_live, rating)
                                VALUES ($1, $2, $3, $4, $5, $6, $7);
                              """;

        command.Parameters.AddWithValue(id);
        command.Parameters.AddWithValue(
            NpgsqlTypes.NpgsqlDbType.Uuid,
            (object?)adventurer.ExpeditionId ?? DBNull.Value);
        command.Parameters.AddWithValue(adventurer.Name);
        command.Parameters.AddWithValue((int)adventurer.Archetype);
        command.Parameters.AddWithValue((int)adventurer.Sex);
        command.Parameters.AddWithValue(adventurer.IsLive);
        command.Parameters.AddWithValue(adventurer.Rating);

        await command.ExecuteNonQueryAsync(cancellationToken);
        return id;
    }

    public async Task<List<AdventurerDto>> GetAdventurers(
        CancellationToken cancellationToken = default)
    {
        var res = new List<AdventurerDto>();

        await using var connect =
            await dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = connect.CreateCommand();

        command.CommandText = """
                              SELECT name, expedition_id, archetype, sex, is_live, rating
                              FROM  adventurers
                              """;
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            var adv = new AdventurerDto
            {
                Name = reader.GetString(0),

                ExpeditionId = reader.IsDBNull(1)
                    ? (Guid?)null
                    : reader.GetGuid(1),

                Archetype = (AdventurerClassEnum)reader.GetInt32(2),
                Sex = (SexEnum)reader.GetInt32(3),
                IsLive = reader.GetBoolean(4),
                Rating = reader.GetInt32(5)
            };

            res.Add(adv);
        }

        return res;
    }

    public async Task DeadAdventurer(Guid adventurerId, CancellationToken cancellationToken = default)
    {
        await using var connect = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = connect.CreateCommand();
        command.CommandText = """
                              UPDATE adventurers
                              SET is_live = FALSE
                              WHERE id = $1;
                              """;
        command.Parameters.AddWithValue(adventurerId);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<AdventurerDto?> GetAdventurer(Guid adventurerId, CancellationToken cancellationToken = default)
    {

        await using var connect = await dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = connect.CreateCommand();

        command.CommandText = """
                              SELECT name, expedition_id, archetype, sex, is_live, rating
                              FROM  adventurers
                              WHERE id = $1;
                              """;
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        
        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }
        
        return new AdventurerDto()
        {
            Name = reader.GetString(0),

            ExpeditionId = reader.IsDBNull(1)
                ? (Guid?)null
                : reader.GetGuid(1),

            Archetype = (AdventurerClassEnum)reader.GetInt32(2),
            Sex = (SexEnum)reader.GetInt32(3),
            IsLive = reader.GetBoolean(4),
            Rating = reader.GetInt32(5)
        };
    }

}