using Ayla.Behaviors.Runtime.Tasks;
using Ayla.Inspector.Editor;
using Ayla.Inspector.SpecialCase;
using Ayla.Inspector.Utilities;
using UnityEditor;
using UnityEngine;

namespace Ayla.Behaviors.Runtime
{
    public class BehaviorTree : MonoBehaviour
    {
        private Task m_RootTask;

        private void Awake()
        {
            m_RootTask = GetRootTask();
            if (m_RootTask == null)
            {
                Debug.LogErrorFormat("There is no entry task.");
                gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            if (m_RootTask)
            {
                Task.ExecuteStandby(m_RootTask);
            }
        }

        private void Update()
        {
            if (m_RootTask == null)
            {
                return;
            }

            if (m_RootTask.lastStatus is not (TaskStatus.Standby or TaskStatus.Running))
            {
                return;
            }

            var status = Task.ExecutePreUpdate(m_RootTask);
            if (status != TaskStatus.Running)
            {
                return;
            }

            status = Task.ExecuteUpdate(m_RootTask);
            if (status != TaskStatus.Running)
            {
                return;
            }

            Task.ExecuteLateUpdate(m_RootTask);
        }

        private void OnEnable()
        {
            if (didAwake && m_RootTask)
            {
                Task.ResetForChildren(m_RootTask);
            }
        }

        public Task GetRootTask()
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                var child = transform.GetChild(i);
                if (child.TryGetComponent<Task>(out var task))
                {
                    return task;
                }
            }

            return null;
        }

#if UNITY_EDITOR
        private enum ErrorId
        {
            None,
            NoEntryTask
        }

        private bool HasError(out ErrorId errorId)
        {
            if (GetRootTask() == null)
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

        [CustomInspector]
        private void ValidateInspector()
        {
            if (HasError(out var errorId))
            {
                using var scope = Scopes.ColorScope(Color.red);
                CustomInspectorLayout.LabelField("Invalid");
                switch (errorId)
                {
                    case ErrorId.NoEntryTask:
                        CustomInspectorLayout.LabelField("There is no entry task in BehaviorTree.");
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
