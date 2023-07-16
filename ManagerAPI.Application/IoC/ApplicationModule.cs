using Autofac;
using Autofac.Core;
using ManagerAPI.Application.TorrentArea;
using ManagerAPI.Application.TorrentArea.Commands.SearchTorrent;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using System.Reflection;

namespace ManagerAPI.Application.IoC;

public class ApplicationModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        //
        ///
        ////
        //Advanced - Registering MediatR on this Microservice's Assembly in order to use the mediator pattern
        var containerMediatRConfiguration = MediatRConfigurationBuilder
            .Create(typeof(ApplicationModule).Assembly)
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();
        builder.RegisterMediatR(containerMediatRConfiguration);
        ////
        ///
        //
    }

}