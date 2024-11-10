using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerClubsApp.Web.ViewModels.GameResults
{
    public class GameResultDetailsModel
    {
        public string UnionName { get; set; } = null!;

        public int PlayerAccountId { get; set; }

        public string Nickname { get; set; } = null!;

        public string ClubName { get; set; } = null!;

        public decimal Result { get; set; }

        public decimal Fee { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string GameType { get; set; } = null!;

    }
}
