using APICarsGQL.Data;
using APICarsGQL.Models;
using HotChocolate;
using HotChocolate.Data;
using System.Linq;

namespace APICarsGQL.GraphQL
{
    public class Query
    {
        [UseDbContext(typeof(AppDbContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<OwnersModel> GetOwners([ScopedService] AppDbContext context)
        {
            var data = context.Owners;

            return data;
        }
        [UseDbContext(typeof(AppDbContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<CarsModel> GetCars([ScopedService] AppDbContext context)
        {
            var data = context.Cars;

            return data;
        }
    }
}
