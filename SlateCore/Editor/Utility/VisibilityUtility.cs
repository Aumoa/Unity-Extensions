using System;

namespace Ayla.SlateCore
{
    public static class VisibilityUtility
    {
        internal const int VISPRIVATE_Visible = 0x1 << 0;
        internal const int VISPRIVATE_Collapsed = 0x1 << 1;
        internal const int VISPRIVATE_Hidden = 0x1 << 2;
        internal const int VISPRIVATE_SelfHitTestVisible = 0x1 << 3;
        internal const int VISPRIVATE_ChildrenHitTestVisible = 0x1 << 4;

        internal const int VIS_Visible = VISPRIVATE_Visible | VISPRIVATE_SelfHitTestVisible | VISPRIVATE_ChildrenHitTestVisible;
        internal const int VIS_Collapsed = VISPRIVATE_Collapsed;
        internal const int VIS_Hidden = VISPRIVATE_Hidden;
        internal const int VIS_HitTestInvisible = VISPRIVATE_Visible;
        internal const int VIS_SelfHitTestInvisible = VISPRIVATE_Visible | VISPRIVATE_ChildrenHitTestVisible;

        internal const int VIS_All = VISPRIVATE_Visible | VISPRIVATE_Hidden | VISPRIVATE_Collapsed | VISPRIVATE_SelfHitTestVisible | VISPRIVATE_ChildrenHitTestVisible;

        internal static int GetValue(this Visibility visibility)
        {
            return visibility switch
            {
                Visibility.Visible => VIS_Visible,
                Visibility.Collapsed => VIS_Collapsed,
                Visibility.Hidden => VIS_Hidden,
                Visibility.HitTestInvisible => VIS_HitTestInvisible,
                Visibility.SelfHitTestInvisible => VIS_SelfHitTestInvisible,
                Visibility.All => VIS_All,
                _ => throw new ArgumentException()
            };
        }

        public static bool DoesVisibilityPassFilter(this Visibility visibility, Visibility filter)
        {
            return 0 != (GetValue(visibility) & GetValue(filter));
        }

        public static bool AreChildrenHitTestVisible(Visibility visibility)
        {
            return 0 != (GetValue(visibility) & VISPRIVATE_ChildrenHitTestVisible);
        }

        public static bool IsHitTestVisible(Visibility visibility)
        {
            return 0 != (GetValue(visibility) & VISPRIVATE_SelfHitTestVisible);
        }

        public static bool IsVisible(Visibility visibility)
        {
            return 0 != (GetValue(visibility) & VIS_Visible);
        }
    }
}