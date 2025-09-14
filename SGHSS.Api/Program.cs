using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using SGHSS.Core.Validations;
using SGHSS.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Serilog;
using SGHSS.Api.Middlewares;
using SGHSS.Core.Services;

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/sghss-.log", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Integrar o Serilog
builder.Host.UseSerilog();

// Configuração de serviços
ConfigureServices(builder);

var app = builder.Build();

// Configuração do pipeline HTTP
ConfigurePipeline(app);

// testar o serilog
Log.Information("🚀 SGHSS API inicializada com sucesso!");

app.Run();

return;

void ConfigureServices(WebApplicationBuilder webApplicationBuilder)
{
    // Controllers
    webApplicationBuilder.Services.AddControllers();
    
    // FluentValidation
    webApplicationBuilder.Services.AddFluentValidationAutoValidation();
    webApplicationBuilder.Services.AddValidatorsFromAssemblyContaining<CreatePacienteDtoValidator>();

    // AutoMapper
    webApplicationBuilder.Services.AddAutoMapper(typeof(SGHSS.Infrastructure.Mappings.MappingProfile));

    // Swagger com auth
    webApplicationBuilder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "SGHSS API", Version = "v1" });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Insira o token JWT assim: Bearer {seu token}"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                []
            }
        });
    });

    // DbContext com SQL Server
    webApplicationBuilder.Services.AddDbContext<SghssContext>(options =>
        options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection")));

    // Autenticação JWT
    var jwtSettings = webApplicationBuilder.Configuration.GetSection("Jwt");
    var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

    webApplicationBuilder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            // Customizando resposta 401 e 403
            options.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.HandleResponse(); // evita resposta padrão
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                    {
                        statusCode = 401,
                        message = "Token inválido ou ausente. Faça login para continuar."
                    }));
                },
                OnForbidden = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                    {
                        statusCode = 403,
                        message = "Você não tem permissão para acessar este recurso."
                    }));
                }
            };
        });
    
    // Serviço de autenticação
    webApplicationBuilder.Services.AddScoped<JwtTokenService>();

}

void ConfigurePipeline(WebApplication webApplication)
{
    if (webApplication.Environment.IsDevelopment())
    {
        webApplication.UseSwagger();
        webApplication.UseSwaggerUI();
    }

    webApplication.UseHttpsRedirection();
    
    webApplication.UseGlobalErrorHandling();

    webApplication.UseAuthentication();
    webApplication.UseAuthorization();

    webApplication.MapControllers();
}