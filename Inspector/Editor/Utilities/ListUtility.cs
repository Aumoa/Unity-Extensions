#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;

namespace Ayla.Inspector
{
    internal static class ListUtility
    {
        public static bool TryPop<T>(List<T> source, out T value)
        {
            if (source.Count > 0)
            {
                int lastIndex = source.Count - 1;
                value = source[lastIndex];
                source.RemoveAt(lastIndex);
                return true;
            }

            value = default;
            return false;
        }

        public static bool IsValidIndex<T>(this IList<T> source, int index)
        {
            return index >= 0 && source.Count > index;
        }

        public static T GetOrDefault<T>(this IList<T> source, int index)
        {
            return IsValidIndex(source, index) ? source[index] : default;
        }

        public static T[] AsArray<T>(this IList<T> source)
        {
            if (source is T[] array)
            {
                return array;
            }
            else
            {
                return source.ToArray();
            }
        }
    }
}
#endif