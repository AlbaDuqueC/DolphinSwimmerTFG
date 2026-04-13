using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using SwimmingApi.Api;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Application.Interfaces.UseCase;
using SwimmingApi.Application.UseCase;
using SwimmingApi.Infraestructura.Database;
using SwimmingApi.Infraestructura.Repositorios;
using SwimmingApi.Infraestructura.Rider;
using SwimmingApi.Infraestructura.Servicios;
using SwimmingApi.Infraestructura.Validaciones;

var builder = WebApplication.CreateBuilder(args);

// ─── Base de datos PostgreSQL ────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ─── Caché en memoria ────────────────────────────────────────────────────────
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<CacheService>();

// ─── Servicios de infraestructura ────────────────────────────────────────────
builder.Services.AddScoped<EncryptionService>();

// ─── Repositorios ────────────────────────────────────────────────────────────
builder.Services.AddScoped<INadadorRepository, NadadorRepository>();
builder.Services.AddScoped<INadadorEquipoRepository, NadadorEquipoRepository>();
builder.Services.AddScoped<IEntrenadorRepository, EntrenadorRepository>();
builder.Services.AddScoped<IEquipoRepository, EquipoRepository>();
builder.Services.AddScoped<IRutinaRepository, RutinaRepository>();
builder.Services.AddScoped<IMarcaRepository, MarcaRepository>();

// ─── Validaciones de infraestructura ─────────────────────────────────────────
builder.Services.AddScoped<NadadorInfraValidation>();
builder.Services.AddScoped<NadadorEquipoInfraValidation>();
builder.Services.AddScoped<EntrenadorInfraValidation>();
builder.Services.AddScoped<EquipoInfraValidation>();
builder.Services.AddScoped<RutinaInfraValidation>();
builder.Services.AddScoped<MarcaDeTiempoInfraValidation>();

// ─── Casos de uso ─────────────────────────────────────────────────────────────
builder.Services.AddScoped<INadadorUseCase, NadadorUseCase>();
builder.Services.AddScoped<INadadorEquipoUseCase, NadadorEquipoUseCase>();
builder.Services.AddScoped<IEntrenadorUseCase, EntrenadorUseCase>();
builder.Services.AddScoped<IEquipoUseCase, EquipoUseCase>();
builder.Services.AddScoped<IRutinaUseCase, RutinaUseCase>();
builder.Services.AddScoped<IMarcaDeTiempoUseCase, MarcaDeTiempoUseCase>();

// ─── Controladores y FluentValidation ────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// ─── Manejador global de excepciones ─────────────────────────────────────────
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// ─── Swagger ──────────────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "SwimmingApi",
        Version = "v1",
        Description = "API REST para gestión de nadadores, equipos, rutinas y marcas de tiempo."
    });

    // Incluir los comentarios XML en Swagger
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

// ─── CORS (para Kotlin/Render) ────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ─── Middleware ───────────────────────────────────────────────────────────────
app.UseExceptionHandler();
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "SwimmingApi v1");
        options.RoutePrefix = string.Empty; // Swagger en la raíz "/"
    });
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
