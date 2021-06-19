using DatCar.Core.Contracts;
using DatCar.PricingEngine.Contracts;
using DatCar.PricingEngine.Models;
using DatCar.PricingEngine.Rules.AdditionalChargesRules;
using DatCar.PricingEngine.Rules.DailyPricingRules;
using DatCar.PricingEngine.Rules.DiscountRules;
using DatCar.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatCar.PricingAPI
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
            services.AddControllers().AddNewtonsoftJson();

            //register repositories
            services.AddSingleton<ICarRepository, CarRepository>();

            //register services
            services.AddSingleton<IRentalService, RentalService>();

            //register pricing rules
            services
                .AddSingleton<IPricingRule<DailyPricingRule>, WeekendSurchargeRule>()
                .AddSingleton<IPricingRule<AdditionalChargeRule>, InsuranceChargeRule>()
                .AddSingleton<IPricingRule<AdditionalChargeRule>, SnappCarChargeRule>()
                .AddSingleton<IPricingRule<DiscountRule>, LongRentalDiscountRule>();

            //register pricing engines
            services.AddSingleton<IPricingEngine, PricingEngine.Engines.PricingEngine>();
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
