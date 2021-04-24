using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FamilijaApi.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using FailijaApi.Data;
using AutoMapper;
using System;
using FamilijaApi.Configuration;

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
            
            services.AddDbContext<FamilijaContext>(options => options.UseSqlServer (Configuration.GetConnectionString("FamilijaDB")));
            services.AddControllers().AddNewtonsoftJson(s => { 
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
                
            services.AddScoped<IUserRepo, SqlUserRepo>();
            services.AddScoped<IUserInfoRepo, SqlUserInfoRepo>();
            services.AddScoped<IRoleRepo, SqlRoleRepo>();
            

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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
