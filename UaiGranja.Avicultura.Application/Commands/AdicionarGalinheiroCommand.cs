using FluentValidation;
using UaiGranja.Core.Messages;

namespace UaiGranja.Avicultura.Application.Commands
{
    public class AdicionarGalinheiroCommand : Command
    {
        public string Codigo { get; private set; }
        public decimal Area { get; private set; }
        public int Capacidade { get; private set; }
        public bool UtilizaLote { get; private set; }

        public AdicionarGalinheiroCommand(string codigo, decimal area, int capacidade, bool utilizaLote)
        {
            Codigo = codigo;
            Area = area;
            Capacidade = capacidade;
            UtilizaLote = utilizaLote;
        }

        public override bool EhValido()
        {
            ValidationResult = new AdicionarGalinheiroValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AdicionarGalinheiroValidation : AbstractValidator<AdicionarGalinheiroCommand>
    {
        public AdicionarGalinheiroValidation()
        {
            RuleFor(c => c.Codigo).NotEmpty().WithMessage("Deve ser informado um código de identificação para o galinheiro.");

            RuleFor(c => c.Area).GreaterThan(0).WithMessage("Área do galinheiro deve ser maior que 0m².");

            RuleFor(c => c).Must(CapacidadeValida).WithMessage("A capacidade de aves em um galinheiro deve permitir ao menos 1 ave ou galinheiro deve utilizar lote.");
        }

        private bool CapacidadeValida(AdicionarGalinheiroCommand command)
            => command.Capacidade > 0 || command.UtilizaLote;
    }
}
