using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desapegando.Business.Models
{
    public class Compra : Entity
    {
        public Guid ProdutoId { get; set; }
        public Guid CondominoId { get; set; }
        public DateTime? DataVenda { get; set; }

        public Produto Produto { get; set; }
        public Condomino Condomino { get; set; }

    }
}
