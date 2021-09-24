using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APICarsGQL.Data.Validation
{
    public class GenderAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string g = value as string;

            return g.Equals("Male") || g.Equals("Female");
        }
    }
}
