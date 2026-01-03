using NCS.Domain.Common;
using NCS.Domain.Enums;

namespace NCS.Domain.Entities;

public class DonationRequest : Entity
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "GBP";

    public DonationType Type { get; set; }
    public DonationCategory Category { get; set; }

    public Guid? AppealId { get; set; }

    public string DonorName { get; set; } = string.Empty;
    public string DonorEmail { get; set; } = string.Empty;

    public DonationStatus Status { get; set; } = DonationStatus.Pending;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
