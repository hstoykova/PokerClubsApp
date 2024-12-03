using Itenso.TimePeriod;
using PokerClubsApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerClubsApp.Web.ViewModels.GameResults
{
    public class IndexGameResultsModel
    {
        public int ClubId { get; set; }

        public IEnumerable<Club> Clubs { get; set; } = new List<Club>();

        public string Week { get; set; } = null!;

        public IEnumerable<Week> Weeks { get; set; } = new List<Week>();

        public IEnumerable<DetailsGameResultModel> GameResults { get; set; } = new List<DetailsGameResultModel>();
    }
}
