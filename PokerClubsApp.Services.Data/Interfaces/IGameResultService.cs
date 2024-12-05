

using Itenso.TimePeriod;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PokerClubsApp.Data.Models;
using PokerClubsApp.Web.ViewModels.Game;
using PokerClubsApp.Web.ViewModels.GameResults;

namespace PokerClubsApp.Services.Data.Interfaces
{
    public interface IGameResultService
    {
        Task<int> CreateGameResultAsync(CreateGameResultsModel inputModel);

        Task<EditGameResultsModel?> GetGameResultForEditAsync(int id); 

        Task<GameResult?> EditGameResultAsync(EditGameResultsModel model, int id);

        Task<bool> DeleteGameResultAsync(int id);

        Task<DetailsGameResultModel?> GetGameResultsDetailsAsync(int id);

		Task<IndexGameResultsModel> IndexGetAllGameResultsAsync(Week? week, int? clubId);

		Task<IndexGameResultsModel> IndexGetAllGameResultsAsync(string user, Week? week, int? clubId);
	}
}
