namespace DesafioApi.Application.Interfaces;

public interface ILockService
{
	Task<bool> TryAcquireLockAsync(string key, TimeSpan ttl, CancellationToken ct = default);
	Task ReleaseLockAsync(string key);
}