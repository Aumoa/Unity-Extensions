using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

namespace Ayla.Editor.Editor.IconExplorer
{
    public class EditorIcon : Button
    {
        public EditorIcon(Texture2D texture, string path)
        {
            Texture = texture;
            Path = path;

            style.width = Width;
            style.height = Height;
            style.marginLeft = 0;
            style.marginTop = 0;
            style.marginRight = 0;
            style.marginBottom = 0;

            Add(new Image
            {
                image = texture,
                style =
                {
                    width = Length.Percent(100), height = Length.Percent(100),
                    marginLeft = 0, marginTop = 0, marginRight = 0, marginBottom = 0,
                }
            });
        }

        public Texture Texture { get; }

        public string Path { get; }

        public string Name => Texture.name;

        public float Width => 32;

        public float Height => 32;
    }
}
