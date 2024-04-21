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
            var status = ExecutePreUpdate(m_RootTask);
            if (status == TaskStatus.Running)
            {
                return TaskStatus.Running;
            }

            return status;
        }

        protected override TaskStatus OnUpdate()
        {
            var status = ExecuteUpdate(m_RootTask);
            if (status == TaskStatus.Running)
            {
                return TaskStatus.Running;
            }

            return status;
        }

        protected override TaskStatus OnLateUpdate()
        {
            var status = ExecuteLateUpdate(m_RootTask);
            if (status == TaskStatus.Running)
            {
                return TaskStatus.Running;
            }

            return status;
        }
    }
}