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
using FamilijaApi.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

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
                    )
            );
            var key= Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);
            var tokenvalidationParans= new TokenValidationParameters{
                ValidateIssuerSigningKey= true,
                IssuerSigningKey= new SymmetricSecurityKey(key),
                ValidateIssuer= false,
                ValidateAudience=false,
                ValidateLifetime=false,
                RequireExpirationTime=true
            };
            services.AddAuthentication(options=>{
               options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
               options.DefaultScheme= JwtBearerDefaults.AuthenticationScheme;
               options.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt=>{
                jwt.SaveToken=true;
                jwt.TokenValidationParameters= tokenvalidationParans;
            });

            services.AddSingleton(tokenvalidationParans);
            
            services.AddDefaultIdentity<IdentityUser>(options=> options.SignIn.RequireConfirmedAccount=true)
                    .AddEntityFrameworkStores<FamilijaDbContext>();

            services.AddControllers().AddNewtonsoftJson(s => { 
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

           

            services.AddScoped<IUserRepo, SqlUserRepo>();
            services.AddScoped<IRoleRepo, SqlRoleRepo>();
            services.AddScoped<IAuthRepo, SqlAuthRepo>();
            

            services.AddScoped<IAdddressesRepo, SqlAddressRepo>();
            services.AddScoped<IContactRepo, SqlContactRepo>();
            services.AddScoped<IFinanceRepo, SqlFinanceRepo>();
            services.AddScoped<IPersonalInfoRepo, SqlPersonalInfoRepo>();
            services.AddScoped<IPasswordRepo, SqlPasswordRepo>();

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
