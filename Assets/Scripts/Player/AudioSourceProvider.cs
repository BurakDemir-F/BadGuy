using System;
using Generic.Providers;
using UnityEngine;

namespace Player
{
    public class AudioSourceProvider : MonoBehaviour, IObjectProvider<AudioSource>
    {
        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        public AudioSource Get()
        {
            return _source;
        }
    }
}