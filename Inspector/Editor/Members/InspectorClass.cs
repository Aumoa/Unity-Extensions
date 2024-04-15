using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ayla.Inspector.Editor.Members
{
    public class InspectorClass : InspectorMember
    {
        private readonly Type classType;
        
        public InspectorClass(InspectorMember parent, Object unityObject, Type classType)
            : base(parent, unityObject, null, null, null, "@class")
        {
            this.classType = classType;
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
            return classType;
        }

        public override IEnumerable<InspectorMember> GetChildren() => Enumerable.Empty<InspectorMember>();

        public override Attribute[] GetCustomAttributes(Type type, bool inherit = false)
        {
            if (inherit)
            {
                return classType.BaseType?.GetCustomAttributes(type, true).OfType<Attribute>().ToArray() ?? Array.Empty<Attribute>();
            }
            else
            {
                return classType.GetCustomAttributes(type, false).OfType<Attribute>().ToArray();
            }
        }

        public override string name => classType.Name;

        public override bool isEditable => true;

        public override bool isExpanded
        {
            get => InspectorDrawer.IsExpanded(this);
            set => InspectorDrawer.UpdateExpanded(this, value);
        }

        public override bool isExpandable => false;

        public override bool isList => false;

        public override bool isVisible => true;
    }
}