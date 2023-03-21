using System;
using System.Collections;
using General;
using Generic.Items.SO;
using Generic.Providers;
using Injector;
using UnityEngine;

namespace Generic.Items
{
    public class BreakableItem : BaseItem, IInteractable
    {
        [SerializeField] private CD_BreakableItem _itemData;
        [InjectReference]
        public IItemHolder<BreakableItem> _itemHolder { get; set; }
        
        [InjectReference]
        public IObjectProvider<AudioSource> _audioSourceProvider { get; set; }

        private AudioSource AudioSource => _audioSourceProvider.Get();

        public event Action<Transform> OnItemBreak;
        private YieldInstruction _interactDuration;
        private bool _isInteracting;

        protected override void Start()
        {
            base.Start();
            RigidbodySetDynamic(false);
            _itemHolder.Add(this);
            AudioSource.clip = _itemData.Clip;
            _interactDuration = new WaitForSeconds(_itemData.Clip.length);
        }

        private void OnDestroy()
        {
            _itemHolder.Remove(this);
        }

        public void Interact(Collider col)
        {
            if (_isInteracting)
                return;
            _isInteracting = true;
            RigidbodySetDynamic(true);
            DisableIndicator();
            var colliderPos = col.transform.position;
            var forceDirection = transform.position - colliderPos;
            _rigidbody.AddForce(forceDirection.normalized * _itemData.Force);
            
            OnItemBreak?.Invoke(col.transform);
            StartCoroutine(InteractCor());
        }

        private IEnumerator InteractCor()
        {
            AudioSource.Play();
            yield return _interactDuration;
            _isInteracting = false;
        }
    }
}