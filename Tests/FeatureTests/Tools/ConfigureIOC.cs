﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EF_Spike.DatabaseContext;
using EF_Spike.Membership.Handler;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using SimpleInjector;

namespace FeatureTests.Tools
{
    public class ConfigureIOC
    {
        private Container container = new Container();

        public Container Configure()
        {
            container.Register(() =>
            {
                var options = new DbContextOptionsBuilder<RegistryContext>();
                if (!options.IsConfigured)
                {
                    options.UseInMemoryDatabase("MembershipTests");
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

            return container;
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            yield return typeof(IMediator).Assembly;
            yield return typeof(GetMembership).Assembly;
        }
    }
}