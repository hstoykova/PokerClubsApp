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

        [Required]
        public int ClubId { get; set; }

        [ForeignKey(nameof(ClubId))]

        public virtual Club Club { get; set; } = null!;
    }
}
