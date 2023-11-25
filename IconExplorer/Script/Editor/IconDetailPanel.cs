using UnityEngine;
using UnityEngine.UIElements;

namespace Ayla.Editor.Editor.IconExplorer
{
    public class IconDetailPanel : VisualElement
    {
        private Image whiteImage;
        private Image blackImage;
        private TextElement displayName;
        private TextElement path;

        public IconDetailPanel()
        {
            style.width = Length.Percent(100);
            style.height = 80;
            style.flexDirection = FlexDirection.Row;
            style.paddingLeft = 0;
            style.paddingTop = 0;
            style.paddingRight = 0;
            style.paddingBottom = 0;

            var previewLayout = new VisualElement
            {
                style =
                {
                    width = 160 - 4, height = 72,
                    marginLeft = 0, marginTop = 0, marginRight = 0, marginBottom = 0,
                    paddingLeft = 4, paddingTop = 4, paddingRight = 4, paddingBottom = 4,
                    flexDirection = FlexDirection.Row
                }
            };
            Add(previewLayout);

            whiteImage = new Image
            {
                style =
                {
                    width = 72, height = 72,
                    marginLeft = 0, marginTop = 0, marginRight = 0, marginBottom = 0,
                    backgroundColor = Color.white
                }
            };
            previewLayout.Add(whiteImage);
            blackImage = new Image
            {
                style =
                {
                    width = 72, height = 72,
                    marginLeft = 4, marginTop = 0, marginRight = 0, marginBottom = 0,
                    backgroundColor = Color.black
                }
            };
            previewLayout.Add(blackImage);

            var detailLayout = new VisualElement
            {
                style =
                {
                    width = Length.Auto(), height = Length.Auto(),
                    borderLeftColor = Color.black, borderLeftWidth = 1,
                    marginLeft = 0, marginTop = 0, marginRight = 0, marginBottom = 0,
                    paddingLeft = 4, paddingTop = 4, paddingRight = 4, paddingBottom = 4
                }
            };
            Add(detailLayout);

            var displayNameLayout = new VisualElement
            {
                style =
                {
                    width = Length.Percent(100), height = Length.Auto(),
                    flexDirection = FlexDirection.Row,
                    marginLeft = 0, marginTop = 0, marginRight = 0, marginBottom = 4,
                    paddingLeft = 0, paddingTop = 0, paddingRight = 0, paddingBottom = 0
                }
            };
            displayNameLayout.Add(new TextElement { text = "Name: " });
            detailLayout.Add(displayNameLayout);

            displayName = new TextElement();
            displayNameLayout.Add(displayName);

            var pathLayout = new VisualElement
            {
                style =
                {
                    width = Length.Percent(100), height = Length.Auto(),
                    flexDirection = FlexDirection.Row,
                    marginLeft = 0, marginTop = 0, marginRight = 0, marginBottom = 4,
                    paddingLeft = 0, paddingTop = 0, paddingRight = 0, paddingBottom = 0
                }
            };
            pathLayout.Add(new TextElement { text = "Path: " });
            detailLayout.Add(pathLayout);

            path = new TextElement();
            pathLayout.Add(path);

            SelectItem(null);
        }

        public void SelectItem(EditorIcon icon)
        {
            if (icon == null)
            {
                whiteImage.image = null;
                whiteImage.style.visibility = Visibility.Hidden;
                blackImage.image = null;
                blackImage.style.visibility = Visibility.Hidden;
                displayName.text = string.Empty;
                path.text = string.Empty;
                return;
            }

            whiteImage.image = icon.Texture;
            whiteImage.style.visibility = Visibility.Visible;
            blackImage.image = icon.Texture;
            blackImage.style.visibility = Visibility.Visible;
            displayName.text = icon.Name;
            path.text = icon.Path;
        }

        public float Height => 80;
    }
}
