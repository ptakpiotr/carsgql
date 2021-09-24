using APICarsGQL.Data;
using APICarsGQL.Models;
using HotChocolate.Types;

namespace APICarsGQL.GraphQL.Types
{
    public class CarType : ObjectType<CarsModel>
    {
        protected override void Configure(IObjectTypeDescriptor<CarsModel> descriptor)
        {
            descriptor.Description("All cars available in the API");

            descriptor
                .Field(f => f.Owner)
                .ResolveWith<Resolvers>(r => r.GetCarOwner(default!, default!))
                .UseDbContext<AppDbContext>()
                .Description("Owner of the particular car")
                .Authorize(new[] { "UserRole" });
        }
    }
}
