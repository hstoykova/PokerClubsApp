using PokerClubsApp.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PokerClubsApp.Common.EntityValidationConstants.Player;

namespace PokerClubsApp.Web.ViewModels.Game
{
    public class AddGameResultsModel
    {
        [Required]
        public int AccountId { get; set; }

        public List<Player> Players { get; set; } = new();

        [Required]
        public int ClubId { get; set; }

        public List<Club> Clubs { get; set; } = new();

        [Required]
        public decimal Result { get; set; }

        [Required]
        public decimal Fee { get; set; }

        [Required]
        public DateTime EndedAt { get; set; }

        [Required]
        public int GameTypeId { get; set; }

        public List<GameType> GameTypes { get; set; } = new();
    }
}
