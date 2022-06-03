using Microsoft.EntityFrameworkCore;
using PrimeiraWebAPI.DAL;
using PrimeiraWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<AlbunsService>();
builder.Services.AddTransient<AvaliacoesService>();

string connectionString = "Server=.\\SQLExpress;Database=PrimeiraAPI;Trusted_Connection=True;";
// se não estiver usando o SQLExpress tente
// Server=localhost;Database=PrimeiraAPI;Trusted_Connection=True;
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors(config => config.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthorization();
app.MapControllers();
app.Run();