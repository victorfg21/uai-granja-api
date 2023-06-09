﻿using FluentValidation;
using UaiGranja.Core.DomainObjects;

namespace UaiGranja.Avicultura.Domain.Entities
{
    public class Galinheiro : Entity, IAggregateRoot
    {
        public string Codigo { get; private set; }
        public decimal Area { get; private set; }
        public int Capacidade { get; private set; }
        public bool UtilizaLote { get; private set; }

        /* EF Relation */

        private readonly List<Ave> _aves;
        public IReadOnlyCollection<Ave> Aves => _aves;

        private readonly List<Lote> _lotes;
        public IReadOnlyCollection<Lote> Lotes => _lotes;

        public Galinheiro(string codigo, decimal area, int capacidadeTotal, bool utilizaLote)
        {
            Codigo = codigo;
            Area = area;
            Capacidade = capacidadeTotal;
            UtilizaLote = utilizaLote;
            _aves = new List<Ave>();
            _lotes = new List<Lote>();
        }

        public Galinheiro()
        {
            _aves = new List<Ave>();
            _lotes = new List<Lote>();
        }

        public void AdicionarLote(Lote lote)
        {
            if (!lote.EhValido()) return;

            if (!UtilizaLote) throw new DomainException("Galinheiro não utiliza lote, alterar para permitir inclusão de lote");

            if (_lotes.Count >= Capacidade) throw new DomainException("Quantidade de lotes permitidos foi excedido");

            lote.AssociarGalinheiro(Id);
            _lotes.Add(lote);
        }

        public void AdicionarAves(IEnumerable<Ave> aves, Guid loteId = default)
        {
            foreach (var ave in aves)
            {
                AdicionarAve(ave, loteId);
            }
        }

        public void AdicionarAve(Ave ave, Guid loteId = default)
        {
            if (UtilizaLote)
            {
                var lote = _lotes.FirstOrDefault(x => x.Id == loteId);
                if (lote is null) throw new DomainException("Lote não encontrado.");
                if (lote.Aves.Count >= lote.Capacidade) throw new DomainException("Quantidade de aves permitidas foi excedida.");

                lote.AdicionarAve(ave);
            }
            else
            {
                if (!ave.EhValido()) throw new AggregateDomainException(AggregateDomainException.GetAggregateDomainException(ave.ValidationResult.Errors));
                if (_aves.Count >= Capacidade) throw new DomainException("Quantidade de aves permitidas foi excedida.");
                if (_aves.Any(x => x.Codigo == ave.Codigo && x.EstaVivo())) throw new DomainException("Código da ave já está cadastrado em ave viva.");
                ave.AssociarGalinheiro(Id);
                _aves.Add(ave);
            }
        }

        public void RealizarAbateAve(Guid aveId, decimal peso)
        {
            if (UtilizaLote) throw new DomainException("Somente lote pode ser abatido.");
            var ave = _aves.FirstOrDefault(x => x.Id == aveId);
            if (ave is null) throw new DomainException("Ave não está cadastrada.");
            ave.RealizarAbate(peso);
        }

        public void RealizarAbateLote(Guid loteId, decimal peso)
        {
            if (!UtilizaLote) throw new DomainException("Galinheiro não utiliza configuração de lote.");
            var lote = _lotes.FirstOrDefault(x => x.Id == loteId);
            if (lote is null) throw new DomainException("Lote não está cadastrado.");
            lote.RealizarAbate(peso);
        }

        public void AlterarEstruturaGalinheiro()
        {
            if (UtilizaLote)
            {
                AtualizarGalinheiroUtilizaLote();
                AdicionarAves(_lotes.SelectMany(x => x.Aves));
                _lotes.RemoveAll(x => x.Id == x.Id);
            }
            else
            {
                AtualizarGalinheiroUtilizaLote();
                var lote = new Lote(capacidade: Capacidade);
                lote.AdicionarAves(_aves);
                AdicionarLote(lote);
                _aves.RemoveAll(x => x.Id == x.Id);
            }
        }

        private void AtualizarGalinheiroUtilizaLote()
            => UtilizaLote = !UtilizaLote;

        public override bool EhValido()
        {
            ValidationResult = new GalinheiroValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class GalinheiroValidation : AbstractValidator<Galinheiro>
    {
        public GalinheiroValidation()
        {
            RuleFor(c => c.Codigo).NotEmpty().WithMessage("Deve ser informado um código de identificação para o galinheiro.");

            RuleFor(c => c.Area).GreaterThan(0).WithMessage("Área do galinheiro deve ser maior que 0m².");

            RuleFor(c => c).Must(CapacidadeValida).WithMessage("A capacidade de aves em um galinheiro deve permitir ao menos 1 ave ou galinheiro deve utilizar lote.");
        }

        private bool CapacidadeValida(Galinheiro galinheiro)
            => galinheiro.Capacidade > 0 || galinheiro.UtilizaLote;
    }
}