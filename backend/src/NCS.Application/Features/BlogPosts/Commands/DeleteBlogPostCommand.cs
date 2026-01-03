using MediatR;
using NCS.Application.Common.Exceptions;
using NCS.Application.Interfaces.Repositories;

namespace NCS.Application.Features.BlogPosts.Commands;

public sealed record DeleteBlogPostCommand(Guid Id) : IRequest<Unit>;

public sealed class DeleteBlogPostCommandHandler(IBlogPostRepository repository) : IRequestHandler<DeleteBlogPostCommand, Unit>
{
    public async Task<Unit> Handle(DeleteBlogPostCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
        {
            throw NotFoundException.For("BlogPost", request.Id);
        }

        await repository.DeleteAsync(entity, cancellationToken);
        return Unit.Value;
    }
}
