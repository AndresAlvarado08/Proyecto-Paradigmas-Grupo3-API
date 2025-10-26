using Cards_Products_API.Data;
using Cards_Products_API.Interfaces;
using Cards_Products_API.Jobs;
using Cards_Products_API.Services;
using Microsoft.EntityFrameworkCore;
using Quartz.Simpl;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Obtener el connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registrar DbContext con MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPurchaseDetailService, PurchaseDetailService>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICardService, CardService>();

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

    app.MapControllers();

    app.Run();