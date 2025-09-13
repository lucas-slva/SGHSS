using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using SGHSS.Core.Validations;
using SGHSS.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Adicionar controllers + FluentValidation
builder.Services.AddControllers();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();

// Descobrir e registrar todos os validators do assembly onde está CreatePacienteDtoValidator
builder.Services.AddValidatorsFromAssemblyContaining<CreatePacienteDtoValidator>();

// Adicionar AutoMapper
builder.Services.AddAutoMapper(typeof(SGHSS.Infrastructure.Mappings.MappingProfile));

// Adicionar Swagger
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