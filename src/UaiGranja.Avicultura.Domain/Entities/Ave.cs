using FluentValidation;
using UaiGranja.Avicultura.Domain.Enums;
using UaiGranja.Core.DomainObjects;

namespace UaiGranja.Avicultura.Domain.Entities
{
    public class Ave : Entity
    {
        public string Codigo { get; private set; }
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

        public Ave(string codigo, GeneroAnimalEnum generoAnimal, DateTime dataNascimento, TipoAve tipoAve)
        {
            Codigo = codigo;
            GeneroAnimal = generoAnimal;
            DataNascimento = dataNascimento;
            TipoAve = tipoAve;
            _historicos = new List<HistoricoAve>();
        }

        public Ave()
        {
            _historicos = new List<HistoricoAve>();
        }

        internal void AssociarGalinheiro(Guid galinheiroId)
        {
            GalinheiroId = galinheiroId;
        }

        internal void AssociarLote(Guid loteId)
        {
            LoteId = loteId;
        }

        public int ObterIdadeAve()
        {
            return (DateTime.Today - DataNascimento).Days;
        }

        public bool EstaVivo()
        {
            if (!Historicos.Any()) return true;
            return !Historicos.Any(x => x.Pesagem.TipoHistorico == TipoHistoricoPesagemEnum.Abate);
        }

        public void RealizarPesagem(decimal peso)
        {
            if (!EstaVivo()) throw new DomainException("Ave abatida não pode ser pesada.");
            var historico = HistoricoAve.HistoricoAveFactory.NovaPesagemAve(this, TipoHistoricoPesagemEnum.Pesagem, TipoPesagemEnum.Unidade, peso);
            _historicos.Add(historico);
        }

        public void RealizarAbate(decimal peso)
        {
            if (!EstaVivo()) throw new DomainException("Ave já foi abatida.");
            var historico = HistoricoAve.HistoricoAveFactory.NovaPesagemAve(this, TipoHistoricoPesagemEnum.Abate, TipoPesagemEnum.Unidade, peso);
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
            RuleFor(c => c.Codigo).NotEmpty().WithMessage("Deve ser informado um código numérico para identificação da ave.");

            RuleFor(c => c.GeneroAnimal).IsInEnum().WithMessage("Deve ser informado uma opção dentre os valores permitido do enum do gênero do animal.");

            RuleFor(c => c.DataNascimento).NotNull().WithMessage("Deve ser informada uma data de nascimento da ave válida.");

            RuleFor(c => c.TipoAve).Must(TipoAveEhValida).WithMessage("O tipo da ave informado deve ser válido.");
        }

        private bool TipoAveEhValida(TipoAve tipoAve)
            => tipoAve.EhValido();
    }
}