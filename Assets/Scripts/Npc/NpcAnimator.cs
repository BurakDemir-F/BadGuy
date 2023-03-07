using Generic.Animation;

namespace Npc
{
    public class NpcAnimator : AnimatorUser<NpcAnimType>
    {
        
    }

    public enum NpcAnimType
    {
        None,
        Walk,
        Chase,
        Attack
    }
}