using PokerClubsApp.Web.ViewModels.Accounts;

namespace PokerClubsApp.Services.Data.Interfaces
{
    public interface IAccountService
    {
        Task ResetPasswordAsync(SetPasswordModel model);
    }
}
