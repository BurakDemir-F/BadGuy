using System;
using System.Collections;
using General;
using Injector;
using LevelSpecific.MobBakery.Npc.SO;
using UnityEngine;
using UnityEngine.AI;

namespace LevelSpecific.MobBakery.Npc
{
    public class BakeryNpc : MonoBehaviour
    {
        [SerializeField] protected NavMeshAgent agent;
        [SerializeField] protected BakeryNpcAnimator animator;
        protected NpcEffects _npcEffects;
        protected BakeryNpcData npcData;

        private bool _isWalking;
        private bool _isAiming;
        private Transform _aimTarget;
        private bool _isTargetNpc;
        public bool IsTargetNpc => _isTargetNpc;
        private Action _destinationReachedCallback;
        
        [InjectReference]
        public IItemHolder<BakeryNpc> NpcHolder { get; set; }

        public BakeryNpcData NpcData => npcData;

        private IEnumerator Start()
        {
            _npcEffects = GetComponent<NpcEffects>();
            animator.AddEventAction(BakeryNpcAnimType.Shoot,1.1f,_npcEffects.PlayEffect);
            yield return new WaitForSeconds(.1f);
            NpcHolder.Add(this);
        }

        public void Init(BakeryNpcData data)
        {
            npcData = data;
        }

        public void SetTarget()
        {
            _isTargetNpc = true;
        }

        private void OnDestroy()
        {
            NpcHolder?.Remove(this);
        }

        public void Move(Vector3 target,Action destinationReachedCallback)
        {
            agent.ResetPath();
            var setDestinationResult = agent.SetDestination(target);
            Debug.Log($"destination result: {setDestinationResult}");
            Animate(BakeryNpcAnimType.Walk);
            _destinationReachedCallback = destinationReachedCallback;
            _isWalking = true;
        }

        public void Aim(Transform target)
        {
            _isAiming = true;
            _aimTarget = target;
        }

        public void Update()
        {
            if (_isAiming)
            {
                transform.LookAt(_aimTarget);
                transform.Rotate(0f,-90f,0f);
            }
            
            if(!_isWalking)
                return;

            var remainingDistance = agent.remainingDistance;
            if (remainingDistance <= .6f)
            {
                _destinationReachedCallback?.Invoke();
                _isWalking = false;
            }

        }

        public float Animate(BakeryNpcAnimType type)
        {
            return animator.Animate(type);
        }

        public void Sit(Vector3 sitPos)
        {
            Move(sitPos,()=> Animate(BakeryNpcAnimType.Sit));
        }

        public void SetNpcAnimator(BakeryNpcAnimator bakeryNpcAnimator)
        {
            animator = bakeryNpcAnimator;
        }

        public void Die(Transform target)
        {
            _npcEffects.PlayPoisonedEffect();
            Move(target.position,()=> Animate(BakeryNpcAnimType.Death));
        }
        
        public void SetAgent(NavMeshAgent meshAgent)
        {
            agent = meshAgent;
        }
        
    }
}