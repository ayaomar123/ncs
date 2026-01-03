namespace NCS.Application.Common.Exceptions;

public sealed class NotFoundException(string message) : Exception(message)
{
    public static NotFoundException For(string entityName, object key) =>
        new($"{entityName} was not found (key: {key}).");
}
