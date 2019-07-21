using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using LibraryTest.BLL.MapProfiles;
using LibraryTest.DAL.Core;
using LibraryTest.DAL.Infrastructure;
using Manlike.BLL.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSpaStaticFiles(config =>
            {
                config.RootPath = "clientapp/dist";
            });
            services.AddDbContext<AppDbContext>(opts => opts.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.ConfigureApplicationCookie(opts =>
            {
                opts.Cookie.HttpOnly = true;
                opts.Cookie.Name = "Libtest";
                opts.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                opts.Cookie.SameSite = SameSiteMode.None;
            });

            // Services DI
            var assemblyTypes = Assembly.GetAssembly(typeof(DataService)).GetTypes();
            var iDataType = typeof(IDataService);
            var dataType = typeof(DataService);
            foreach (var servType in assemblyTypes.Where(x => x != dataType && x.IsSubclassOf(dataType)))
            {
                services.AddTransient(assemblyTypes.First(x => x.IsInterface && x.IsAssignableFrom(servType) && x != iDataType), servType);
            }
            // Repository DI
            assemblyTypes = Assembly.GetAssembly(typeof(Repository<>)).GetTypes();
            var repoType = typeof(Repository<>);
            var iRepoType = typeof(IRepository<>);
            foreach (var rt in assemblyTypes.Where(x => x != repoType && IsSubclassOfRawGeneric(repoType, x)))
            {
                services.AddTransient(
                    assemblyTypes.First(x => x.IsInterface 
                        && x.IsAssignableFrom(rt) 
                        && x != iRepoType), 
                    rt);
            }

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var mapConfig = new MapperConfiguration(mc =>
            {
                // Скан сборки для получения профилей (LibraryTest.BLL/MapProfiles)
                mc.AddMaps(Assembly.GetAssembly(typeof(ClientProfile)));
            });
            services.AddSingleton(mapConfig.CreateMapper());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(builder => 
                    builder.WithOrigins("http://localhost:8080")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                );

            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            //app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "clientapp/dist/pwa";
            });
        }

        bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}
