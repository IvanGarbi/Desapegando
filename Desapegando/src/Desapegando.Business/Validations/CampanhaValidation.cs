using Desapegando.Business.Models;
using FluentValidation;

namespace Desapegando.Business.Validations
{
    public class CampanhaValidation : AbstractValidator<Campanha>
    {
        public CampanhaValidation()
        {
            RuleFor(x => x.Nome)
                .NotNull()
                .NotEmpty()
                .WithMessage("O {PropertyName} deve ser informado.")
                .MaximumLength(100)
                .WithMessage("O {PropertyName} deve ter menos que 100 caracteres.");

            RuleFor(x => x.NomeInstituicao)
                .NotNull()
                .NotEmpty()
                .WithMessage("O {PropertyName} deve ser informado.")
                .MaximumLength(100)
                .WithMessage("O {PropertyName} deve ter menos que 100 caracteres.");

            RuleFor(x => x.NomeResponsavel)
                .NotNull()
                .NotEmpty()
                .WithMessage("O {PropertyName} deve ser informado.")
                .MaximumLength(100)
                .WithMessage("O {PropertyName} deve ter menos que 100 caracteres.");

            RuleFor(x => x.Descricao)
                .NotNull()
                .NotEmpty()
                .WithMessage("O {PropertyName} deve ser informado.")
                .MaximumLength(200)
                .WithMessage("O {PropertyName} deve ter menos que 200 caracteres.");

            RuleFor(x => x.TelefoneResponsavel)
                .NotNull()
                .NotEmpty()
                .WithMessage("O {PropertyName} deve ser informado.")
                .Matches("^[0-9]+$")
                .WithMessage("O {PropertyName} deve conter somente números.")
                .MaximumLength(20)
                .WithMessage("O {PropertyName} deve ter menos que 20 caracteres.");

            RuleFor(x => x.EmailResponsavel)
                .NotNull()
                .NotEmpty()
                .WithMessage("O {PropertyName} deve ser informado.")
                .EmailAddress()
                .WithMessage("O {PropertyName} está em formato inválido.")
                .MaximumLength(100)
                .WithMessage("O {PropertyName} deve ter menos que 100 caracteres.");

            RuleFor(x => x.LocalDeEncontro)
                .NotEmpty()
                .NotNull()
                .WithMessage("O {PropertyName} deve ser informado.")
                .MaximumLength(50)
                .WithMessage("O {PropertyName} deve ter menos que 50 caracteres.");

            RuleFor(x => x.DataInicio)
                .NotEmpty()
                .NotNull()
                .WithMessage("O {PropertyName} deve ser informado.");

            RuleFor(x => x.DataFinal)
                .NotEmpty()
                .NotNull()
                .WithMessage("O {PropertyName} deve ser informado.");

            RuleFor(x => x.CondominoId)
                .NotNull()
                .NotEmpty()
                .WithMessage("O {PropertyName} deve ser informado.");
        }
    }
}
