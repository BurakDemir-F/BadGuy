using AYellowpaper;
using UnityEngine;
using UnityEngine.AI;

namespace Npc
{
    public class NpcBehaviour : MonoBehaviour
    {
        [SerializeField] protected InterfaceReference<INPCDataProvider> _npcDataProvider;
        [SerializeField] protected NavMeshAgent _agent;
        
        protected Transform _currentAttackTarget;
        protected bool _hasActiveTarget;

        private int _colliderTrackCount = 3;
        private Collider[] _touchedCollider;

        private void Start()
        {
            _touchedCollider = new Collider[_colliderTrackCount];
            _agent.speed = _npcDataProvider.Value.MoveSpeed;
        }

        private void Update()
        {
            CheckRangeRadius();
            
            if(!_hasActiveTarget)
                return;

            var distance = Vector3.Distance(_currentAttackTarget.position, transform.position);
            var attackRadius = _npcDataProvider.Value.AttackRangeRadius;
            
            if (distance <= attackRadius)
                Attack();
            else
                Chase();
        }

        protected void CheckRangeRadius()
        {
            var size = Physics.OverlapSphereNonAlloc(transform.position,
                _npcDataProvider.Value.ChaseRangeRadius,
                _touchedCollider,
                _npcDataProvider.Value.AttackLayer);

            if (size > 0)
            {
                _hasActiveTarget = true;
                _currentAttackTarget = _touchedCollider[0].transform;
            }
            else
            {
                _hasActiveTarget = false;
                _currentAttackTarget = null;
            }
        }

        protected void Chase()
        {
            _agent.SetDestination(_currentAttackTarget.position);
        }

        protected virtual void Attack()
        {
            
        }
    }

    public interface INPCDataProvider
    {
        AttackType AttackType { get;}
        LayerMask AttackLayer { get;}
        float ChaseRangeRadius { get; }
        float AttackRangeRadius { get; }
        float MoveSpeed { get; }
    }

    public enum AttackType
    {
        None,
        Ranged,
        CloseAttack
    }
}