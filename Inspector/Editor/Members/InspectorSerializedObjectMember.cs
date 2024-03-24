using System.Collections.Generic;
using System.Linq;
using Ayla.Inspector.Editor.Extensions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ayla.Inspector.Editor.Members
{
    public class InspectorSerializedObjectMember : InspectorMember
    {
        private readonly SerializedObject serializedObject;

        private InspectorMember[] cachedChildren;

        public InspectorSerializedObjectMember(InspectorMember parent, Object unityObject, SerializedObject serializedObject, string pathName)
            : base(parent, unityObject, () => serializedObject.targetObject, null, null, pathName)
        {
            this.serializedObject = serializedObject;
            CacheChildren();
        }

        public override string name => serializedObject.targetObject.name;

        public override bool isEditable => true;

        public override bool isExpanded => true;

        public override bool isList => false;

        public override ReorderableList GenerateReorderableList()
        {
            return null;
        }

        public override IEnumerable<InspectorMember> GetChildren()
        {
            return cachedChildren;
        }

        public override float GetHeight()
        {
            if (isVisible)
            {
                float height = 0f;
                foreach (var child in GetChildren())
                {
                    height += InspectorDrawer.GetHeight_Element(child);
                }
                return height;
            }
            return 0.0f;
        }

        public override void OnGUI(Rect rect, GUIContent label, bool isLayout)
        {
            var position = rect.position;
            foreach (var child in GetChildren())
            {
                position = InspectorDrawer.OnGUI_Element(child, position, isLayout);
            }
        }

        private void CacheChildren()
        {
            cachedChildren = GetValue().GetInspectorChildren(GetUnityObject(), this, serializedObject.GetChildren()).ToArray();
        }
    }
}
