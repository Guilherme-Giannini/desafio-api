using Xunit;
using DesafioApi.Infrastructure.Lock;
using Microsoft.Extensions.Logging.Abstractions;

namespace DesafioApi.Tests.Lock
{
    public class RedisLockServiceTests
    {
        [Fact]
        public async Task TesteConcorrenciaLock()
        {
            var lockService = new RedisLockService("localhost:6379", new NullLogger<RedisLockService>());
            var key = "teste-lock";

            var resultados = await Task.WhenAll(
                lockService.TryAcquireLockAsync(key, TimeSpan.FromSeconds(5)),
                lockService.TryAcquireLockAsync(key, TimeSpan.FromSeconds(5)),
                lockService.TryAcquireLockAsync(key, TimeSpan.FromSeconds(5))
            );

            Assert.Equal(1, resultados.Count(r => r));

            await lockService.ReleaseLockAsync(key);
        }
    }
}