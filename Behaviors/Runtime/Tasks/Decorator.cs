using System.Collections.Generic;
using System.Linq;
using Ayla.Inspector.Runtime.Utilities.Scopes;
using Ayla.Inspector.SpecialCase;
using UnityEditor;
using UnityEngine;

namespace Ayla.Behaviors.Runtime.Tasks
{
    public abstract class Decorator : Task
    {
        protected override void OnStandby()
        {
        }

        protected override void OnAborted()
        {
        }

        protected override void OnExit()
        {
        }

        protected override void OnFailure()
        {
        }

        protected override void OnStart()
        {
        }

        protected override void OnSuccess()
        {
        }

        public Task GetChild()
        {
            return InternalGetChildren().FirstOrDefault();
        }

#if UNITY_EDITOR
        private enum ErrorId
        {
            None,
            NoChildren,
            OverChildren,
        }

        private IEnumerable<Task> InternalGetChildren()
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                var child = transform.GetChild(i);
                if (child.TryGetComponent<Task>(out var task))
                {
                    yield return task;
                }
            }
        }

        private bool HasError(out ErrorId errorId)
        {
            int childCount = InternalGetChildren().Count();
            if (childCount <= 0)
            {
                errorId = ErrorId.NoChildren;
                return true;
            }

            if (childCount > 1)
            {
                errorId = ErrorId.OverChildren;
                return true;
            }

            errorId = ErrorId.None;
            return false;
        }

        private float validateInspectorHeight
        {
            get
            {
                if (HasError(out _))
                {
                    return EditorGUIUtility.singleLineHeight * 2.0f + EditorGUIUtility.standardVerticalSpacing;
                }
                else
                {
                    return EditorGUIUtility.singleLineHeight;
                }
            }
        }

        [CustomInspector(nameof(validateInspectorHeight))]
        private void ValidateInspector()
        {
            if (HasError(out var errorId))
            {
                using var scope = new ColorScope(Color.red);
                GUILayout.Label("Invalidate");
                switch (errorId)
                {
                    case ErrorId.NoChildren:
                        GUILayout.Label("There is no child in Decorator.");
                        break;
                    case ErrorId.OverChildren:
                        GUILayout.Label("Decorator only allow single child.");
                        break;
                }
            }
            else
            {
                using var scope = new ColorScope(Color.green);
                GUILayout.Label("Validate");
            }
        }
#endif
    }
}
