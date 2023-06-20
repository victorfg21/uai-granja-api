using FluentValidation;
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

        public Lote(string codigo = null, int capacidade = 1)
        {
            Capacidade = capacidade;
            Codigo = !string.IsNullOrEmpty(codigo) ? codigo : GerarCodigoLotePadrao();
        }

        public Lote() { } // ORM

        internal void AssociarGalinheiro(Guid galinheiroId)
        {
            GalinheiroId = galinheiroId;
        }

        internal string GerarCodigoLotePadrao()
        {
            var idString = Id.ToString();
            return $"L-{idString.Substring(idString.Length - 12, idString.Length).ToUpper()}";
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
            if (!ave.EhValido()) throw new AggregateDomainException(AggregateDomainException.GetAggregateDomainException(ValidationResult.Errors));
            if (!Galinheiro.UtilizaLote) throw new DomainException("Galinheiro não utiliza lote, alterar para permitir inclusão de aves.");
            if (_aves.Count > Capacidade) throw new DomainException("Quantidade de aves permitidas foi excedida.");

            ave.AssociarLote(Id);
            _aves.Add(ave);
        }

        public void RealizarPesagem(HistoricoAve historico)
        {
            if (!historico.EhValido()) return;

            historico.AssociarAve(Id);
            _historicos.Add(historico);
        }

        public void AlterarCodigoLote(string codigo)
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