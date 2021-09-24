using APICarsGQL.Data;
using APICarsGQL.GraphQL.Inputs;
using APICarsGQL.GraphQL.Payloads;
using APICarsGQL.Models;
using AutoMapper;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace APICarsGQL.GraphQL
{
    public class Mutations
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public Mutations(IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _configuration = configuration;
        }

        [UseDbContext(typeof(AppDbContext))]
        public async Task<AddCarPayload> AddCarAsync(AddCarInput input, [ScopedService] AppDbContext context)
        {
            var md = _mapper.Map<CarsModel>(input);

            await context.Cars.AddAsync(md);
            await context.SaveChangesAsync();

            return new AddCarPayload(md);
        }

        [UseDbContext(typeof(AppDbContext))]
        public async Task<AddOwnerPayload> AddOwnerAsync(AddOwnerInput input, [ScopedService] AppDbContext context,
                                                            [Service] ITopicEventSender sender, CancellationToken token)
        {
            var md = _mapper.Map<OwnersModel>(input);

            await context.Owners.AddAsync(md);

            await context.SaveChangesAsync(token);

            await sender.SendAsync(nameof(Subscriptions.OnOwnerAdded), md, token);

            return new AddOwnerPayload(md);
        }

        [UseDbContext(typeof(AppDbContext))]
        public async Task<DeletePayload> DeleteOwnerAsync(DeleteInput input, [ScopedService] AppDbContext context)
        {
            var data = context.Owners.FirstOrDefault(o => o.Id == input.Id);

            if (data == null)
            {
                return new DeletePayload { Code = -DateTime.Now.Year };
            }

            await Task.Run(() => context.Owners.Remove(data));
            await context.SaveChangesAsync();

            return new DeletePayload { Code = -1 };
        }

        public async Task<DeletePayload> RegisterUserAsync(RegisterModel input,
            [Service] UserManager<IdentityUser> userManager, [Service] RoleManager<IdentityRole> roleManager)
        {
            if (input.Password == input.ConfirmPassword)
            {
                var user = new IdentityUser { UserName = input.Email, Email = input.Email };

                var res = await userManager.CreateAsync(user, input.Password);
                if (res.Succeeded)
                {
                    if (!await roleManager.RoleExistsAsync("UserRole"))
                    {
                        await roleManager.CreateAsync(new IdentityRole { Name = "UserRole" });
                    }

                    await userManager.AddToRoleAsync(user, "UserRole");

                    return new DeletePayload { Code = 1 };
                }
            }

            return new DeletePayload { Code = -DateTime.Now.Year };

        }

        public async Task<LoginPayload> LoginUserAsync(LoginModel input,
            [Service] UserManager<IdentityUser> userManager, [Service] RoleManager<IdentityRole> roleManager,
            [Service] EmailSender sender)
        {
            var user = await userManager.FindByEmailAsync(input.Email);

            if (user == null)
            {
                return new LoginPayload { Message = "" };
            }

            if (await userManager.CheckPasswordAsync(user, input.Password))
            {

                var userRoles = await userManager.GetRolesAsync(user);
                var newClaims = new List<Claim>() { new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti,user.SecurityStamp) };

                List<Claim> allClaims = new();
                allClaims.AddRange(newClaims);

                foreach (var role in userRoles)
                {
                    allClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: allClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                var writtenToken = new JwtSecurityTokenHandler().WriteToken(token);

                await sender.SendEmailAsync(user.Email, "CarsGQLApi JWT Token", writtenToken);

                return new LoginPayload { Message = writtenToken };
            }

            return new LoginPayload { Message = "" };
        }
    }
}
