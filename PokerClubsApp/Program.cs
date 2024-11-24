using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PokerClubsApp.Data;
using PokerClubsApp.Data.Models;
using PokerClubsApp.Data.Repository;
using PokerClubsApp.Data.Repository.Interfaces;
using PokerClubsApp.Services.Data;
using PokerClubsApp.Services.Data.Interfaces;
using PokerClubsApp.Web.Infrastructure.Extensions;



namespace PokerClubsApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<PokerClubsDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<PokerClubsDbContext>();

            //builder.Services.RegisterRepositories(typeof(ApplicationUser).Assembly);
            //builder.Services.RegisterUserDefinedServices(typeof(IGameResultService).Assembly);

            builder.Services.AddScoped<IRepository<Club, int>, BaseRepository<Club, int>>();
            builder.Services.AddScoped<IRepository<Player, int>, BaseRepository<Player, int>>();
            builder.Services.AddScoped<IRepository<GameType, int>, BaseRepository<GameType, int>>();
            builder.Services.AddScoped<IRepository<Membership, int>, BaseRepository<Membership, int>>();
            builder.Services.AddScoped<IRepository<GameResult, int>, BaseRepository<GameResult, int>>();
            builder.Services.AddScoped<IGameResultService, GameResultService>();

            builder.Services.AddTransient<IRolesSeedService, RolesSeedService>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            //Seed roles and users
            using (var scope = app.Services.CreateScope())
            {
                var rolesSeeder = scope.ServiceProvider.GetRequiredService<IRolesSeedService>();

                await rolesSeeder.SeedRolesAsync();
            }

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseMigrationsEndPoint();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
