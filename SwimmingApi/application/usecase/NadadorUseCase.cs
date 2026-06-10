using SwimmingApi.Application.Dtos.Nadador;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Application.Interfaces.UseCase;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Infraestructura.Rider;
using SwimmingApi.Infraestructura.Servicios;
using SwimmingApi.Infraestructura.Validaciones;

namespace SwimmingApi.Application.UseCase;

/// <summary>
/// Casos de uso para la entidad Nadador.
/// Aquí se define la lógica de negocio del nadador, incluyendo la vinculación
/// con un equipo a través del código generado por el entrenador.
/// </summary>
public class NadadorUseCase : INadadorUseCase
{
    // Acceso a la base de datos a través del repositorio de nadadores.
    private readonly INadadorRepository _repository;

    // Repositorio de NadadorEquipo, necesario para vincular cuentas con fichas del equipo.
    private readonly INadadorEquipoRepository _nadadorEquipoRepository;

    // Servicio que encripta las contraseñas con BCrypt antes de guardarlas.
    private readonly EncryptionService _encryption;

    // Servicio de caché en memoria.
    private readonly CacheService _cache;

    // Validaciones de infraestructura (ej: comprobar que el email no exista).
    private readonly NadadorInfraValidation _validation;

    /// <summary>
    /// Constructor con inyección de dependencias.
    /// </summary>
    public NadadorUseCase(
        INadadorRepository repository,
        INadadorEquipoRepository nadadorEquipoRepository,
        EncryptionService encryption,
        CacheService cache,
        NadadorInfraValidation validation)
    {
        _repository = repository;
        _nadadorEquipoRepository = nadadorEquipoRepository;
        _encryption = encryption;
        _cache = cache;
        _validation = validation;
    }

    /// <summary>
    /// Obtiene un nadador por su ID.
    /// Aplica el patrón cache-aside: busca primero en caché y, si no está,
    /// consulta la base de datos y guarda el resultado.
    /// </summary>
    public async Task<NadadorResponseDto?> ObtenerPorIdAsync(int id)
    {
        var claveCache = _cache.GenerarClave("nadador", id);
        var resultado = _cache.Obtener<NadadorResponseDto>(claveCache);
        if (resultado == null)
        {
            var nadador = await _repository.ObtenerPorIdAsync(id);
            resultado = nadador != null ? MapearAResponse(nadador) : null;
            if (resultado != null)
                _cache.Guardar(claveCache, resultado);
        }
        return resultado;
    }

    /// <summary>
    /// Busca un nadador por su correo electrónico.
    /// Utilizado principalmente al iniciar sesión para identificar al usuario.
    /// </summary>
    public async Task<NadadorResponseDto?> ObtenerPorEmailAsync(string email)
    {
        var nadador = await _repository.ObtenerPorEmailAsync(email);
        var resultado = nadador != null ? MapearAResponse(nadador) : null;
        return resultado;
    }

    /// <summary>
    /// Obtiene la lista completa de nadadores activos.
    /// La lista se cachea para acelerar lecturas posteriores.
    /// </summary>
    public async Task<IEnumerable<NadadorResponseDto>> ObtenerTodosAsync()
    {
        var claveCache = _cache.GenerarClaveLista("nadador");
        var resultado = _cache.Obtener<IEnumerable<NadadorResponseDto>>(claveCache);
        if (resultado == null)
        {
            var nadadores = await _repository.ObtenerTodosAsync();
            resultado = nadadores.Select(MapearAResponse);
            _cache.Guardar(claveCache, resultado);
        }
        return resultado;
    }

    /// <summary>
    /// Crea un nuevo nadador.
    /// Valida que el email no esté ya registrado, encripta la contraseña
    /// y limpia la caché de la lista para que el nuevo registro sea visible.
    /// </summary>
    public async Task<NadadorResponseDto> CrearAsync(NadadorRequestDto dto)
    {
        // Se valida que el email esté disponible antes de crear el registro.
        var emailDisponible = await _validation.EmailDisponibleAsync(dto.Email);
        if (!emailDisponible)
            throw new InvalidOperationException("El email ya está registrado.");

        // Se construye la entidad y se encripta la contraseña.
        var nadador = new Nadador
        {
            Nombre = dto.Nombre,
            Apellidos = dto.Apellidos,
            Email = dto.Email,
            PasswordHash = _encryption.HashPassword(dto.Password),
            FotoPerfil = dto.FotoPerfil,
            IdEquipo = dto.IdEquipo,
        };

        Nadador nadadorCreado;
        try
        {
            nadadorCreado = await _repository.CrearAsync(nadador);
        }
        catch
        {
            // Rollback: si falla la creación, no hay nada que revertir manualmente
            // porque EF Core no persiste nada si SaveChanges falla.
            throw;
        }

        _cache.Eliminar(_cache.GenerarClaveLista("nadador"));
        var resultado = MapearAResponse(nadadorCreado);
        return resultado;
    }

