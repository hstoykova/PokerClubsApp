using System.ComponentModel.DataAnnotations;
using static PokerClubsApp.Common.EntityValidationConstants.Union;

namespace PokerClubsApp.Data.Models
{
    public class Union
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(UnionNameMaxLength)]
        public string Name { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
