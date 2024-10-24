
using System.ComponentModel.DataAnnotations.Schema;


namespace PokerClubsApp.Data.Models
{
    public class Club
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int UnionId { get; set; }

        [ForeignKey(nameof(UnionId))]
        public virtual Union Union { get; set; } = null!;
    }
}
