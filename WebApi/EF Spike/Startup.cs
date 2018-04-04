﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EF_Spike.DatabaseContext;
using EF_Spike.Membership.Handler;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using MediatR.Pipeline;

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

            //container.Register<IGetMembership, GetMembershipHandler>();

            container.Register(() =>
            {
                var options = new DbContextOptionsBuilder<RegistryContext>();
                if (!options.IsConfigured)
                {
                    options.UseSqlServer(connection);
                }

                return new RegistryContext(options.Options);
            }, Lifestyle.Singleton);

            var assemblies = GetAssemblies().ToArray();
            container.RegisterSingleton<IMediator, Mediator>();
            container.Register(typeof(IRequestHandler<,>), assemblies);
            container.Register(typeof(IRequestHandler<>), assemblies);
            container.RegisterCollection(typeof(INotificationHandler<>), assemblies);
            container.RegisterSingleton(Console.Out);
            container.RegisterCollection(typeof(IPipelineBehavior<,>), new[]
            {
                typeof(RequestPreProcessorBehavior<,>),
                typeof(RequestPostProcessorBehavior<,>),
            });
            container.RegisterCollection(typeof(IRequestPreProcessor<>), assemblies);
            container.RegisterCollection(typeof(IRequestPostProcessor<,>), assemblies);
            container.RegisterSingleton(new SingleInstanceFactory(container.GetInstance));
            container.RegisterSingleton(new MultiInstanceFactory(container.GetAllInstances));

            container.Verify();

            AutoMapper.Mapper.Initialize(x =>
            {
                x.CreateMap<TblMembership, Membership.Model.Membership>();
                x.CreateMap<Membership.Model.Membership, TblMembership>();
            });

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

        private static IEnumerable<Assembly> GetAssemblies()
        {
            yield return typeof(IMediator).Assembly;
            yield return typeof(GetMembership).Assembly;
        }
    }
}
