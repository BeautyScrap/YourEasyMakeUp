using SubscriberAPI.Application;
using SubscriberAPI.Infrastructure;
using FluentValidation;
using SubscriberAPI.Contracts;
using SubscriberAPI.Presentanion;
using SubscriberAPI.Presentanion.Clients;
using SubscriberAPI.Infrastructure.RabbitQM;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.IncludeFields = true;
    });

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production"
    ? Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING")
    : builder.Configuration.GetConnectionString("DefaultConnection");

//if (string.IsNullOrEmpty(connectionString))
//{
//    throw new InvalidOperationException("Database connection string is not configured.");
//}

builder.Services.AddSingleton(connectionString);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<IProductApiClient, ProductApiClient>();
builder.Services.AddHttpClient<ITelegramApiClient, TelegramApiClient>();
builder.Services.AddScoped<ISubscriberRabbitMessageProducer, RabbitMessageProducer>();
builder.Services.AddScoped<ISubscribersRepository, SubscribersRepository>();
builder.Services.AddScoped<ISubscrieberService, SubscriberService>();
builder.Services.AddScoped<IValidator<SubscriptionRequest>, CreateSubscriptionRequestValidator>();  


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
