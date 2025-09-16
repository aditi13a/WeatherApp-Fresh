var builder = WebApplication.CreateBuilder(args);

// Add MongoDbSettings configuration binding
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

// Register services
builder.Services.AddSingleton<WeatherHistoryService>();
builder.Services.AddSingleton<FavoritesService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddSingleton(provider =>
{
    var url = "https://gqvdbvqaimnjvwwerpio.supabase.co";
    var key = "<your_supabase_key>";

    var options = new SupabaseOptions { AutoRefreshToken = true };
    return new Supabase.Client(url, key, options);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Middleware
app.UseCors("AllowAll");

// ⚡ Serve Blazor Client static files
app.UseStaticFiles();

app.UseRouting();

// Only use HTTPS redirection locally
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

// Optional WeatherForecast endpoint
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
