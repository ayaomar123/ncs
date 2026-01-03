using MediatR;
using NCS.Application.Common.Exceptions;
using NCS.Application.Interfaces.Repositories;

namespace NCS.Application.Features.Appeals.Commands;

public sealed record DeleteAppealCommand(Guid Id) : IRequest<Unit>;

public sealed class DeleteAppealCommandHandler(IAppealRepository repository) : IRequestHandler<DeleteAppealCommand, Unit>
{
    public async Task<Unit> Handle(DeleteAppealCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
        {
            throw NotFoundException.For("Appeal", request.Id);
        }

        await repository.DeleteAsync(entity, cancellationToken);
        return Unit.Value;
    }
}
