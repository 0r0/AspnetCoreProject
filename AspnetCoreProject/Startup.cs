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
using Microsoft.AspNetCore.Identity;
using AspnetCoreProject.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;

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
            //Identity database
            //Add-Migration CreateIdentitySchema -context AppIdentityDbContext 
            //or
            // dotnet ef migrations add CreateIdentitySchema -c AppIdentityDbContext
            //dotnet ef database update -c AppIdentityDbContext
            services.AddDbContext<AppIdentityDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("EmployeeProject")));
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                //Password Settings
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                //Lockout Settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                //User Settings
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

                //for email confirmation
                options.SignIn.RequireConfirmedEmail = true;


            }).AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options => {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
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
                    // docker run --name ProjectCacheRedis -p 6379:6379 -d redis
                    //docker exec -it ProjectCacheRedis sh
                    // redis-cli
                    //ping
                    //
                case "REDIS":
                    services.AddStackExchangeRedisCache(options=>
                    {
                        options.Configuration = "localhost:6379";
                        options.InstanceName = "ProjectCacheRedis";
                    });
                    break;
                default:
                    services.AddDistributedMemoryCache();
                    break;
            }
            //cookie authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options=>
            {
                options.Cookie.HttpOnly = false;
                options.ExpireTimeSpan = TimeSpan.FromDays(3);
                options.LoginPath = "/Account/LoginRememberMe";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            services.AddScoped<IDistributedCacheService, DistributedCacheService>();
            //email services
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, MailService>();
            //recaptcha services
            services.Configure<ReCaptchaSettings>(Configuration.GetSection("ReCaptchaSettings"));
            services.AddTransient<IRecaptchaService, ReCaptchaService>();

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
            app.UseAuthentication();
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