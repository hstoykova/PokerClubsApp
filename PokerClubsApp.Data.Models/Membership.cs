using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PokerClubsApp.Data.Models
{
    public class Membership
    {
        [Key]
        public int Id { get; set; }

        public int PlayerId { get; set; }

        [ForeignKey(nameof(PlayerId))]
        public virtual Player Player { get; set; } = null!;

        public int ClubId { get; set; }

        [ForeignKey(nameof(ClubId))]
        public virtual Club Club { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public virtual IEnumerable<GameResult> GameResults { get; set; } = new List<GameResult>();
    }
}
