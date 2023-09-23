using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieServiceWebAPI.Data;
using MovieServiceWebAPI.Services;
using Serilog;

try
{

    var builder = WebApplication.CreateBuilder(args);

    // Add logger
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration).CreateLogger();

    Log.Information("Starting Movie Web API");


    builder.Host.ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    });

    // Add services to the container.
    builder.Services.AddScoped<IGenreRepository, GenreRepository>();
    builder.Services.AddScoped<IMovieRepository, MovieRepository>();

    builder.Services.AddControllers();

    builder.Services.AddDbContext<MyDBContext>(options => {
        options.UseSqlServer(builder.Configuration.GetConnectionString("connectionString"));
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

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
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

