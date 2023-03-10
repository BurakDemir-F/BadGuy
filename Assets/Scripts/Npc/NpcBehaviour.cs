using System;
using AYellowpaper;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Npc
{
    public class NpcBehaviour : MonoBehaviour
    {
        [SerializeField] protected InterfaceReference<INPCDataProvider> _npcDataProvider;
        [SerializeField] protected NavMeshAgent _agent;
        [SerializeField] protected NpcAnimator _animator;
        [SerializeField] private NpcStatusTrack statusTrack;
        
        protected Transform _currentAttackTarget;
        protected bool _hasActiveTarget;

        private int _colliderTrackCount = 3;
        private Collider[] _touchedCollider;

        private void Start()
        {
            _touchedCollider = new Collider[_colliderTrackCount];
            _agent.speed = _npcDataProvider.Value.MoveSpeed;
            statusTrack = new NpcStatusTrack();
            statusTrack.CurrentStatus = NpcStatus.Patrol;
            statusTrack.NpcStatusChanged += OnNpcStatusChanged;
        }

        private void OnDestroy()
        {
            statusTrack.NpcStatusChanged -= OnNpcStatusChanged;
        }

        private void Update()
        {
            CheckRangeRadius();
            
            if(!_hasActiveTarget)
                return;

            var distance = Vector3.Distance(_currentAttackTarget.position, transform.position);
            var attackRadius = _npcDataProvider.Value.AttackRangeRadius;
            
            if (distance <= attackRadius)
            {
                statusTrack.CurrentStatus = NpcStatus.Attack;
                Attack();
            }
            else
            {
                statusTrack.CurrentStatus = NpcStatus.Chase;
                Chase();
            }
        }

        private void OnNpcStatusChanged(NpcStatus status)
        {
            switch (status)
            {
                case NpcStatus.Patrol:
                    _animator.Animate(NpcAnimType.Idle);
                    break;
                case NpcStatus.Chase:
                    _animator.Animate(NpcAnimType.Run);
                    break;
                case NpcStatus.Attack:
                    _animator.Animate(NpcAnimType.Attack);
                    break;
            }
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
        
        [Serializable]
        private struct NpcStatusTrack
        {
            public event Action<NpcStatus> NpcStatusChanged;
            private NpcStatus _currentStatus;

            public NpcStatus CurrentStatus
            {
                get => _currentStatus;

                set
                {
                    if (_currentStatus != value)
                    {
                        _currentStatus = value;
                        NpcStatusChanged?.Invoke(_currentStatus);
                    }
                }
            }
        }
        
        public enum NpcStatus
        {
            None,
            Patrol,
            Chase,
            Attack
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