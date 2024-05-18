using System.Collections.Generic;
using System.Linq;
using Ayla.Inspector;
using UnityEngine;

namespace Ayla.Behaviors.Runtime.Tasks
{
    public abstract class Composite : Task
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

        protected virtual int canAcceptChildren => int.MaxValue;

        public IEnumerable<Task> GetChildren()
        {
            return InternalGetChildren().Take(canAcceptChildren);
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

            if (childCount > canAcceptChildren)
            {
                errorId = ErrorId.OverChildren;
                return true;
            }

            errorId = ErrorId.None;
            return false;
        }

        [CustomInspector]
        private void ValidateInspector()
        {
            if (HasError(out var errorId))
            {
                using var scope = Scopes.ColorScope(Color.red);
                CustomInspectorLayout.LabelField("Invalidate");
                switch (errorId)
                {
                    case ErrorId.NoChildren:
                        CustomInspectorLayout.LabelField("There is no children in Composite.");
                        break;
                    case ErrorId.OverChildren:
                        CustomInspectorLayout.LabelField("There are more children than allowed.");
                        break;
                }
            }
            else
            {
                using var scope = Scopes.ColorScope(Color.green);
                CustomInspectorLayout.LabelField("Validate");
            }
        }
#endif
    }
}
