﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerClubsApp.Data.Repository.Interfaces
{
    public interface IRepository<TType, TId>
    {
        Task<IEnumerable<TType>> GetAllAsync();

        IQueryable<TType> GetAllAttached();
    }
}