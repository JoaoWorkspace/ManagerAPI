using Autofac;
using Autofac.Core;
using Raven.Client.Documents;
using System.Reflection;

namespace ManagerAPI.Persistence.IoC;

public class PersistenceModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // register all services
        builder.RegisterAssemblyTypes(ThisAssembly)
            .AsImplementedInterfaces().InstancePerLifetimeScope();
    }

}