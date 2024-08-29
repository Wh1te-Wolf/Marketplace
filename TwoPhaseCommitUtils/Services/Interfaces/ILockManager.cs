namespace TwoPhaseCommitUtils.Services.Interfaces;

public interface ILockManager
{
    Task<bool> LockAsync(string objectType, Guid objectUUID);

    Task ReleaseLockAsync(string objectType, Guid objectUUID);

    Task InitialazeAsync();
}
