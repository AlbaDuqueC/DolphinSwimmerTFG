namespace SwimmingApi.Application.Dtos.Rutina;

/// <summary>
/// DTO para crear o actualizar una rutina.
/// </summary>
public class RutinaRequestDto
{
    public string Contenido { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public bool Mostrar { get; set; }
    public int IdUsuario { get; set; }
}
