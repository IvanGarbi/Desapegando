using Desapegando.Business.Models;
using FluentValidation;

namespace Desapegando.Business.Validations;

public class CondominoValidation : AbstractValidator<Condomino>
{
    public CondominoValidation()
    {
        RuleFor(x => x.Sexo)
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

        RuleFor(x => x.DataNascimento)
            .NotEmpty()
            .NotNull()
            .WithMessage("O {PropertyName} deve ser informado.")
            .Must(dataNascimento => ValidarIdade(dataNascimento))
            .WithMessage("Deve ser maior de 18 anos.");

        RuleFor(x => x.Cpf)
            .NotEmpty()
            .NotNull()
            .WithMessage("O {PropertyName} deve ser informado.")
            .Matches("^[0-9]+$")
            .WithMessage("O {PropertyName} deve conter somente números.")
            .MinimumLength(11)
            .MaximumLength(11)
            .WithMessage("O {PropertyName} deve ter 11 caracteres.")
            .Must(cpf => ValidarCpf(cpf))
            .WithMessage("O {PropertyName} não está em formato de CPF adequado.");

    }

    private static bool ValidarIdade(DateTime dataNascimento)
    {
        DateTime dateNow = DateTime.Today;

        var idade = dateNow.Year - dataNascimento.Year;

        if (dataNascimento.Date > dateNow.AddYears(-idade)) idade--;

        if (idade < 18)
        {
            return false;
        }

        return true;
    }

    private static bool ValidarCpf(string cpf)
    {
        if (cpf.Length > 11)
            return false;

        while (cpf.Length != 11)
            cpf = '0' + cpf;

        var igual = true;
        for (var i = 1; i < 11 && igual; i++)
            if (cpf[i] != cpf[0])
                igual = false;

        if (igual || cpf == "12345678909")
            return false;

        var numeros = new int[11];

        for (var i = 0; i < 11; i++)
            numeros[i] = int.Parse(cpf[i].ToString());

        var soma = 0;
        for (var i = 0; i < 9; i++)
            soma += (10 - i) * numeros[i];

        var resultado = soma % 11;

        if (resultado == 1 || resultado == 0)
        {
            if (numeros[9] != 0)
                return false;
        }
        else if (numeros[9] != 11 - resultado)
            return false;

        soma = 0;
        for (var i = 0; i < 10; i++)
            soma += (11 - i) * numeros[i];

        resultado = soma % 11;

        if (resultado == 1 || resultado == 0)
        {
            if (numeros[10] != 0)
                return false;
        }
        else if (numeros[10] != 11 - resultado)
            return false;

        return true;
    }
}