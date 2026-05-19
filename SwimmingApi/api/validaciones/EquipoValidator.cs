using FluentValidation;
using SwimmingApi.Application.Dtos.Equipo;

namespace SwimmingApi.Api.Validaciones;

/// <summary>
/// Validador del DTO de Equipo.
/// Comprueba que el nombre del equipo sea válido al crearlo o renombrarlo.
/// </summary>
public class EquipoValidator : AbstractValidator<EquipoRequestDto>
{
    /// <summary>
    /// Define las reglas de validación para los campos del DTO.
    /// </summary>
    public EquipoValidator()
    {
        // El nombre del equipo es obligatorio y no puede tener más de 150 caracteres.
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del equipo es obligatorio.")
            .MaximumLength(150).WithMessage("El nombre no puede superar 150 caracteres.");
    }
}