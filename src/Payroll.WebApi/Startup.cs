using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Payroll.Application;
using Payroll.Infrastructure;
using Payroll.Infrastructure.Persistence;
using Payroll.Infrastructure.Persistence.Paycycles;

namespace Payroll.WebApi
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
            services.AddHttpClient();
            
            services.AddCors(options =>
            {
                options.AddPolicy("AnyOrigin", builder =>
                    {
                        builder.WithOrigins("https://dev2.marine-orchestrator.com")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });
            
            services.AddApplication();
            services.AddInfrastructure(Configuration);
            
            services.AddHealthChecks()
                .AddDbContextCheck<EmployeesContext>();
            
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme
                        = AuthenticationSchemeConstants.TokenValidationScheme;
                })
                .AddScheme<TokenValidationSchemeOptions, TokenValidationHandler>
                    (AuthenticationSchemeConstants.TokenValidationScheme, op => { });

            
            services.AddControllers()
                .AddNewtonsoftJson(opts =>
                {
                    opts.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    opts.SerializerSettings.Converters.Add(new StringEnumConverter());
                    opts.SerializerSettings.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd" });
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Payroll.WebApi", Version = "v1" });
                
                OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri("https://dev2.marine-orchestrator.com/openam/oauth2/realms/root/realms/marcura/access_token", UriKind.Absolute),
                            Scopes = new Dictionary<string, string>
                            {
                                {"employees:read", "Employees API Read"},
                                {"employees:write", "Employees API Write"},
                                {"payroll:read", "Payroll API Read"},
                                {"payroll:write", "Payroll API Write"},
                            }
                        }
                    }
                };

                c.AddSecurityDefinition("oauth2", securityScheme);
                
                var basePath = AppContext.BaseDirectory;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                c.IncludeXmlComments(Path.Combine(basePath, fileName));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, EmployeesContext employeesContext, PaycyclesContext paycyclesContext)
        {
            // migrate any database changes on startup (includes initial db creation)
            employeesContext.Database.Migrate();
            paycyclesContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payroll.WebApi v1");
                });
            }

            
            app.UseRouting();

            app.UseCors("AnyOrigin");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
