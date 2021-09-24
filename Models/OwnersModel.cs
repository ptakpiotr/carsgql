using APICarsGQL.Data.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APICarsGQL.Models
{
    public class OwnersModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Gender]
        public string Gender { get; set; }

        [Required]
        public string Country { get; set; }

        public ICollection<CarsModel> Cars { get; set; }
    }
}
