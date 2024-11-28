using PokerClubsApp.Data.Models;
using System.ComponentModel.DataAnnotations;
using static PokerClubsApp.Common.EntityValidationConstants.Club;

namespace PokerClubsApp.Web.ViewModels.Clubs
{
    public class CreateClubModel
    {
        [Required]
        [MinLength(ClubNameMinLength)]
        [MaxLength(ClubNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        public int UnionId { get; set; }

        public List<Union> Unions { get; set; } = new();
    }
}
