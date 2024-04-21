using Ayla.Inspector.Meta;
using UnityEngine;

namespace Ayla.Behaviors.Runtime.Tasks
{
    public abstract class Task : MonoBehaviour
    {
        [ShowNativeMember]
        public TaskStatus lastStatus { get; private set; } = TaskStatus.Standby;

        protected abstract void OnStandby();

        protected abstract void OnStart();

        protected abstract TaskStatus OnPreUpdate();

        protected abstract TaskStatus OnUpdate();

        protected abstract TaskStatus OnLateUpdate();

        protected abstract void OnSuccess();

        protected abstract void OnFailure();

        protected abstract void OnAborted();

        protected abstract void OnExit();

        public static void ExecuteStandby(Task task)
        {
            task.OnStandby();

            if (task is Composite)
            {
                var transform = task.transform;
                for (int i = 0; i < transform.childCount; ++i)
                {
                    ExecuteStandby(transform.GetChild(i).GetComponent<Task>());
                }
            }
        }

        public static void ResetForChildren(Task task)
        {
            foreach (var child in task.GetComponentsInChildren<Task>())
            {
                child.lastStatus = TaskStatus.Standby;
            }
        }

        public static TaskStatus ExecutePreUpdate(Task task)
        {
            if (task.lastStatus == TaskStatus.Standby)
            {
                task.OnStart();
                task.lastStatus = TaskStatus.Running;
            }

            var status = task.OnPreUpdate();
            if (status != TaskStatus.Running)
            {
                HandleTaskCompletedStatus(task, status);
                task.lastStatus = status;
            }

            return status;
        }

        public static TaskStatus ExecuteUpdate(Task task)
        {
            if (task.lastStatus != TaskStatus.Running)
            {
                Debug.LogErrorFormat("Invalid status detected. task.lastStatus must be TaskStatus.Running after handled by ExecuteBeforeUpdate(Task) function.");
                task.lastStatus = TaskStatus.Aborted;
                return TaskStatus.Aborted;
            }

            var status = task.OnUpdate();
            if (status != TaskStatus.Running)
            {
                HandleTaskCompletedStatus(task, status);
                task.lastStatus = status;
            }

            return status;
        }

        public static TaskStatus ExecuteLateUpdate(Task task)
        {
            if (task.lastStatus != TaskStatus.Running)
            {
                Debug.LogErrorFormat("Invalid status detected. task.lastStatus must be TaskStatus.Running after handled by ExecuteBeforeUpdate(Task) function.");
                task.lastStatus = TaskStatus.Aborted;
                return TaskStatus.Aborted;
            }

            var status = task.OnLateUpdate();
            if (status != TaskStatus.Running)
            {
                HandleTaskCompletedStatus(task, status);
                task.lastStatus = status;
            }

            return status;
        }

        private static void HandleTaskCompletedStatus(Task task, TaskStatus status)
        {
            switch (status)
            {
                case TaskStatus.Success:
                    task.OnSuccess();
                    break;
                case TaskStatus.Failure:
                    task.OnFailure();
                    break;
                case TaskStatus.Aborted:
                    task.OnAborted();
                    break;
            }

            task.OnExit();
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
        }
#endif
    }
}
