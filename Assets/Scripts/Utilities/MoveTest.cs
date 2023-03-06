using System;
using UnityEngine;

namespace Utilities
{
    public class MoveTest : MonoBehaviour
    {
        public Transform first;
        public Transform second;
        public Transform target;
        public float timer;
        public float movementTime;
        private Vector3 currentVelocity = Vector3.one;
        private float distance;
        
        private void Start()
        {
            target = second;
            distance = Vector3.Distance(first.position, second.position);
        }

        private void Update()
        {
            
            if (currentVelocity == Vector3.zero)
            {
                timer = 0f;
                target = target == first ? second : first;
                currentVelocity = Vector3.one;
            }

            transform.position =
                Vector3.SmoothDamp(transform.position, target.position, ref currentVelocity, movementTime);
            
            Debug.Log($"current velocity: {currentVelocity}");

            timer += Time.deltaTime;
        }
    }
}