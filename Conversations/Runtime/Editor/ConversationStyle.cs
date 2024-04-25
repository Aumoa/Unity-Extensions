using UnityEditor;
using UnityEngine;

namespace Ayla.Conversations
{
    internal static class ConversationStyle
    {
        public const string k_Elipsis = "¡¦";

        public static readonly GUIStyle s_BreadCrumbLeft;
        public static readonly GUIStyle s_BreadCrumbMid;
        public static readonly GUIStyle s_BreadCrumbLeftBg;
        public static readonly GUIStyle s_BreadCrumbMidBg;
        public static readonly GUIStyle s_BreadCrumbMidSelected;
        public static readonly GUIStyle s_BreadCrumbMidBgSelected;

        public static readonly Texture s_TimelineIcon;

        public static Color colorBackgroundColor => new(0.1f, 0.1f, 0.1f);
        public static Color colorSubSequenceDurationLine => new(0.000f, 1.000f, 0.880f, 0.460f);

        static ConversationStyle()
        {
            s_BreadCrumbLeft = "GUIEditor.BreadcrumbLeft";
            s_BreadCrumbMid = "GUIEditor.BreadcrumbMid";
            s_BreadCrumbLeftBg = "GUIEditor.BreadcrumbLeftBackground";
            s_BreadCrumbMidBg = "GUIEditor.BreadcrumbMidBackground";

            s_BreadCrumbMidSelected = s_BreadCrumbMid;
            s_BreadCrumbMidSelected.normal = s_BreadCrumbMidSelected.onNormal;

            s_BreadCrumbMidBgSelected = s_BreadCrumbMidBg;
            s_BreadCrumbMidBgSelected.normal = s_BreadCrumbMidBgSelected.onNormal;
            s_TimelineIcon = EditorGUIUtility.IconContent("TimelineAsset Icon").image;
        }
    }
}