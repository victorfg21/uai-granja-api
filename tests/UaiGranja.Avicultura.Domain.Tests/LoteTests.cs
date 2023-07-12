using FluentAssertions;
using UaiGranja.Avicultura.Domain.Entities;
using UaiGranja.Avicultura.Domain.Tests.Fixtures;
using UaiGranja.Core.DomainObjects;
using Xunit;

namespace UaiGranja.Avicultura.Domain.Tests
{
    [Collection(nameof(LoteCollection))]
    public class LoteTests
    {
        private readonly LoteTestsFixture _loteTestsFixture;

        public LoteTests(LoteTestsFixture loteTestsFixture)
        {
            _loteTestsFixture = loteTestsFixture;
        }

        [Fact(DisplayName = "Não Deve Adicionar Ave Inválida")]
        [Trait("Lote", "Lote Entity Trait")]
        public void Lote_AdicionarAve_AveInvalidaNaoDeveAdicionar()
        {
            //Arrange
            var lote = _loteTestsFixture.ObterLoteValidoVivo();

            //Assert
            Assert.Throws<AggregateDomainException>(() => lote.AdicionarAve(new Ave()));
        }

        [Fact(DisplayName = "Não Deve Adicionar Capacidade Atingida")]
        [Trait("Lote", "Lote Entity Trait")]
        public void Lote_AdicionarAve_CapacidadeLoteExcedidaNaoDeveAdicionar()
        {
            //Arrange
            int capacidadeTotal = 2;
            var lote = _loteTestsFixture.ObterLoteValidoVivo(capacidadeTotal);
            var aveValida = lote.Aves.FirstOrDefault();

            //Assert
            Assert.Throws<DomainException>(() => lote.AdicionarAve(aveValida!));
        }

        [Fact(DisplayName = "Realizar Pesagem Lote Vivo")]
        [Trait("Lote", "Lote Entity Trait")]
        public void Lote_RealizarPesagem_RealizarPesagemLoteVivo()
        {
            //Arrange
            var lote = _loteTestsFixture.ObterLoteValidoVivo();

            //Act
            lote.RealizarPesagem(1000);

            //Assert
            lote.Historicos.Should().HaveCount(1);
        }

        [Fact(DisplayName = "Não Deve Realizar Pesagem Lote Sem Aves")]
        [Trait("Lote", "Lote Entity Trait")]
        public void Lote_RealizarPesagem_NaoDeveRealizarPesagemLoteNaoPossuiAve()
        {
            //Arrange
            var lote = _loteTestsFixture.ObterLoteInvalido();

            //Assert
            Assert.Throws<DomainException>(() => lote.RealizarPesagem(1000));
        }

        [Fact(DisplayName = "Realizar Pesagem Lote Já Abatido")]
        [Trait("Lote", "Lote Entity Trait")]
        public void Lote_RealizarAbate_RealizarPesagemLoteAbatido()
        {
            //Arrange
            var lote = _loteTestsFixture.ObterLoteValidoVivo();

            //Act
            lote.RealizarAbate(1000);

            // Act & Assert
            Assert.Throws<DomainException>(() => lote.RealizarPesagem(1500));
        }

        [Fact(DisplayName = "Não Deve Realizar Abate Lote Sem Aves")]
        [Trait("Lote", "Lote Entity Trait")]
        public void Lote_RealizarAbate_NaoDeveRealizarAbateLoteNaoPossuiAve()
        {
            //Arrange
            var lote = _loteTestsFixture.ObterLoteInvalido();

            //Assert
            Assert.Throws<DomainException>(() => lote.RealizarAbate(1500));
        }

        [Fact(DisplayName = "Realizar Pesagem Lote Já Abatido")]
        [Trait("Lote", "Lote Entity Trait")]
        public void Lote_RealizarAbate_RealizarAbateLoteAbatido()
        {
            //Arrange
            var lote = _loteTestsFixture.ObterLoteValidoVivo();

            //Act
            lote.RealizarAbate(1000);

            // Act & Assert
            Assert.Throws<DomainException>(() => lote.RealizarAbate(1500));
        }

        [Fact(DisplayName = "Verificar Lote Está Vivo")]
        [Trait("Lote", "Lote Entity Trait")]
        public void Lote_EstaVivo_LoteDeveEstarVivo()
        {
            //Arrange
            var lote = _loteTestsFixture.ObterLoteValidoVivo();

            //Act
            var vivo = lote.EstaVivo();

            //Assert
            vivo.Should().BeTrue();
        }

        [Theory(DisplayName = "Verificar Lote Estar Abatido")]
        [Trait("Lote", "Lote Entity Trait")]
        [InlineData(1500)]
        [InlineData(1800.52)]
        [InlineData(1)]
        public void Lote_EstaVivo_LoteDeveEstarAbatido(decimal peso)
        {
            //Arrange
            var lote = _loteTestsFixture.ObterLoteValidoVivo();

            //Act
            lote.RealizarAbate(peso);
            var vivo = lote.EstaVivo();

            //Assert
            vivo.Should().BeFalse();
        }

        [Fact(DisplayName = "Verificar Lote Está Vivo e Não Possui Historico")]
        [Trait("Lote", "Lote Entity Trait")]
        public void Lote_EstaVivo_LoteDeveEstarVivoNaoPossuiHistorico()
        {
            //Arrange
            var lote = new Lote();

            //Act
            var vivo = lote.EstaVivo();

            //Assert
            vivo.Should().BeTrue();
        }

        [Fact(DisplayName = "Verificar Lote é Valido")]
        [Trait("Lote", "Lote Entity Trait")]
        public void Lote_EstaVivo_LoteDeveEstarValido()
        {
            //Arrange
            var lote = _loteTestsFixture.ObterLoteValidoVivo();

            //Act
            lote.EhValido();

            //Assert
            lote.ValidationResult.Errors.Should().HaveCount(0);
        }

        [Fact(DisplayName = "Verificar Lote é Invalido")]
        [Trait("Lote", "Lote Entity Trait")]
        public void Lote_EstaVivo_LoteDeveEstarInvalido()
        {
            //Arrange
            var lote = _loteTestsFixture.ObterLoteInvalido();

            //Act
            lote.EhValido();

            //Assert
            lote.ValidationResult.Errors.Should().HaveCountGreaterThan(0, "deve possuir erro");
        }

        [Theory(DisplayName = "Alterar Código Lote")]
        [Trait("Lote", "Lote Entity Trait")]
        [InlineData("1500")]
        [InlineData("X2321")]
        [InlineData(null)]
        public void Lote_AlterarCodigo_AlterarNovoCodigo(string codigoLote)
        {
            //Arrange
            var lote = _loteTestsFixture.ObterLoteValidoVivo();
            var codigoAntigo = lote.Codigo;

            //Act
            lote.AlterarCodigo(codigoLote);

            //Assert
            lote.Codigo.Should().Be(codigoLote);
            lote.Codigo.Should().NotBe(codigoAntigo); 
        }

        [Theory(DisplayName = "Alterar Capacidade Lote")]
        [Trait("Lote", "Lote Entity Trait")]
        [InlineData(1500)]
        [InlineData(5)]
        public void Lote_AlterarCapacidade_AlterarNovaCapacidade(int capacidade)
        {
            //Arrange
            var lote = _loteTestsFixture.ObterLoteValidoVivo();
            var capacidadeAntiga = lote.Capacidade;

            //Act
            lote.AlterarCapacidade(capacidade);

            //Assert
            lote.Capacidade.Should().Be(capacidade);
            lote.Capacidade.Should().NotBe(capacidadeAntiga);
        }
    }
}
