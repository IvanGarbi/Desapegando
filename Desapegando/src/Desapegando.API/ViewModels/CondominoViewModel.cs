using Desapegando.Business.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Desapegando.API.ViewModels;

public class CondominoRegisterViewModel
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Nome")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Sobrenome")]
    public string Sobrenome { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Sexo")]
    public Sexo? Sexo { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Telefone")]
    public string Telefone { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
    [DisplayName("E-mail")]
    public string Email { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("CPF")]
    public string Cpf { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Apartamento")]
    public string Apartamento { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Data de Nascimento")]
    public DateTime? DataNascimento { get; set; }

    public bool Administrador { get; set; }

    [Required(ErrorMessage = "É obrigatório aceitar os termos.")]
    [Range(typeof(bool), "true", "true", ErrorMessage = "É obrigatório aceitar os termos.")]
    public bool TermosPrivacidade { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
    public string Senha { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Confirme sua senha")]
    [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
    public string ConfirmarSenha { get; set; }
}

public class CondominoLoginViewModel
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
    [DisplayName("E-mail")]
    public string Email { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
    public string Senha { get; set; }
}

public class CondominoInativoViewModel
{
    public Guid Id { get; set; }

    [DisplayName("Nome")]
    public string Nome { get; set; }

    [DisplayName("Sobrenome")]
    public string Sobrenome { get; set; }

    [DisplayName("Sexo")]
    public Sexo Sexo { get; set; }

    [DisplayName("Telefone")]
    public string Telefone { get; set; }

    [DisplayName("E-mail")]
    public string Email { get; set; }

    [DisplayName("CPF")]
    public string Cpf { get; set; }

    [DisplayName("Apartamento")]
    public string Apartamento { get; set; }

    [DisplayName("Data de Nascimento")]
    public DateTime DataNascimento { get; set; }
}

public class CondominoViewModel
{
    public Guid Id { get; set; }

    [DisplayName("Nome")]
    public string Nome { get; set; }

    [DisplayName("Sobrenome")]
    public string Sobrenome { get; set; }

    [DisplayName("Sexo")]
    public Sexo Sexo { get; set; }

    [DisplayName("Telefone")]
    public string Telefone { get; set; }

    [DisplayName("E-mail")]
    public string Email { get; set; }

    [DisplayName("CPF")]
    public string Cpf { get; set; }

    [DisplayName("Apartamento")]
    public string Apartamento { get; set; }

    [DisplayName("Data de Nascimento")]
    public DateTime DataNascimento { get; set; }
    public bool Administrador { get; set; }
    public bool Ativo { get; set; }
}

public class GetCondominoViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public Sexo Sexo { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public string Cpf { get; set; }
    public string Apartamento { get; set; }
    public DateTime DataNascimento { get; set; }
    public bool Administrador { get; set; }
    public bool Ativo { get; set; }
}

public class PostCondominoViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public Sexo Sexo { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public string Cpf { get; set; }
    public string Apartamento { get; set; }
    public DateTime DataNascimento { get; set; }
    public bool Administrador { get; set; }
    public bool Ativo { get; set; }
}