using Microsoft.Extensions.Configuration;
using YourEasyRent.DataBase;
using MongoDB.Driver;
using DnsClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using YourEasyRent.DataBase.Interfaces;
using YourEasyRent.Services;
using System;
using YourEasyRent.Entities;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using YourEasyRent.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<DataBaseConfig>(builder.Configuration.GetSection("DataBaseSettings"));//  ��������� ������� DataBaseConfig, ������� ������������ ����� �����, ���������� ���������������� ��������� ��� ���� ������. ���������������� ��������� ��� DataBaseConfig ����� ����������� �� ������ "DataBaseSettings"

builder.Services.AddSingleton<ProductRepository>(); // � ���� ������ �������������� ������ MongoCollection � ���������� ������������.

var botToken = "6081137075:AAH52hfdtr9lGG1imfafvIDUIwNchtMlkjw";

// ok as of now, but should be changed to a background task later
var botController = new TelegramPoller(botToken);
botController.StartReceivingMessages();


// This is the same as it used to be
var databaseConfig = new DataBaseConfig();
builder.Configuration.Bind("DatabaseSettings", databaseConfig);
builder.Services.AddSingleton(databaseConfig);
//

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMongoClient>(new MongoClient(databaseConfig.ConnectionString));
builder.Services.AddHttpClient<ISephoraProductSiteClient, SephoraClient>();// �������� 2 ������� 
builder.Services.AddHttpClient<IDouglasProductSiteClient, DouglasClient>();
builder.Services.AddSingleton<IProductRepository, ProductRepository>();


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