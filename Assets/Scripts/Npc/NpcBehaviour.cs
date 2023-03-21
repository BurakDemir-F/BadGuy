using System;
using AYellowpaper;
using UnityEngine;
using UnityEngine.AI;

namespace Npc
{
    public class NpcBehaviour : MonoBehaviour
    {
        [SerializeField] protected InterfaceReference<INPCDataProvider> _npcDataProvider;
        [SerializeField] protected NavMeshAgent _agent;
        [SerializeField] protected NpcAnimator _animator;
        [SerializeField] private NpcStatusTrack statusTrack;

        private bool _isNavMeshActive;
        
        protected Transform _currentAttackTarget;
        protected bool _hasActiveTarget;

        private int _colliderTrackCount = 3;
        private Collider[] _touchedCollider;
        private bool _continuesFollow;

        public event Action<NpcBehaviour,Vector3> PlayerCatched;

        protected virtual void Start()
        {
            _touchedCollider = new Collider[_colliderTrackCount];
            _agent.speed = _npcDataProvider.Value.MoveSpeed;
            statusTrack = new NpcStatusTrack();
            statusTrack.CurrentStatus = NpcStatus.Idle;
            statusTrack.NpcStatusChanged += OnNpcStatusChanged;
            _isNavMeshActive = true;
        }

        private void OnDestroy()
        {
            statusTrack.NpcStatusChanged -= OnNpcStatusChanged;
        }

        protected virtual void Update()
        {
            if(!_isNavMeshActive)
                return;
            
            if(!_continuesFollow)
                CheckRangeRadius();
            
            if(!_hasActiveTarget)
            {
                statusTrack.CurrentStatus = NpcStatus.Idle;
                return;
            }

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

        public void SetDestination(Vector3 pos)
        {
            _agent.ResetPath();
            _agent.SetDestination(pos);
        }

        public void LockToTarget(Transform target)
        {
            _agent.ResetPath();
            _currentAttackTarget = target;
            _hasActiveTarget = true;
            _continuesFollow = true;
        }
        
        protected void InvokeCatchEvent(Vector3 pos)
        {
            PlayerCatched?.Invoke(this,pos);
        }

        private void OnNpcStatusChanged(NpcStatus status)
        {
            switch (status)
            {
                case NpcStatus.Idle:
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

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!_agent.isActiveAndEnabled)
                return;
            
            if(other.CompareTag("Player"))
            {
                DisableAgent();
                HarmPlayer(other);
            }
        }

        public virtual void DisableAgent()
        {
            _isNavMeshActive = false;
            _agent.isStopped = true;
            _agent.ResetPath();
            _agent.enabled = false;
        }

        protected virtual void HarmPlayer(Collider other)
        {
            InteractPlayer(other);
            InvokeCatchEvent(other.transform.position);
        }

        protected void InteractPlayer(Collider other)
        {
            other.GetComponent<Generic.IInteractable>()?.Interact(GetComponent<Collider>());
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
            Idle,
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