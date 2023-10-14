using Desapegando.Business.Models;

namespace Desapegando.API.ViewModels
{
    public class CompraViewModel
    {
        public Guid ProdutoId { get; set; }
        public Guid CondominoId { get; set; }
        public DateTime? DataVenda { get; set; }

        public Produto Produto { get; set; }
        public Condomino Condomino { get; set; }
    }


    public class GetCompraViewModel
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid CondominoId { get; set; }
        public DateTime? DataVenda { get; set; }

        public Produto Produto { get; set; }
        public Condomino Condomino { get; set; }
    }

    public class PostCompraViewModel
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid CondominoId { get; set; }
        public DateTime? DataVenda { get; set; }
    }

    public class PatchCompraViewModel
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid CondominoId { get; set; }
        public DateTime? DataVenda { get; set; }
    }
}
