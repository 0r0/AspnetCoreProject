using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspnetCoreProject.Services;
using AspnetCoreProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AspnetCoreProject
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
            services.AddControllersWithViews();
            services.AddSingleton<IMyService, MyService>();
            services.AddDbContext<EmployeeProjectContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("EmployeeProject")));
            services.AddSession();
            string distributed = Configuration["Distributed"];
            switch (distributed)
            {
                case "MEMORY":
                    services.AddDistributedMemoryCache();
                    services.AddSession(options =>
                    {
                        options.IdleTimeout = TimeSpan.FromSeconds(10);
                        options.Cookie.HttpOnly = true;
                        options.Cookie.IsEssential = true;
                    });
                    break;
                case "SQLSERVER":
                    services.AddDistributedSqlServerCache(options=>{
                        options.ConnectionString = Configuration.GetConnectionString("EmployeeProject");
                        options.SchemaName = "dbo";
                        options.TableName = "ProjectCache";
                    });
                    break;
                case "REDIS":
                    services.AddStackExchangeRedisCache(options=>
                    {
                        options.Configuration = "localhost:6379";
                        options.InstanceName = "ProjectRedis";
                    });
                    break;
                default:
                    services.AddDistributedMemoryCache();
                    break;
            }
            //services.AddScoped<IDistributedCacheService, DistributedCacheService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Home";
                    await next();
                }
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
      
        }
    }
}
//Scaffold - DbContext - Provider Microsoft.EntityFrameworkCore.SqlServer - OutputDir Models - Connection "Data Source=DESKTOP-ADPA0TV;Initial Catalog=EmployeeProject;Integrated Security=True"