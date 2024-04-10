namespace Ayla.Behaviors.Runtime.Tasks
{
    public abstract class Action : Task
    {
        protected override void OnStandby()
        {
        }

        protected override void OnAborted()
        {
        }

        protected override TaskStatus OnPreUpdate()
        {
            return TaskStatus.Running;
        }

        protected override TaskStatus OnLateUpdate()
        {
            return TaskStatus.Running;
        }

        protected override void OnExit()
        {
        }

        protected override void OnFailure()
        {
        }

        protected override void OnStart()
        {
        }

        protected override void OnSuccess()
        {
        }
    }
}
