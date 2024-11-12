using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PokerClubsApp.Common.EntityValidationConstants.Player;

namespace PokerClubsApp.Data.Models
{
    public class Player
    {
        [Key]
        public int AccountId { get; set; }

        [Required]
        [MaxLength(PlayerNicknameMaxLength)] 
        public string Nickname { get; set; } = null!;

        public virtual List<Membership> Memberships { get; set; } = new();

        public bool IsDeleted { get; set; }
    }
}
