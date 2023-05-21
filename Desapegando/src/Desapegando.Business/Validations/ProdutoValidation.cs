using Desapegando.Business.Models;
using FluentValidation;

namespace Desapegando.Business.Validations
{
    public class ProdutoValidation :  AbstractValidator<Produto>
    {
        public ProdutoValidation()
        {
            RuleFor(x => x.Categoria)
                .IsInEnum();

            RuleFor(x => x.EstadoProduto)
                .IsInEnum();

            RuleFor(x => x.CondominoId)
                .NotNull()
                .NotEmpty()
                .WithMessage("O {PropertyName} deve ser informado.");

            RuleFor(x => x.DataPublicacao)
                .NotEmpty()
                .NotNull()
                .WithMessage("O {PropertyName} deve ser informado.");

            RuleFor(x => x.Descricao)
                .NotNull()
                .NotEmpty()
                .WithMessage("O {PropertyName} deve ser informado.")
                .MaximumLength(250)
                .WithMessage("O {PropertyName} deve ter menos que 250 caracteres.");

            RuleFor(x => x.Curtida)
                .GreaterThanOrEqualTo(0)
                .WithMessage("O {PropertyName} não pode ser negativo.");

            RuleFor(x => x.Nome)
                .NotNull()
                .NotEmpty()
                .WithMessage("O {PropertyName} deve ser informado.")
                .MaximumLength(150)
                .WithMessage("O {PropertyName} deve ter menos que 150 caracteres.");

            RuleFor(x => x.Preco)
                .NotNull()
                .NotEmpty()
                .WithMessage("O {PropertyName} deve ser informado.")
                .GreaterThanOrEqualTo(0)
                .WithMessage("Não é possível ter preço negativo.");

            RuleFor(x => x.Quantidade)
                .NotNull()
                .NotEmpty()
                .WithMessage("O {PropertyName} deve ser informado.")
                .GreaterThanOrEqualTo(0)
                .WithMessage("Não é possível ter quantidade negativa.");

        }
    }
}
