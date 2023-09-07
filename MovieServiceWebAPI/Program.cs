using Microsoft.EntityFrameworkCore;
using MovieServiceWebAPI.Data;
using MovieServiceWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

// Add services to the container.
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
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
