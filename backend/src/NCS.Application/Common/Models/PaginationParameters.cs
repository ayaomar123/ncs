namespace NCS.Application.Common.Models;

public sealed record PaginationParameters(int PageNumber = 1, int PageSize = 10)
{
    public int Skip => (PageNumber - 1) * PageSize;
}
