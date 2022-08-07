using Swashbuckle.AspNetCore.Swagger;
using Newtonsoft.Json.Converters;
using ManagerAPI.Application.TorrentArea;
using MediatR;
using NsfwSpyNS;
using ManagerAPI.Caching;
using System.Reflection;

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
//Advanced - Register our services through DependencyInjection
//builder.Services.AddScoped<IService, Service>();
builder.Services.AddScoped<ITorrentService, TorrentService>();
builder.Services.AddScoped<ICacheService, CacheService>();
////
///
//Advanced - Register our Cache as a Singleton
builder.Services.AddSingleton<ICache, Cache>();
////
///
//Also necessary to register NsfwSpy so we can inject it into the controllers
builder.Services.AddScoped<INsfwSpy, NsfwSpy>();
////
///
//Also necessary to register MediatR on all assemblies (except dynamically generated assemblies (by Reflection)
//builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToArray());
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
//End of Advanced
////
///
//

#endregion builder
#region app

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
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
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

#endregion app