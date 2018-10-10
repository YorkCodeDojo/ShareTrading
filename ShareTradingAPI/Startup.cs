using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace ShareTradingAPI
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddSingleton(new DataAccess.SQLServer.SQLServerDatabaseConnection(connectionString));

            services.AddTransient<DataAccess.ICreateAccountAction, DataAccess.SQLServer.CreateAccountAction>();
            services.AddTransient<DataAccess.ICreateTransactionAction, DataAccess.SQLServer.CreateTransactionAction>();
            services.AddTransient<DataAccess.IProductsQuery, DataAccess.ProductsQuery>();
            services.AddTransient<DataAccess.ICurrentPriceQuery, DataAccess.CurrentPriceQuery>();
            services.AddTransient<DataAccess.IAccountQuery, DataAccess.SQLServer.AccountQuery>();
            services.AddTransient<DataAccess.IAccountByNameQuery, DataAccess.SQLServer.AccountByNameQuery>();
            services.AddTransient<DataAccess.ITransactionsForAccountQuery, DataAccess.SQLServer.TransactionsForAccountQuery>();
            services.AddTransient<Pricing.ITimeCalculator, Pricing.DojoTimeCalculator>();

            services.AddSingleton<Pricing.ProductA>();
            services.AddSingleton<Pricing.ProductB>();
            services.AddSingleton<Pricing.ProductC>();
            services.AddSingleton<Pricing.ProductD>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "ShareTrading API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShareTrading API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
