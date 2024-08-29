using System.Collections.Concurrent;
using TwoPhaseCommitUtils.Entities;
using TwoPhaseCommitUtils.Repository;
using TwoPhaseCommitUtils.Services.Interfaces;

namespace TwoPhaseCommitUtils.Services;

public class PrepareManager : ILockManager
{
    private ConcurrentDictionary<Guid, string> _locks = new ConcurrentDictionary<Guid, string>();

    public async Task InitialazeAsync()
    {
        await using TwoPhaseCommitContext context = new TwoPhaseCommitContext();
        foreach (var lockData in context.LockDatas)
        {
            _locks.TryAdd(lockData.UUID, lockData.Type);
        }
    }

    public async Task<bool> LockAsync(string objectType, Guid objectUUID)
    {
        try
        {
            if (_locks.TryAdd(objectUUID, objectType))
            {
                await using TwoPhaseCommitContext context = new TwoPhaseCommitContext();
                context.LockDatas.Add(new LockData(objectUUID, objectType));
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        { 
            _locks.TryRemove(objectUUID, out _);
            return false;
        }
    }

    public async Task ReleaseLockAsync(string objectType, Guid objectUUID)
    {
        _locks.TryRemove(objectUUID, out _);
        await using TwoPhaseCommitContext context = new TwoPhaseCommitContext();
        context.LockDatas.Remove(new LockData(objectUUID, objectType));
        context.SaveChanges();
    }
}
