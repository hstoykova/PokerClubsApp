using Microsoft.AspNetCore.Identity;
using PokerClubsApp.Data.Models;
using PokerClubsApp.Data.Repository.Interfaces;
using PokerClubsApp.Services.Data.Interfaces;
using PokerClubsApp.Web.ViewModels.Invitations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerClubsApp.Services.Data
{
    public class InvitationService : IInvitationService
    {
        private readonly IRepository<Player, int> playerRepository;
        private readonly IRepository<Membership, int> membershipRepository;
        private readonly UserManager<IdentityUser> userManager;

        public InvitationService(IRepository<Player, int> playerRepository, IRepository<Membership, int> membershipRepository, UserManager<IdentityUser> userManager)
        {
            this.playerRepository = playerRepository;
            this.membershipRepository = membershipRepository;
            this.userManager = userManager;
        }

        public async Task<bool> CreateInvitationAsync(CreateInvitationModel model)
        {
            var existingUser = await userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                throw new ArgumentException("User already invited");
            }

            var user = new IdentityUser()
            {
                Email = model.Email,
                UserName = model.Email
            };

            await userManager.CreateAsync(user);

            var player = new Player()
            {
                Nickname = model.Nickname,
                AccountId = int.Parse(model.AccountId),
                User = user
            };

            await playerRepository.AddAsync(player);

            var membership = new Membership() 
            { 
                Player = player,
                ClubId = model.ClubId
            };

            await membershipRepository.AddAsync(membership);

            return true;
        }
    }
}
