#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Avalon.Inspector.Utilities
{
    internal static class ListUtility
    {
        private static readonly Dictionary<Type, List<IList>> s_Collection = new();

        public struct Scoped<T> : IDisposable
        {
            public List<T> m_Source;

            public Scoped(List<T> source)
            {
                m_Source = source;
            }

            public void Dispose()
            {
                if (m_Source != null)
                {
                    Release(m_Source);
                    m_Source = null;
                }
            }
        }

        public static List<T> Acquire<T>()
        {
            var type = typeof(T);
            if (s_Collection.TryGetValue(type, out var scratchBuffers) == false)
            {
                scratchBuffers = new List<IList>();
                s_Collection.Add(type, scratchBuffers);
            }

            if (scratchBuffers.Count > 0)
            {
                var lastIndex = scratchBuffers.Count - 1;
                var scratchBuffer = scratchBuffers[lastIndex];
                scratchBuffers.RemoveAt(lastIndex);
                scratchBuffer.Clear();
                return (List<T>)scratchBuffer;
            }
            else
            {
                return new List<T>();
            }
        }

        public static Scoped<T> AcquireScoped<T>()
        {
            return new Scoped<T>(Acquire<T>());
        }

        public static void Release<T>(List<T> scratchBuffer)
        {
            var type = typeof(T);
            if (s_Collection.TryGetValue(type, out var scratchBuffers))
            {
                scratchBuffers.Add(scratchBuffer);
            }
        }

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