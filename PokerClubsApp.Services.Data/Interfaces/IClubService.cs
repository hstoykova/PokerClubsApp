using PokerClubsApp.Data.Models;

namespace PokerClubsApp.Services.Data.Interfaces
{
    public interface IClubService
    {
        Task<IEnumerable<Club>> GetAllClubsAsync();
    }
}
