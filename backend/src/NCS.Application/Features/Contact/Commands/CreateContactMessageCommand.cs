using MediatR;
using NCS.Application.Interfaces.Repositories;
using NCS.Domain.Entities;

namespace NCS.Application.Features.Contact.Commands;

public sealed record CreateContactMessageCommand(
    string Name,
    string Email,
    string Subject,
    string Message) : IRequest<Guid>;

public sealed class CreateContactMessageCommandHandler(IContactRepository repository) : IRequestHandler<CreateContactMessageCommand, Guid>
{
    public async Task<Guid> Handle(CreateContactMessageCommand request, CancellationToken cancellationToken)
    {
        var entity = new ContactMessage
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Email = request.Email.Trim(),
            Subject = request.Subject.Trim(),
            Message = request.Message.Trim(),
            CreatedAt = DateTimeOffset.UtcNow
        };

        await repository.AddAsync(entity, cancellationToken);
        return entity.Id;
    }
}
