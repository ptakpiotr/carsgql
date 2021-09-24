using APICarsGQL.Data;
using APICarsGQL.Models;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICarsGQL.GraphQL.Types
{
    public class OwnerType : ObjectType<OwnersModel>
    {
        protected override void Configure(IObjectTypeDescriptor<OwnersModel> descriptor)
        {
            descriptor.Description("Owners of the cars");

            descriptor.Authorize(new[] { "UserRole"});

            descriptor
                .Field(f => f.Cars)
                .ResolveWith<Resolvers>(r=>r.GetOwnersCars(default!,default!))
                .UseDbContext<AppDbContext>()
                .Description("Cars of the particular owner");
        }
    }
}
