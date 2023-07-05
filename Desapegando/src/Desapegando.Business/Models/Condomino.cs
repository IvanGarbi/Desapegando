using Desapegando.Business.Models.Enums;

namespace Desapegando.Business.Models;

public class Condomino : Entity
{
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public Sexo Sexo { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public string Cpf { get; set; }
    public string Apartamento { get; set; }
    public DateTime DataNascimento { get; set; }
    public DateTime DataRegistro { get; set; } = DateTime.Now;
    public bool Ativo { get; set; } = false;

    public ICollection<Produto> Produtos { get; set; }
    public ICollection<Campanha> Campanhas { get; set; }
    public ICollection<ProdutoCurtida> ProdutoCurtidas { get; set; }
}