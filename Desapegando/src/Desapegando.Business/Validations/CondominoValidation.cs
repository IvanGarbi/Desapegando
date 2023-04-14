using Desapegando.Business.Models;
using FluentValidation;

namespace Desapegando.Business.Validations;

public class CondominoValidation : AbstractValidator<Condomino>
{
    public CondominoValidation()
    {
        RuleFor(x => x.Sexo)
 //           .NotEmpty()
 //           .NotNull()
 //           .WithMessage("O {PropertyName} deve ser informado.")
            .IsInEnum();

        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage("O {PropertyName} deve ser informado.")
            .EmailAddress()
            .WithMessage("O {PropertyName} está em formato inválido.")
            .MaximumLength(100)
            .WithMessage("O {PropertyName} deve ter menos que 100 caracteres.");

        RuleFor(x => x.Sobrenome)
            .NotNull()
            .NotEmpty()
            .WithMessage("O {PropertyName} deve ser informado.")
            .MaximumLength(100)
            .WithMessage("O {PropertyName} deve ter menos que 100 caracteres.");

        RuleFor(x => x.Nome)
            .NotNull()
            .NotEmpty()
            .WithMessage("O {PropertyName} deve ser informado.")
            .MaximumLength(100)
            .WithMessage("O {PropertyName} deve ter menos que 100 caracteres.");

        RuleFor(x => x.Telefone)
            .NotNull()
            .NotEmpty()
            .WithMessage("O {PropertyName} deve ser informado.")
            .Matches("^[0-9]+$")
            .WithMessage("O {PropertyName} deve conter somente números.")
            .MaximumLength(20)
            .WithMessage("O {PropertyName} deve ter menos que 20 caracteres.");

        RuleFor(x => x.Apartamento)
            .NotEmpty()
            .NotNull()
            .WithMessage("O {PropertyName} deve ser informado.")
            .MaximumLength(20)
            .WithMessage("O {PropertyName} deve ter menos que 20 caracteres."); ;

        RuleFor(x => x.Idade)
            .NotEmpty()
            .NotNull()
            .WithMessage("O {PropertyName} deve ser informado.");

        RuleFor(x => x.Cpf)
            .NotEmpty()
            .NotNull()
            .WithMessage("O {PropertyName} deve ser informado.")
            .Matches("^[0-9]+$")
            .WithMessage("O {PropertyName} deve conter somente números.")
            .MinimumLength(11)
            .MaximumLength(11)
            .WithMessage("O {PropertyName} deve ter 11 caracteres.");

        //RuleFor(x => x.Administrador)
        //    .NotEmpty()
        //    .NotNull()
        //    .WithMessage("O {PropertyName} deve ser informado.");
    }
}