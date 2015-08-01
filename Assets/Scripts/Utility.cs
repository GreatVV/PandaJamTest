using System.Collections.Generic;
using System.Linq;

public static class Utility
{
    public static T Random<T>(this IEnumerable<T> array)
    {
        var random = UnityEngine.Random.Range(0, array.Count());
        return array.ElementAt(random);
    }

}