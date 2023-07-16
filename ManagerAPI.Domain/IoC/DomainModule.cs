using Autofac;

namespace ManagerAPI.Domain.IoC;

public class DomainModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // register all services
        builder.RegisterAssemblyTypes(typeof(DomainModule).Assembly)
            .AsImplementedInterfaces();
    }
}