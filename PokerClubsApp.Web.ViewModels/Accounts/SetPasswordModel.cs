using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerClubsApp.Web.ViewModels.Accounts
{
    public class SetPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public string Token { get; set; } = null!;
    }
}
