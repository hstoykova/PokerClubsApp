
using System.ComponentModel.DataAnnotations;
using static PokerClubsApp.Common.EntityValidationConstants.GameType;

namespace PokerClubsApp.Data.Models
{
    public class GameType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GameTypeNameMaxLength)]
        public string Name { get; set; } = null!;
    }
}
