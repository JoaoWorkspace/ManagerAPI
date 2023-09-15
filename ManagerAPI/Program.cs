using Newtonsoft.Json.Converters;
using NsfwSpyNS;
using ManagerAPI.Caching;
using ManagerAPI.Startup;
using QBittorrent.Client;
using ManagerAPI.Settings;
using ManagerAPI.Application.FileArea;
using ManagerAPI.Application.TorrentArea;
using Microsoft.Data.SqlClient.Server;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;
using ManagerAPI.Persistence.Settings;
using BencodeNET.Parsing;

#region builder

var builder = WebApplication.CreateBuilder(args);
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
//Advanced - Instance and Register our Configuration Classes
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection(CacheSettings.SectionKey));
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection(DatabaseSettings.SectionKey));
//builder.Services.Configure<MusicSettings>(builder.Configuration.GetSection(MusicSettings.SectionKey));
//builder.Services.Configure<TorrentSettings>(builder.Configuration.GetSection(TorrentSettings.SectionKey));
builder.Services.Configure<SwaggerSettings>(builder.Configuration.GetSection(SwaggerSettings.SectionKey));
//End of Advanced
////
///
//


//
///
////
//Advanced - Register our Scoped and Singleton Services/Classes through DependencyInjection
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ITorrentService, TorrentService>();
builder.Services.AddSingleton<ICache, Cache>();
builder.Services.AddSingleton<IQBittorrentClient>(new QBittorrentClient(new Uri("http://127.0.0.1:8080/")));
builder.Services.AddSingleton<IBencodeParser, BencodeParser>();
//End of Advanced
////
///
//


//
///
////
//Advanced - Register NsfwSpy so we can inject it into the controllers
builder.Services.AddScoped<INsfwSpy, NsfwSpy>();
//End of Advanced
////
///
//


//B:\Rocky.Balboa.2006.BluRay.Remux.1080p.AVC.DTS-HD.MA.5.1-TDD.mkv

//Also necessary to register assemblies in all projects associated with our API (otherwise MediatR cannot properly detect the CommandHandlers from other Assemblies)
builder.Services.ConfigureManagerAPIMicroservice(builder);

#endregion builder

#region app

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(
   //
   ///
   ////
   //Advanced - This little piece of code greatly enhances performance when returning large amounts of data by disabling syntax highlights
   //Configuration: AppSettings.json -> Strongly Typed into SwaggerSettings
   config =>
   config.ConfigObject.AdditionalItems["syntaxHighlight"] = new Dictionary<string, object>
   {
       ["activated"] = app.Services.GetService<IOptions<SwaggerSettings>>().Value.UseSyntaxHighlight
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