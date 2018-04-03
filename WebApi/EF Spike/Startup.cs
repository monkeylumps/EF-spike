using EF_Spike.DatabaseContext;
using EF_Spike.Membership.Handler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;

namespace EF_Spike
{
    public class Startup
    {
        public string connection { get; set; }
        private Container container = new Container();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            connection = @"Server=.;Database=Registry;Trusted_Connection=True;";

            services.AddDbContext<RegistryContext>(builder =>
            {
                if (!builder.IsConfigured)
                {
                    builder.UseSqlServer(connection);
                }
            });

            IntegrateSimpleInjector(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            container.Register<IGetMembership, GetMembership>();

            container.Register(() =>
            {
                var options = new DbContextOptionsBuilder<RegistryContext>();
                if (!options.IsConfigured)
                {
                    options.UseSqlServer(connection);
                }

                return new RegistryContext(options.Options);
            }, Lifestyle.Singleton);

            container.Verify();

            app.UseMvc();
        }

        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IControllerActivator>(
                new SimpleInjectorControllerActivator(container));

            services.EnableSimpleInjectorCrossWiring(container);
            services.UseSimpleInjectorAspNetRequestScoping(container);
        }
    }
}
