using System.Text.Json.Serialization;
using Onoicrm.Api.Utils;
using Onoicrm.Domain.Services;
using Onoicrm.DataContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddControllers().AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
//asdasd
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var envFilePath = "/var/www/dev1/api/.env";
if (builder.Environment.IsDevelopment())
{
    envFilePath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent!.Parent!.FullName, ".env");
}
var connectionString = configuration.GetEnvConnectionString(envFilePath);
builder.Services.SetupEfInfrastructure(configuration,connectionString);
builder.Services.RegisterEfServices();
builder.Services.AddScoped<WappiService>();


var app = builder.Build();
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<ApplicationDataContext>();
if (context.Database.GetPendingMigrations().Any())
{   
    context.Database.Migrate();
}
app.UseCors(b =>
    {
        b.WithOrigins("http://localhost:3030", "http://localhost:3000", "http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    }
);
    
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();