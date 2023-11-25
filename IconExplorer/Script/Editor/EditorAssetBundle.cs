using System.Reflection;
using UnityEditor;

using UnityEngine;

namespace Ayla.Editor.Editor
{
    public static class EditorAssetBundle
    {
        private static AssetBundle s_AssetBundle;

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            var t_EditorGUIUtility = typeof(EditorGUIUtility);
            var m_GetEditorAssetBundle = t_EditorGUIUtility.GetMethod("GetEditorAssetBundle", BindingFlags.NonPublic | BindingFlags.Static);
            s_AssetBundle = (AssetBundle)m_GetEditorAssetBundle.Invoke(null, null);
        }

        public static AssetBundle GetAssetBundle()
        {
            return s_AssetBundle;
        }
    }
}
