using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FamilijaApi.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using FailijaApi.Data;
using System;
using FamilijaApi.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;

namespace FamilijaApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Jwtconfig>(Configuration.GetSection("Jwtconfig"));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<FamilijaDbContext>(options => 
                options.UseSqlServer(
                    Configuration.GetConnectionString("FamilijaDB")
                    ));

            services.AddAuthentication(options=>{
               options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
               options.DefaultScheme= JwtBearerDefaults.AuthenticationScheme;
               options.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt=>{
                var key= Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);

                jwt.SaveToken=true;
                jwt.TokenValidationParameters= new TokenValidationParameters{
                    ValidateIssuerSigningKey= true,
                    IssuerSigningKey= new SymmetricSecurityKey(key),
                    ValidateIssuer= false,
                    ValidateAudience=false,
                    ValidateLifetime=true,
                    RequireExpirationTime=false
                };
            });
            services.AddDefaultIdentity<IdentityUser>(options=> options.SignIn.RequireConfirmedAccount=true)
                    .AddEntityFrameworkStores<FamilijaDbContext>();

            services.AddControllers().AddNewtonsoftJson(s => { 
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            services.AddScoped<IUserRepo, SqlUserRepo>();
            services.AddScoped<IRoleRepo, SqlRoleRepo>();
            services.AddScoped<IUserRepoTemp, SqlUserRepoTemp>();

            services.AddScoped<IAdddressesRepo, SqlAddressRepo>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
