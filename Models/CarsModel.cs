using APICarsGQL.Data.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APICarsGQL.Models
{
    public class CarsModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        [Year]
        public int Year { get; set; }

        [Required]
        public string VIN { get; set; }


        public int OwnerId { get; set; }
        public OwnersModel Owner { get; set; }
    }
}
