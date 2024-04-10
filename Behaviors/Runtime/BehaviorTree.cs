using System.Linq;
using Ayla.Behaviors.Runtime.Tasks;
using Ayla.Behaviors.Runtime.Utilities;
using Ayla.Inspector.Runtime.Utilities.Scopes;
using Ayla.Inspector.SpecialCase;
using UnityEditor;
using UnityEngine;

namespace Ayla.Behaviors.Runtime
{
    public class BehaviorTree : MonoBehaviour
    {
        private Task entryTask;

        private void Awake()
        {
            entryTask = transform.GetChildren().Select(p => p.GetComponent<Task>()).FirstOrDefault();
            if (entryTask == null)
            {
                Debug.LogErrorFormat("There is no entry task.");
                gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (entryTask == null)
            {
                return;
            }

            if (entryTask.lastStatus is not (TaskStatus.Standby or TaskStatus.Running))
            {
                return;
            }

            var status = Task.ExecutePreUpdate(entryTask);
            if (status != TaskStatus.Running)
            {
                return;
            }

            status = Task.ExecuteUpdate(entryTask);
            if (status != TaskStatus.Running)
            {
                return;
            }

            status = Task.ExecuteLateUpdate(entryTask);
            if (status != TaskStatus.Running)
            {
                return;
            }
        }

        private void OnEnable()
        {
            if (didAwake && entryTask)
            {
                Task.ResetForChildren(entryTask);
            }
        }

#if UNITY_EDITOR
        private enum ErrorId
        {
            None,
            NoEntryTask
        }

        private bool HasError(out ErrorId errorId)
        {
            if (transform.GetChildren().Any(p => p.GetComponent<Task>()) == false)
            {
                errorId = ErrorId.NoEntryTask;
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
        private void ValidateInspector(Rect position, GUIContent label)
        {
            if (HasError(out var errorId))
            {
                using var scope = new ColorScope(Color.red);
                position.height = EditorGUIUtility.singleLineHeight;
                GUI.Label(position, "Invalidate");
                position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
                switch (errorId)
                {
                    case ErrorId.NoEntryTask:
                        GUI.Label(position, "There is no entry task in BehaviorTree.");
                        break;
                }
            }
            else
            {
                using var scope = new ColorScope(Color.green);
                GUI.Label(position, "Validate");
            }
        }
#endif
    }
}
