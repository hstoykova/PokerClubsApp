using PokerClubsApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerClubsApp.Services.Data.Interfaces
{
    public interface IGameTypeService
    {
        Task<IEnumerable<GameType>> GetAllGameTypesAsync();
    }
}
