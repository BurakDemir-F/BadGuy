using System;
using System.Collections;
using Generic;
using LevelSpecific.MobBakery.IngredientSystem;
using LevelSpecific.MobBakery.Npc;
using UnityEngine;

namespace LevelSpecific.MobBakery.Machines.CustomerArea
{
    public class CustomerArea : MonoBehaviour, IInteractable
    {
        [SerializeField] private BakeryNpcManager _npcManager;
        [SerializeField] private Transform _npcWaitArea;
        [SerializeField] private IngredientHolder wantedFoodHolder;
        [SerializeField] private TimerUI _timerUI;
        [SerializeField] private SitController _sitController;
        [SerializeField] private Transform _outsideTransform;
        [SerializeField] private Transform _shootTransform;
        [SerializeField] private InformUI _informUI;

        private IngredientHolder CurrentHolder;
        private BakeryNpc _currentNpc;
        private bool _isStarted;
        public event Action MissionSuccess;
        public event Action MissionFail;

        public void CallNewCustomer()
        {
            _currentNpc = _npcManager.GetNpc();
            Debug.Log($"current npc name: {_currentNpc.name}");
            _currentNpc.Move(_npcWaitArea.position, OnNpcWaitingArea);
        }

        private void OnNpcWaitingArea()
        {
            _isStarted = true;
            var data = _currentNpc.NpcData;
            _currentNpc.Animate(BakeryNpcAnimType.Idle);
            _timerUI.StartTimer(data.WaitDuration, () =>
            {
                _informUI.TimeOver();
                KillPlayer();
                wantedFoodHolder.Release();
            });
            wantedFoodHolder.Hold(data.WantedProduct);
        }

        private void KillPlayer()
        {
            Debug.Log("player killed.");
            _currentNpc.Move(_shootTransform.position, AnimateShoot);

            void AnimateShoot()
            {
                _currentNpc.Animate(BakeryNpcAnimType.Shoot);
            }

            CallLevelFailed();
        }

        public void Interact(Collider col)
        {
            Debug.Log("customer area interact started.");
            if (!_isStarted)
                return;

            CurrentHolder = col.GetComponent<IngredientHolder>();
            if (!CurrentHolder.HasIngredient)
                return;

            if (_currentNpc.IsTargetNpc)
            {
                if (CurrentHolder.Ingredient.Type == IngredientType.FriedLoafWithPoison)
                {
                    MissionSuccess?.Invoke();
                    return;
                }
                MissionFail?.Invoke();
                return;
            }

            var type = CurrentHolder.Ingredient.Type;
            if (type != _currentNpc.NpcData.WantedProduct.Item.Type)
            {
                WrongService();
                return;
            }
            
            GoNextCustomer();
            
            void WrongService()
            {
                _informUI.WrongService();
                KillPlayer();
                wantedFoodHolder.Release();
            }

            void GoNextCustomer()
            {
                _isStarted = false;
                _timerUI.ResetUI();
                _timerUI.DeactivateUI();
                _timerUI.StopTimer();
                CurrentHolder.Release();
                wantedFoodHolder.Release();
                NpcSit();
                _currentNpc = null;
                CallNewCustomer();
            }
        }

        private void NpcSit()
        {
            if (_sitController.TryGetSeat(out var sit))
            {
                _currentNpc.Sit(sit.SitTransform.position);
                sit.IsOccupied = true;
            }
            else
            {
                _currentNpc.Sit(_outsideTransform.position);
            }
        }

        public void InteractEnd(Collider col)
        {
            Debug.Log("customer area interact end.");
            if (!_isStarted)
                return;
        }

        private void CallLevelFailed()
        {
            StartCoroutine(LevelFailCor());
        }

        private IEnumerator LevelFailCor()
        {
            yield return new WaitForSeconds(5f);
            GameManager.Instance.GameLoose();
        }
    }
}