using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WeatherApp.Client;
using System.Net.Http; // ensure this using exists


using Supabase;

var options = new SupabaseOptions
{
    AutoRefreshToken = true
};

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSingleton(sp =>
{
    var supabaseUrl = "https://gqvdbvqaimnjvwwerpio.supabase.co";
    var supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImdxdmRidnFhaW1uanZ3d2VycGlvIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTE1MzA4ODAsImV4cCI6MjA2NzEwNjg4MH0.iZ0AxhK53q2DFalSLVSJHvbLqRovAqCvq8REtgXB6iE";

    return new Supabase.Client(supabaseUrl, supabaseKey, options);
});

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri("http://localhost:5249/") }); // server API URL and port
await builder.Build().RunAsync();
