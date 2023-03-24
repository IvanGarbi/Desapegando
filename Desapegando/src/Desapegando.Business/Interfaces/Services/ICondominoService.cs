using Desapegando.Business.Models;

namespace Desapegando.Business.Interfaces.Services;

public interface ICondominoService : IService
{
    Task Create(Condomino condomino);
    Task Update(Condomino condomino);
    Task Delete(Guid id);
}