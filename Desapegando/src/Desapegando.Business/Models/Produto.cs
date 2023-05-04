using Desapegando.Business.Models.Enums;

namespace Desapegando.Business.Models;

public class Produto : Entity
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public Categoria Categoria { get; set; }
    public DateTime DataPublicacao { get; set; }
    public bool Ativo { get; set; } = true;
    public EstadoProduto EstadoProduto { get; set; }
    public int Curtida { get; set; }
    public string Imagem { get; set; }
    public Guid CondominoId { get; set; }

    /* EF Relation */
    public Condomino Condomino { get; set; }
}