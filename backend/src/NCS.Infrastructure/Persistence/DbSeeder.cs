using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NCS.Application.Interfaces.Auth;
using NCS.Domain.Entities;
using NCS.Domain.Enums;

namespace NCS.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(NcsDbContext db, IConfiguration config, IPasswordHasher passwordHasher, CancellationToken cancellationToken)
    {
        if (!await db.AdminUsers.AnyAsync(cancellationToken))
        {
            var adminEmail = config["Admin:Email"];
            var adminPassword = config["Admin:Password"];

            if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
            {
                db.AdminUsers.Add(new AdminUser
                {
                    Id = Guid.NewGuid(),
                    Email = adminEmail.Trim(),
                    PasswordHash = passwordHasher.HashPassword(adminPassword),
                    CreatedAt = DateTimeOffset.UtcNow
                });
            }
        }

        if (!await db.Appeals.AnyAsync(cancellationToken))
        {
            db.Appeals.AddRange(
                new Appeal
                {
                    Id = Guid.NewGuid(),
                    Title = "Emergency Food Relief",
                    Slug = "emergency-food-relief",
                    Summary = "Provide emergency food packages to families affected by crisis.",
                    Description = "Your donation will help NCS deliver emergency food parcels to families in urgent need.",
                    CountryTag = "Nigeria",
                    IsUrgent = true,
                    TargetAmount = 20000,
                    RaisedAmount = 3500,
                    CoverImageUrl = "/uploads/sample-food.jpg",
                    GalleryUrls = [],
                    Status = AppealStatus.Published,
                    CreatedAt = DateTimeOffset.UtcNow.AddDays(-10),
                    PublishedAt = DateTimeOffset.UtcNow.AddDays(-9)
                },
                new Appeal
                {
                    Id = Guid.NewGuid(),
                    Title = "Clean Water Wells",
                    Slug = "clean-water-wells",
                    Summary = "Help communities access clean and safe drinking water.",
                    Description = "We build and maintain water wells and sanitation points in underserved areas.",
                    CountryTag = "Kenya",
                    IsUrgent = false,
                    TargetAmount = 50000,
                    RaisedAmount = 12000,
                    CoverImageUrl = "/uploads/sample-water.jpg",
                    GalleryUrls = [],
                    Status = AppealStatus.Published,
                    CreatedAt = DateTimeOffset.UtcNow.AddDays(-20),
                    PublishedAt = DateTimeOffset.UtcNow.AddDays(-18)
                });
        }

        if (!await db.BlogPosts.AnyAsync(cancellationToken))
        {
            db.BlogPosts.AddRange(
                new BlogPost
                {
                    Id = Guid.NewGuid(),
                    Title = "How your donations create impact",
                    Slug = "how-your-donations-create-impact",
                    Excerpt = "A quick look at how NCS turns generosity into real-world support.",
                    Content = "## Impact\n\nEvery donation helps us deliver food, water, and medical aid where it is needed most.",
                    CoverImageUrl = "/uploads/sample-impact.jpg",
                    Tags = ["impact", "stories"],
                    CreatedAt = DateTimeOffset.UtcNow.AddDays(-14),
                    PublishedAt = DateTimeOffset.UtcNow.AddDays(-14),
                    IsPublished = true
                },
                new BlogPost
                {
                    Id = Guid.NewGuid(),
                    Title = "Emergency response update",
                    Slug = "emergency-response-update",
                    Excerpt = "Latest update from our on-the-ground team.",
                    Content = "## Update\n\nThanks to supporters, we have reached more families this week.",
                    CoverImageUrl = "/uploads/sample-update.jpg",
                    Tags = ["news"],
                    CreatedAt = DateTimeOffset.UtcNow.AddDays(-7),
                    PublishedAt = DateTimeOffset.UtcNow.AddDays(-7),
                    IsPublished = true
                });
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}
