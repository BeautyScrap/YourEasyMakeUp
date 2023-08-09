using Microsoft.Extensions.Configuration;
using YourEasyRent.DataBase;
using MongoDB.Driver;
using DnsClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

 var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<DataBaseConfig>(
    builder.Configuration.GetSection("DataBaseSettings"));


// Add services to the container.
//var configuration = builder.Configuration;
//var DataaseConfig = configuration.GetValue<string>("value");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var databaseConfig = RegisterConfigs(services);
builder.Services.AddSingleton<IMongoClient>(new MongoClient(databaseConfig.ConnectionString));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

//public partial class Program
//{
//    public static void Main(string[] args)
//    {
//        CreateHostBuilder(args).Build().Run();
//    }

//    public static IHostBuilder CreateHostBuilder(string[] args) =>
//        Host.CreateDefaultBuilder(args)
//            .ConfigureWebHostDefaults(webBuilder =>
//            {
//                webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
//                {
//                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
//                    config.AddEnvironmentVariables();
//                    // You can add more configuration sources here
//                });
//                webBuilder.UseStartup<Startup>();
//            });
//}

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; set; }

    // Configure services here

    // Configure the application's request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure pipeline here
    }
    static DataBaseConfig RegisterConfigs(IServiceCollection services)
    {
        var databaseConfig = new DataBaseConfig();
        
        Configuration.Bind("DatabaseSettings", databaseConfig);
        // ConfigurationBinder.Bind(configuration.GetSection("DataBaseSettings"), databaseConfig); // добавить  public class startup configuration и iconfiguration в начало файла

        services.AddSingleton(databaseConfig);
        return databaseConfig;


}
}




