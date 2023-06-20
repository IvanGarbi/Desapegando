
namespace Desapegando.Business.Notifications
{
    public class Notificacao
    {
        public string Mensagem { get; set; }

        public Notificacao(string mensagem)
        {
            this.Mensagem = mensagem;
        }
    }
}
