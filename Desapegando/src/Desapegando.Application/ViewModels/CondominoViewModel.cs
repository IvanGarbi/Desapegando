using Desapegando.Business.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Desapegando.Application.ViewModels;

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

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    [DisplayName("Foto de Perfil")]
    public IFormFile ImageUpload { get; set; }

    public string ImageFileName { get; set; }
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

    //[DisplayName("Foto de Perfil")]
    public string ImageFileName { get; set; }
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

    //[DisplayName("Foto de Perfil")]
    public string ImageFileName { get; set; }
    public IFormFile ImageUpload { get; set; }
    public bool NovaImagem { get; set; } = false;
}

public class CondominoCompraViewModel
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
    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }

    //[DisplayName("Foto de Perfil")]
    public string ImageFileName { get; set; }
    public IFormFile ImageUpload { get; set; }
}

public class GetCondominoResponse
{
    public bool Success { get; set; }
    public IEnumerable<CondominoViewModel> Data { get; set; }
}

public class GetCondominoCompraResponse
{
    public bool Success { get; set; }
    public IEnumerable<CondominoCompraViewModel> Data { get; set; }
}

public class GetCondominoResponseId
{
    public bool Success { get; set; }
    public CondominoViewModel Data { get; set; }
}

public class CondominoResponse
{
    public bool Success { get; set; }
    public DataCondomino Data { get; set; }
}

public class DataCondomino
{
    public ResponseResult ResponseResult { get; set; }
}

public class GetAllCondominoResponse
{
    public bool Success { get; set; }
    public IEnumerable<CondominoViewModel> Data { get; set; }
}