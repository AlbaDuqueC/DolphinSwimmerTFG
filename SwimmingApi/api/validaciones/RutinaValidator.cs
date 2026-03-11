using FluentValidation;
using SwimmingApi.Application.Dtos.Rutina;

namespace SwimmingApi.Api.Validaciones;

/// <summary>
/// Validador del DTO de Rutina.
/// </summary>
public class RutinaValidator : AbstractValidator<RutinaRequestDto>
{
    public RutinaValidator()
    {
        RuleFor(x => x.Contenido)
            .NotEmpty().WithMessage("El contenido de la rutina es obligatorio.");

        RuleFor(x => x.Fecha)
            .NotEmpty().WithMessage("La fecha es obligatoria.");

        RuleFor(x => x.IdUsuario)
            .GreaterThan(0).WithMessage("El ID de usuario es obligatorio.");
    }
}
