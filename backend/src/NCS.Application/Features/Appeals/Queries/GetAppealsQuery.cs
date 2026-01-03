using MediatR;
using NCS.Application.Common.Models;
using NCS.Application.Features.Appeals.Dtos;
using NCS.Application.Features.Appeals.Mappings;
using NCS.Application.Interfaces.Repositories;

namespace NCS.Application.Features.Appeals.Queries;

public sealed record GetAppealsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    bool? IsUrgent = null,
    string? CountryTag = null) : IRequest<PagedResult<AppealDto>>;

public sealed class GetAppealsQueryHandler(IAppealRepository repository) : IRequestHandler<GetAppealsQuery, PagedResult<AppealDto>>
{
    public async Task<PagedResult<AppealDto>> Handle(GetAppealsQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            request.IsUrgent,
            request.CountryTag,
            includeUnpublished: false,
            cancellationToken);

        return new PagedResult<AppealDto>(
            result.Items.Select(x => x.ToDto()).ToList(),
            result.PageNumber,
            result.PageSize,
            result.TotalCount);
    }
}
