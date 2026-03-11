using FluentValidation;
using SwimmingApi.Application.Dtos.Equipo;

namespace SwimmingApi.Api.Validaciones;

/// <summary>
/// Validador del DTO de Equipo.
/// </summary>
public class EquipoValidator : AbstractValidator<EquipoRequestDto>
{
    public EquipoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del equipo es obligatorio.")
            .MaximumLength(150).WithMessage("El nombre no puede superar 150 caracteres.");
    }
}
