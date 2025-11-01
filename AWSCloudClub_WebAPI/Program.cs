using AWSCloudClubEventManagement.Data;
using AWSCloudClub_DataService;
using AWSCloudClubEventManagement.Services;
using AWSCloudClub_BusinessLogic;

var builder = WebApplication.CreateBuilder(args);

// for swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();



// Register data services
builder.Services.AddSingleton<DatabaseHelper>();
builder.Services.AddScoped<MemberDataService>();
builder.Services.AddScoped<AdminDataService>();
builder.Services.AddScoped<EventDataService>();
builder.Services.AddScoped<TicketDataService>();

// Register business logic
builder.Services.AddBusinessLogic();

// Register email service
builder.Services.AddScoped<EventEmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

try
{
    var logger = app.Services.GetService(typeof(Microsoft.Extensions.Logging.ILogger<Program>)) as Microsoft.Extensions.Logging.ILogger;
    var hasAdminService = app.Services.GetService(typeof(AWSCloudClub_DataService.AdminDataService)) != null;
    var hasDbHelper = app.Services.GetService(typeof(AWSCloudClubEventManagement.Data.DatabaseHelper)) != null;
    logger?.LogInformation("DI check - AdminDataService registered: {HasAdmin}, DatabaseHelper registered: {HasDb}", hasAdminService, hasDbHelper);
}
catch
{
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
