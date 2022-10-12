using System.Collections.Concurrent;

namespace Netcool.Caching;

public class DynamicLock
{
    private static readonly ConcurrentDictionary<string, Guid> _locksByKeys = new();

    public static void ExecuteLock(string key, Action action)
    {
        var guid = Guid.NewGuid();

        while (!_locksByKeys.TryAdd(key, guid)) Thread.Yield();

        try
        {
            action.Invoke();
        }
        finally
        {
            if (!_locksByKeys.TryRemove(key, out var guidOut) || guidOut != guid)
            {
                throw new InvalidOperationException("It all went wrong");
            }
        }
    }
   
}
