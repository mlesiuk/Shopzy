using Shopzy.Api;
using Shopzy.Application;
using Shopzy.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services
    .AddInfrastructure(configuration)
    .AddApplication(configuration)
    .AddApi();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.AddSerilog(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
