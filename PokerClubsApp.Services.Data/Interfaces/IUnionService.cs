using PokerClubsApp.Data.Models;
using PokerClubsApp.Web.ViewModels.Clubs;
using PokerClubsApp.Web.ViewModels.Unions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerClubsApp.Services.Data.Interfaces
{
    public interface IUnionService
    {
        Task<Union> CreateUnionAsync(CreateUnionModel model);

        Task<CreateUnionModel?> GetUnionForEditAsync(int id);

        Task<Union?> EditUnionAsync(CreateUnionModel model, int id);

        Task<IEnumerable<Union>> IndexGetAllUnionsAsync();
    }
}
