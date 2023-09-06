using Desapegando.Business.Models.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Desapegando.Application.ViewModels
{
    public class ProdutoViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Categoria")]
        public Categoria? Categoria { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Condição")]
        public EstadoProduto? EstadoProduto { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Preço")]
        public decimal? Preco { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Quantidade")]
        public int? Quantidade { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Imagens do Produto")]
        public List<IFormFile> ImagensUpload { get; set; }
    }

    public class UpdateProdutoViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Categoria")]
        public Categoria? Categoria { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Condição")]
        public EstadoProduto? EstadoProduto { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Preço")]
        public decimal? Preco { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Quantidade")]
        public int? Quantidade { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Imagens do Produto")]
        public List<IFormFile> ImagensUpload { get; set; }

        [DisplayName("Desistir da Venda")]
        public bool Desistencia { get; set; }

        [DisplayName("Disponível para Venda")]
        public bool Ativo { get; set; }

        [DisplayName("Produto Vendido")]
        public bool ProdutoVendido { get; set; }
    }

    public class GetProdutoViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }

        public string Descricao { get; set; }

        public Categoria Categoria { get; set; }

        public EstadoProduto EstadoProduto { get; set; }

        public decimal Preco { get; set; }

        public int Quantidade { get; set; }
        public int Curtida { get; set; }

        public List<ProdutoImagemViewModel> ProdutoImagemViewModels { get; set; }
        public List<ProdutoCurtidaViewModel> ProdutoCurtidaViewModels { get; set; }
        public CondominoViewModel CondominoViewModel { get; set; }

        public bool Ativo { get; set; }
    }

    public class FiltrarProdutoViewModel
    {
        public List<Categoria> Categorias { get; set; }
        public List<EnumModel> CheckBoxItems { get; set; }
        public decimal? PrecoMinimo { get; set; }
        public decimal? PrecoMaximo { get; set; }
    }

    public class EnumModel
    {
        public EstadoProduto EstadoProduto { get; set; }
        public bool IsSelected { get; set; }
    }

    public class PostProdutoViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public Categoria? Categoria { get; set; }
        public EstadoProduto? EstadoProduto { get; set; }
        public decimal? Preco { get; set; }
        public int? Quantidade { get; set; }
        //public List<IFormFile> ImagensUpload { get; set; }
        public List<string> ImagensUploadNames { get; set; }
        public bool Desistencia { get; set; }
        //public bool Ativo { get; set; }
        public bool ProdutoVendido { get; set; }
        public DateTime DataPublicacao { get; set; }
        public DateTime? DataVenda { get; set; }
        public int Curtida { get; set; } = 0;
        public Guid CondominoId { get; set; }
    }

    public class PatchProdutoViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public Categoria? Categoria { get; set; }
        public EstadoProduto? EstadoProduto { get; set; }
        public decimal? Preco { get; set; }
        public int? Quantidade { get; set; }
        //public List<IFormFile> ImagensUpload { get; set; }
        public List<string> ImagensUploadNames { get; set; }
        public bool Desistencia { get; set; }
        public bool Ativo { get; set; }
        public bool ProdutoVendido { get; set; }
        //public DateTime DataPublicacao { get; set; }
        public DateTime? DataVenda { get; set; }
        public int Curtida { get; set; } = 0;
        public Guid CondominoId { get; set; }
    }

    public class GetProdutoResponse
    {
        public bool Success { get; set; }
        public IEnumerable<ProdutoViewModel> Data { get; set; }
    }

    public class GetAllProdutoResponse
    {
        public bool Success { get; set; }
        public IEnumerable<GetProdutoViewModel> Data { get; set; }
    }

    public class GetProdutoResponseId
    {
        public bool Success { get; set; }
        //public ProdutoViewModel Data { get; set; }
        public GetProdutoViewModel Data { get; set; }
    }

    public class ProdutoResponse
    {
        public bool Success { get; set; }
        public DataProduto Data { get; set; }
    }
    
    public class DataProduto
    {
        public ResponseResult ResponseResult { get; set; }
    }

    public class GetMeusProdutoResponse
    {
        public bool Success { get; set; }
        public IEnumerable<GetProdutoViewModel> Data { get; set; }
    }

    public class CurtidaViewModel
    {
        public Guid ProdutoId { get; set; }
        public Guid CondominoId { get; set; }
    }
    public class DescurtidaViewModel
    {
        public Guid ProdutoId { get; set; }
        public Guid CondominoId { get; set; }
    }
}