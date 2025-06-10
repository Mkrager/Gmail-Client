using GmailClient.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Caching.Memory;

public class GoogleOAuthStateService : IGoogleOAuthStateService
{
    private readonly IMemoryCache _cache;

    public GoogleOAuthStateService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void StoreState(string state, string userId, TimeSpan expiration)
    {
        _cache.Set(state, userId, expiration);
    }

    public bool TryGetUserIdByState(string state, out string userId)
    {
        return _cache.TryGetValue(state, out userId);
    }
}
