using Swashbuckle.AspNetCore.Swagger;
using Newtonsoft.Json.Converters;

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

#endregion builder
#region app

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

#endregion app