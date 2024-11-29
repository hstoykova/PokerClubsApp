using PokerClubsApp.Web.ViewModels.Invitations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerClubsApp.Services.Data.Interfaces
{
    public interface IInvitationService
    {
        Task<bool> CreateInvitationAsync(CreateInvitationModel model);
    }
}
