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
using Microsoft.AspNetCore.Authentication.JwtBearer; // <-- AGREGADO (import necesario)

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

// HttpClient para Keycloak
builder.Services.AddHttpClient();


// AUTENTICACIÓN Y AUTORIZACIÓN JWT (KEYCLOAK)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // URL del realm (importante)
    options.MetadataAddress = "http://26.9.80.46:8080/realms/Paradigmas/.well-known/openid-configuration";
    options.Authority = "http://26.9.80.46:8080/realms/Paradigmas";

    // El client-id configurado en Keycloak
    options.Audience = "payment-api";

    options.RequireHttpsMetadata = false; // desarrollo

    // Extra recommended configurations
    options.TokenValidationParameters.ValidateAudience = false; // Keycloak suele no validar audience estrictamente
});

builder.Services.AddAuthorization();

// Swagger: Añadir soporte para JWT Bearer
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Paradigmas", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando el esquema Bearer.\nEjemplo: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Quartz Jobs
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

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://26.140.16.194:5173", "https://26.140.16.194:5173",
                         "http://26.130.97.77:5173", "https://26.130.97.77:5173",
                         "http://26.131.211.94:5173", "https://26.131.211.94:5173",
                         "http://26.129.232.215:5173", "https://26.129.232.215:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
