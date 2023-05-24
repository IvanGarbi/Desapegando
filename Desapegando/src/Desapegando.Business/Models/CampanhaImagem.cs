
namespace Desapegando.Business.Models
{
    public class CampanhaImagem : Entity
    {
        public string FileName { get; set; }
        public Guid CampanhaId { get; set; }

        /* EF Relation */
        public Campanha Campanha { get; set; }
    }
}
