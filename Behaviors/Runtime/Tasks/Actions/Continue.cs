using UnityEngine;

namespace Ayla.Behaviors.Runtime.Tasks.Actions
{
    [AddComponentMenu("Behaviors/Actions/Continue")]
    public class Continue : Action
    {
        protected override TaskStatus OnUpdate()
        {
            return TaskStatus.Running;
        }
    }
}