using Microsoft.EntityFrameworkCore;
using UaiGranja.Avicultura.Domain.Entities;
using UaiGranja.Avicultura.Domain.Interfaces.Repository;
using UaiGranja.Core.Data;

namespace UaiGranja.Avicultura.Data.Repository
{
    public class GalinheiroRepository : IGalinheiroRepository
    {
        private readonly AviculturaContext _context;

        public GalinheiroRepository(AviculturaContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<Galinheiro> ObterPorId(Guid id)
        {
            return await _context.Galinheiros
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Galinheiro> ObterPorIdNoTracking(Guid id)
        {
            return await _context.Galinheiros
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Galinheiro>> ObterTodosNoTracking()
        {
            return await _context.Galinheiros
                .ToListAsync();
        }

        public void Adicionar(Galinheiro galinheiro)
        {
            _context.Galinheiros.Add(galinheiro);
        }

        public void Atualizar(Galinheiro galinheiro)
        {
            _context.Galinheiros.Update(galinheiro);
        }
    }
}
