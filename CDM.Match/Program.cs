using CDM.Match;
using CDM.Match.Repository;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configuration Dbcontext
builder.Services.AddDbContext<MatchDbContext>(options =>
    options.UseNpgsql(connectionString));

// Injection de dépendances des repositories
builder.Services.AddScoped<IMatchRepository, MatchRepository>();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

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