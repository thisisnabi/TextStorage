using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using TextStorage.Models;
using TextStorage.Persistence;

namespace TextStorage.Features.PasteText;

public static class PasteTextEndpoint
{

    public static void MapPasteTextEndpoint(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapPost("/texts", async (CreatePasteRequest request,
                                            TenantPrincipal tenantPrincipal,
                                           TextStorageDbContext dbContext) =>
        { 
            var randomCode = tenantPrincipal.Predicate +( Random.Shared.Next(77777, int.MaxValue)).ToString();

            var paste = Paste.Create(request.Content, randomCode, request.Password, request.ExpireOn);

            dbContext.Pastes.Add(paste);
            await dbContext.SaveChangesAsync();

            return Results.Ok($"http://localhost:5063/{randomCode}");
        });
    }
}


public record CreatePasteRequest
{
    [Required]
    public required string Content { get; set; }

    public string? Password { get; set; }

    public DateTime? ExpireOn { get; set; }
}