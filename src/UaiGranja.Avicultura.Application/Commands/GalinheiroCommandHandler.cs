using MediatR;
using UaiGranja.Avicultura.Domain.Interfaces.Repository;
using UaiGranja.Core.Communication.Mediator;

namespace UaiGranja.Avicultura.Application.Commands
{
    public class GalinheiroCommandHandler :
        IRequestHandler<AdicionarGalinheiroCommand, bool>
    {
        private readonly IGalinheiroRepository _galinheiroRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public GalinheiroCommandHandler(IGalinheiroRepository galinheiroRepository,
                                    IMediatorHandler mediatorHandler)
        {
            _galinheiroRepository = galinheiroRepository;
            _mediatorHandler = mediatorHandler;
        }

        public Task<bool> Handle(AdicionarGalinheiroCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
