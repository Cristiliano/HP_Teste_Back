using HP.Clima.Infra.Extensions;
using HP.Clima.Service.Extensions;
using HP.Clima.API.Configuration;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.ConfigureServices(configuration);

builder.Services.AddInfrastructure(configuration);
builder.Services.AddServices();
builder.Services.AddCepHandlers();
builder.Services.AddWeatherHandlers();
builder.Services.AddProxies();
builder.Services.AddMemoryCache();

var app = builder.Build();

app.ConfigureSetup();

app.Run();

public partial class Program { }