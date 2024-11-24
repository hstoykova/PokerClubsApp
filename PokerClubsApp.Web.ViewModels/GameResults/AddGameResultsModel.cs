using Itenso.TimePeriod;
using PokerClubsApp.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PokerClubsApp.Common.EntityValidationConstants.GameResult;

namespace PokerClubsApp.Web.ViewModels.Game
{
    public class AddGameResultsModel
    {
        [Required]
        public int PlayerId { get; set; }

        public List<Player> Players { get; set; } = new();

        [Required]
        public int ClubId { get; set; }

        public List<Club> Clubs { get; set; } = new();

        [Required]
        [Range(ResultMinValue, ResultsMaxValue)]
        public decimal Result { get; set; }

        [Required]
        [Range(FeeMinValue, FeeMaxValue)]
        public decimal Fee { get; set; }

        [Required]
        public string FromDate { get; set; } = DateTime.Today.ToString(FromDateFormat);

        [Required]
        public string ToDate { get; set; } = DateTime.Today.ToString(ToDateFormat);

        [Required]
        public int GameTypeId { get; set; }

        public List<GameType> GameTypes { get; set; } = new();

        [Required]
        public string Week { get; set; } = null!;

        public List<Week> Weeks { get; set; } = new();
    }
}
