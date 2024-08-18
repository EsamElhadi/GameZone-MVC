using CrudOpeartion.Models;
using CrudOpeartion.ViewModels;

namespace CrudOpeartion.Services;

public interface IGameService
{
    Game? GetGameById(int id);

    IEnumerable<Game> GetAllGames();

    Task Create(CreateGameFormViewModel model);

    Task<Game?> Update(EditGameFormViewModel model);

    bool Delete(int id);
}
