using API.Worker.Publish.EDA.RabbitMQ.Domain.Implementation.Interfaces;
using API.Worker.Publish.EDA.RabbitMQ.Domain.Implementation.Services;
using API.Worker.Publish.EDA.RabbitMQ.Models.Events;
using MassTransit;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

var rabbitMQ_Queue = builder.Configuration["RabbitMQ_Queue"]!;
if (string.IsNullOrEmpty(rabbitMQ_Queue))
    throw new ArgumentException($"Error {assemblyName}. Parameter RabbitMQ_Queue not found.");

var rabbitMQ_Host = builder.Configuration["RabbitMQ_Host"]!;
if (string.IsNullOrEmpty(rabbitMQ_Host))
    throw new ArgumentException($"Error {assemblyName}. Parameter RabbitMQ_Host not found.");

var rabbitMQ_Port = builder.Configuration["RabbitMQ_Port"]! ?? "";

var rabbitMQ_VHost = builder.Configuration["RabbitMQ_VHost"]!;
if (string.IsNullOrEmpty(rabbitMQ_VHost))
    throw new ArgumentException($"Error {assemblyName}. Parameter rabbitMQ_VHost not found.");

var rabbitMQ_Username = builder.Configuration["RabbitMQ_Username"]!;
if (string.IsNullOrEmpty(rabbitMQ_Username))
    throw new ArgumentException($"Error {assemblyName}. Parameter RabbitMQ_Username not found.");

var rabbitMQ_Password = builder.Configuration["RabbitMQ_Password"]!;
if (string.IsNullOrEmpty(rabbitMQ_Password))
    throw new ArgumentException($"Error {assemblyName}. Parameter RabbitMQ_Password not found.");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddMassTransit(bus =>
{
    bus.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(rabbitMQ_Host.Replace("15672", "5672"), rabbitMQ_VHost, h =>
        {
            h.Username(rabbitMQ_Username);
            h.Password(rabbitMQ_Password);
        });

        cfg.Message<RegisterUserEvent>(message =>
        {
            message.SetEntityName(rabbitMQ_Queue);
        });
    });

});

builder.Services.Configure<MassTransitHostOptions>(options =>
{
    options.WaitUntilStarted = true;
    options.StartTimeout = TimeSpan.FromSeconds(30);
    options.StopTimeout = TimeSpan.FromMinutes(1);
});

builder.Services.AddMassTransitHostedService();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProducerService, ProducerService>();

var app = builder.Build();

app.UseCors("AllowAnyOrigin");

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
