using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using TestApi.Interfaces;
using TestApi.Services;

namespace TestApi
{
    public class Startup
    {

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Add Middleware
            services.AddCors();
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });
    
            services.AddHttpContextAccessor();
            services.AddMemoryCache();
            #endregion
            #region JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
             )
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateActor = true,
                     ValidateAudience = false,
                     ValidateIssuer = false,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(
                         Encoding.UTF8.GetBytes("SwaggerClone"))
                 };

                 options.Events = new JwtBearerEvents
                 {
                     OnTokenValidated = context =>
                     {
                         return Task.CompletedTask;
                     },
                     OnAuthenticationFailed = context =>
                     {
                         Console.WriteLine("Exception:{0}", context.Exception.Message);
                         return Task.CompletedTask;
                     }
                 };
             });
            #endregion
            #region Header Configuration
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            #endregion
            #region Swagger            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("SwaggerClone", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "SwaggerClone",
                    Description = "Swagger Clone Web Api Version 1",
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                       {
                         new OpenApiSecurityScheme
                         {
                           Reference = new OpenApiReference
                           {
                             Type = ReferenceType.SecurityScheme,
                             Id = "Bearer"
                           }
                          },
                          new string[] { }
                        }
                      });

                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile));
            });
            #endregion
            #region DI
            services.AddScoped<IDocument, SDocument>();
            services.AddScoped<IMethod, SMethod>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            #region Use Middleware
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(x => x
                           .AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader());
            app.UseSwagger()
            .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/SwaggerClone/swagger.json", "Swagger Clone");
            });
            app.UseMvc();
            #endregion
        }
    }
}
