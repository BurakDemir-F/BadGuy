using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    public class PlayerEffects : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _dieParticles;
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioClip _clip;
        [SerializeField] private ParticleSystem _walkingParticles;
        [SerializeField] private Vector2 _walkSoundBounds;
        [SerializeField] private List<AudioClip> _walkingSounds;
        [SerializeField] private PlayerMovementHandler _movementHandler;
        private bool _isWalking;
        private Coroutine _walkCor;

        private void Start()
        {
            _movementHandler.MovementStarted += PlayWalkingEffect;
            _movementHandler.MovementEnd += StopWalkingEffect;
        }

        private void OnDestroy()
        {
            _movementHandler.MovementStarted -= PlayWalkingEffect;
            _movementHandler.MovementEnd -= StopWalkingEffect;
        }

        public void PlayDieEffect()
        {
            _dieParticles.Play();
            _source.PlayOneShot(_clip);
        }

        public void PlayWalkingEffect()
        {
            if(_walkingParticles)
                _walkingParticles.Play();
            _isWalking = true;
            //_walkCor = StartCoroutine(PlayWalkingSound());
        }

        public void StopWalkingEffect()
        {
            if(_walkingParticles)
                _walkingParticles.Stop();
            _isWalking = false;
            //if(_walkCor != null)
                //StopCoroutine(_walkCor);
        }

        private IEnumerator PlayWalkingSound()
        {
            while (_isWalking)
            {
                var randomTime = Random.Range(_walkSoundBounds.x, _walkSoundBounds.y);
                var randomClipIndex = Random.Range(0, _walkingSounds.Count);
                var randomClip = _walkingSounds[randomClipIndex];
                yield return new WaitForSeconds(randomTime);
                _source.PlayOneShot(randomClip);
            }
        }
    }
}