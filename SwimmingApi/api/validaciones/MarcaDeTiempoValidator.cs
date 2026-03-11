using FluentValidation;
using SwimmingApi.Application.Dtos.MarcaDeTiempo;

namespace SwimmingApi.Api.Validaciones;

/// <summary>
/// Validador del DTO de MarcaDeTiempo.
/// </summary>
public class MarcaDeTiempoValidator : AbstractValidator<MarcaDeTiempoRequestDto>
{
    public MarcaDeTiempoValidator()
    {
        RuleFor(x => x.Tiempo)
            .NotEmpty().WithMessage("El tiempo es obligatorio.")
            .Must(t => t > TimeSpan.Zero).WithMessage("El tiempo debe ser mayor que cero.");

        RuleFor(x => x.Descripcion)
            .NotEmpty().WithMessage("La descripción de la prueba es obligatoria.")
            .MaximumLength(200).WithMessage("La descripción no puede superar 200 caracteres.");

        RuleFor(x => x.IdNadadorEquipo)
            .GreaterThan(0).WithMessage("El ID de NadadorEquipo es obligatorio.");
    }
}
