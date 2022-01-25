using MovieConnect.Website.Models;
using MovieConnect.Website.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieConnect.Website.Services
{
    public interface IProducersService : IEntityBaseRepository<Producer>
    {
    }
}
