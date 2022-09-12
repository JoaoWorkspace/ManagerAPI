using Swashbuckle.AspNetCore.Swagger;
using Newtonsoft.Json.Converters;
using ManagerAPI.Application.TorrentArea;
using MediatR;
using NsfwSpyNS;
using ManagerAPI.Caching;
using System.Reflection;
using ManagerAPI.Startup;
using QBittorrent.Client;

#region builder

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
//
///
////
//Advanced - This allows me to format swagger and return the enum string value instead of the int value.
builder.Services.AddControllers()
    .AddNewtonsoftJson(options => options.SerializerSettings.Converters
        .Add(new StringEnumConverter())
     );
builder.Services.AddSwaggerGenNewtonsoftSupport();
//End of Advanced
////
///
//

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//
///
////
//Advanced - Instance and Register our Configuration Classes (which need to reflect the nested properties)
//builder.Services.Configure<AppSettingsClass>(builder.Configuration.GetSection(AppSettingsClass.SectionKey));
//builder.Services.Configure<TorrentSettings>(builder.Configuration.GetSection(TorrentSettings.SectionKey));
//builder.Services.Configure<MusicSettings>(builder.Configuration.GetSection(MusicSettings.SectionKey));
//End of Advanced
////
///
//


//
///
////
//Advanced - Register our Scoped and Singleton Services/Classes through DependencyInjection
builder.Services.AddScoped<ITorrentService, TorrentService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddSingleton<ICache, Cache>();
builder.Services.AddSingleton<IQBittorrentClient>(new QBittorrentClient(new Uri("http://127.0.0.1:8080/")));
//End of Advanced
////
///
//


//
///
////
//Advanced - Register NsfwSpy so we can inject it into the controllers
builder.Services.AddScoped<INsfwSpy, NsfwSpy>();
//Also necessary to register MediatR on all assemblies (by Reflection)
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToArray());
//Also necessary to register assemblies in all projects associated with our API (otherwise MediatR cannot properly detect the CommandHandlers from other Assemblies)
builder.Services.ConfigureManagerAPIMicroservice(builder);
//End of Advanced
////
///
//




#endregion builder
#region app

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(
   //
   ///
   ////
   //Advanced - This little piece of code greatly enhances performance when returning large amounts of data
    //Configuration: AppSettings.json
   config =>
   config.ConfigObject.AdditionalItems["syntaxHighlight"] = new Dictionary<string, object>
   {
       ["activated"] = bool.Parse(builder.Configuration["SwaggerOptions:UseSyntaxHighlight"])
   }
   //End of Advanced
   ////
   ///
   //
);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

#endregion app