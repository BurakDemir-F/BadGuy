using System;
using General;
using Injector;
using LevelSpecific.MobBakery.IngredientSystem;
using LevelSpecific.MobBakery.Npc.SO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace LevelSpecific.MobBakery.Npc
{
    public class BakeryNpc : MonoBehaviour
    {
        [SerializeField]protected NavMeshAgent agent;
        [SerializeField] protected BakeryNpcAnimator animator;
        protected BakeryNpcData npcData;

        private bool _isWalking;
        private Action _destinationReachedCallback;
        private Vector3 _currentDestination;
        
        [InjectReference]
        public IItemHolder<BakeryNpc> NpcHolder { get; set; }

        public BakeryNpcData NpcData => npcData;
        
        public void Init(BakeryNpcData data)
        {
            npcData = data;
            NpcHolder.Add(this);
        }

        private void OnDestroy()
        {
            NpcHolder.Remove(this);
        }

        public void Move(Vector3 target,Action destinationReachedCallback)
        {
            agent.ResetPath();
            agent.SetDestination(target);
            Animate(BakeryNpcAnimType.Walk);
            _currentDestination = target;
            _destinationReachedCallback = destinationReachedCallback;
            _isWalking = true;
        }

        public void Update()
        {
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

        public void Sit()
        {
            Animate(BakeryNpcAnimType.Sit);
        }

        public void SetNpcAnimator(BakeryNpcAnimator bakeryNpcAnimator)
        {
            animator = bakeryNpcAnimator;
        }

        public void SetAgent(NavMeshAgent meshAgent)
        {
            agent = meshAgent;
        }
        
    }
}