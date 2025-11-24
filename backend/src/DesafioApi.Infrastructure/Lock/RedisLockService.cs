using DesafioApi.Application.Interfaces;

using Microsoft.Extensions.Logging;

using StackExchange.Redis;

namespace DesafioApi.Infrastructure.Lock;

public class RedisLockService : ILockService, IDisposable
{
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _db;
    private readonly ILogger<RedisLockService> _logger;

    public RedisLockService(string connectionString, ILogger<RedisLockService> logger)
    {
        _logger = logger;
        _redis = ConnectionMultiplexer.Connect($"{connectionString},abortConnect=false");
        _db = _redis.GetDatabase();
    }

    public async Task<bool> TryAcquireLockAsync(string key, TimeSpan ttl, CancellationToken ct = default)
    {
        var lockValue = Guid.NewGuid().ToString();
        _logger.LogInformation("Tentando adquirir lock {Key} com valor {Value}", key, lockValue);

        var acquired = await _db.StringSetAsync(key, lockValue, ttl, when: When.NotExists);
        return acquired;
    }

    public async Task ReleaseLockAsync(string key)
    {
        _logger.LogInformation("Lock liberado: {Key}", key);
        await _db.KeyDeleteAsync(key);
    }

    public void Dispose()
    {
        _redis?.Dispose();
    }
}
