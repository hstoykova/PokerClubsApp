using PokerClubsApp.Data.Models;
using PokerClubsApp.Web.ViewModels.Clubs;

namespace PokerClubsApp.Services.Data.Interfaces
{
    public interface IClubService
    {
        Task<IEnumerable<Club>> GetAllClubsAsync();

        Task<Club> CreateClubAsync(AddClubModel model);
    }
}
