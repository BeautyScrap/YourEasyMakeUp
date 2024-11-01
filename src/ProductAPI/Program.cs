using MongoDB.Driver;
using ProductAPI.Application;
using ProductAPI.Infrastructure;
using ProductAPI.Infrastructure.Client;
using ProductAPI.Infrastructure.Persistence;
using Serilog;
using SubscriberAPI.Application.RabbitQM;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ContentRootPath = AppContext.BaseDirectory,
    Args = args,
});
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("DataBaseSettings"));
builder.Services.AddSingleton<ProductRepository>();
var mongoDBSettings = new MongoDBSettings();
builder.Configuration.Bind("DataBaseSettings", mongoDBSettings);
if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
{
    mongoDBSettings.ConnectionString = Environment.GetEnvironmentVariable("ATLAS_URI")!;
}
builder.Services.AddSingleton(mongoDBSettings);
// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.IncludeFields = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoDBSettings.ConnectionString));
builder.Services.AddHttpClient<IProductsSiteClient, SephoraClient>();
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IProductForSubService,  ProductForSubService>();
builder.Services.AddSingleton<IRabbitMessageProducer, RabbitMessageProducer>();
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("log.txt",
        rollingInterval: RollingInterval.Day,
        rollOnFileSizeLimit: true)
    .CreateLogger();



var app = builder.Build();

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
