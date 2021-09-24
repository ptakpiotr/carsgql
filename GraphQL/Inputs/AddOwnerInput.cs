using APICarsGQL.Data.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APICarsGQL.GraphQL.Inputs
{
    public class AddOwnerInput
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Gender]
        public string Gender { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
