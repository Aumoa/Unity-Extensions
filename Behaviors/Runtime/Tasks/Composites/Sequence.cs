using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ayla.Behaviors.Runtime.Tasks.Composites
{
    [AddComponentMenu("Behaviors/Composites/Sequence")]
    public class Sequence : Composite
    {
        private IEnumerator<Task> child;
        private bool frameSkip;

        private Task current => child?.Current;

        protected override void OnStart()
        {
            child = GetChildren().GetEnumerator();
            if (child.MoveNext() == false)
            {
                child.Dispose();
                child = null;
            }
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
            if (current == null)
            {
                return TaskStatus.Aborted;
            }

            if (frameSkip)
            {
                return TaskStatus.Running;
            }

            var status = handler(current);
            if (status == TaskStatus.Running)
            {
                return status;
            }

            if (status == TaskStatus.Success)
            {
                if (child.MoveNext())
                {
                    frameSkip = true;
                    return TaskStatus.Running;
                }

                return TaskStatus.Success;
            }

            return status;
        }
    }
}
