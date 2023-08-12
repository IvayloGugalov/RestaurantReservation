using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace RestaurantReservation.Core.Model;

public static class Mapper
{
    private static readonly Dictionary<(Type, Type), List<(MethodInfo Get, MethodInfo Set)>> _cache = new();

    public static T Map<T>(object from) where T : class, new ()
    {
        var key = (from: from.GetType(), to: typeof(T));
        if (!_cache.ContainsKey(key))
        {
            PopulateCacheKey(key);
        }

        var result = Activator.CreateInstance<T>()!;
        var entry = _cache[key];
        foreach (var e in entry)
        {
            var val = e.Get.Invoke(from, null);
            e.Set.Invoke(result, new[] { val });
        }

        return result;
    }


    private static void PopulateCacheKey((Type from, Type to) key)
    {
        var fromProps = key.from.GetProperties();
        var toProps = key.to.GetProperties();

        var entry = new List<(MethodInfo, MethodInfo)>();
        foreach (var from in fromProps)
        {
            var to = toProps.FirstOrDefault(x => x.Name == from.Name);
            if (to == null)
            {
                continue;
            }
            entry.Add((from.GetMethod, to.GetMethod));
        }
        _cache[key] = entry;
    }
}
