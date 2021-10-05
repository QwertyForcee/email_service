using ApiClients;
using ApiClients.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using WepApp.Api.Auth;
using WepApp.Api.DataAccess;
using WepApp.Api.DataAccess.Repositories;
using WepApp.Api.DataAccess.Repositories.DapperImplementations;
using WepApp.Api.DataAccess.Repositories.Implementations;
using WepApp.Api.EmailManager;
using WepApp.Api.Entities;
using WepApp.Api.Services;

namespace WepApp.Api
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
            services.AddCors();
            services.AddControllers();

            var authOptionsConfiguration = Configuration.GetSection("Auth");
            services.Configure<AuthOptions>(authOptionsConfiguration);

            services.AddSingleton(new DbConfig { ConnectionString = Configuration["Default"] });
            services.Configure<EmailManagerConfig>(Configuration.GetSection("EmailManager"));

            services.AddScoped<IUserRepository<User>, UserRepository>();
            services.AddScoped<IQuoteTaskRepository<QuoteTask>, QuoteTaskRepository>();
            services.AddScoped<ICurrencyTaskRepository<CurrencyTask>, CurrencyTaskRepository>();
            services.AddScoped<ICoinTaskRepository<CoinTask>,CoinTaskRepository>();

            services.AddScoped<IEmailSender,EmailSender>();
            services.AddScoped<ICsvManager,CsvManager>();

            services.AddScoped<ICurrencyExchangeCaller, CurrencyExchangeCaller>();
            services.AddScoped<ICoinrankingCaller,CoinrankingCaller>();
            services.AddScoped<IRandomQuotesCaller,RandomQuotesCaller>();


            services.AddHostedService<CoinCronJobService>();
            services.AddHostedService<CurrencyCronJobService>();
            services.AddHostedService<QuoteCronJobService>();

            var authOptions = authOptionsConfiguration.Get<AuthOptions>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = authOptions.Audience,

                    ValidateLifetime = true,

                    IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
            });

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
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
