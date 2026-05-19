using FluentValidation;
using SwimmingApi.Application.Dtos.Rutina;

namespace SwimmingApi.Api.Validaciones;

/// <summary>
/// Validador del DTO de Rutina.
/// Comprueba que el contenido, la fecha y el usuario asociado sean correctos
/// antes de crear o actualizar una rutina.
/// </summary>
public class RutinaValidator : AbstractValidator<RutinaRequestDto>
{
    /// <summary>
    /// Define las reglas de validación para los campos del DTO.
    /// </summary>
    public RutinaValidator()
    {
        // El contenido de la rutina es obligatorio.
        RuleFor(x => x.Contenido)
            .NotEmpty().WithMessage("El contenido de la rutina es obligatorio.");

        // La fecha es obligatoria.
        RuleFor(x => x.Fecha)
            .NotEmpty().WithMessage("La fecha es obligatoria.");

        // El usuario al que pertenece la rutina debe estar identificado.
        RuleFor(x => x.IdUsuario)
            .GreaterThan(0).WithMessage("El ID de usuario es obligatorio.");
    }
}