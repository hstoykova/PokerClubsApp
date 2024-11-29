using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PokerClubsApp.Common.EntityValidationConstants.Player;

namespace PokerClubsApp.Data.Models
{
    public class Player
    {
        [Key]
        public int Id { get; set; }

        public int AccountId { get; set; }

        [Required]
        [MaxLength(PlayerNicknameMaxLength)] 
        public string Nickname { get; set; } = null!;

        public virtual List<Membership> Memberships { get; set; } = new();

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public virtual IdentityUser User { get; set; } = null!;

        [Required]
        public bool IsDeleted { get; set; }
    }
}
