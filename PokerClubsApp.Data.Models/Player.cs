using System.ComponentModel.DataAnnotations.Schema;

namespace PokerClubsApp.Data.Models
{
    public class Player
    {
        public int AccountId { get; set; }
        public string Nickname { get; set; } = null!;

        public int ClubId { get; set; }

        [ForeignKey(nameof(ClubId))]
        public virtual Club Club { get; set; } = null!;
    }
}
