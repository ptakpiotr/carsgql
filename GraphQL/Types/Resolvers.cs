using APICarsGQL.Data;
using APICarsGQL.Models;
using HotChocolate;
using HotChocolate.Data;
using System.Collections.Generic;
using System.Linq;

namespace APICarsGQL.GraphQL.Types
{
    public class Resolvers
    {
        public OwnersModel GetCarOwner(CarsModel md, [ScopedService] AppDbContext context)
        {
            var owner = context.Owners.Where(o => o.Id == md.OwnerId).FirstOrDefault();

            return owner;
        }

        public List<CarsModel> GetOwnersCars(OwnersModel md, [ScopedService] AppDbContext context)
        {
            var cars = context.Cars.Where(c => c.OwnerId == md.Id).ToList();

            return cars;
        }
    }
}
