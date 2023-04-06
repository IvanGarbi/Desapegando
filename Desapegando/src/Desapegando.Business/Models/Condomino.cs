using Desapegando.Business.Models.Enums;

namespace Desapegando.Business.Models;

public class Condomino : Entity
{
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public Sexo Sexo { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public string Cpf { get; set; }
    public string Apartamento { get; set; }
    public int Idade { get; set; }
    public bool Administrador { get; set; } = false;
    public bool Ativo { get; set; } = false;
}