using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly IEmailSender emailSender;
        private readonly IHttpContextAccessor httpContextAccessor;

        public InvitationService(IRepository<Player, int> playerRepository, IRepository<Membership, int> membershipRepository, 
            UserManager<IdentityUser> userManager, IEmailSender emailSender, IHttpContextAccessor httpContextAccessor)
        {
            this.playerRepository = playerRepository;
            this.membershipRepository = membershipRepository;
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.httpContextAccessor = httpContextAccessor;
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

            var token = await userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, "InviteUser");
            var encodedToken = Uri.EscapeDataString(token);
            var host = httpContextAccessor.HttpContext?.Request.Host;

            if (host == null)
            {
                throw new ArgumentException("Host is missing");
            }

            var url = $"{host}/Players/FinishRegistration?userId={user.Id}&token={token}";

            // For debugging purposes
            Console.WriteLine(url);

            var subject = "You are invited to PokerClubsApp";
            var message = $"Please click <a href=\"{url}\"> here</a> to finish your registration";

            await emailSender.SendEmailAsync(user.Email, subject, message);

            return true;
        }
    }
}
