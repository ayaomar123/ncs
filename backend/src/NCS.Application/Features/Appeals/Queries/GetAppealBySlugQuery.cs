using MediatR;
using NCS.Application.Common.Exceptions;
using NCS.Application.Features.Appeals.Dtos;
using NCS.Application.Features.Appeals.Mappings;
using NCS.Application.Interfaces.Repositories;

namespace NCS.Application.Features.Appeals.Queries;

public sealed record GetAppealBySlugQuery(string Slug) : IRequest<AppealDto>;

public sealed class GetAppealBySlugQueryHandler(IAppealRepository repository) : IRequestHandler<GetAppealBySlugQuery, AppealDto>
{
    public async Task<AppealDto> Handle(GetAppealBySlugQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetBySlugAsync(request.Slug, cancellationToken);
        if (entity is null)
        {
            throw NotFoundException.For("Appeal", request.Slug);
        }

        if (entity.PublishedAt is null)
        {
            throw NotFoundException.For("Appeal", request.Slug);
        }

        return entity.ToDto();
    }
}
