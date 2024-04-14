using System;
using UnityEngine;

namespace Ayla.Timers.Editor.Utility
{
    public static class LayoutUtility
    {
        public readonly struct HorizontalScoped : IDisposable
        {
            public readonly void Dispose()
            {
                GUILayout.EndHorizontal();
            }
        }

        public static HorizontalScoped HorizontalScope()
        {
            GUILayout.BeginHorizontal();
            return new HorizontalScoped();
        }
    }
}