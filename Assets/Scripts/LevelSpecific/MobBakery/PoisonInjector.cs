using System;
using System.Collections;
using DG.Tweening;
using General;
using Injector;
using InputRelated;
using LevelSpecific.MobBakery.IngredientSystem;
using Managers;
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

        [InjectReference] public InputManager InputManager { get; set; }
        public InputType InputType { get; }

        public IEnumerator Start()
        {
            ChangeInjectorActivationStatus(false);
            yield return new WaitForSeconds(.1f);
            InputManager.Add(this);
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