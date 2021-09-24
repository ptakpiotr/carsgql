using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICarsGQL.Data
{
    public class LoginDbContext : IdentityDbContext
    {
        public LoginDbContext(DbContextOptions<LoginDbContext> opts):base(opts)
        {

        }
    }
}
