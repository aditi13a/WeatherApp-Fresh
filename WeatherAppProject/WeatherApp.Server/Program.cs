using WeatherApp.Server.Settings;
using WeatherApp.Server.Services;
using Supabase;


var builder = WebApplication.CreateBuilder(args);

// Add MongoDbSettings configuration binding
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

// Register WeatherHistoryService
builder.Services.AddSingleton<WeatherHistoryService>();
// Register FavoritesService
builder.Services.AddSingleton<FavoritesService>();

// Add services to the container.
builder.Services.AddControllers(); // <-- Add this to enable your WeatherController
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddSingleton(provider =>
{
    var url = " https://gqvdbvqaimnjvwwerpio.supabase.co ";
    var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImdxdmRidnFhaW1uanZ3d2VycGlvIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTE1MzA4ODAsImV4cCI6MjA2NzEwNjg4MH0.iZ0AxhK53q2DFalSLVSJHvbLqRovAqCvq8REtgXB6iE";

    var options = new SupabaseOptions
    {
        AutoRefreshToken = true
    };

    var supabaseClient = new Supabase.Client(url, key, options);
    return supabaseClient;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Optional: Keep weatherforecast endpoint if you want to test
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
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
