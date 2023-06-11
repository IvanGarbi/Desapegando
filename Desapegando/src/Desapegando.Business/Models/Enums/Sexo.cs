using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Desapegando.Business.Models.Enums;

public enum Sexo
{
    [Display(Name = "Masculino")]
    Masculino = 1,

    [Display(Name = "Feminino")]
    Feminino = 2
}