using Autofac;
using Autofac.Extensions.DependencyInjection;
using ManagerAPI.Application.IoC;
using ManagerAPI.Domain.IoC;
using ManagerAPI.Persistence.Database;
using ManagerAPI.Persistence.IoC;
using ManagerAPI.Persistence.Settings;
using ManagerAPI.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Raven.Client.Documents;

namespace ManagerAPI.Startup
{
    public static class ConfigureOtherAssemblies
    {
        public static void ConfigureManagerAPIMicroservice(this IServiceCollection services, WebApplicationBuilder builder)
        {
            #region Registering Other Layers
            // Call UseServiceProviderFactory on the Host sub property 
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            //Advanced - Registering other microservice's modules with AutoFAC at Startup
            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                containerBuilder.RegisterModule(new ApplicationModule());
                containerBuilder.RegisterModule(new DomainModule());
                containerBuilder.RegisterModule(new PersistenceModule());

                //containerBuilder.Register(c =>
                //{
                //    var optionsBuilderPostgreContext = new DbContextOptionsBuilder<PostgreContext>().UseNpgsql(builder.Configuration.GetConnectionString("PostgreDatabase"));

                //    return optionsBuilder.Options;
                //});
            });
            #endregion Registering Other Layers

            #region  Register all Databases

            //Registering a normal SQLServer (PostegreSQL in this case) database
            var serviceCollection = services.BuildServiceProvider();

            var databaseSettings = serviceCollection
               .GetRequiredService<IOptions<DatabaseSettings>>();
            //services.AddDbContextFactory<CarDatabaseContext, CarDatabaseContextFactory>();

            //Registering a CosmosDB Document Database using Configuration from AppSettings
            //services.AddDbContextFactory<CarInteractionDatabaseContext>((serviceProvider, options) =>
            //{
            //    var cosmosSettings = serviceProvider
            //        .GetRequiredService<IOptions<CosmosSettings>>()
            //        .Value;

            //    options.UseCosmos(cosmosSettings.EndPoint, cosmosSettings.AccessKey, cosmosSettings.DatabaseName);
            //});

            //Registering a RavenDB Document Database using Configuration from AppSettings
            if (databaseSettings.Value.RavenDB.IsSetup)
            {
                services.AddSingleton(new IdentityDocumentStoreHolder(databaseSettings));
            }

            builder.Services.AddSingleton<IDocumentStore>((serviceProvider) =>
            {
                var databaseSettings = serviceProvider.GetService<IOptions<DatabaseSettings>>().Value;
                return IdentityDocumentStoreHolder.Store;

            });

            //services.AddDbContextFactory<IdentityDocumentStoreHolder>((options) =>
            //{
            //    options.

            //    options.UseCosmos(cosmosSettings.EndPoint, cosmosSettings.AccessKey, cosmosSettings.DatabaseName);
            //});

            #endregion  Register all Databases

            //
            ///
            ////
            // Advanced - Register All AutoMapper Profiles
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfiles(Application.Mapping.MappingConfiguration.GetMappingProfile());
            });
            //End of Advanced
            ////
            ///
            //

            #region Authorization Configuration
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
            #endregion Authorization Configuration


        }
    }
}
