using FluentValidation;
using UaiGranja.Avicultura.Domain.Enums;
using UaiGranja.Core.DomainObjects;

namespace UaiGranja.Avicultura.Domain.Entities
{
    public class TipoAve : Entity, IAggregateRoot
    {
        public RacaEnum Raca { get; private set; }
        public PropositoCriacaoEnum PropositoCriacao { get; private set; }
        public decimal PesoCorte { get; private set; }
        public int IdadeCorte { get; private set; }

        public TipoAve(RacaEnum raca, PropositoCriacaoEnum propositoCriacao, decimal pesoCorte, int idadeCorte)
        {
            Raca = raca;
            PropositoCriacao = propositoCriacao;
            PesoCorte = pesoCorte;
            IdadeCorte = idadeCorte;
        }

        public TipoAve() { } // ORM

        public override bool EhValido()
        {
            ValidationResult = new TipoAveValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class TipoAveValidation : AbstractValidator<TipoAve>
    {
        public TipoAveValidation()
        {
            RuleFor(c => c.Raca).IsInEnum().WithMessage("Deve ser informado uma opção dentre os valores permitido do enum da raça da ave.");

            RuleFor(c => c.PropositoCriacao).IsInEnum().WithMessage("Deve ser informado uma opção dentre os valores permitido do enum do propósito de crição da ave.");

            RuleFor(c => c.PesoCorte).GreaterThan(0).WithMessage("Deve ser informado um peso superior a 0 quilograma.");

            RuleFor(c => c.IdadeCorte).GreaterThan(0).WithMessage("Deve ser informado uma idade superior a 0 dias.");
        }
    }
}