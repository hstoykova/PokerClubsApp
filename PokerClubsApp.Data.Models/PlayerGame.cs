
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PokerClubsApp.Data.Models
{
    public class PlayerGame
    {
        [Key]
        public int Id { get; set; }

        public int PlayerAccountId { get; set; }

        [ForeignKey(nameof(PlayerAccountId))]
        public virtual Player Player { get; set; } = null!;

        public int GameTypeId { get; set; }

        [ForeignKey(nameof(GameTypeId))]
        public virtual GameType GameType { get; set; } = null!;

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public decimal Result { get; set; }

        public decimal Fee { get; set; }
    }
}
