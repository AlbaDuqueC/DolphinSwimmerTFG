namespace SwimmingApi.Application.Dtos.MarcaDeTiempo;

/// <summary>
/// DTO de respuesta con los datos de una marca de tiempo.
/// </summary>
public class MarcaDeTiempoResponseDto
{
    public int Id { get; set; }
    public int IdMarca { get; set; }
    public TimeSpan Tiempo { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public int? IdNadadorEquipo { get; set; }
    public int? IdNadador { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdateAt { get; set; }
}
