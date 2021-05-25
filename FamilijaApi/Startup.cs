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
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;

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
            services.AddMvc();

            services.AddCors(options =>
            {
                options.AddPolicy("Policy1",
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:3000",
                                                          "http://www.contoso.com");
                                                           
                                  });
                                    options.AddPolicy("AnotherPolicy",
                                    builder =>
                                    {
                                        builder.WithOrigins("http://localhost:3000")
                                                            .AllowAnyHeader()
                                                            .AllowAnyMethod();
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FamilijaApi", Version = "v1" });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {securityScheme, new string[] { }}
                    });

                // add Basic Authentication
                //var basicSecurityScheme = new OpenApiSecurityScheme
                //{
                //    Type = SecuritySchemeType.Http,
                //    Scheme = "basic",
                //    Reference = new OpenApiReference { Id = "BasicAuth", Type = ReferenceType.SecurityScheme }
                //};
                //c.AddSecurityDefinition(basicSecurityScheme.Reference.Id, basicSecurityScheme);
                
                //c.AddSecurityRequirement(new OpenApiSecurityRequirement
                //    {
                //        {basicSecurityScheme, new string[] { }}
                //    });
            });

            services.AddSwaggerGenNewtonsoftSupport();

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
                RequireSignedTokens=true,
                ClockSkew=TimeSpan.Zero,
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
           

            app.UseSwagger();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "FamilijaApi V1");
            });
            app.UseEndpoints(endpoints =>
            {
            //    endpoints.MapGet("/echo",
            //    context => context.Response.WriteAsync("echo"))
            //    .RequireCors(MyAllowSpecificOrigins);

            //    endpoints.MapControllers()
            //             .RequireCors(MyAllowSpecificOrigins);

            //    endpoints.MapGet("/echo2",
            //        context => context.Response.WriteAsync("echo2"));


                endpoints.MapControllers();
            });
        }
    }
}
