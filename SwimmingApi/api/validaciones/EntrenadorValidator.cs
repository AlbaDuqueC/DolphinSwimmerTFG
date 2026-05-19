using FluentValidation;
using SwimmingApi.Application.Dtos.Entrenador;

namespace SwimmingApi.Api.Validaciones;

/// <summary>
/// Validador del DTO de Entrenador.
/// Comprueba que los datos enviados al crear o actualizar un entrenador
/// sean correctos antes de que lleguen al caso de uso.
/// </summary>
public class EntrenadorValidator : AbstractValidator<EntrenadorRequestDto>
{
    /// <summary>
    /// Define las reglas de validación para los campos del DTO.
    /// </summary>
    public EntrenadorValidator()
    {
        // El nombre es obligatorio y no puede tener más de 100 caracteres.
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres.");

        // Los apellidos son obligatorios y no pueden tener más de 150 caracteres.
        RuleFor(x => x.Apellidos)
            .NotEmpty().WithMessage("Los apellidos son obligatorios.")
            .MaximumLength(150).WithMessage("Los apellidos no pueden superar 150 caracteres.");

        // El email es obligatorio y debe tener formato válido.
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es obligatorio.")
            .EmailAddress().WithMessage("El formato del email no es válido.");

        // La contraseña solo se valida cuando viene rellena.
        // Al actualizar perfil llega vacía y no se quiere cambiar.
        RuleFor(x => x.Password)
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Password));
    }
}