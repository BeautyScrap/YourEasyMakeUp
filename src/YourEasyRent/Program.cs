using YourEasyRent.DataBase;
using MongoDB.Driver;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Services;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ContentRootPath = AppContext.BaseDirectory,
    Args = args,
});

var test2 = builder.Configuration.GetValue<string>("DataBaseSettings:DataBaseName");
builder.Services.Configure<DataBaseConfig>(builder.Configuration.GetSection("DataBaseSettings"));  

builder.Services.AddSingleton<ProductRepository>(); 

// This is the same as it used to be
var databaseConfig = new DataBaseConfig();
builder.Configuration.Bind("DatabaseSettings", databaseConfig);

// Should be changed to be based on evironment value
if( Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
{
    databaseConfig.ConnectionString = Environment.GetEnvironmentVariable("ATLAS_URI")!;
}

builder.Services.AddSingleton(databaseConfig);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMongoClient>(new MongoClient(databaseConfig.ConnectionString));
builder.Services.AddHttpClient<IProductsSiteClient, SephoraClient>();
builder.Services.AddHttpClient<IProductsSiteClient, DouglasClient>();
builder.Services.AddSingleton<IProductRepository, ProductRepository>();


builder.Services.AddSingleton<ITelegramActionsHandler,TelegramActionsHandler>();    
builder.Services.AddSingleton<ITelegramMenu, TelegramMenu>();   
var botToken = "6081137075:AAH52hfdtr9lGG1imfafvIDUIwNchtMlkjw";
builder.Services.AddSingleton<ITelegramBotClient>(_ =>new TelegramBotClient(botToken));
builder.Services.AddSingleton<ITelegramCallbackHandler, TelegramCallbackHandler>();

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