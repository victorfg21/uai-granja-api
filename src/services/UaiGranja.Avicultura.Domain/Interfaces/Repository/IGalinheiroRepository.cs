using UaiGranja.Avicultura.Domain.Entities;
using UaiGranja.Core.Data;

namespace UaiGranja.Avicultura.Domain.Interfaces.Repository
{
    public interface IGalinheiroRepository : IRepository<Galinheiro>
    {
        void Adicionar(Galinheiro pedido);
        void Atualizar(Galinheiro pedido);
    }
}
