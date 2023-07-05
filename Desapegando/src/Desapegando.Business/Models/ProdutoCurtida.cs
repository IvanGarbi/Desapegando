namespace Desapegando.Business.Models
{
    public class ProdutoCurtida : Entity
    {
        public Guid ProdutoId { get; set; }
        public Guid CondominoId { get; set; }

        public Produto Produto { get; set; }
        public Condomino Condomino { get; set; }

    }
}