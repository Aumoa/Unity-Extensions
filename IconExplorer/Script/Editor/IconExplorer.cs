using System.Collections.Generic;

using UnityEngine;

namespace Ayla.Editor.Editor.IconExplorer
{
    public static class IconExplorer
    {
        public static IEnumerable<EditorIcon> GetEditorIcons()
        {
            var assetBundle = EditorAssetBundle.GetAssetBundle();
            foreach (var assetPath in assetBundle.GetAllAssetNames())
            {
                Texture2D texture = assetBundle.LoadAsset<Texture2D>(assetPath);
                if (texture == null)
                {
                    continue;
                }

                yield return new EditorIcon(texture, assetPath);
            }
        }
    }
}
