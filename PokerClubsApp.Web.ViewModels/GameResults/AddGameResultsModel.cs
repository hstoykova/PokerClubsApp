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

        [Required]
        [MinLength(PlayerNicknameMinLength)]
        [MaxLength(PlayerNicknameMaxLength)]
        public string Nickname { get; set; } = null!;

        [Required]
        public int ClubId { get; set; }

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
