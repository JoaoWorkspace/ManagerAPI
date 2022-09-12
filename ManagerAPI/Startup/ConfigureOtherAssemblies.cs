using Autofac;
using Autofac.Extensions.DependencyInjection;
using ManagerAPI.Application.IoC;
using MediatR;
using Microsoft.Extensions.Options;

using Microsoft.AspNetCore.Authorization;
using QBittorrent.Client;
//using Microsoft.EntityFrameworkCore;

namespace ManagerAPI.Startup
{
    public static class ConfigureOtherAssemblies
    {
        public static void ConfigureManagerAPIMicroservice(this IServiceCollection services, WebApplicationBuilder builder)
        {
            // Call UseServiceProviderFactory on the Host sub property 
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            // Call ConfigureContainer on the Host sub property 
            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                containerBuilder.RegisterModule(new ApplicationModule());
                //containerBuilder.RegisterModule(new DomainModule());
                //containedBuilder.RegisterModule(new PersistenceModule());

                //containerBuilder.Register(c =>
                //{
                //    var optionsBuilder = new DbContextOptionsBuilder<PostgreContext>().UseNpgsql(builder.Configuration.GetConnectionString("CarDatabase"));

                //    return optionsBuilder.Options;
                //});
            });

            // Add services to the container.
            services.AddAutoMapper(cfg =>
            {
                //cfg.AddProfiles(Application.Mapping.MappingConfiguration.GetMappingProfiles());
                //cfg.AddProfiles(Cars.Persistence.Mapping.MappingConfiguration.GetMappingProfiles());
                //cfg.AddProfiles(Interaction.Persistence.Mapping.MappingConfiguration.GetMappingProfiles());
            }, AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToArray());

            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToArray());

            //services.AddSingleton<ITimeProvider, TimeProvider>();
            //services.AddSingleton<IAccountProvider, HttpAccountProvider>();
            //services.AddSingleton<IHashId, HashId>();


            //Add Authorization, AuthorizationCache and AuthorizationOptions
            //services.AddAuthorization();
            //services.AddSingleton<IAuthorizationCache, AuthorizationCache>();

            //Configure AuthorizationOptions
            //services.Configure<AuthorizationOptions>(options =>
            //{
            //    options.AddPolicy(CarAccessPolicyRequirement.PolicyRequirementName, policy =>
            //    {
            //        policy.Requirements.Add(new CarAccessPolicyRequirement());
            //    });

            //    options.AddPolicy(CarOwnerPolicyRequirement.PolicyRequirementName, policy =>
            //    {
            //        policy.Requirements.Add(new CarOwnerPolicyRequirement());
            //    });

            //    options.AddPolicy(CarCommentOwnerPolicyRequirement.PolicyRequirementName, policy =>
            //    {
            //        policy.Requirements.Add(new CarCommentOwnerPolicyRequirement());
            //    });
            //});

            //Adding AuthorizationScopes
            //services.AddScoped<IAuthorizationHandler, CarAccessPermissionHandler>();
            //services.AddScoped<IAuthorizationHandler, CarOwnerPermissionHandler>();
            //services.AddScoped<IAuthorizationHandler, CarCommentOwnerPermissionHandler>();

            //Registering a normal SQLServer (PostegreSQL in this case) database
            //services.AddDbContextFactory<CarDatabaseContext, CarDatabaseContextFactory>();

            //Registering a CosmosDB Document Database using Configuration from AppSettings
            //services.AddDbContextFactory<CarInteractionDatabaseContext>((serviceProvider, options) =>
            //{
            //    var cosmosSettings = serviceProvider
            //        .GetRequiredService<IOptions<CosmosSettings>>()
            //        .Value;

            //    options.UseCosmos(cosmosSettings.EndPoint, cosmosSettings.AccessKey, cosmosSettings.DatabaseName);
            //});
        }
    }
}
