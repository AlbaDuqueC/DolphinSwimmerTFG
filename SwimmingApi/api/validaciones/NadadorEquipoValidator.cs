using FluentValidation;
using SwimmingApi.Application.Dtos.NadadorEquipo;

namespace SwimmingApi.Api.Validaciones;

/// <summary>
/// Validador del DTO de NadadorEquipo.
/// </summary>
public class NadadorEquipoValidator : AbstractValidator<NadadorEquipoRequestDto>
{
    public NadadorEquipoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres.");

        RuleFor(x => x.Apellidos)
            .NotEmpty().WithMessage("Los apellidos son obligatorios.")
            .MaximumLength(150).WithMessage("Los apellidos no pueden superar 150 caracteres.");

        RuleFor(x => x.IdEquipo)
            .GreaterThan(0).WithMessage("El ID del equipo es obligatorio y debe ser mayor que 0.");
    }
}
