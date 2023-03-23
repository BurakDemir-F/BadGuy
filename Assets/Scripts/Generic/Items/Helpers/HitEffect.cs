using Injector;
using Npc;
using UnityEngine;

namespace Generic.Items.Helpers
{
    public class HitEffect : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;
        private MovementBasedRotationItem _item;
        [InjectReference]
        public NpcManager Manager { get; set; }

        private void Start()
        {
            _item = GetComponent<MovementBasedRotationItem>();
            _item.OnCollision += OnCollision;
        }

        private void OnDestroy()
        {
            _item.OnCollision -= OnCollision;
        }

        private void OnCollision(Transform t)
        {
            var pos = t.position;
            AudioSource.PlayClipAtPoint(_clip,pos);
            Manager.SetDestination(pos);
        }
    }
}