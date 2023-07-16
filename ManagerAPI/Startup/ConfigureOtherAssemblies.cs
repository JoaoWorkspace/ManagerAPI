using Autofac;
using Autofac.Extensions.DependencyInjection;
using ManagerAPI.Application.IoC;
using ManagerAPI.Domain.IoC;
using ManagerAPI.Persistence.IoC;
using Microsoft.EntityFrameworkCore;

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

                //    var optionsBuilderMongoContext = new DbContextOptionsBuilder<MongoContext>().usen(builder.Configuration.GetConnectionString("MongoDatabase"));

                //    return optionsBuilder.Options;
                //});
            });
            #endregion Registering Other Layers

            #region  Register all Databases
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
