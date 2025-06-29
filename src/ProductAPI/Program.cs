using ProductAPI.Application;
using ProductAPI.Application.RabbitMQ;
using ProductAPI.Infrastructure;
using ProductAPI.Infrastructure.Client;
using Serilog;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ContentRootPath = AppContext.BaseDirectory,
    Args = args,
});
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production"
    ? Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING")
    : builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(connectionString);


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.IncludeFields = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<IProductsSiteClient, SephoraClient>();
//builder.Services.AddHttpClient<IProductsSiteClient, DouglasClient>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductForSubService,  ProductForSubService>();
builder.Services.AddScoped<IProductForUserService, ProductForUserService>(); 
builder.Services.AddScoped<IProductHandler, ProductHandler>();
builder.Services.AddScoped<IRabbitMessageProducer, RabbitMessageProducer>();
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
