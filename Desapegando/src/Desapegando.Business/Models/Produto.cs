using Desapegando.Business.Models.Enums;

namespace Desapegando.Business.Models;

public class Produto : Entity
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public Categoria Categoria { get; set; }
    public DateTime DataPublicacao { get; set; }
    public DateTime? DataVenda { get; set; }
    public decimal Preco { get; set; }
    public bool Ativo { get; set; } = true;
    public bool Desistencia { get; set; } = false;
    public EstadoProduto EstadoProduto { get; set; }
    public int Curtida { get; set; } = 0;
    public int Quantidade { get; set; }
    public Guid CondominoId { get; set; }

    /* EF Relation */
    public Condomino Condomino { get; set; }
    public ICollection<ProdutoImagem> ProdutoImagens { get; set; }
    public ICollection<ProdutoCurtida> ProdutoCurtidas { get; set; }
}