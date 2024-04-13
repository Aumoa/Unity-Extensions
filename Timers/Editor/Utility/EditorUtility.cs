using UnityEngine;

namespace Ayla.Timers.Editor.Utility
{
    internal static class EditorUtility
    {
        public static bool IsEnterKeyPressed()
        {
            return Event.current.isKey && Event.current.keyCode == KeyCode.Return;
        }
    }
}