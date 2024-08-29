using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using TextStorage.Models;
using TextStorage.Persistence;

namespace TextStorage.Features.Reading;

public static class ReadingEndpoint
{

    public static void MapReadingEndpoint(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet("/{code}", async (
            [FromRoute(Name = "code")] string code,
            IDistributedCache _cache,
            IServiceProvider serviceProvider) =>
        {
            var content = await _cache.GetStringAsync(code);

            if (!string.IsNullOrEmpty(content))
                 return Results.Ok(content);

            var dbContext = serviceProvider.GetRequiredService<TextStorageDbContextReadOnly>();
            var item = (await dbContext.Pastes.FirstOrDefaultAsync(d => d.ShortenCode == code));
            if(item is null)
                    return Results.NotFound();

            _cache.SetStringAsync(code, item.Content);

            return Results.Ok(item.Content);
        }).CacheOutput();
    }
}

 