    /// <summary>
    /// Actualiza los datos de un nadador existente.
    /// Si la contraseña viene vacía, se conserva la actual sin tocarla.
    /// Si FotoPerfil viene nula, se conserva la foto actual.
    /// </summary>
    public async Task<NadadorResponseDto> ActualizarAsync(int id, NadadorRequestDto dto)
    {
        var nadador = await _repository.ObtenerPorIdAsync(id)
            ?? throw new KeyNotFoundException($"Nadador con ID {id} no encontrado.");

        nadador.Nombre = dto.Nombre;
        nadador.Apellidos = dto.Apellidos;
        nadador.Email = dto.Email;
        nadador.IdEquipo = dto.IdEquipo;

        // La contraseña solo se actualiza si llega informada (al editar perfil suele venir vacía).
        if (!string.IsNullOrWhiteSpace(dto.Password))
            nadador.PasswordHash = _encryption.HashPassword(dto.Password);

        // La foto solo se actualiza si llega informada; si no, se conserva la actual.
        if (dto.FotoPerfil != null)
            nadador.FotoPerfil = dto.FotoPerfil;

        var nadadorActualizado = await _repository.ActualizarAsync(nadador);

        // Se invalidan las cachés afectadas por el cambio.
        _cache.Eliminar(_cache.GenerarClave("nadador", id));
        _cache.Eliminar(_cache.GenerarClaveLista("nadador"));

        var resultado = MapearAResponse(nadadorActualizado);
        return resultado;
    }

    /// <summary>
    /// Elimina lógicamente un nadador por su ID.
    /// El registro permanece en la base de datos pero se marca como inactivo.
    /// </summary>
    public async Task<bool> EliminarAsync(int id)
    {
        var existe = await _validation.NadadorExisteAsync(id);
        if (!existe)
            throw new KeyNotFoundException($"Nadador con ID {id} no encontrado.");

        var eliminado = await _repository.EliminarLogicoAsync(id);
        if (eliminado)
        {
            _cache.Eliminar(_cache.GenerarClave("nadador", id));
            _cache.Eliminar(_cache.GenerarClaveLista("nadador"));
        }

        var resultado = eliminado;
        return resultado;
    }

    /// <summary>
    /// Vincula un nadador (usuario) con un NadadorEquipo (ficha del equipo)
    /// utilizando el código de 6 dígitos que le ha proporcionado el entrenador.
    /// Falla si el código no existe o si ya está siendo usado por otro nadador.
    /// </summary>
    public async Task<NadadorResponseDto> VincularConCodigoAsync(int idNadador, int codigo)
    {
        // 1) Se busca al nadador (usuario) por su ID.
        var nadador = await _repository.ObtenerPorIdAsync(idNadador)
            ?? throw new KeyNotFoundException($"Nadador con ID {idNadador} no encontrado.");

        // 2) Se comprueba que el nadador no esté ya vinculado a otro equipo.
        if (nadador.IdNadadorEquipo != null)
            throw new InvalidOperationException("Ya estás vinculado a un equipo.");

        // 3) Se busca el NadadorEquipo correspondiente al código introducido.
        var nadadorEquipo = await _nadadorEquipoRepository.ObtenerPorCodigoAsync(codigo)
            ?? throw new KeyNotFoundException("No existe ningún nadador con ese código.");

        // 4) Se comprueba que ese código no esté ya en uso por otro nadador.
        var todos = await _repository.ObtenerTodosAsync();
        var ocupado = todos.Any(n => n.IdNadadorEquipo == nadadorEquipo.Id);
        if (ocupado)
            throw new InvalidOperationException("Ese código ya está siendo usado por otro nadador.");

        // 5) Se realiza la vinculación actualizando el nadador con su nueva ficha y equipo.
        nadador.IdNadadorEquipo = nadadorEquipo.Id;
        nadador.IdEquipo = nadadorEquipo.IdEquipo;
        var actualizado = await _repository.ActualizarAsync(nadador);

        // 6) Se limpia la caché del nadador para que la próxima lectura sea fresca.
        _cache.Eliminar(_cache.GenerarClave("nadador", nadador.Id));
        return MapearAResponse(actualizado);
    }

    /// <summary>
    /// Convierte una entidad Nadador del dominio en su DTO de respuesta para la API.
    /// </summary>
    private NadadorResponseDto MapearAResponse(Nadador nadador)
    {
        var resultado = new NadadorResponseDto
        {
            Id = nadador.Id,
            IdNadador = nadador.Id,
            Nombre = nadador.Nombre,
            Apellidos = nadador.Apellidos,
            Email = nadador.Email,
            FotoPerfil = nadador.FotoPerfil,
            IdEquipo = nadador.IdEquipo,
            IdNadadorEquipo = nadador.IdNadadorEquipo,
            CreatedAt = nadador.CreatedAt,
            UpdateAt = nadador.UpdateAt
        };
        return resultado;
    }
}
