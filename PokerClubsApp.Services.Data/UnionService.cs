using PokerClubsApp.Data.Models;
using PokerClubsApp.Data.Repository.Interfaces;
using PokerClubsApp.Services.Data.Interfaces;
using PokerClubsApp.Web.ViewModels.Clubs;
using PokerClubsApp.Web.ViewModels.Unions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerClubsApp.Services.Data
{
    public class UnionService : IUnionService
    {
        private readonly IRepository<Union, int> unionRepository;

        public UnionService(IRepository<Union, int> unionRepository)
        {
            this.unionRepository = unionRepository;
        }

        public async Task<Union> CreateUnionAsync(CreateUnionModel model)
        {
            Union union = new Union() 
            { 
                Name = model.Name
            };

            await unionRepository.AddAsync(union);

            return union;
        }
    }
}
