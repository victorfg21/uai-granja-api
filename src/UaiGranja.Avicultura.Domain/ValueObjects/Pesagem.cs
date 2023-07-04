using FluentValidation;
using UaiGranja.Avicultura.Domain.Enums;

namespace UaiGranja.Avicultura.Domain.ValueObjects
{
    public class Pesagem
    {
        public TipoHistoricoPesagemEnum TipoHistorico { get; private set; }
        public TipoPesagemEnum TipoPesagem { get; private set; }
        public decimal Peso { get; private set; }
        public int IdadePesagem { get; private set; }
        public DateTime DataPesagem { get; private set; }

        public Pesagem(TipoHistoricoPesagemEnum tipoHistorico, TipoPesagemEnum tipoPesagem, decimal peso, int idadePesagem)
        {
            TipoHistorico = tipoHistorico;
            TipoPesagem = tipoPesagem;
            Peso = peso;
            IdadePesagem = idadePesagem;
            DataPesagem = DateTime.Now;
        }

        public class PesagemValidation : AbstractValidator<Pesagem>
        {
            public PesagemValidation()
            {
                RuleFor(c => c.Peso).GreaterThan(0).WithMessage("Deve ser informado um peso superior a 0 quilograma.");

                RuleFor(c => c.IdadePesagem).GreaterThan(0).WithMessage("Deve ser informado uma idade superior a 0 dias.");

                RuleFor(c => c.TipoHistorico).IsInEnum().WithMessage("Deve ser informado uma opção dentre os valores permitido do enum do tipo do histórico de pesagem.");

                RuleFor(c => c.TipoPesagem).IsInEnum().WithMessage("Deve ser informado uma opção dentre os valores permitido do enum do tipo da pesagem.");
            }
        }
    }
}