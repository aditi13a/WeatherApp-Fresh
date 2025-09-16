# Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy everything
COPY . .

# Restore and publish the server project
RUN dotnet restore "WeatherApp.sln"
RUN dotnet publish "WeatherApp.Server/WeatherApp.Server.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Run the server
ENTRYPOINT ["dotnet", "WeatherApp.Server.dll"]
