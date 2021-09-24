using APICarsGQL.Data;
using APICarsGQL.GraphQL;
using APICarsGQL.GraphQL.Types;
using APICarsGQL.Profiles;
using GraphQL.Server.Ui.Voyager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

namespace APICarsGQL
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(opts =>
            {
                opts.AddPolicy("NotRestrictedPolicy", policyOpts =>
                {
                    policyOpts.AllowAnyOrigin().AllowAnyHeader().WithMethods("POST");
                });
            });

            services.AddDbContext<LoginDbContext>(opts =>
            {
                opts.UseNpgsql(Configuration.GetConnectionString("IdentityConn"));
            }).AddIdentity<IdentityUser, IdentityRole>()
              .AddRoles<IdentityRole>()
              .AddEntityFrameworkStores<LoginDbContext>()
              .AddDefaultTokenProviders();

            services.AddPooledDbContextFactory<AppDbContext>(opts =>
            {
                opts.UseNpgsql(Configuration.GetConnectionString("MainConn"));
            });

            services.AddGraphQLServer()
                    .AddAuthorization()
                    .AddQueryType<Query>()
                    .AddType<OwnerType>()
                    .AddType<CarType>()
                    .AddMutationType<Mutations>()
                    .AddSorting()
                    .AddFiltering()
                    .AddSubscriptionType<Subscriptions>()
                    .AddInMemorySubscriptions()
                    .ModifyRequestOptions(opts =>
                    {
                        opts.IncludeExceptionDetails = _env.IsDevelopment();
                    });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                     new TokenValidationParameters
                     {
                         ValidIssuer = Configuration.GetSection("JWT:ValidIssuer").Value,
                         ValidAudience = Configuration.GetSection("JWT:ValidAudience").Value,
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JWT:SecretKey").Value))
                     };
            });

            services.AddAuthorization();

            services.AddAutoMapper(typeof(MainProfile));

            services.AddSingleton<EmailSender>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("NotRestrictedPolicy");

            app.UseWebSockets();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseSerilogRequestLogging();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
                endpoints.MapGraphQLVoyager(new VoyagerOptions()
                {
                    GraphQLEndPoint = "/graphql"
                });
            });
        }
    }
}
