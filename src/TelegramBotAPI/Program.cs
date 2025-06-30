using YourEasyRent.DataBase;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Services;
using Telegram.Bot;
using Serilog;
using TelegramBotAPI.Services;
using TelegramBotAPI.Application.TelegramMenu;
using TelegramBotAPI.Infrastructure.RabbitQM;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ContentRootPath = AppContext.BaseDirectory,
    Args = args,
});

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>() 
    .AddEnvironmentVariables();

var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production"
    ? Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING")
    : builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(connectionString);
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver
    {
        NamingStrategy = new SnakeCaseNamingStrategy()
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();   
builder.Services.AddSingleton<ITelegramSender,  TelegramSender>();
builder.Services.AddHttpClient<IProductApiClient, ProductApiClient>()
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        return new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(2), // Более частое обновление TCP-соединений
            PooledConnectionIdleTimeout = TimeSpan.FromSeconds(30), // Закрывать неиспользуемые соединения
            MaxConnectionsPerServer = 50 // Этот параметр ограничивает количество одновременных TCP-соединений к одному серверу
        };
    })
    .SetHandlerLifetime(Timeout.InfiniteTimeSpan); 
builder.Services.AddSingleton<IUserStateRepository, UserStateRepository>();


var botToken = builder.Configuration["TelegramBot:Token"];
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