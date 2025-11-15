using Cards_Products_API.Data;
using Cards_Products_API.Interfaces;
using Cards_Products_API.Jobs;
using Cards_Products_API.Services;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Logs;
using Quartz;
using Quartz.Simpl;

var builder = WebApplication.CreateBuilder(args);

var serviceName = "Main-Database";
var serviceVersion = "1.0.0";
var endpoint = new Uri(Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") ?? "http://otel-collector:4317");

// Obtener el connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registrar DbContext con MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// LOGS
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddOpenTelemetry(options =>
{
    options.IncludeFormattedMessage = true;
    options.ParseStateValues = true;

    options.SetResourceBuilder(
        ResourceBuilder.CreateDefault()
                       .AddService(serviceName: serviceName, serviceVersion: serviceVersion));

    options.AddOtlpExporter(opt => { opt.Endpoint = endpoint; });
});

// Agregar servicios
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new ConvertDateOnly());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<RabbitMQService>();
builder.Services.AddScoped<IPurchaseDetailService, PurchaseDetailService>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PurchaseDetailJob>();

//Configuracion de Keycloak
builder.Services.AddHttpClient();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.MetadataAddress = "http://26.9.80.46:8080/realms/Paradigmas/.well-known/openid-configuration";
        options.Authority = "http://26.9.80.46:8080/realms/Paradigmas";
        options.Audience = "payment-api";
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = " API", Version = "v1" });

    // Configure Swagger to use JWT Bearer authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
                }
        });
});

// Quartz Job
builder.Services.AddQuartz(q =>
{
    q.UseJobFactory<MicrosoftDependencyInjectionJobFactory>();

    q.AddJob<GenerateDataJob>(opts => opts.WithIdentity("GenerateDataJob").StoreDurably());
    q.AddJob<PurchaseJob>(opts => opts.WithIdentity("PurchaseJob").StoreDurably());
});

builder.Services.AddQuartzHostedService(opt =>
{
    opt.WaitForJobsToComplete = true;
});

var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.UseHttpsRedirection();

    app.UseAuthorization();
    app.UseAuthorization();

app.MapControllers();

    app.Run();