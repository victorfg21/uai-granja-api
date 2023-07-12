using UaiGranja.Avicultura.Domain.Entities;
using UaiGranja.Avicultura.Domain.Enums;
using Xunit;

namespace UaiGranja.Avicultura.Domain.Tests.Fixtures
{
    [CollectionDefinition(nameof(LoteCollection))]
    public class LoteCollection : ICollectionFixture<LoteTestsFixture>
    { }

    public class LoteTestsFixture : IDisposable
    {
        public void Dispose() { }

        public Lote ObterLoteValidoVivo(int capacidade = 10)
        {
            var lote = new Lote("001", capacidade);
            lote.AdicionarAves(new List<Ave>
            {
                new("001", GeneroAnimalEnum.Macho, new DateTime(2023, 1, 1), new TipoAve(RacaEnum.CaipiraComum, PropositoCriacaoEnum.Hibrido, 2000, 182)),
                new("002", GeneroAnimalEnum.Macho, new DateTime(2023, 1, 1), new TipoAve(RacaEnum.CaipiraComum, PropositoCriacaoEnum.Hibrido, 2000, 182))
            });

            return lote;
        }

        public Lote ObterLoteInvalido()
        {
            return new Lote(string.Empty, 0);
        }
    }
}
