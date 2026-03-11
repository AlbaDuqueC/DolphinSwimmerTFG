# SwimmingApi

API REST en C# (.NET 8) con PostgreSQL, Entity Framework Core y Swagger.
Diseñada para conectarse con una aplicación móvil en Kotlin, desplegada en Render.

---

## Arquitectura en capas

```
api/              → Solo conoce Application
application/      → Conoce Api e Infraestructura
infraestructura/  → Conoce Application y Domain
domain/           → No conoce a nadie
```

### Carpetas principales

| Carpeta | Responsabilidad |
|---|---|
| `api/controller` | Endpoints HTTP, recibe peticiones y devuelve respuestas |
| `api/validaciones` | Validación de DTOs con FluentValidation |
| `application/usecase` | Lógica de negocio, try/catch y rollback |
| `application/dtos` | Objetos de transferencia de datos |
| `application/interfaces` | Contratos entre capas |
| `infraestructura/repositorios` | Consultas a PostgreSQL |
| `infraestructura/validaciones` | Validaciones que requieren BD |
| `infraestructura/rider` | Caché en memoria (CacheService) |
| `infraestructura/servicios` | Encriptación de contraseñas |
| `domain/entities` | Entidades del dominio |
| `domain/relaciones` | Configuración de FKs con EF Core |
| `domain/extensiones` | SlugExtension (fechas por defecto) |

---

## Configuración inicial

### 1. Requisitos
- .NET 8 SDK
- PostgreSQL 14+

### 2. Cadena de conexión

Edita `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=SwimmingDb;Username=postgres;Password=TU_PASSWORD"
}
```

En **Render**, configura la variable de entorno `ConnectionStrings__DefaultConnection` con la URL de tu base de datos PostgreSQL.

### 3. Migraciones

Las migraciones se crean desde el código y se aplican automáticamente al arrancar la API.

```bash
# Crear una migración inicial
dotnet ef migrations add InitialCreate

# Aplicar migraciones manualmente (opcional, la API lo hace sola al arrancar)
dotnet ef database update
```

### 4. Arrancar la API

```bash
dotnet run
```

Swagger disponible en: `http://localhost:5000`

---

## Entidades principales

| Entidad | Hereda de | Descripción |
|---|---|---|
| `EntityBase` | — | Base con Id, CreatedAt, UpdateAt, DeleteAt |
| `Usuario` | EntityBase | Usuario base del sistema |
| `Nadador` | Usuario | Nadador registrado |
| `Entrenador` | Usuario | Entrenador que gestiona equipos |
| `NadadorEquipo` | EntityBase | Registro de nadador dentro de un equipo |
| `Equipo` | EntityBase | Equipo de natación |
| `Rutina` | EntityBase | Rutina de entrenamiento |
| `MarcaDeTiempo` | EntityBase | Marca de tiempo de un nadador |

### Eliminación lógica
Ninguna entidad se elimina de la base de datos. Se inserta la fecha en `DeleteAt`
y un filtro global de EF Core las excluye automáticamente de todos los listados.

---

## Endpoints principales

### Nadadores `GET /api/nadador`
- `GET /api/nadador` — Todos los nadadores
- `GET /api/nadador/{id}` — Por ID
- `POST /api/nadador` — Crear
- `PUT /api/nadador/{id}` — Actualizar
- `DELETE /api/nadador/{id}` — Eliminar lógicamente

### NadadorEquipo `GET /api/nadadorequipo`
- `GET /api/nadadorequipo/equipo/{idEquipo}` — Nadadores de un equipo
- `GET /api/nadadorequipo/codigo/{codigo}` — Por código de conexión
- `POST /api/nadadorequipo` — Crear (solo entrenador)

### Equipos, Entrenadores, Rutinas, MarcasDeTiempo
Misma estructura CRUD con rutas `/api/equipo`, `/api/entrenador`, `/api/rutina`, `/api/marcadetiempo`.

---

## Despliegue en Render

1. Conecta tu repositorio en Render como **Web Service**.
2. Build Command: `dotnet publish -c Release -o out`
3. Start Command: `dotnet out/SwimmingApi.dll`
4. Añade la variable de entorno `ConnectionStrings__DefaultConnection` con tu PostgreSQL de Render.
5. La API aplica las migraciones automáticamente al arrancar.
