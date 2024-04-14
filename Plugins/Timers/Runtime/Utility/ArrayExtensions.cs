using System;

namespace Ayla.Timers.Runtime
{
    internal static class ArrayExtensions
    {
        public static T[] Insert<T>(this T[] array, int index, in T value)
        {
            var newArray = new T[array.Length + 1];
            Array.Copy(array, 0, newArray, 0, index);
            newArray[index] = value;
            Array.Copy(array, index, newArray, index + 1, array.Length - index);
            return newArray;
        }

        public static int FindIndex<T>(this T[] array, Predicate<T> predicate)
        {
            for (int i = 0; i < array.Length; ++i)
            {
                if (predicate(array[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public static T Find<T>(this T[] array, Predicate<T> predicate)
        {
            for (int i = 0; i < array.Length; ++i)
            {
                if (predicate(array[i]))
                {
                    return array[i];
                }
            }

            return default;
        }

        public static ref T FindRef<T>(this T[] array, Predicate<T> predicate)
        {
            for (int i = 0; i < array.Length; ++i)
            {
                if (predicate(array[i]))
                {
                    return ref array[i];
                }
            }

            throw new ArgumentOutOfRangeException();
        }

        public static T[] Add<T>(this T[] array, in T value)
        {
            var newArray = new T[array.Length + 1];
            Array.Copy(array, 0, newArray, 0, array.Length);
            newArray[array.Length] = value;
            return newArray;
        }

        public static void AddRef<T>(ref T[] array, in T value)
        {
            var newArray = new T[array.Length + 1];
            Array.Copy(array, 0, newArray, 0, array.Length);
            newArray[array.Length] = value;
            array = newArray;
        }
    }
}