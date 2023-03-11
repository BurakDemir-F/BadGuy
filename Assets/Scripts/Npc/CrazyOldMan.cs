using UnityEngine;

namespace Npc
{
    public class CrazyOldMan : NpcBehaviour
    {
        protected override void HarmPlayer(Collider other)
        {
            base.HarmPlayer(other);
            //Play holding animation
            //whistle
        }
    }
}