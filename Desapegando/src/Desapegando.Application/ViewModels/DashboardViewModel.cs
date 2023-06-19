namespace Desapegando.Application.ViewModels
{
    public class DashboardViewModel
    {
        public int NovosCondominos { get; set; }
        public int ProdutosVendidos { get; set; }
        public int ProdutosDisponiveis { get; set; }

        public IEnumerable<NovosCondominos7DiasViewModel> NovosCondominos7Dias { get; set; }
        public IEnumerable<Vendas7DiasViewModel> Vendas7DiasViewModel { get; set; }
        //public IEnumerable<VendasPeriodoMesViewModel> VendasPeriodoMesViewModel { get; set; }
        public IEnumerable<VendasPeriodoMesViewModel> VendasPeriodo1MesViewModel { get; set; }
        public IEnumerable<VendasPeriodoMesViewModel> VendasPeriodo2MesViewModel { get; set; }
        public IEnumerable<VendasPeriodoMesViewModel> VendasPeriodo3MesViewModel { get; set; }

        public decimal TotalProdutosVendidosUltimos7Dias { get; set; }
        public decimal TotalProdutosDisponiveisUltimos7Dias { get; set; }
        public decimal TotalProdutosDesistidosUltimos7Dias { get; set; }

        public int NovasCampanhasDisponiveisUlitmos30Dias { get; set; }
    }

    public class NovosCondominos7DiasViewModel
    {
        public DateTime DataRegistro { get; set; }
        public int Quantidade { get; set; }
    }

    public class Vendas7DiasViewModel
    {
        public DateTime DataVenda { get; set; }
        public int Quantidade { get; set; }
    }

    public class VendasPeriodoMesViewModel
    {
        public DateTime DataVenda { get; set; }
        public int Quantidade { get; set; }
    }
}
