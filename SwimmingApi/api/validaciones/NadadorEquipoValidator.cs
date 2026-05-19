using FluentValidation;
using SwimmingApi.Application.Dtos.MarcaDeTiempo;
using SwimmingApi.Application.Dtos.NadadorEquipo;

namespace SwimmingApi.Api.Validaciones;

/// <summary>
/// Validador del DTO de NadadorEquipo.
/// Comprueba que los datos del nadador que se va a registrar en el equipo sean correctos.
/// </summary>
public class NadadorEquipoValidator : AbstractValidator<NadadorEquipoRequestDto>
{
    /// <summary>
    /// Define las reglas de validación para los campos del DTO.
    /// </summary>
    public NadadorEquipoValidator()
    {
        // El nombre es obligatorio y no puede tener más de 100 caracteres.
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres.");

        // Los apellidos son obligatorios y no pueden tener más de 150 caracteres.
        RuleFor(x => x.Apellidos)
            .NotEmpty().WithMessage("Los apellidos son obligatorios.")
            .MaximumLength(150).WithMessage("Los apellidos no pueden superar 150 caracteres.");
    }
}