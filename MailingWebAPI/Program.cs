using System;
using System.IO;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    config.IncludeXmlComments(xmlPath);
});
builder.Services.AddSingleton(typeof(Dictionary<string, int>));

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(config =>
{
    config.RoutePrefix = String.Empty;
    config.SwaggerEndpoint("swagger/v1/swagger.json", "Mailing API");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
