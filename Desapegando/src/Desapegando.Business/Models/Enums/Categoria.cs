using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Desapegando.Business.Models.Enums;

public enum Categoria
{
    [Display(Name = "Artigos Esportivos")]
    ArtigosEsportivos = 1,

    [Display(Name = "Arte")]
    Arte = 2,

    [Display(Name = "Computadores Desktop")]
    Computadores = 3,

    [Display(Name = "Notebook")]
    Notebook = 4,

    [Display(Name = "Monitores")]
    Monitores = 5,

    [Display(Name = "Tablet")]
    Tablet = 6,

    [Display(Name = "Câmera")]
    Camera = 7,

    [Display(Name = "TV")]
    Tv = 8,

    [Display(Name = "Celulares")]
    Celulares = 9,

    [Display(Name = "Acessórios de Celular")]
    AcessoriosCelular = 10,

    [Display(Name = "Roupa Feminina")]
    RoupaFeminina = 11,

    [Display(Name = "Roupa Masculina")]
    RoupaMasculina = 12,

    [Display(Name = "Jóias")]
    Joias = 13,

    [Display(Name = "Relógios")]
    Relogios = 14,

    [Display(Name = "Sapatos")]
    Sapatos = 15,

    [Display(Name = "Acessórios de Vestuário")]
    AcessoriosVestuario = 16,

    [Display(Name = "Artigos de Jardinagem")]
    ArtigosJardinagem = 17,

    [Display(Name = "Artigos de Animais")]
    ArtigosAnimais = 18,

    [Display(Name = "Automóveis")]
    Automoveis = 19,

    [Display(Name = "Motocicletas")]
    Motocicleta = 20,

    [Display(Name = "Rádio")]
    Radio = 21,

    [Display(Name = "Acessórios Automotivos")]
    AcessoriosAutomotivos = 22,

    [Display(Name = "Acessórios de Motocicletas")]
    AcessoriosMotociclismo = 23,

    [Display(Name = "Artigos Musicais")]
    ArtigosMusicais = 24,

    [Display(Name = "Equipamentos de Áudio")]
    AudioSom = 25,

    [Display(Name = "Instrumentos Musicais")]
    InstrumentosMusicais = 26,

    [Display(Name = "Brinquedos Infantis")]
    BrinquedosInfantis = 27,

    [Display(Name = "Videogames")]
    VideoGames = 28,

    [Display(Name = "Produtos de Beleza")]
    ProdutosBeleza = 29,

    [Display(Name = "Perfumaria")]
    Perfumaria = 30,

    [Display(Name = "Artigos Antigos")]
    ArtigosAntigos = 31,

    [Display(Name = "Outros")]
    Outros = 32,

    [Display(Name = "Malas/Mochilas")]
    MalasMochilas = 33
}