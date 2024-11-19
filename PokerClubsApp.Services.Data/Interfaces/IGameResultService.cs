

using Microsoft.AspNetCore.Mvc;
using PokerClubsApp.Web.ViewModels.Game;

namespace PokerClubsApp.Services.Data.Interfaces
{
    public interface IGameResultService
    {
        Task<int> AddGameResultAsync(AddGameResultsModel inputModel);

    }
}
