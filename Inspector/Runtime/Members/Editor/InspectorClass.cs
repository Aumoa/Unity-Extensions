using System;
using System.Linq;
using Ayla.Inspector.Editor;
using Ayla.Inspector.Editor.Members;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ayla.Inspector.Members.Editor
{
    public class InspectorClass : InspectorMember
    {
        private readonly Type m_ClassType;
        
        public InspectorClass(InspectorMember parent, Object unityObject, Type classType)
            : base(parent, unityObject, null, null, null, "@class")
        {
            m_ClassType = classType;
        }

        public override void OnGUI(Rect rect, GUIContent label)
        {
            InspectorDrawer.EvaluateDecorators(rect, this, false, true);
        }

        public override float GetHeight()
        {
            return InspectorDrawer.EvaluateDecorators(default, this, false, false);
        }

        public override ReorderableList GenerateReorderableList()
        {
            return null;
        }

        public override Type GetMemberType()
        {
            return m_ClassType;
        }

        public override InspectorMember[] GetChildren() => Array.Empty<InspectorMember>();

        public override Attribute[] GetCustomAttributes(Type type, bool inherit = false)
        {
            if (inherit)
            {
                return m_ClassType.BaseType?.GetCustomAttributes(type, true).OfType<Attribute>().ToArray() ?? Array.Empty<Attribute>();
            }
            else
            {
                return m_ClassType.GetCustomAttributes(type, false).OfType<Attribute>().ToArray();
            }
        }

        public override string name => m_ClassType.Name;

        public override bool isEditable => true;

        public override bool isExpanded
        {
            get => InspectorDrawer.IsExpanded(this);
            set => InspectorDrawer.UpdateExpanded(this, value);
        }

        public override bool isExpandable => false;

        public override bool isList => false;

        public override bool isInline => true;

        public override bool isVisible => true;
    }
}