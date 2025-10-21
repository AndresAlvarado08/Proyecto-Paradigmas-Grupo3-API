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
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICardService, CardService>();

// Quartz Job
builder.Services.AddQuartz(q =>
{
    q.UseJobFactory<MicrosoftDependencyInjectionJobFactory>();

    var generateJobKey = new JobKey("GenerateDataJob");
    var jobKey = new JobKey("PurchaseJob");

    q.AddJob<GenerateDataJob>(opts => opts.WithIdentity(generateJobKey));
    q.AddJob<PurchaseJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(generateJobKey)
        .WithIdentity("GenerateDataJob-trigger")
        .WithSimpleSchedule(x => x
            .WithInterval(TimeSpan.FromSeconds(10))  // cada 10 segundos
            .WithRepeatCount(5)));                   // se repite 5 veces

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("PurchaseJob-trigger")
        .WithSimpleSchedule(x => x
            .WithInterval(TimeSpan.FromSeconds(10)) // cada 10 segundos
            .WithRepeatCount(3)));                  // se repite 3 veces
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