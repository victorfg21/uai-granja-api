using FluentValidation;
using UaiGranja.Avicultura.Domain.Enums;
using UaiGranja.Core.DomainObjects;

namespace UaiGranja.Avicultura.Domain.Entities
{
    public class HistoricoAve : Entity
    {
        public decimal Peso { get; private set; }
        public int IdadePesagem { get; private set; }
        public TipoHistoricoEnum TipoHistorico { get; private set; }
        public Guid? AveId { get; private set; }
        public Guid? LoteId { get; private set; }

        // EF Rel.
        public Ave Ave { get; private set; }
        public Lote Lote { get; private set; }

        public HistoricoAve(decimal peso, int idadePesagem)
        {
            Peso = peso;
            IdadePesagem = idadePesagem;
        }

        public HistoricoAve() { } // ORM 

        internal void AssociarAve(Guid aveId)
        {
            AveId = aveId;
            TipoHistorico = TipoHistoricoEnum.Unidade;
        }

        internal void AssociarLote(Guid loteId)
        {
            LoteId = loteId;
            TipoHistorico = TipoHistoricoEnum.Media;
        }

        public override bool EhValido()
        {
            ValidationResult = new HistoricoAveValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class HistoricoAveValidation : AbstractValidator<HistoricoAve>
    {
        public HistoricoAveValidation()
        {
            RuleFor(c => c.Peso).GreaterThan(0).WithMessage("Deve ser informado um peso superior a 0 quilograma.");

            RuleFor(c => c.IdadePesagem).GreaterThan(0).WithMessage("Deve ser informado uma idade superior a 0 dias.");
        }
    }
}