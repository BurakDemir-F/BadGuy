using System;
using System.Collections;
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
        
        [InjectReference]
        public IItemHolder<BakeryNpc> NpcHolder { get; set; }

        public BakeryNpcData NpcData => npcData;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(.1f);
            NpcHolder.Add(this);
        }

        public void Init(BakeryNpcData data)
        {
            npcData = data;
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

        public void Sit(Vector3 sitPos)
        {
            Move(sitPos,()=> Animate(BakeryNpcAnimType.Sit));
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