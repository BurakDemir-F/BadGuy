using LevelSpecific.MobBakery.IngredientSystem;
using UnityEngine;

namespace LevelSpecific.MobBakery.Machines
{
    public class Trash: TimeBasedMachine
    {
        private Coroutine _timerCor;
        public override void Interact(Collider col)
        {
            OpenMachine(col);
            if (!CurrentHolder.HasIngredient)
            {
                return;
            }
            
            var duration = Foods.MachineProcessDuration;
            _isProducing = true;
            _timerCor = StartTimer(duration,OnTrashThrow);
        }

        public override void InteractEnd(Collider col)
        {
            CloseMachine();
            if (_isProducing)
            {
                StopCoroutine(_timerCor);
                _timeUI.ResetUI();
                _isProducing = false;
            }
        }

        protected override void OpenMachine(Collider col)
        {
            OpenDoor();
            CurrentHolder = col.GetComponent<IngredientHolder>();
        }

        protected override void CloseMachine()
        {
            CloseDoor();
            CurrentHolder = null;
        }

        private void OnTrashThrow()
        {
            CurrentHolder.Release();
        }

        protected override void OnIngredientSelected(IngredientSO ingredientSo)
        {
        }
    }
}