
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PokerClubsApp.Data.Models
{
    public class GameResult
    {
        [Key]
        public int Id { get; set; }

        public int MembershipId { get; set; }

        [ForeignKey(nameof(MembershipId))]
        public virtual Membership Membership { get; set; } = null!;

        public int GameTypeId { get; set; }

        [ForeignKey(nameof(GameTypeId))]
        public virtual GameType GameType { get; set; } = null!;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public decimal Result { get; set; }

        public decimal Fee { get; set; }

        public bool IsDeleted { get; set; }
    }
}
