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

// ═══════════════════════════════════════════════════════════════════════════════
// PUNTO DE ENTRADA DE LA APLICACIÓN
// Configura todos los servicios (inyección de dependencias), la base de datos,
// el middleware HTTP y arranca el servidor web.
// ═══════════════════════════════════════════════════════════════════════════════

var builder = WebApplication.CreateBuilder(args);

// ─── Base de datos PostgreSQL ────────────────────────────────────────────────
// Selección de la cadena de conexión:
//   - En Render: se obtiene de la variable de entorno DATABASE_URL.
//   - En local:  se lee del fichero appsettings.json (sección ConnectionStrings).
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

// Render proporciona la URL en formato "postgres://usuario:contraseña@host:puerto/bd",
// pero Npgsql (el driver de PostgreSQL para .NET) necesita el formato "Host=...;Port=...;...".
// Aquí parseamos la URL y la transformamos al formato que Npgsql entiende.
if (!string.IsNullOrEmpty(connectionString) &&
    (connectionString.StartsWith("postgres://") || connectionString.StartsWith("postgresql://")))
{
    var uri = new Uri(connectionString);
    var userInfo = uri.UserInfo.Split(':');
    var dbPort = uri.Port > 0 ? uri.Port : 5432; // Puerto por defecto de PostgreSQL si no viene en la URL.
    var database = uri.AbsolutePath.TrimStart('/');
    // SSL Mode=Require: obligatorio en Render para conexiones seguras a la BD.
    // Trust Server Certificate=true: acepta el certificado del servidor sin validar la CA.
    connectionString = $"Host={uri.Host};Port={dbPort};Database={database};Username={userInfo[0]};Password={Uri.UnescapeDataString(userInfo[1])};SSL Mode=Require;Trust Server Certificate=true";
}

// Registro del contexto de Entity Framework con el proveedor de PostgreSQL (Npgsql).
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// ─── Caché en memoria ────────────────────────────────────────────────────────
// AddMemoryCache: habilita el sistema de caché en memoria de .NET.
// CacheService: el servicio personalizado se registra como Singleton para que sea único en toda la app.
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<CacheService>();

// ─── Servicios de infraestructura ────────────────────────────────────────────
// Servicio de encriptación de contraseñas. Scoped: una instancia por petición HTTP.
builder.Services.AddScoped<EncryptionService>();

// ─── Repositorios ────────────────────────────────────────────────────────────
// Cada interfaz se asocia a su implementación concreta.
// Scoped: se crea una nueva instancia por cada petición HTTP, lo que es lo recomendado
// para clases que dependen del DbContext (también Scoped).
builder.Services.AddScoped<INadadorRepository, NadadorRepository>();
builder.Services.AddScoped<INadadorEquipoRepository, NadadorEquipoRepository>();
builder.Services.AddScoped<IEntrenadorRepository, EntrenadorRepository>();
builder.Services.AddScoped<IEquipoRepository, EquipoRepository>();
builder.Services.AddScoped<IRutinaRepository, RutinaRepository>();
builder.Services.AddScoped<IMarcaRepository, MarcaRepository>();

// ─── Validaciones de infraestructura ─────────────────────────────────────────
// Validaciones que necesitan acceder a la base de datos (ej: comprobar emails duplicados).
builder.Services.AddScoped<NadadorInfraValidation>();
builder.Services.AddScoped<NadadorEquipoInfraValidation>();
builder.Services.AddScoped<EntrenadorInfraValidation>();
builder.Services.AddScoped<EquipoInfraValidation>();
builder.Services.AddScoped<RutinaInfraValidation>();
builder.Services.AddScoped<MarcaDeTiempoInfraValidation>();

// ─── Casos de uso ─────────────────────────────────────────────────────────────
// Lógica de negocio de la aplicación. Cada caso de uso se inyecta en su controller.
builder.Services.AddScoped<INadadorUseCase, NadadorUseCase>();
builder.Services.AddScoped<INadadorEquipoUseCase, NadadorEquipoUseCase>();
builder.Services.AddScoped<IEntrenadorUseCase, EntrenadorUseCase>();
builder.Services.AddScoped<IEquipoUseCase, EquipoUseCase>();
builder.Services.AddScoped<IRutinaUseCase, RutinaUseCase>();
builder.Services.AddScoped<IMarcaDeTiempoUseCase, MarcaDeTiempoUseCase>();

// ─── Controladores y FluentValidation ────────────────────────────────────────
// AddControllers: habilita los controladores REST.
// AddFluentValidationAutoValidation: ejecuta automáticamente los validadores
//   antes de que el controller reciba el DTO, devolviendo 400 si los datos son inválidos.
// AddValidatorsFromAssemblyContaining: registra todos los validadores definidos en el proyecto.
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// ─── Manejador global de excepciones ─────────────────────────────────────────
// Registra el GlobalExceptionHandler para que capture cualquier excepción no controlada
// y devuelva una respuesta JSON estandarizada en lugar de un error 500 sin formato.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// ─── Swagger ──────────────────────────────────────────────────────────────────
// Genera automáticamente una interfaz web para probar los endpoints de la API
// y muestra la documentación basada en los comentarios XML del código.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "SwimmingApi",
        Version = "v1",
        Description = "API REST para gestión de nadadores, equipos, rutinas y marcas de tiempo."
    });

    // Carga los comentarios XML generados durante la compilación
    // para que se muestren como descripciones en la UI de Swagger.
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

// ─── CORS (para Kotlin/Render) ────────────────────────────────────────────────
// Política que permite que la app Android (desde cualquier origen) consuma la API.
// En producción real conviene restringirlo a los dominios concretos.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ═══════════════════════════════════════════════════════════════════════════════
// CONSTRUCCIÓN DE LA APLICACIÓN Y CONFIGURACIÓN DEL PIPELINE HTTP
// ═══════════════════════════════════════════════════════════════════════════════

var app = builder.Build();

// ─── Middleware ───────────────────────────────────────────────────────────────
// El orden importa: cada middleware se ejecuta en el orden en que se registra.

// 1. Captura cualquier excepción no controlada y la convierte en una respuesta JSON.
app.UseExceptionHandler();

// 2. Aplica la política CORS para permitir peticiones desde el cliente.
app.UseCors("AllowAll");

// Swagger se habilita tanto en desarrollo como en producción para poder probar
// los endpoints directamente desde la URL pública de la API.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "SwimmingApi v1");
        options.RoutePrefix = string.Empty; // Swagger queda accesible en la raíz "/".
    });
}

//app.UseHttpsRedirection();    // Redirección a HTTPS desactivada (Render ya lo gestiona).
app.UseAuthorization();         // Habilita el middleware de autorización.
app.MapControllers();            // Mapea las rutas de los controllers.

// ─── Configuración del puerto en Render ──────────────────────────────────────
// Render asigna dinámicamente el puerto del servidor a través de la variable de entorno PORT.
// En local esta variable no existe, por lo que se usa la configuración por defecto
// definida en launchSettings.json (típicamente https://localhost:5001).
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    app.Urls.Clear();
    app.Urls.Add($"http://0.0.0.0:{port}");
}

app.Run();