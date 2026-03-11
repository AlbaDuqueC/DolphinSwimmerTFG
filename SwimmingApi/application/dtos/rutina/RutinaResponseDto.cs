namespace SwimmingApi.Application.Dtos.Rutina;

/// <summary>
/// DTO de respuesta con los datos de una rutina.
/// </summary>
public class RutinaResponseDto
{
    public int Id { get; set; }
    public int IdRutina { get; set; }
    public string Contenido { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public bool Mostrar { get; set; }
    public int IdUsuario { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdateAt { get; set; }
}
