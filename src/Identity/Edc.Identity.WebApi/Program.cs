using System.Diagnostics;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Edc.Identity;
using Edc.Identity.Infrastructure;
using Edc.Identity.Infrastructure.MongoDb;
using Edc.Identity.Infrastructure.MongoDb.Settings;
using NLog.Web;
using Edc.Identity.WebApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
logger.Info("-------- start identity service !!! --------");

var activitySource = new ActivitySource("identity-activity-source");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddSingleton(activitySource);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Logging.ClearProviders();
builder.Host.UseNLog();

AddAuthentication(builder);
builder.Services.AddAuthorization();

SetIocContainer(builder);

AddOpenTelemetry(builder);

AddMongoConnection(builder);

AddConfiguration(builder);

AddConfiguration(builder);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAllOrigins");

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

static void SetIocContainer(WebApplicationBuilder builder)
{
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule<IdentityModule>();
        containerBuilder.RegisterModule<IdentityInfrastructureModule>();
        containerBuilder.RegisterModule<IdentityWebApiModule>();
    });
}

static void AddOpenTelemetry(WebApplicationBuilder builder)
{
    var resourceBuilder = ResourceBuilder
        .CreateDefault()
        .AddService(".Net Log Service");

    builder.Logging.AddOpenTelemetry(logging =>
    {
        logging.IncludeFormattedMessage = true;
        logging.IncludeScopes = true;

        logging.SetResourceBuilder(resourceBuilder)
            .AddOtlpExporter();
    });

    // builder.Services
    //     .AddOpenTelemetry()
    //     .ConfigureResource(resource => resource.AddService(serviceName: DiagnosticConfig.ServiceName))
    //     .WithTracing(opt =>
    //     {
    //         opt
    //             .AddAspNetCoreInstrumentation()
    //             .AddHttpClientInstrumentation()
    //             .AddOtlpExporter();
    //     })
    //     .WithMetrics(opt =>
    //     {
    //         opt
    //             .AddMeter(DiagnosticConfig.Meter.Name)
    //             .AddAspNetCoreInstrumentation()
    //             .AddRuntimeInstrumentation()
    //             .AddProcessInstrumentation()
    //             .AddHttpClientInstrumentation()
    //             .AddOtlpExporter();
    //     });
}

static void AddAuthentication(WebApplicationBuilder builder)
{
    var secretKey = builder.Configuration.GetValue<string>("JwtSettings:Key")
                    ?? throw new AggregateException();
    var issuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer")
                 ?? throw new AggregateException();
    var audience = builder.Configuration.GetValue<string>("JwtSettings:Audience")
                   ?? throw new AggregateException();
    
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            var config = builder.Configuration;
        
            var secretKey = config["JwtSettings:Key"];
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };
        });
}

static void AddConfiguration(WebApplicationBuilder builder)
{
    builder.Services.Configure<Edc.Identity.Infrastructure.MongoDb.Settings.MongoUsersSettings>(
        builder.Configuration.GetSection("MongoUsers"));
    
    builder.Services.Configure<Edc.Identity.WebApi.Settings.JwtSettings>(
        builder.Configuration.GetSection("JwtSettings"));
}

static void AddMongoConnection(WebApplicationBuilder builder)
{
    var mongoHost = builder.Configuration.GetValue<string>("MongoHosts") 
                    ?? throw new ApplicationException("failed fetch mongodb host details");
    var mongoClientHolder = new MongoClientHolder(mongoHost);
    builder.Services.AddSingleton<IMongoClientHolder>(mongoClientHolder);
}
