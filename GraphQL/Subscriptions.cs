using APICarsGQL.Models;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;

namespace APICarsGQL.GraphQL
{
    public class Subscriptions
    {
        [Subscribe]
        [Topic]
        [Authorize]
        public OwnersModel OnOwnerAdded([EventMessage] OwnersModel model)
        {
            return model;
        }
    }
}
