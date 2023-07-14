using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using UaiGranja.Avicultura.Domain.Entities;
using UaiGranja.Core.Communication.Mediator;
using UaiGranja.Core.Data;
using UaiGranja.Core.Messages;

namespace UaiGranja.Avicultura.Data
{
    public class AviculturaContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;


        public AviculturaContext(DbContextOptions<AviculturaContext> options, IMediatorHandler mediatorHandler)
            : base(options)
        {
            _mediatorHandler = mediatorHandler;
        }

        public DbSet<Ave> Aves { get; set; }
        public DbSet<Galinheiro> Galinheiros { get; set; }
        public DbSet<HistoricoAve> HistoricosAves { get; set; }
        public DbSet<Lote> Lotes { get; set; }
        public DbSet<TipoAve> TiposAves { get; set; }

        public async Task<bool> Commit()
        {
            var sucesso = await base.SaveChangesAsync() > 0;
            if (sucesso) await _mediatorHandler.PublicarEventos(this);

            return sucesso;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Ignore<Event>();

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetMaxLength(100);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AviculturaContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
