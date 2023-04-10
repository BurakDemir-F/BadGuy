using System;
using System.Collections;
using DG.Tweening;
using General;
using Injector;
using InputRelated;
using LevelSpecific.MobBakery.IngredientSystem;
using LevelSpecific.MobBakery.Machines.CustomerArea;
using LevelSpecific.MobBakery.Npc;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace LevelSpecific.MobBakery
{
    public class PoisonInjector : InputReceiver<PoisonInput>, IInputControlProvider
    {
        [SerializeField] private IngredientSO _loafWithPoison;
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private Transform _injector;
        [SerializeField] private ParticleSystem _poison;
        [SerializeField] private float _moveDuration;
        [SerializeField] private float _poisonDuration;
        [SerializeField] private IngredientHolder _holder;
        [SerializeField] private TextMeshProUGUI _correctFoodText;
        [SerializeField] private TextMeshProUGUI _correctNpcText;
        [SerializeField] private CustomerArea _customerArea;

        [InjectReference] public InputManager InputManager { get; set; }
        public InputType InputType => InputType.PoisonInput;

        public IEnumerator Start()
        {
            ChangeInjectorActivationStatus(false);
            yield return new WaitForSeconds(.1f);
            InputManager.Add(this);
            //DisableInput();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            InputManager.Remove(this);
        }

        protected override void OnGameActionPerformed(PoisonInput gameActionType)
        {
            if (gameActionType == PoisonInput.AddPoison)
            {

                if (_customerArea.CurrentNpc == null || !_customerArea.CurrentNpc.IsTargetNpc)
                {
                    _correctNpcText.gameObject.SetActive(true);
                    DOVirtual.DelayedCall(3f, () => _correctNpcText.gameObject.SetActive(false));
                    return;
                }
                
                var holderType = _holder.Ingredient.Type;
                if(holderType != IngredientType.FriedLoaf && holderType != IngredientType.Loaf)
                {
                    _correctFoodText.gameObject.SetActive(true);
                    DOVirtual.DelayedCall(3f, () => _correctFoodText.gameObject.SetActive(false));
                    return;
                }

                ChangeInjectorActivationStatus(true);
                DisableOtherInputs();

                _injector.DOMove(_targetTransform.position, _moveDuration).OnComplete(() =>
                {
                    _poison.Play();
                    DOVirtual.DelayedCall(_poisonDuration, () =>
                    {
                        _poison.Stop();
                        _holder.Release();
                        _holder.Hold(_loafWithPoison);
                        EnableOtherInputs();
                        ChangeInjectorActivationStatus(false);
                    });
                });
            }
        }

        private void DisableOtherInputs()
        {
            InputManager.DeactivateInput(InputType.Movement);
            InputManager.DeactivateInput(InputType.UI);
            InputManager.DeactivateInput(InputType.InGameUI);
        }

        private void EnableOtherInputs()
        {
            InputManager.ActivateInput(InputType.Movement);
            InputManager.ActivateInput(InputType.UI);
            InputManager.ActivateInput(InputType.InGameUI);
        }

        private void ChangeInjectorActivationStatus(bool status)
        {
            _injector.gameObject.SetActive(status);
        }
    }

    public enum PoisonInput
    {
        None,
        AddPoison
    }
}