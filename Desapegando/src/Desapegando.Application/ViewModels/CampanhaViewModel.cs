using Desapegando.Business.Models.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Desapegando.Application.ViewModels
{
    public class CampanhaViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Nome Instituição")]
        public string NomeInstituicao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Nome do Responsável")]
        public string NomeResponsavel { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        [DisplayName("E-mail do Responsável")]
        public string EmailResponsavel { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Telefone do Responsável")]
        public string TelefoneResponsavel { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Local de Encontro")]
        public string LocalDeEncontro { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Data de Início")]
        public DateTime? DataInicio { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Data Final")]
        public DateTime? DataFinal { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Imagens do Produto")]
        public List<IFormFile> ImagensUpload { get; set; }
    }

    public class UpdateCampanhaViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Nome Instituição")]
        public string NomeInstituicao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Nome do Responsável")]
        public string NomeResponsavel { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        [DisplayName("E-mail do Responsável")]
        public string EmailResponsavel { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Telefone do Responsável")]
        public string TelefoneResponsavel { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Local de Encontro")]
        public string LocalDeEncontro { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Data de Início")]
        public DateTime? DataInicio { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Data Final")]
        public DateTime? DataFinal { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Imagens do Produto")]
        public List<IFormFile> ImagensUpload { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DisplayName("Campanha Ativa")]
        public bool Ativo { get; set; }
    }

    public class GetCampanhaViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string NomeInstituicao { get; set; }
        public string Descricao { get; set; }
        public string NomeResponsavel { get; set; }
        public string EmailResponsavel { get; set; }
        public string TelefoneResponsavel { get; set; }
        public string LocalDeEncontro { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFinal { get; set; }
        public List<IFormFile> ImagensUpload { get; set; }
        public bool Ativo { get; set; }
        public List<CampanhaImagemViewModel> CampanhaImagemViewModels { get; set; }
    }

    public class FiltrarCampanhaViewModel
    {
        public string Nome { get; set; }
    }
}
