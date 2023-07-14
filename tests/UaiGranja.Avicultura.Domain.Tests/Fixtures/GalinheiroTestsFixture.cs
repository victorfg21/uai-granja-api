using UaiGranja.Avicultura.Domain.Entities;
using UaiGranja.Avicultura.Domain.Enums;
using Xunit;

namespace UaiGranja.Avicultura.Domain.Tests.Fixtures
{
    [CollectionDefinition(nameof(GalinheiroCollection))]
    public class GalinheiroCollection : ICollectionFixture<GalinheiroTestsFixture>
    { }

    public class GalinheiroTestsFixture : IDisposable
    {
        public void Dispose() { }

        public Galinheiro ObterGalinheiroValido(string codigo = "GAL001", int area = 200, int capacidade = 100, bool utilizaLote = false)
            => new(codigo, area, capacidade, utilizaLote);

        public Galinheiro ObterGalinheiroInvalido()
            => new();

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

        public Lote ObterLoteValidoSemAve(int capacidade = 10)
            => new Lote("001", capacidade);

        public Ave ObterAveValidaViva()
        {
            return new Ave("001", GeneroAnimalEnum.Macho, new DateTime(2023, 1, 1), new TipoAve(RacaEnum.CaipiraComum, PropositoCriacaoEnum.Hibrido, 2000, 182));
        }

        public List<Ave> ObterAvesValidasVivas()
        {
            return new List<Ave>
            {
                new("001", GeneroAnimalEnum.Macho, new DateTime(2023, 1, 1), new TipoAve(RacaEnum.CaipiraComum, PropositoCriacaoEnum.Hibrido, 2000, 182)),
                new("002", GeneroAnimalEnum.Macho, new DateTime(2023, 1, 1), new TipoAve(RacaEnum.CaipiraComum, PropositoCriacaoEnum.Hibrido, 2000, 182))
            };
        }
    }
}
