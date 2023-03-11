using DG.Tweening;
using Generic;
using UnityEngine;

namespace Npc
{
    public class DogNpc : NpcBehaviour
    {
        [SerializeField] private Transform _mouthTransform;
        protected override void HarmPlayer(Collider other)
        {
            base.HarmPlayer(other);
            other.transform.DOMove(_mouthTransform.position, .1f);
            other.transform.DORotate(_mouthTransform.rotation.eulerAngles, .1f);
        }
    }
}