﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PokerClubsApp.Data.Models
{
    public class PlayerClub
    {
        public int PlayerAccountId { get; set; }

        [ForeignKey(nameof(PlayerAccountId))]
        public virtual Player Player { get; set; } = null!;

        public int ClubId { get; set; }

        [ForeignKey(nameof(ClubId))]
        public virtual Club Club { get; set; } = null!;
    }
}
