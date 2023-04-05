using UnityEngine;

namespace Player
{
    public class PlayerEffects : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _dieParticles;
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioClip _clip;

        public void PlayDieEffect()
        {
            _dieParticles.Play();
            _source.PlayOneShot(_clip);
        }
    }
}