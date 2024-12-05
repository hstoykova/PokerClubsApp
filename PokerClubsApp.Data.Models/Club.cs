using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PokerClubsApp.Common.EntityValidationConstants.Club;


namespace PokerClubsApp.Data.Models
{
    public class Club
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ClubNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        public int UnionId { get; set; }

        [ForeignKey(nameof(UnionId))]
        public virtual Union Union { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public virtual IEnumerable<Membership> Memberships { get; set; } = new List<Membership>();
    }
}
