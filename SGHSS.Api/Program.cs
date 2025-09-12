using Microsoft.EntityFrameworkCore;
using SGHSS.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Adicionar controllers
builder.Services.AddControllers();

// Adicionar Swagger (documentação)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adicionar DbContext com SQL Server
builder.Services.AddDbContext<SghssContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configurar pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();