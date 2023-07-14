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
        [Trait("Avicultura", "Galinheiro Entity Trait")]
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
        [Trait("Avicultura", "Galinheiro Entity Trait")]
        public void Galinheiro_AdicionarLote_NaoDeveAdicionarLoteNaoUtilizaLote()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroValido(utilizaLote: false);

            //Assert
            Assert.Throws<DomainException>(() => galinheiro.AdicionarLote(_galinheiroTestsFixture.ObterLoteValidoVivo()));
        }

        [Fact(DisplayName = "Não Deve Adicionar Lote Capacidade Excedida")]
        [Trait("Avicultura", "Galinheiro Entity Trait")]
        public void Galinheiro_AdicionarLote_NaoDeveAdicionarLoteCapacidadeExcedida()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroValido(capacidade: 0, utilizaLote: true);

            //Assert
            Assert.Throws<DomainException>(() => galinheiro.AdicionarLote(_galinheiroTestsFixture.ObterLoteValidoVivo()));
        }

        [Fact(DisplayName = "Deve Adicionar Ave")]
        [Trait("Avicultura", "Galinheiro Entity Trait")]
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
        [Trait("Avicultura", "Galinheiro Entity Trait")]
        public void Galinheiro_AdicionarAve_NaoDeveAdicionarAveCapacidadeExcedida()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroValido(capacidade: 0);

            //Assert
            Assert.Throws<DomainException>(() => galinheiro.AdicionarAve(_galinheiroTestsFixture.ObterAveValidaViva()));
        }

        [Fact(DisplayName = "Não Deve Adicionar Ave Já Cadastrada")]
        [Trait("Avicultura", "Galinheiro Entity Trait")]
        public void Galinheiro_AdicionarAve_NaoDeveAdicionarAveJaCadastrada()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroValido();

            //Act
            galinheiro.AdicionarAve(_galinheiroTestsFixture.ObterAveValidaViva());

            //Assert
            Assert.Throws<DomainException>(() => galinheiro.AdicionarAve(_galinheiroTestsFixture.ObterAveValidaViva()));
        }

        [Fact(DisplayName = "Deve Adicionar Código Não Utilizado em Ave Viva")]
        [Trait("Avicultura", "Galinheiro Entity Trait")]
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

        [Fact(DisplayName = "Deve Adicionar Ave Em Lote")]
        [Trait("Avicultura", "Galinheiro Entity Trait")]
        public void Galinheiro_AdicionarAve_DeveAdicionarAveEmLote()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroValido(utilizaLote: true);

            //Act
            var lote = _galinheiroTestsFixture.ObterLoteValidoSemAve();
            galinheiro.AdicionarLote(lote);
            galinheiro.AdicionarAve(_galinheiroTestsFixture.ObterAveValidaViva(), lote.Id);

            //Assert
            galinheiro.Lotes.Should().HaveCount(1);
            lote.Aves.Should().HaveCount(1);
        }

        [Fact(DisplayName = "Não Deve Abater Ave Não Cadastrada")]
        [Trait("Avicultura", "Galinheiro Entity Trait")]
        public void Galinheiro_RealizarAbate_NaoDeveAbaterAveNaoCadastrada()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroValido();

            //Act
            var ave = _galinheiroTestsFixture.ObterAveValidaViva();

            //Assert
            Assert.Throws<DomainException>(() => galinheiro.RealizarAbateAve(ave.Id, 1500));
        }

        [Fact(DisplayName = "Não Deve Abater Ave Utiliza Lote")]
        [Trait("Avicultura", "Galinheiro Entity Trait")]
        public void Galinheiro_RealizarAbate_NaoDeveAbaterAveUtilizaLote()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroValido(utilizaLote: true);

            //Assert
            Assert.Throws<DomainException>(() => galinheiro.RealizarAbateAve(Guid.NewGuid(), 1500));
        }

        [Fact(DisplayName = "Não Deve Adicionar Ave Em Lote Abatido")]
        [Trait("Avicultura", "Galinheiro Entity Trait")]
        public void Galinheiro_AdicionarAve_NaoDeveAdicionarAveEmLoteAbatido()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroValido(utilizaLote: true);

            //Act
            var lote = _galinheiroTestsFixture.ObterLoteValidoVivo();
            galinheiro.AdicionarLote(lote);
            galinheiro.RealizarAbateLote(lote.Id, 1500);

            //Assert
            Assert.Throws<DomainException>(() => galinheiro.AdicionarAve(_galinheiroTestsFixture.ObterAveValidaViva(), lote.Id));
        }


        [Fact(DisplayName = "Não Deve Adicionar Ave Lote Não Encontrado")]
        [Trait("Avicultura", "Galinheiro Entity Trait")]
        public void Galinheiro_AdicionarAve_NaoDeveAdicionarAveLoteNaoEncontrado()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroValido(utilizaLote: true);

            //Act
            var lote = _galinheiroTestsFixture.ObterLoteValidoSemAve();
            galinheiro.AdicionarLote(lote);            

            //Assert
            Assert.Throws<DomainException>(() => galinheiro.AdicionarAve(_galinheiroTestsFixture.ObterAveValidaViva(), Guid.NewGuid()));
        }

        [Fact(DisplayName = "Não Deve Adicionar Ave Lote Capacidade Superada")]
        [Trait("Avicultura", "Galinheiro Entity Trait")]
        public void Galinheiro_AdicionarAve_NaoDeveAdicionarAveLoteCapacidadeSuperada()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroValido(utilizaLote: true);

            //Act
            var lote = _galinheiroTestsFixture.ObterLoteValidoVivo(capacidade: 2);
            galinheiro.AdicionarLote(lote);

            //Assert
            Assert.Throws<DomainException>(() => galinheiro.AdicionarAve(_galinheiroTestsFixture.ObterAveValidaViva(), lote.Id));
        }

        [Fact(DisplayName = "Não Deve Abter Lote Não É Utilizado")]
        [Trait("Avicultura", "Galinheiro Entity Trait")]
        public void Galinheiro_RealizarAbateLote_NaoDeveAbaterLoteNaoEhUtilizado()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroValido(utilizaLote: false);

            //Assert
            Assert.Throws<DomainException>(() => galinheiro.RealizarAbateLote(Guid.NewGuid(), 1500));
        }

        [Fact(DisplayName = "Não Deve Adicionar Ave Lote Não É Utilizado")]
        [Trait("Avicultura", "Galinheiro Entity Trait")]
        public void Galinheiro_RealizarAbateLote_NaoDeveAbaterLoteNaoEncontrado()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroValido(utilizaLote: true);

            //Assert
            Assert.Throws<DomainException>(() => galinheiro.RealizarAbateLote(Guid.NewGuid(), 1500));
        }

        [Fact(DisplayName = "Galinheiro Deve Ser Valido")]
        [Trait("Avicultura", "Galinheiro Entity Trait")]
        public void Galinheiro_EstaVivo_GalinheiroDeveEstarValido()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroValido();

            //Act
            galinheiro.EhValido();

            //Assert
            galinheiro.ValidationResult.Errors.Should().HaveCount(0);
        }

        [Fact(DisplayName = "Galinheiro Deve Ser Invalido")]
        [Trait("Avicultura", "Galinheiro Entity Trait")]
        public void Galinheiro_EstaVivo_GalinheiroDeveEstarInvalido()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroInvalido();

            //Act
            galinheiro.EhValido();

            //Assert
            galinheiro.ValidationResult.Errors.Should().HaveCountGreaterThan(0, "deve possuir erro");
        }

        [Fact(DisplayName = "Galinheiro Não Deve Utilizar Lote")]
        [Trait("Avicultura", "Galinheiro Entity Trait")]
        public void Galinheiro_EstaVivo_GalinheiroNaoDeveUtilizarLote()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroValido(utilizaLote: true);

            //Act
            var lote = _galinheiroTestsFixture.ObterLoteValidoVivo();
            galinheiro.AdicionarLote(lote);
            galinheiro.AlterarEstruturaGalinheiro();

            //Assert
            galinheiro.UtilizaLote.Should().BeFalse();
            galinheiro.Lotes.Should().HaveCount(0);
            galinheiro.Lotes.SelectMany(x => x.Aves).Should().HaveCount(0);
            galinheiro.Aves.Should().HaveCount(2);
        }

        [Fact(DisplayName = "Galinheiro Deve Utilizar Lote")]
        [Trait("Avicultura", "Galinheiro Entity Trait")]
        public void Galinheiro_EstaVivo_GalinheiroNDeveUtilizarLote()
        {
            //Arrange
            var galinheiro = _galinheiroTestsFixture.ObterGalinheiroValido(utilizaLote: false);

            //Act
            galinheiro.AdicionarAves(_galinheiroTestsFixture.ObterAvesValidasVivas());
            galinheiro.AlterarEstruturaGalinheiro();

            //Assert
            galinheiro.UtilizaLote.Should().BeTrue();
            galinheiro.Lotes.Should().HaveCount(1);
            galinheiro.Lotes.SelectMany(x => x.Aves).Should().HaveCount(2);
            galinheiro.Aves.Should().HaveCount(0);
        }
    }
}
