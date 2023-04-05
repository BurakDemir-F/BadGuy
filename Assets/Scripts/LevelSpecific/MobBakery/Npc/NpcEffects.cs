using System;
using UnityEngine;

namespace LevelSpecific.MobBakery.Npc
{
    public class NpcEffects : MonoBehaviour
    {
        [SerializeField] protected ParticleSystem _muzzleFlash;
        [SerializeField] protected AudioClip _shootSound;
        [SerializeField] protected AudioSource _source;

        [ContextMenu("Close Play On Awake")]
        private void ClosePlayOnAwake()
        {
            var children = GetComponentsInChildren<ParticleSystem>();
            foreach (var child in children)
            {
                var mainModule = child.main;
                mainModule.playOnAwake = false;
            }
        }
        
        [ContextMenu("Place Muzzle Flash")]
        private void PlaceMuzzleFlash()
        {
            var children = GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                if (!child.name.Contains("pistol", StringComparison.OrdinalIgnoreCase))
                    continue;

                var flash = Instantiate(_muzzleFlash, child);
                flash.transform.localPosition = new Vector3(-0.271f, 0.094f, 0.001f);
                flash.transform.localRotation = Quaternion.Euler(0, 180, 0);
                _muzzleFlash = flash;
                break;
            }
        }

        [ContextMenu("Delete wrong flashes")]
        private void DeleteWrongMuzzleFlashes()
        {
            var children = GetComponentsInChildren<ParticleSystem>();
            for (var i = 0; i < children.Length; i++)
            {
                var child = children[i];
                
                if(child)
                    DestroyImmediate(child.gameObject);
                
                // var parent = child.transform.parent;
                // if (parent.gameObject.TryGetComponent<ParticleSystem>(out var system))
                // {
                //     DestroyImmediate(system.gameObject);
                // }
                // else
                // {
                //     if(child)
                //         DestroyImmediate(child.gameObject);
                // }
                
            }
        }

        public void PlayEffect()
        {
            _source.PlayOneShot(_shootSound);
            _muzzleFlash.Emit(11);
        }
    }
}