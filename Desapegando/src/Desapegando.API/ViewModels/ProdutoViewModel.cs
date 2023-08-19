using Desapegando.Business.Models.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Desapegando.API.ViewModels
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
    }

    public class FiltrarProdutoViewModel
    {
        public List<Categoria> Categorias { get; set; }
    }
}