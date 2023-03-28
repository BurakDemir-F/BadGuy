using System;
using System.Collections;
using System.Collections.Generic;
using Generic;
using Generic.ShowSystem;
using LevelSpecific.MobBakery.Npc;
using UnityEngine;

namespace LevelSpecific.MobBakery.Machines.CustomerArea
{
    public class CustomerArea : MonoBehaviour,IInteractable
    {
        [SerializeField] private BakeryNpcManager _npcManager;
        [SerializeField] private Transform _npcWaitArea;
        [SerializeField] private InputButton _wantedButton;
        [SerializeField] private TimerUI _timerUI;
        [SerializeField] private List<Transform> _sitTransforms;
        [SerializeField] private Transform _outsideTransform;
        
        private IngredientHolder CurrentHolder;
        private BakeryNpc _currentNpc;
        private bool _isStarted;
        private Coroutine _timerCor;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            CallNewCustomer();
        }

        public void CallNewCustomer()
        {
            _isStarted = true;
            _currentNpc = _npcManager.GetNpc();
            _currentNpc.Move(_npcWaitArea.position, OnNpcWaitingArea);
        }

        private void OnNpcWaitingArea()
        {
            _currentNpc.Animate(BakeryNpcAnimType.Idle);
            _timerCor = _timerUI.StartTimer(_currentNpc.NpcData.WaitDuration, KillPlayer);
        }

        private void KillPlayer()
        {
            Debug.Log("player killed.");
            _currentNpc.Animate(BakeryNpcAnimType.Shoot);
        }

        public void Interact(Collider col)
        {
            if (_isStarted)
                return;

            CurrentHolder = col.GetComponent<IngredientHolder>();
            if (!CurrentHolder.HasIngredient)
                return;

            var type = CurrentHolder.Ingredient.Type;
            if(type != _currentNpc.NpcData.WantedProduct.Item.Type)
                return;
            StopCoroutine(_timerCor);
            CurrentHolder.Release();
            CallNewCustomer();
            
        }

        public void InteractEnd(Collider col)
        {
            if (_isStarted)
                return;
        }
    }
}