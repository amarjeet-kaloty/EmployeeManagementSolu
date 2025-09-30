using Application.Mappers;
using EmployeeManagementSolu.Domain.Entities;
using EmployeeManagementSolu.Domain.Interfaces;
using EmployeeManagementSolu.Domain.Validation;
using EmployeeManagementSolu.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Presentation.Filters;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(typeof(EmployeeManagementSolu.Application.Command.EmployeeCommands.CreateEmployeeCommand).Assembly);
var connectionString = builder.Configuration.GetConnectionString("MongoDB");
builder.Services.AddSingleton(new MongoClient(connectionString));
builder.Services.AddSingleton(serviceProvider =>
{
    var client = serviceProvider.GetRequiredService<MongoClient>();
    return client.GetDatabase(MongoUrl.Create(connectionString).DatabaseName);
});
builder.Services.AddSingleton<ConnectionFactory>();

builder.Services.AddSingleton<IConnectionFactory>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new ConnectionFactory()
    {
        HostName = configuration["RabbitMQ:HostName"],
        Port = configuration.GetValue<int>("RabbitMQ:Port"),
        UserName = configuration["RabbitMQ:Username"],
        Password = configuration["RabbitMQ:Password"]
    };
});
builder.Services.AddSingleton<Application.Messaging.MessagePublisher>();
builder.Services.AddHostedService<Application.Messaging.EmployeeCreatedConsumer>();
builder.Services.AddDbContext<DataContext>(options =>
{
    var database = builder.Services.BuildServiceProvider().GetRequiredService<IMongoDatabase>();
    options.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName);
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IValidator<Employee>, EmployeeValidator>();
builder.Services.AddScoped<EmployeeValidationService>();
builder.Services.AddScoped<CustomExceptionFilterAttribute>();
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            return new BadRequestObjectResult(new { message = "Invalid request data." });
        };
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile(new EmployeeProfile());
});
builder.Services.AddControllers().AddDapr();

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