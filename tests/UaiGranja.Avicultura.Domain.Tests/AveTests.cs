using FluentAssertions;
using UaiGranja.Avicultura.Domain.Entities;
using UaiGranja.Avicultura.Domain.Tests.Fixtures;
using UaiGranja.Core.DomainObjects;
using Xunit;

namespace UaiGranja.Avicultura.Domain.Tests
{
    [Collection(nameof(AveCollection))]
    public class AveTests
    {
        private readonly AveTestsFixture _aveTestsFixture;

        public AveTests(AveTestsFixture aveTestsFixture)
        {
            _aveTestsFixture = aveTestsFixture;
        }

        [Fact(DisplayName = "Realizar Pesagem Ave Viva")]
        [Trait("Ave", "Ave Entity Trait")]
        public void Ave_RealizarPesagem_RealizarPesagemAveViva()
        {
            //Arrange
            var ave = _aveTestsFixture.ObterAveViva();

            //Act
            ave.RealizarPesagem(1000);

            //Assert
            ave.Historicos.Should().HaveCount(1);
        }

        [Fact(DisplayName = "Realizar Pesagem Ave Já Abatida")]
        [Trait("Ave", "Ave Entity Trait")]
        public void Ave_RealizarAbate_RealizarPesagemAveAbatida()
        {
            //Arrange
            var ave = _aveTestsFixture.ObterAveViva();

            //Act
            ave.RealizarAbate(1000);

            // Act & Assert
            Assert.Throws<DomainException>(() => ave.RealizarPesagem(1500));
        }

        [Fact(DisplayName = "Realizar Pesagem Ave Já Abatida")]
        [Trait("Ave", "Ave Entity Trait")]
        public void Ave_RealizarAbate_RealizarAbateAveAbatida()
        {
            //Arrange
            var ave = _aveTestsFixture.ObterAveViva();

            //Act
            ave.RealizarAbate(1000);

            // Act & Assert
            Assert.Throws<DomainException>(() => ave.RealizarAbate(1500));
        }

        [Fact(DisplayName = "Verificar Ave Está Viva")]
        [Trait("Ave", "Ave Entity Trait")]
        public void Ave_EstaVivo_AveDeveEstarViva()
        {
            //Arrange
            var ave = _aveTestsFixture.ObterAveViva();

            //Act
            var vivo = ave.EstaVivo();

            //Assert
            vivo.Should().BeTrue();
        }

        [Theory(DisplayName = "Verificar Ave Estar Abatida")]
        [Trait("Ave", "Ave Entity Trait")]
        [InlineData(1500)]
        [InlineData(1800.52)]
        [InlineData(1)]
        public void Ave_EstaVivo_AveDeveEstarAbatida(decimal peso)
        {
            //Arrange
            var ave = _aveTestsFixture.ObterAveViva();

            //Act
            ave.RealizarAbate(peso);
            var vivo = ave.EstaVivo();

            //Assert
            vivo.Should().BeFalse();
        }

        [Fact(DisplayName = "Verificar Ave Está Viva e Não Possui Historico")]
        [Trait("Ave", "Ave Entity Trait")]
        public void Ave_EstaVivo_AveDeveEstarVivaNaoPossuiHistorico()
        {
            //Arrange
            var ave = new Ave();

            //Act
            var vivo = ave.EstaVivo();

            //Assert
            vivo.Should().BeTrue();
        }

        [Fact(DisplayName = "Verificar Ave é Valida")]
        [Trait("Ave", "Ave Entity Trait")]
        public void Ave_EstaVivo_AveDeveEstarValida()
        {
            //Arrange
            var ave = _aveTestsFixture.ObterAveViva();

            //Act
            ave.EhValido();

            //Assert
            ave.ValidationResult.Errors.Should().HaveCount(0);
        }

        [Fact(DisplayName = "Verificar Ave é Invalida")]
        [Trait("Ave", "Ave Entity Trait")]
        public void Ave_EstaVivo_AveDeveEstarInvalida()
        {
            //Arrange
            var ave = _aveTestsFixture.ObterAveInvalida();

            //Act
            ave.EhValido();

            //Assert
            ave.ValidationResult.Errors.Should().HaveCountGreaterThan(0, "deve possuir erro");
        }
    }
}
