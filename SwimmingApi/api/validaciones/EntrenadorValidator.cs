using FluentValidation;
using SwimmingApi.Application.Dtos.Entrenador;

namespace SwimmingApi.Api.Validaciones;

/// <summary>
/// Validador del DTO de Entrenador.
/// </summary>
public class EntrenadorValidator : AbstractValidator<EntrenadorRequestDto>
{
    public EntrenadorValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres.");

        RuleFor(x => x.Apellidos)
            .NotEmpty().WithMessage("Los apellidos son obligatorios.")
            .MaximumLength(150).WithMessage("Los apellidos no pueden superar 150 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es obligatorio.")
            .EmailAddress().WithMessage("El formato del email no es válido.");

        // Password solo se valida cuando viene rellena (no al actualizar perfil).
        RuleFor(x => x.Password)
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Password));
    }
}
