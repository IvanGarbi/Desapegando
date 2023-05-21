
namespace Desapegando.Business.Models
{
    public class ProdutoImagem : Entity
    {
        public string FileName { get; set; }
        public Guid ProdutoId { get; set; }

        /* EF Relation */
        public Produto Produto { get; set; }
    }
}
