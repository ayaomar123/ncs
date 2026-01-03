using MediatR;
using NCS.Application.Common.Exceptions;
using NCS.Application.Features.Appeals.Dtos;
using NCS.Application.Features.Appeals.Mappings;
using NCS.Application.Interfaces.Repositories;

namespace NCS.Application.Features.Appeals.Queries;

public sealed record GetAppealByIdQuery(Guid Id) : IRequest<AppealDto>;

public sealed class GetAppealByIdQueryHandler(IAppealRepository repository) : IRequestHandler<GetAppealByIdQuery, AppealDto>
{
    public async Task<AppealDto> Handle(GetAppealByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
        {
            throw NotFoundException.For("Appeal", request.Id);
        }

        return entity.ToDto();
    }
}
