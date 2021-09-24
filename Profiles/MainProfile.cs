using APICarsGQL.GraphQL.Inputs;
using APICarsGQL.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICarsGQL.Profiles
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            CreateMap<AddCarInput, CarsModel>();

            CreateMap<AddOwnerInput, OwnersModel>();
        }
    }
}
