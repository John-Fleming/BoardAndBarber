using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardAndBarber.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace BoardAndBarber
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
            services.AddControllers();

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddTransient<CustomerRepository>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.IncludeErrorDetails = true;
                    opt.Authority = "https://securetoken.google.com/fish-store-a71e6";
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidAudience = "fish-store-a71e6",
                        ValidIssuer = "https://securetoken.google.com/fish-store-a71e6"
                    };
                });

            //services.AddTransient<>();
            //services.AddSingleton<>();
            //services.AddScoped<>();
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

            app.UseCors(policy => policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
