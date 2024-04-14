using System.Collections.Generic;
using UnityEngine;

namespace Ayla.Behaviors.Runtime.Utilities
{
    public static class TransformUtility
    {
        private static readonly List<Transform> s_CachedList = new();

        public static Transform[] GetChildren(this Transform parent, bool includeInactive = false)
        {
            s_CachedList.Clear();

            for (int i = 0; i < parent.childCount; ++i)
            {
                var child = parent.GetChild(i);
                if (includeInactive || child.gameObject.activeSelf)
                {
                    s_CachedList.Add(child);
                }
            }

            return s_CachedList.ToArray();
        }
    }
}
