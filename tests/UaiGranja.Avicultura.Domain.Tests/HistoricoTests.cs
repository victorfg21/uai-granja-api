using UaiGranja.Avicultura.Domain.Entities;
using UaiGranja.Avicultura.Domain.Enums;
using UaiGranja.Avicultura.Domain.Tests.Fixtures;
using UaiGranja.Core.DomainObjects;
using Xunit;

namespace UaiGranja.Avicultura.Domain.Tests
{
    public class HistoricoAveTests
    {
        [Fact(DisplayName = "Não Deve Pesar Ave Inválida")]
        [Trait("Historico", "Historico Entity Trait")]
        public void Historico_NovaPesagemAve_AveInvalidaNaoDevePesar()
        {
            //Assert
            Assert.Throws<DomainException>(()
                => HistoricoAve.HistoricoAveFactory.NovaPesagemAve(
                    null, TipoHistoricoPesagemEnum.Pesagem, TipoPesagemEnum.Unidade, 0));
            Assert.Throws<DomainException>(()
                => HistoricoAve.HistoricoAveFactory.NovaPesagemAve(
                    new Ave(), TipoHistoricoPesagemEnum.Pesagem, TipoPesagemEnum.Unidade, 0));
        }

        [Fact(DisplayName = "Não Deve Pesar Lote Inválido")]
        [Trait("Historico", "Historico Entity Trait")]
        public void Historico_NovaPesagemLote_LoteInvalidoNaoDevePesar()
        {
            //Assert
            Assert.Throws<DomainException>(()
                => HistoricoAve.HistoricoAveFactory.NovaPesagemLote(
                    null, TipoHistoricoPesagemEnum.Pesagem, TipoPesagemEnum.Unidade, 0));
            Assert.Throws<DomainException>(()
                => HistoricoAve.HistoricoAveFactory.NovaPesagemLote(
                    new Lote(), TipoHistoricoPesagemEnum.Pesagem, TipoPesagemEnum.Unidade, 0));
        }
    }
}
