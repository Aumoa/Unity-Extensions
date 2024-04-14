using UnityEngine;

namespace Ayla.Inspector.Utilities
{
    public static class KnownPropertyNames
    {
        public const string k_Application_isPlaying = "@k_Application_isPlaying";

        public static bool TryGetKnownProperty(string propertyName, out object value)
        {
            if (propertyName == k_Application_isPlaying)
            {
                value = Application.isPlaying;
                return true;
            }

            value = null;
            return false;
        }
    }
}
