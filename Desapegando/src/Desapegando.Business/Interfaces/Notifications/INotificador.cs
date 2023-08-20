using Desapegando.Business.Notifications;

namespace Desapegando.Business.Interfaces.Notifications
{
    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> GetNotificacoes();
        void AdicionarNotificacao(Notificacao notificacao);
    }
}
