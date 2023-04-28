namespace Desapegando.Application.Services;

public class EmailSender
{
    public string PrimaryDomain { get; set; }
    public int PrimaryPort { get; set; }
    public string UsernameEmail { get; set; }
    public string UsernamePassword { get; set; }
    public string FromEmail { get; set; }
}