using FluentValidation;
using SwimmingApi.Application.Dtos.Nadador;

namespace SwimmingApi.Api.Validaciones;

/// <summary>
/// Validador de los datos del DTO de Nadador.
/// Controla que los campos sean correctos antes de llegar al UseCase.
/// </summary>
public class NadadorValidator : AbstractValidator<NadadorRequestDto>
{
    public NadadorValidator()
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

        // Password solo se valida cuando viene rellena.
        // Al actualizar perfil llega vacía y no la cambiamos.
        RuleFor(x => x.Password)
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Password));

        RuleFor(x => x.IdEquipo)
            .GreaterThan(0).When(x => x.IdEquipo.HasValue)
            .WithMessage("El ID de equipo debe ser mayor que 0.");
    }
}