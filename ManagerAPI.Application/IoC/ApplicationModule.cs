using Autofac;
using Cqrs.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerAPI.Application.IoC;

public class ApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // register all services
        builder.RegisterAssemblyTypes(typeof(ApplicationModule).Assembly)
            .AsImplementedInterfaces();

        //builder.RegisterType<ProfanityFilter>().As<IProfanityFilter>().SingleInstance();
        //builder.RegisterType<CacheService>().As<ICacheService>().SingleInstance();

        // register all command handlers
        builder.RegisterAssemblyTypes(typeof(ApplicationModule).Assembly)
            .As(t =>
            {
                return t.GetInterfaces()
                         .Where(a => a.IsClosedTypeOf(typeof(IRequestHandler<,>)));
            });

        //// register mediatr behaviors for commands
        //builder.RegisterGeneric(typeof(PerformanceCommandBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        //builder.RegisterGeneric(typeof(ValidateCommandBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        //builder.RegisterGeneric(typeof(TryCatchCommandBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        //builder.RegisterGeneric(typeof(AuditCommandBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        //builder.RegisterGeneric(typeof(LogCommandBehavior<,>)).As(typeof(IPipelineBehavior<,>));

        // register all query handlers
        //builder.RegisterAssemblyTypes(typeof(ApplicationModule).Assembly)
        //    .As(t =>
        //    {
        //        return t.GetInterfaces()
        //                 .Where(a => a.IsClosedTypeOf(typeof(IQueryHandler<,>)));
        //    });

        //// register mediatr behaviors for queries
        //builder.RegisterGeneric(typeof(PerformanceQueryBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        //builder.RegisterGeneric(typeof(LogQueryBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        //builder.RegisterGeneric(typeof(ValidateQueryBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        //builder.RegisterGeneric(typeof(TryCatchQueryBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        //builder.RegisterGeneric(typeof(AuditQueryBehavior<,>)).As(typeof(IPipelineBehavior<,>));
    }
}