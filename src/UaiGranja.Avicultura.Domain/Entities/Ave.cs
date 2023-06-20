using FluentValidation;
using UaiGranja.Avicultura.Domain.Enums;
using UaiGranja.Core.DomainObjects;

namespace UaiGranja.Avicultura.Domain.Entities
{
    public class Ave : Entity, IAggregateRoot
    {
        public GeneroAnimalEnum GeneroAnimal { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public Guid TipoAveId { get; private set; }
        public Guid? LoteId { get; private set; }
        public Guid? GalinheiroId { get; private set; }

        // EF Rel.
        public TipoAve TipoAve { get; private set; }
        public Lote Lote { get; private set; }
        public Galinheiro Galinheiro { get; private set; }
        private readonly List<HistoricoAve> _historicos;
        public IReadOnlyCollection<HistoricoAve> Historicos => _historicos;

        public Ave(GeneroAnimalEnum generoAnimal, DateTime dataNascimento, TipoAve tipoAve)
        {
            GeneroAnimal = generoAnimal;
            DataNascimento = dataNascimento;
            TipoAve = tipoAve;
        }

        public Ave() { } // ORM

        internal void AssociarGalinheiro(Guid galinheiroId)
        {
            GalinheiroId = galinheiroId;
        }

        internal void AssociarLote(Guid loteId)
        {
            LoteId = loteId;
        }

        public void RealizarPesagem(HistoricoAve historico)
        {
            if (!historico.EhValido()) return;

            historico.AssociarAve(Id);
            _historicos.Add(historico);
        }

        public override bool EhValido()
        {
            ValidationResult = new AveValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AveValidation : AbstractValidator<Ave>
    {
        public AveValidation()
        {
            RuleFor(c => c.GeneroAnimal).IsInEnum().WithMessage("Deve ser informado uma opção dentre os valores permitido do enum do gênero do animal.");

            RuleFor(c => c.DataNascimento).NotNull().WithMessage("Deve ser informada uma data de nascimento da ave válida.");

            RuleFor(c => c.TipoAve).Must(TipoAveEhValida).WithMessage("O tipo da ave informado deve ser válido.");
        }

        private bool TipoAveEhValida(TipoAve tipoAve)
            => tipoAve.EhValido();
    }
}