using UnityEngine;

namespace Ayla.Behaviors.Runtime.Tasks.Actions
{
    [AddComponentMenu("Beahviors/Actions/Continue")]
    public class Continue : Action
    {
        protected override TaskStatus OnUpdate()
        {
            return TaskStatus.Running;
        }
    }
}