using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UaiGranja.Avicultura.Domain.Entities;
using UaiGranja.Avicultura.Domain.Enums;
using Xunit;

namespace UaiGranja.Avicultura.Domain.Tests.Fixtures
{
    [CollectionDefinition(nameof(AveCollection))]
    public class AveCollection : ICollectionFixture<AveTestsFixture>
    { }

    public class AveTestsFixture : IDisposable
    {
        public void Dispose() { }

        public Ave ObterAveViva()
        {
            return new Ave("001", GeneroAnimalEnum.Macho, new DateTime(2023, 1, 1), new TipoAve(RacaEnum.CaipiraComum, PropositoCriacaoEnum.Hibrido, 2000, 182));
        }

        public Ave ObterAveInvalida()
        {
            return new Ave(string.Empty, GeneroAnimalEnum.Macho, new DateTime(2023, 1, 1), new TipoAve(RacaEnum.CaipiraComum, PropositoCriacaoEnum.Hibrido, 2000, 182));
        }
    }
}
