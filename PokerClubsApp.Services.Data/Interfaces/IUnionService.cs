using PokerClubsApp.Data.Models;
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
    }
}
