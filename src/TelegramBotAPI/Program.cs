using YourEasyRent.DataBase;
using MongoDB.Driver;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Services;
using Telegram.Bot;
using Serilog;
using TelegramBotAPI.Services;
using TelegramBotAPI.Application.TelegramMenu;
using TelegramBotAPI.Infrastructure.RabbitQM;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ContentRootPath = AppContext.BaseDirectory,
    Args = args,
});
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production"
    ? Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING")
    : builder.Configuration.GetConnectionString("DefaultConnection");

//builder.Services.Configure<DataBaseConfig>(builder.Configuration.GetSection("DataBaseSettings"));  

//builder.Services.AddSingleton<UserStateRepository>(); 

//// This is the same as it used to be
//var databaseConfig = new DataBaseConfig();
//builder.Configuration.Bind("DatabaseSettings", databaseConfig);

//// Should be changed to be based on evironment value
//if( Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
//{
//    databaseConfig.ConnectionString = Environment.GetEnvironmentVariable("ATLAS_URI")!;
//}

//builder.Services.AddSingleton(databaseConfig);

builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSingleton<IMongoClient>(new MongoClient(databaseConfig.ConnectionString));    
builder.Services.AddSingleton<ITelegramSender,  TelegramSender>(); 
builder.Services.AddHttpClient<IProductApiClient, ProductApiClient>();
//builder.Services.AddSingleton<IUserStateRepository, UserStateRepository>();


var botToken = "6081137075:AAH52hfdtr9lGG1imfafvIDUIwNchtMlkjw";
builder.Services.AddSingleton<ITelegramBotClient>(_ =>new TelegramBotClient(botToken));
builder.Services.AddSingleton<IRabbitMessageProducer, RabbitMessageProducer>();
builder.Services.AddSingleton<ITelegramCallbackHandler, TelegramCallbackHandler>();
builder.Services.AddSingleton<ITelegramUpdateHandler, TelegramUpdateHandler>();



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