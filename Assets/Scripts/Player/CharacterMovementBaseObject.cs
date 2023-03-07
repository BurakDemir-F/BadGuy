using Generic.Providers;
using UnityEngine;

namespace Player
{
    public class CharacterMovementBaseObject : MonoBehaviour,IObjectProvider<Transform>
    {
        [SerializeField] private Transform _character;
        private Vector3 _offset;

        private void Start()
        {
            _offset = _character.position - transform.position;
        }

        private void Update()
        {
            transform.position = _character.position + _offset;
        }

        public Transform Get()
        {
            return transform;
        }
    }
}
