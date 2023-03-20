using UnityEngine;
using UnityEngine.AI;
using Utilities;

namespace Npc
{
    public class CrazyOldMan : NpcBehaviour
    {
        [SerializeField] private Transform _dogBiteTransform;
        private bool _isPullingPlayer;
        private Transform _playerTransform;
        private Vector3 _playerDistanceOffset;

        protected override void OnTriggerEnter(Collider other)
        {
            if (!_agent.isActiveAndEnabled)
                return;
            
            if(!IsInHouse())
                base.OnTriggerEnter(other);
            else
                HarmPlayer(other);
        }

        protected override void HarmPlayer(Collider other)
        {
            InteractPlayer(other);
            
            transform.LookAt(other.transform.position.SetY(0f));

            _playerTransform = other.transform;
            _playerDistanceOffset = transform.position - _playerTransform.position;
            
            _agent.SetDestination(_dogBiteTransform.position);
            _isPullingPlayer = true;

        }

        protected override void Update()
        {
            base.Update();
            
            if(!_isPullingPlayer)
                return;

            var myPosition = transform.position;
            _playerTransform.position = myPosition + _playerDistanceOffset;
            transform.LookAt(_playerTransform.position.SetY(myPosition.y));
            var remainingDistance = _agent.remainingDistance;

            if (remainingDistance <= .5f)
            {
                InvokeCatchEvent(_playerTransform.position);
                DisableAgent();
                _isPullingPlayer = false;
            }
        }

        private bool IsInHouse()
        {
            return true;
            
            var isInHouse = NavMesh.SamplePosition(transform.position, out var hit, 3f, NavMesh.GetAreaFromName("House")); 
            if (isInHouse)
            {
                Debug.Log($"hit area : {hit.mask}");
            }

            return isInHouse;
        }
    }
}