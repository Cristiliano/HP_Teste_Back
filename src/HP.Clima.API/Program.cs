using HP.Clima.Infra.Extensions;
using HP.Clima.Service.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using HP.Clima.API.Validators;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CepRequestValidator>();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddServices();
builder.Services.AddProxies(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

public partial class Program { }