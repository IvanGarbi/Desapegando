using Desapegando.Business.Models;

namespace Desapegando.Application.ViewModels
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

    public class GetCompraResponse
    {
        public bool Success { get; set; }
        public IEnumerable<CompraViewModel> Data { get; set; }
    }

    public class GetAllCompraResponse
    {
        public bool Success { get; set; }
        public IEnumerable<GetCompraViewModel> Data { get; set; }
    }

    public class CompraResponse
    {
        public bool Success { get; set; }
        public DataCompra Data { get; set; }
    }
    
    public class DataCompra
    {
        public ResponseResult ResponseResult { get; set; }
    }
}
