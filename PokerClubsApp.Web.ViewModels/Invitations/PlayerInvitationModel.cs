using PokerClubsApp.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PokerClubsApp.Common.EntityValidationConstants.Invitation;

namespace PokerClubsApp.Web.ViewModels.Invitations
{
    public class PlayerInvitationModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(NicknameMinLength)]
        [MaxLength(NicknameMaxLength)]
        public string Nickname { get; set; } = null!;

        [Required]
        [MinLength(AccountIdMinLength)]
        [MaxLength(AccountIdMaxLength)]
        public string AccountId { get; set; } = null!;

        [Required]
        public int ClubId { get; set; }

        public List<Club> Clubs { get; set; } = new();
    }
}
