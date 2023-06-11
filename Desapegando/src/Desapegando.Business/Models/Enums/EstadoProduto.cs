using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Desapegando.Business.Models.Enums;

public enum EstadoProduto
{
    [Display(Name = "Novo")]
    Novo = 1,

    [Display(Name = "Seminovo")]
    Seminovo = 2,

    [Display(Name = "Usado")]
    Usado = 3
}