using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QianGeRobot.Core;
using QianGeRobot.Services;
using System.Net.Http;

namespace QianGeRobot.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)/*.AddControllersAsServices()*/;

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // QianGeConfigs
            builder.Register(c =>
            {
                var qianGeConfigs = Configuration.Get<QianGeConfigs>();
                return qianGeConfigs;
            }).SingleInstance();

            // HttpClient
            builder.Register(c =>
            {
                return new HttpClient();
            }).SingleInstance();

            builder.RegisterType<DingTalkService>();
            builder.RegisterType<TracService>();
            builder.RegisterType<QianGeService>();
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
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.Use((context, next) =>
            {
                context.Request.PathBase = new PathString("/qiange");
                return next();
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
