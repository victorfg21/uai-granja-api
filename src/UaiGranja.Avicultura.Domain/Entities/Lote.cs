using FluentValidation;
using UaiGranja.Avicultura.Domain.Enums;
using UaiGranja.Core.DomainObjects;

namespace UaiGranja.Avicultura.Domain.Entities
{
    public class Lote : Entity
    {
        public string Codigo { get; private set; }
        public int Capacidade { get; private set; }
        public Guid GalinheiroId { get; private set; }

        /* EF Relation */
        public Galinheiro Galinheiro { get; private set; }
        private readonly List<Ave> _aves;
        public IReadOnlyCollection<Ave> Aves => _aves;
        private readonly List<HistoricoAve> _historicos;
        public IReadOnlyCollection<HistoricoAve> Historicos => _historicos;
        public DateTime? DataNascimento => Aves.FirstOrDefault()?.DataNascimento;

        public Lote(string codigo = "", int capacidade = 1)
        {
            Capacidade = capacidade;
            Codigo = !string.IsNullOrEmpty(codigo) ? codigo : GerarCodigoLotePadrao();
            _aves = new List<Ave>();
            _historicos = new List<HistoricoAve>();
        }

        public Lote()
        {
            _aves = new List<Ave>();
            _historicos = new List<HistoricoAve>();
        } 

        internal void AssociarGalinheiro(Guid galinheiroId)
        {
            GalinheiroId = galinheiroId;
        }

        internal string GerarCodigoLotePadrao()
        {
            var idString = Id.ToString();
            return $"L-{idString.Substring(idString.Length - 12, 12).ToUpper()}";
        }

        public void AdicionarAves(IEnumerable<Ave> aves)
        {
            foreach (var ave in aves)
            {
                AdicionarAve(ave);
            }
        }

        public void AdicionarAve(Ave ave)
        {
            if (!ave.EhValido()) throw new AggregateDomainException(AggregateDomainException.GetAggregateDomainException(ave.ValidationResult.Errors));
            if (_aves.Count >= Capacidade) throw new DomainException("Quantidade de aves permitidas foi excedida.");

            ave.AssociarLote(Id);
            _aves.Add(ave);
        }

        public int ObterIdadeLote()
        {
            if(!DataNascimento.HasValue) return 0;
            return (DateTime.Today - DataNascimento.Value).Days;
        }

        public bool EstaVivo()
        {
            return !Historicos.Any(x => x.Pesagem.TipoHistorico == TipoHistoricoPesagemEnum.Abate);
        }

        public void RealizarPesagem(decimal peso)
        {
            if (!Aves.Any()) throw new DomainException("Lote não possui aves.");
            if (!EstaVivo()) throw new DomainException("Lote abatido não pode ser pesado.");
            var historico = HistoricoAve.HistoricoAveFactory.NovaPesagemLote(this, TipoHistoricoPesagemEnum.Pesagem, TipoPesagemEnum.Unidade, peso);
            _historicos.Add(historico);
        }

        public void RealizarAbate(decimal peso)
        {
            if (!Aves.Any()) throw new DomainException("Lote não possui aves.");
            if (!EstaVivo()) throw new DomainException("Lote já foi abatido.");
            var historico = HistoricoAve.HistoricoAveFactory.NovaPesagemLote(this, TipoHistoricoPesagemEnum.Abate, TipoPesagemEnum.Unidade, peso);
            _historicos.Add(historico);
        }

        public void AlterarCodigo(string codigo)
        {
            Codigo = codigo;
        }

        public void AlterarCapacidade(int capacidade)
        {
            Capacidade = capacidade;
        }

        public override bool EhValido()
        {
            ValidationResult = new LoteValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class LoteValidation : AbstractValidator<Lote>
    {
        public LoteValidation()
        {
            RuleFor(c => c.Capacidade).GreaterThan(0).WithMessage("A capacidade de aves em um lote deve permitir ao menos 1 ave.");
        }
    }
}