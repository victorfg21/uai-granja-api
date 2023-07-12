using FluentAssertions;
using UaiGranja.Avicultura.Domain.Entities;
using UaiGranja.Avicultura.Domain.Tests.Fixtures;
using UaiGranja.Core.DomainObjects;
using Xunit;

namespace UaiGranja.Avicultura.Domain.Tests
{
    [Collection(nameof(GalinheiroCollection))]
    public class GalinheiroTests
    {
        private readonly GalinheiroTestsFixture _galinheiroTestsFixture;

        public GalinheiroTests(GalinheiroTestsFixture galinheiroTestsFixture)
        {
            _galinheiroTestsFixture = galinheiroTestsFixture;
        }

        [Fact(DisplayName = "Deve Adicionar Lote")]
        [Trait("Galinheiro", "Lote Entity Trait")]
        public void Galinheiro_AdicionarLote_DeveAdicionarLote()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroValido(utilizaLote: true);

            //Act
            galinheiro.AdicionarLote(_galinheiroTestsFixture.ObterLoteValidoVivo());

            //Assert
            galinheiro.Lotes.Should().HaveCount(1);
        }

        [Fact(DisplayName = "Não Deve Adicionar Lote Não Utiliza Lote")]
        [Trait("Galinheiro", "Lote Entity Trait")]
        public void Galinheiro_AdicionarLote_NaoDeveAdicionarLoteNaoUtilizaLote()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroValido(utilizaLote: false);

            //Assert
            Assert.Throws<DomainException>(() => galinheiro.AdicionarLote(_galinheiroTestsFixture.ObterLoteValidoVivo()));
        }

        [Fact(DisplayName = "Não Deve Adicionar Lote Capacidade Excedida")]
        [Trait("Galinheiro", "Lote Entity Trait")]
        public void Galinheiro_AdicionarLote_NaoDeveAdicionarLoteCapacidadeExcedida()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroValido(capacidade: 0, utilizaLote: true);

            //Assert
            Assert.Throws<DomainException>(() => galinheiro.AdicionarLote(_galinheiroTestsFixture.ObterLoteValidoVivo()));
        }

        [Fact(DisplayName = "Deve Adicionar Ave")]
        [Trait("Galinheiro", "Lote Entity Trait")]
        public void Galinheiro_AdicionarAve_DeveAdicionarAve()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroValido();

            //Act
            galinheiro.AdicionarAve(_galinheiroTestsFixture.ObterAveValidaViva());

            //Assert
            galinheiro.Aves.Should().HaveCount(1);
        }

        [Fact(DisplayName = "Não Deve Adicionar Ave Capacidade Excedida")]
        [Trait("Galinheiro", "Lote Entity Trait")]
        public void Galinheiro_AdicionarAve_NaoDeveAdicionarAveCapacidadeExcedida()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroValido(capacidade: 0);

            //Assert
            Assert.Throws<DomainException>(() => galinheiro.AdicionarAve(_galinheiroTestsFixture.ObterAveValidaViva()));
        }

        [Fact(DisplayName = "Não Deve Adicionar Ave Já Cadastrada")]
        [Trait("Galinheiro", "Lote Entity Trait")]
        public void Galinheiro_AdicionarAve_NaoDeveAdicionarAveJaCadastrada()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroValido();

            //Act
            galinheiro.AdicionarAve(_galinheiroTestsFixture.ObterAveValidaViva());

            //Assert
            Assert.Throws<DomainException>(() => galinheiro.AdicionarAve(_galinheiroTestsFixture.ObterAveValidaViva()));
        }

        [Fact(DisplayName = "Não Deve Adicionar Ave Galinheiro Inválido")]
        [Trait("Galinheiro", "Lote Entity Trait")]
        public void Galinheiro_AdicionarAve_NaoDeveAdicionarAveGalinheiroInvalido()
        {
            //Arrange
            var galinheiro = new Galinheiro();

            //Act
            galinheiro.AdicionarAve(_galinheiroTestsFixture.ObterAveValidaViva());

            //Assert
            Assert.Throws<DomainException>(() => galinheiro.AdicionarAve(_galinheiroTestsFixture.ObterAveValidaViva()));
        }

        [Fact(DisplayName = "Deve Adicionar Código Não Utilizado em Ave Viva")]
        [Trait("Galinheiro", "Lote Entity Trait")]
        public void Galinheiro_AdicionarAve_DeveAdicionarAveCodigoNaoUtilizadoEmAveViva()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroValido();

            //Act
            var ave = _galinheiroTestsFixture.ObterAveValidaViva();
            galinheiro.AdicionarAve(ave);
            galinheiro.RealizarAbateAve(ave.Id, 1500);
            galinheiro.AdicionarAve(_galinheiroTestsFixture.ObterAveValidaViva());

            //Assert
            galinheiro.Aves.Should().HaveCount(2);
        }

        [Fact(DisplayName = "Não Deve Abater Ave Não Cadastrada")]
        [Trait("Galinheiro", "Lote Entity Trait")]
        public void Galinheiro_RealizarAbate_NaoDeveAbaterAveNaoCadastrada()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroValido();

            //Act
            var ave = _galinheiroTestsFixture.ObterAveValidaViva();

            //Assert
            Assert.Throws<DomainException>(() => galinheiro.RealizarAbateAve(ave.Id, 1500));
        }
    }
}
