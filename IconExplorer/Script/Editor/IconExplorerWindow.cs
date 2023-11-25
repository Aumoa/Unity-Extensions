using System.Diagnostics.CodeAnalysis;
using System.Linq;

using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

namespace Ayla.Editor.Editor.IconExplorer
{
    public class IconExplorerWindow : EditorWindow
    {
        private ScrollView scrollLayout;
        private EditorIcon selected;
        private IconDetailPanel detailPanel;
        private Vector2 layoutSize;

        private void OnEnable()
        {
            rootVisualElement.style.flexDirection = FlexDirection.Column;
            rootVisualElement.style.height = Length.Percent(100);

            scrollLayout = new ScrollView
            {
                style =
                {
                    width = Length.Auto(), height = Length.Percent(100),
                    flexDirection = FlexDirection.Column
                }
            };
            rootVisualElement.Add(scrollLayout);

            detailPanel = new IconDetailPanel()
            {
                style =
                {
                }
            };
            rootVisualElement.Add(detailPanel);

            foreach (var icon in IconExplorer.GetEditorIcons().ToArray())
            {
                scrollLayout.Add(icon);
                icon.clicked += () => OnEditorIconClicked(icon);
            }

            PrepassLayout();
        }

        private void OnGUI()
        {
            if (layoutSize != position.size)
            {
                PrepassLayout();
            }
        }

        private void OnEditorIconClicked(EditorIcon icon)
        {
            if (selected != icon)
            {
                selected = icon;
                OnSelectedIconChanged();
            }
        }

        private void OnSelectedIconChanged()
        {
            detailPanel.SelectItem(selected);
        }

        [SuppressMessage("Style", "IDE0220:명시적 캐스트 추가", Justification = "<보류 중>")]
        private void PrepassLayout()
        {
            const float margin = 2;
            const float scrollBarSpace = 16;
            float offsetX = margin;
            float offsetY = margin;

            foreach (EditorIcon icon in scrollLayout.Children())
            {
                float appX = icon.Width + margin;
                if (offsetX >= position.width - scrollBarSpace - appX + margin)
                {
                    offsetY += icon.Height + margin;
                    offsetX = margin;
                }

                icon.style.left = offsetX;
                icon.style.top = offsetY;
                icon.style.paddingLeft = margin;
                icon.style.paddingTop = margin;
                icon.style.paddingRight = margin;
                icon.style.paddingBottom = margin;
                icon.style.position = Position.Absolute;

                offsetX += appX;
            }

            scrollLayout.style.height = position.height - 80;
            layoutSize = position.size;
        }
    }
}
