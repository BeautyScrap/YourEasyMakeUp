using YourEasyRent.DataBase;
using MongoDB.Driver;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Services;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<DataBaseConfig>(builder.Configuration.GetSection("DataBaseSettings"));//  ��������� ������� DataBaseConfig, ������� ������������ ����� �����, ���������� ���������������� ��������� ��� ���� ������. ���������������� ��������� ��� DataBaseConfig ����� ����������� �� ������ "DataBaseSettings"

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
builder.Services.AddHttpClient<IProductsSiteClient, SephoraClient>();
builder.Services.AddHttpClient<IProductsSiteClient, DouglasClient>();
builder.Services.AddSingleton<IProductRepository, ProductRepository>();


builder.Services.AddSingleton<ITelegramActionsHandler,TelegramActionsHandler>();    
builder.Services.AddSingleton<ITelegramMenu, TelegramMenu>();   
var botToken = "6081137075:AAH52hfdtr9lGG1imfafvIDUIwNchtMlkjw";
builder.Services.AddSingleton<ITelegramBotClient>(_ =>new TelegramBotClient(botToken));
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

var poller = app.Services.GetService<TelegramPoller>();
poller.StartReceivingMessages();

app.Run();