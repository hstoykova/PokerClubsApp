using PokerClubsApp.Data.Models;
using PokerClubsApp.Web.ViewModels.Clubs;
using PokerClubsApp.Web.ViewModels.GameResults;

namespace PokerClubsApp.Services.Data.Interfaces
{
    public interface IClubService
    {
        Task<IEnumerable<Club>> GetAllClubsAsync();

        Task<Club> CreateClubAsync(CreateClubModel model);

        Task<CreateClubModel?> GetClubForEditAsync(int id);

        Task<Club?> EditClubAsync(CreateClubModel model, int id);

        Task<IEnumerable<Club>> IndexGetAllClubsAsync();

        Task<Club?> GetClubDetailsAsync(int id);
    }
}
