using System.Text.Json;
using CDM.Match;
using CDM.Match.Consumer;
using CDM.Match.Repository;
using CMD.Match.Messaging;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using UrbanFlow_trips.Infrastucture.Messaging;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configuration Dbcontext
builder.Services.AddDbContext<MatchDbContext>(options =>
    options.UseNpgsql(connectionString));

// Injection de dépendances des repositories
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IRabbitMQService, RabbitMQService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

// MassTransit configuration
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<GetOddsConsumer>();

    x.SetDefaultEndpointNameFormatter();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h =>
        {
            h.Username("user");
            h.Password("password");
        });

        cfg.ConfigureJsonSerializerOptions(options =>
        {
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            return options;
        });
        
        
        cfg.ReceiveEndpoint("get-odds", e =>
        {
            
            e.UseRawJsonDeserializer();
            e.ConfigureConsumer<GetOddsConsumer>(context);
            
        });
    });
});

var app = builder.Build();


// Update de la ddb à partir de la dernière migration
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<MatchDbContext>();
        
        context.Database.Migrate();
        
        Console.WriteLine("Database migrations OK.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database migration NOT OK: {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    app.MapScalarApiReference(options =>
    {
        options.Title = "Match API";
        options.Theme = ScalarTheme.Moon;
    });
}

app.UseHttpsRedirection();  
app.UseAuthorization();
app.MapControllers();

app.Run();