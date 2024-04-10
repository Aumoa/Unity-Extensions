using UnityEngine;

namespace Ayla.Behaviors.Runtime.Tasks.Actions
{
    [AddComponentMenu("Behaviors/Actions/Delay")]
    public class Delay : Action
    {
        [SerializeField]
        private float delaySeconds = 1.0f;

        [SerializeField]
        private bool unscaled;

        private float timer;

        protected override void OnStart()
        {
            timer = 0;
        }

        protected override TaskStatus OnUpdate()
        {
            if (unscaled)
            {
                timer += Time.unscaledDeltaTime;
            }
            else
            {
                timer += Time.deltaTime;
            }

            return timer >= delaySeconds ? TaskStatus.Success : TaskStatus.Running;
        }
    }
}