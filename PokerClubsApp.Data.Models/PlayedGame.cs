
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PokerClubsApp.Data.Models
{
    public class PlayedGame
    {
        [Key]
        public int Id { get; set; }

        public int GameTypeId { get; set; }

        [ForeignKey(nameof(GameTypeId))]

        public virtual GameType GameType { get; set; } = null!;

        public DateTime EndedAt { get; set; }
    }
}
