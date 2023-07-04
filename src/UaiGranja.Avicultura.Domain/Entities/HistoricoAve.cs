using FluentValidation;
using UaiGranja.Avicultura.Domain.Enums;
using UaiGranja.Avicultura.Domain.ValueObjects;
using UaiGranja.Core.DomainObjects;

namespace UaiGranja.Avicultura.Domain.Entities
{
    public class HistoricoAve : Entity
    {
        public Pesagem Pesagem { get; private set; }
        public Guid? AveId { get; private set; }
        public Guid? LoteId { get; private set; }

        // EF Rel.
        public Ave Ave { get; private set; }
        public Lote Lote { get; private set; }

        public HistoricoAve() { } // ORM 

        public override bool EhValido()
        {
            ValidationResult = new Pesagem.PesagemValidation().Validate(Pesagem);
            var pesagemValida = ValidationResult.IsValid;

            ValidationResult = new HistoricoAveValidation().Validate(this);
            var historicoValido = ValidationResult.IsValid;

            return pesagemValida && historicoValido;
        }

        public static class HistoricoAveFactory
        {
            public static HistoricoAve NovaPesagemAve(Ave ave, TipoHistoricoPesagemEnum tipoHistorico, TipoPesagemEnum TipoPesagem, decimal peso)
            {
                var historico = new HistoricoAve
                {
                    AveId = ave.Id,
                    Pesagem = new Pesagem(tipoHistorico, TipoPesagem, peso, ave.ObterIdadeAve())
                };

                return historico.EhValido() ? historico : throw new DomainException("Histórico inválido.");
            }

            public static HistoricoAve NovaPesagemLote(Lote lote, TipoHistoricoPesagemEnum tipoHistorico, TipoPesagemEnum TipoPesagem, decimal peso)
            {
                var historico = new HistoricoAve
                {
                    LoteId = lote.Id,
                    Pesagem = new Pesagem(tipoHistorico, TipoPesagem, peso, lote.ObterIdadeLote())
                };

                return historico.EhValido() ? historico : throw new DomainException("Histórico inválido.");
            }
        }

        public class HistoricoAveValidation : AbstractValidator<HistoricoAve>
        {
            public HistoricoAveValidation()
            {
                RuleFor(c => c).Must(HistoricoEstaAssociado).WithMessage("Histórico deve ser associado com uma ave ou um lote válido.");
            }

            private bool HistoricoEstaAssociado(HistoricoAve historico)
            {
                return (historico.AveId.HasValue && historico.AveId.Value != Guid.Empty)
                        || (historico.LoteId.HasValue && historico.LoteId.Value != Guid.Empty);
            }
        }
    }
}