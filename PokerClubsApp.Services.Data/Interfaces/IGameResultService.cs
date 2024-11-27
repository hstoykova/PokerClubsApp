

using Microsoft.AspNetCore.Mvc;
using PokerClubsApp.Data.Models;
using PokerClubsApp.Web.ViewModels.Game;
using PokerClubsApp.Web.ViewModels.GameResults;

namespace PokerClubsApp.Services.Data.Interfaces
{
    public interface IGameResultService
    {
        Task<int> AddGameResultAsync(AddGameResultsModel inputModel);

        Task<GameResult?> EditGameResultAsync(EditGameResultsModel model, int id);

        Task<bool> DeleteGameResultAsync(int id);

    }
}
