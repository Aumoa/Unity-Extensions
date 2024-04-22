using UnityEngine;

namespace Ayla.Behaviors.Runtime.Tasks.Actions
{
    [AddComponentMenu("Behaviors/Actions/External Behavior Tree")]
    public class ExternalBehaviorTree : Action
    {
        [SerializeField]
        private BehaviorTree m_ExternalBehaviorTree;

        private Task m_RootTask;

        protected override void OnStandby()
        {
            var rootTaskAsset = m_ExternalBehaviorTree.GetRootTask();
            if (rootTaskAsset == null)
            {
                return;
            }

            m_RootTask = Instantiate(rootTaskAsset, transform);
            ExecuteStandby(m_RootTask);
        }

        protected override TaskStatus OnPreUpdate()
        {
            if (m_RootTask == null)
            {
                return TaskStatus.Failure;
            }

            var status = ExecutePreUpdate(m_RootTask);
            if (status == TaskStatus.Running)
            {
                return TaskStatus.Running;
            }

            return status;
        }

        protected override TaskStatus OnUpdate()
        {
            if (m_RootTask == null)
            {
                return TaskStatus.Failure;
            }

            var status = ExecuteUpdate(m_RootTask);
            if (status == TaskStatus.Running)
            {
                return TaskStatus.Running;
            }

            return status;
        }

        protected override TaskStatus OnLateUpdate()
        {
            if (m_RootTask == null)
            {
                return TaskStatus.Failure;
            }

            var status = ExecuteLateUpdate(m_RootTask);
            if (status == TaskStatus.Running)
            {
                return TaskStatus.Running;
            }

            return status;
        }

        protected override void OnExit()
        {
            if (m_RootTask)
            {
                Destroy(m_RootTask.gameObject);
                m_RootTask = null;
            }
        }
    }
}