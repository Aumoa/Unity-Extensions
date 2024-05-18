using System;
using Ayla.Inspector;
using UnityEngine;

namespace Ayla.Behaviors.Runtime.Tasks.Decorators
{
    [AddComponentMenu("Behaviors/Decorators/Repeater")]
    public class Repeater : Decorator
    {
        [SerializeField]
        private bool infinity;

        [SerializeField, ShowIf(nameof(infinity), false)]
        private int repeatCount = 1;

        [SerializeField]
        private bool stopWhenAbort;

        private Task child;
        private int counter;
        private bool frameSkip;

        protected override void OnStart()
        {
            child = GetChild();
            counter = 0;
        }

        protected override TaskStatus OnPreUpdate()
        {
            frameSkip = false;
            return ProcessUpdateLoop(ExecutePreUpdate);
        }

        protected override TaskStatus OnUpdate()
        {
            return ProcessUpdateLoop(ExecuteUpdate);
        }

        protected override TaskStatus OnLateUpdate()
        {
            return ProcessUpdateLoop(ExecuteLateUpdate);
        }

        private TaskStatus ProcessUpdateLoop(Func<Task, TaskStatus> handler)
        {
            if (child == null)
            {
                return TaskStatus.Aborted;
            }

            if (frameSkip)
            {
                return TaskStatus.Running;
            }

            var status = handler(child);
            if (status == TaskStatus.Running)
            {
                return status;
            }

            if (status == TaskStatus.Success || stopWhenAbort == false)
            {
                if (infinity || ++counter < repeatCount)
                {
                    Task.ResetForChildren(child);
                    frameSkip = true;
                    return TaskStatus.Running;
                }
            }

            return status;
        }
    }
}