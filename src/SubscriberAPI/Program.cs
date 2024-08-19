using SubscriberAPI.Application;
using SubscriberAPI.Application.RabbitQM;
using SubscriberAPI.Infrastructure;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.IncludeFields = true;
    });

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(connectionString);

// Add services to the container.
//builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRabbitMessageProducer, RabbitMessageProducer>();
builder.Services.AddScoped<ISubscribersRepository, SubscribersRepository>();
builder.Services.AddScoped<ISubscrieberService, SubscriberService>();


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
