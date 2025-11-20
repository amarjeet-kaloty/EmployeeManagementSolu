using Application.Mappers;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Conventions;
using Domain.Services;
using EmployeeManagementSolu.Domain.Entities;
using EmployeeManagementSolu.Domain.Interfaces;
using EmployeeManagementSolu.Domain.Validation;
using EmployeeManagementSolu.Infrastructure;
using FluentValidation;
using Infrastructure.Services;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Presentation.Filters;
using Presentation.Swagger;
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

builder.Services.AddScoped<Presentation.Messaging.MessagePublisher>();

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
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile(new EmployeeProfile());
});
builder.Services.AddControllers().AddDapr();

// Configuration: Swagger for Authorization and Authentication
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Description = "Please enter a valid Bearer Authorization token into the field.",
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new string[] {}
        }
    });

    options.SupportNonNullableReferenceTypes();
    options.OperationFilter<SimplifyErrorResponseFilter>();
});

// API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.ConfigureOptions<SwaggerAPIVersioningOptions>();

// OAuth 2.0 Local Config
builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);

builder.Services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.Events = new JwtBearerEvents();

    options.Events.OnChallenge = context =>
    {
        context.HandleResponse();
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "text/plain";
         
        string responseString = "Access Denied. A valid Bearer token is required.";
        if (context.AuthenticateFailure != null)
        {
            responseString = $"Authentication Failed. Reason: {context.AuthenticateFailure.Message}";
        }

        return context.Response.WriteAsync(responseString);
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SupervisorOnly", policy =>
    {
        policy.RequireRole("supervisor");
    });

    options.AddPolicy("ManagerOnly", policy =>
    {
        policy.RequireRole("manager");
    });

    options.AddPolicy("SupervisorOrManager", policy =>
    {
        policy.RequireRole("supervisor", "manager");
    });
}).AddKeycloakAuthorization(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });

}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();