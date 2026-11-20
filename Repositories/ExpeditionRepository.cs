using Npgsql;

namespace GaNe.Repositories;

public interface IExpeditionsRepository
{
    Task CreateExpedition();
}

public class ExpeditionRepository(NpgsqlDataSource dataSource) : IExpeditionsRepository
{
    public Task CreateExpedition()
    {
        throw new NotImplementedException();
    }
}