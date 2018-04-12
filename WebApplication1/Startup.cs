using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Swashbuckle;
using Swashbuckle.AspNetCore.Swagger;

namespace WebApplication1
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
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddDbContext<SISContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionString"],
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.
                        MigrationsAssembly(
                        typeof(Startup).
                        GetTypeInfo().
                        Assembly.
                        GetName().Name);

                        //Configuring Connection Resiliency:
                        sqlOptions.
                              EnableRetryOnFailure(maxRetryCount: 5,
                              maxRetryDelay: TimeSpan.FromSeconds(30),
                              errorNumbersToAdd: null);

                    });

                // Changing default behavior when client evaluation occurs to throw.
                // Default in EFCore would be to log warning when client evaluation is done.
                options.ConfigureWarnings(warnings => warnings.Throw(
                    RelationalEventId.QueryClientEvaluationWarning));
            });

            // Add framework services.
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Info
                {
                    Title = "eShopOnContainers - Catalog HTTP API",
                    Version = "v1",
                    Description = "The Catalog Microservice HTTP API. This is a Data-Driven/CRUD microservice sample",
                    TermsOfService = "Terms Of Service"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                }); ;

        }
    }
}
