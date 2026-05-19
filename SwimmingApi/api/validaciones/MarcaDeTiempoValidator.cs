using FluentValidation;
using SwimmingApi.Application.Dtos.MarcaDeTiempo;
using SwimmingApi.Application.Dtos.NadadorEquipo;

namespace SwimmingApi.Api.Validaciones;

/// <summary>
/// Validador del DTO de MarcaDeTiempo.
/// Comprueba que el tiempo y la descripción de la marca sean válidos
/// antes de registrarla en el sistema.
/// </summary>
public class MarcaDeTiempoValidator : AbstractValidator<MarcaDeTiempoRequestDto>
{
    /// <summary>
    /// Define las reglas de validación para los campos del DTO.
    /// </summary>
    public MarcaDeTiempoValidator()
    {
        // El tiempo es obligatorio y debe ser estrictamente mayor que cero.
        RuleFor(x => x.Tiempo)
            .NotEmpty().WithMessage("El tiempo es obligatorio.")
            .Must(t => t > TimeSpan.Zero).WithMessage("El tiempo debe ser mayor que cero.");

        // La descripción de la prueba es obligatoria y no puede superar 200 caracteres.
        RuleFor(x => x.Descripcion)
            .NotEmpty().WithMessage("La descripción de la prueba es obligatoria.")
            .MaximumLength(200).WithMessage("La descripción no puede superar 200 caracteres.");
    }
}