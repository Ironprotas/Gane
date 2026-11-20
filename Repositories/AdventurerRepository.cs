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
                              INSERT INTO adventurers (id, expedition_id, name, archetype, sex, is_live, rating)
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

        await using var connect = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = connect.CreateCommand();

        command.CommandText = """
                              SELECT id, name, expedition_id, archetype, sex, is_live, rating
                              FROM adventurers
                              """;
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            res.Add(MapAdventurer(reader));
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
                              SELECT id, name, expedition_id, archetype, sex, is_live, rating
                              FROM adventurers
                              WHERE id = $1;
                              """;
        command.Parameters.AddWithValue(adventurerId);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        return MapAdventurer(reader);
    }

    private static AdventurerDto MapAdventurer(NpgsqlDataReader reader) => new()
    {
        Id = reader.GetGuid(0),
        Name = reader.GetString(1),
        ExpeditionId = reader.IsDBNull(2)
            ? null
            : reader.GetGuid(2),
        Archetype = (AdventurerClassEnum)reader.GetInt32(3),
        Sex = (SexEnum)reader.GetInt32(4),
        IsLive = reader.GetBoolean(5),
        Rating = reader.GetInt32(6)
    };
}
