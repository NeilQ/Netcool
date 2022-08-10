using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Netcool.Caching;
using NUnit.Framework;

namespace Netcool.Tests;

public class RedisCacheTests
{
    private NetcoolRedisCache CreateConnection()
    {
        var options = Options.Create(new NetcoolRedisCacheOptions()
            { Configuration = "localhost", InstanceName = "netcool-test:" });
        return new NetcoolRedisCache(options);
    }

    private readonly DistributedCacheEntryOptions _entryOptions =
        new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(5));

    [Test]
    public void Increase()
    {
        var cache = CreateConnection();

        cache.SetString("t1", "1", _entryOptions);
        var curr = cache.Increase("t1", 2);
        var res = cache.GetString("t1");
        Assert.AreEqual("3", res);
        Assert.AreEqual(3, curr);
    }

    [Test]
    public async Task IncreaseAsync()
    {
        var cache = CreateConnection();

        cache.SetString("t1", "1", _entryOptions);
        var curr = await cache.IncreaseAsync("t1", 2);
        var res = cache.GetString("t1");
        Assert.AreEqual(3, curr);
        Assert.AreEqual("3", res);
    }

    [Test]
    public void IncreaseWithLimit()
    {
        var cache = CreateConnection();

        cache.SetString("t1", "1", _entryOptions);
        var curr = cache.Increase("t1", 2, 2);
        var res = cache.GetString("t1");
        Assert.AreEqual(2, curr);
        Assert.AreEqual("2", res);

        cache.SetString("t1", "1", _entryOptions);
        var curr2 = cache.Increase("t1", 2, 10);
        var res2 = cache.GetString("t1");
        Assert.AreEqual(3, curr2);
        Assert.AreEqual("3", res2);
    }

    [Test]
    public async Task IncreaseWithLimitAsync()
    {
        var cache = CreateConnection();

        cache.SetString("t1", "1", _entryOptions);
        var curr = await cache.IncreaseAsync("t1", 2, 2);
        var res = cache.GetString("t1");
        Assert.AreEqual(2, curr);
        Assert.AreEqual("2", res);

        cache.SetString("t1", "1", _entryOptions);
        var curr2 = await cache.IncreaseAsync("t1", 2, 10);
        var res2 = cache.GetString("t1");
        Assert.AreEqual(3, curr2);
        Assert.AreEqual("3", res2);
    }


    [Test]
    public void Decrease()
    {
        var cache = CreateConnection();

        cache.SetString("t1", "1", _entryOptions);
        var curr = cache.Decrease("t1", 2);
        var res = cache.GetString("t1");
        Assert.AreEqual("-1", res);
        Assert.AreEqual(-1, curr);
    }


    [Test]
    public async Task DecreaseAsync()
    {
        var cache = CreateConnection();

        cache.SetString("t1", "1", _entryOptions);
        var curr = await cache.DecreaseAsync("t1", 2);
        var res = cache.GetString("t1");
        Assert.AreEqual("-1", res);
        Assert.AreEqual(-1, curr);
    }


    [Test]
    public void DecreaseWithLimit()
    {
        var cache = CreateConnection();

        cache.SetString("t1", "1", _entryOptions);
        var curr = cache.Decrease("t1", 2, 1);
        var res = cache.GetString("t1");
        Assert.AreEqual(1, curr);
        Assert.AreEqual("1", res);

        cache.SetString("t1", "1", _entryOptions);
        var curr2 = cache.Decrease("t1", 2, -10);
        var res2 = cache.GetString("t1");
        Assert.AreEqual(-1, curr2);
        Assert.AreEqual("-1", res2);
    }

    [Test]
    public async Task DecreaseWithLimitAsync()
    {
        var cache = CreateConnection();

        cache.SetString("t1", "1", _entryOptions);
        var curr = await cache.DecreaseAsync("t1", 2, 1);
        var res = cache.GetString("t1");
        Assert.AreEqual(1, curr);
        Assert.AreEqual("1", res);

        cache.SetString("t1", "1", _entryOptions);
        var curr2 = await cache.DecreaseAsync("t1", 2, -10);
        var res2 = cache.GetString("t1");
        Assert.AreEqual(-1, curr2);
        Assert.AreEqual("-1", res2);
    }
}
