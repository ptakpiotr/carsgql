using APICarsGQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICarsGQL.GraphQL.Payloads
{
    public record AddOwnerPayload(OwnersModel output);
}
