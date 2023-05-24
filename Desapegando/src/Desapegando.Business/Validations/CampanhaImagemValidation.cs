using Desapegando.Business.Models;
using FluentValidation;

namespace Desapegando.Business.Validations
{
    public class CampanhaImagemValidation : AbstractValidator<CampanhaImagem>
    {
        public CampanhaImagemValidation()
        {
            RuleFor(x => x.FileName)
                .NotNull()
                .NotEmpty()
                .WithMessage("O {PropertyName} deve ser informado.")
                .EmailAddress()
                .WithMessage("O {PropertyName} está em formato inválido.")
                .MaximumLength(150)
                .WithMessage("O {PropertyName} deve ter menos que 150 caracteres.");

            RuleFor(x => x.CampanhaId)
                .NotNull()
                .NotEmpty()
                .WithMessage("O {PropertyName} deve ser informado.");
        }
    }
}
