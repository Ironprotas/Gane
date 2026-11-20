using GaNe.Repositories;

namespace GaNe.Services;

public interface IExpeditionsService
{
    public Task CreateExpedition();
}

public class ExpeditionsService(ExpeditionRepository expeditionRepository) : IExpeditionsService
{
    public Task CreateExpedition()
    {
        return Task.CompletedTask;
    }
}