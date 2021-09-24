using System;
using System.ComponentModel.DataAnnotations;

namespace APICarsGQL.Data.Validation
{
    public class YearAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int y = int.Parse(value as string);

            return y >= 1900 && y <= DateTime.Now.Year;
        }
    }
}
