using Desapegando.Business.Models;
using FluentValidation;

namespace Desapegando.Business.Validations
{
    public class ProdutoImagemValidation : AbstractValidator<ProdutoImagem>
    {
        public ProdutoImagemValidation()
        {
            RuleFor(x => x.FileName)
                .NotNull()
                .NotEmpty()
                .WithMessage("O {PropertyName} deve ser informado.")
                .EmailAddress() // ??
                .WithMessage("O {PropertyName} está em formato inválido.")
                .MaximumLength(150)
                .WithMessage("O {PropertyName} deve ter menos que 150 caracteres.");

            RuleFor(x => x.ProdutoId)
                .NotNull()
                .NotEmpty()
                .WithMessage("O {PropertyName} deve ser informado.");
        }
    }
}
