using System;
using UnityEngine;

namespace Runtime.Main.Extensions
{
    public class OnEnableFinder : MonoBehaviour
    {
        private void OnEnable()
        {
            Debug.Log($"{gameObject.name} enabled!",gameObject);
        }   
        private void OnDisable()
        {
            Debug.Log($"{gameObject.name} disabled!",gameObject);
        }
    }
}