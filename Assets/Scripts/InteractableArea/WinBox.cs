using System;
using Generic.Interaction;
using Player;
using Object = UnityEngine.Object;

namespace InteractableArea
{
    public class WinBox : InteractionListener
    {
        public override string Tag => tag;
        public override Type RequestedComponentType => typeof(IMissionGoalInformer);
        public override ComponentProvider ComponentProvider => ComponentProvider.Trigger;
        public event Action OnGoalAccomplished;

        public override void OnTriggerEntered(Object obj)
        {
            var goal = obj as IMissionGoalInformer;
            if (goal.GoalAccomplished)
                OnGoalAccomplished?.Invoke();
        }

        public override void OnTriggerExited(Object obj)
        {
        }
    }
}