
using System.ComponentModel.DataAnnotations.Schema;


namespace PokerClubsApp.Data.Models
{
    public class PlayerGame
    {
        public int Id { get; set; }

        public int PlayerAccountId { get; set; }

        [ForeignKey(nameof(PlayerAccountId))]
        public virtual Player Player { get; set; } = null!;

        public int GameId { get; set; }

        [ForeignKey(nameof(GameId))]
        public virtual PlayedGame PlayedGame { get; set; } = null!;

        public decimal Result { get; set; }

        public decimal Fee { get; set; }
    }
}
