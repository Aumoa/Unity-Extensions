using System.Collections.Generic;

namespace Ayla.Numerics
{
    internal static class ArrayUtility
    {
        private static readonly Dictionary<int, List<double[]>> s_CachedArray = new();

        public static double[] AcquireCached(int count)
        {
            if (s_CachedArray.TryGetValue(count, out var list) == false)
            {
                return new double[count];
            }

            var cached = list[^1];
            list.RemoveAt(list.Count - 1);
            return cached;
        }

        public static void ReleaseCached(double[] scratch)
        {
            int count = scratch.Length;
            if (s_CachedArray.TryGetValue(count, out var list))
            {
                list.Add(scratch);
            }

            list = new List<double[]>
            {
                scratch
            };
            s_CachedArray.Add(count, list);
        }
    }
}