using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ayla.Inspector
{
    public static class KnownPropertyNames
    {
        public const string k_Application_isPlaying = "@k_Application_isPlaying";

        private static readonly Dictionary<string, Func<object>> s_KnownProperties = new();

        public static bool TryGetKnownProperty(string propertyName, out object value)
        {
            if (propertyName == null)
            {
                value = null;
                return false;
            }
            
            
            if (propertyName == k_Application_isPlaying)
            {
                value = Application.isPlaying;
                return true;
            }

            if (s_KnownProperties.TryGetValue(propertyName, out var predicate))
            {
                value = predicate();
                return true;
            }

            value = null;
            return false;
        }

        public static void RegisterCustomKnownProperty(string propertyName, Func<object> predicate)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("propertyName cannot be null or empty.");
            }

            if (propertyName[0] != '@')
            {
                throw new ArgumentException("propertyName must starts with '@' character.");
            }

            if (s_KnownProperties.TryAdd(propertyName, predicate) == false)
            {
                throw new InvalidOperationException($"Key '{propertyName}' is duplicated.");
            }
        }
    }
}
