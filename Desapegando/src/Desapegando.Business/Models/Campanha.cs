using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desapegando.Business.Models
{
    public class Campanha : Entity
    {
        public string Nome { get; set; }
        public string NomeInstituicao { get; set; }
        public string Descricao { get; set; }
        public string NomeResponsavel { get; set; }
        public string EmailResponsavel { get; set; }
        public string TelefoneResponsavel { get; set; }
        public string LocalDeEncontro { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFinal { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
