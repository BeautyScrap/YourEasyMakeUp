using YourEasyRent.DataBase;

using MongoDB.Driver;

using Telegram.Bot;

using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<DataBaseConfig>(
    builder.Configuration
        .GetSection("DataBaseSettings")); //  ��������� ������� DataBaseConfig, ������� ������������ ����� �����, ���������� ���������������� ��������� ��� ���� ������. ���������������� ��������� ��� DataBaseConfig ����� ����������� �� ������ "DataBaseSettings"

// No russian allowed! Practice only english comments
builder.Services.AddSingleton<ProductRepository>(); // � ���� ������ �������������� ������ MongoCollection � ���������� ������������.

// This is the same as it used to be
var databaseConfig = new DataBaseConfig();
builder.Configuration.Bind("DatabaseSettings", databaseConfig);
builder.Services.AddSingleton(databaseConfig);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMongoClient>(new MongoClient(databaseConfig.ConnectionString));
builder.Services.AddHttpClient<ISephoraProductSiteClient, SephoraClient>(); // �������� 2 ������� 
builder.Services.AddHttpClient<IDouglasProductSiteClient, DouglasClient>();
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<ITelegramActionsHandler, TelegramActionsHandler>();

var botToken = "6081137075:AAH52hfdtr9lGG1imfafvIDUIwNchtMlkjw";
// I register ITelegramBotClient as a singleton, so that I can inject it into my TelegramPoller
builder.Services.AddSingleton<ITelegramBotClient>(_ => new TelegramBotClient(botToken));

// Noone depends on TelegramPoller, so I register it as a class
//builder.Services.AddSingleton<ITelegramPoller,TelegramPoller>(); - is kind of the same
builder.Services.AddSingleton<TelegramPoller>();

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

// I pull service from a dependency container and start it
var poller = app.Services.GetService<TelegramPoller>();
poller.StartReceivingMessages();

app.Run();
