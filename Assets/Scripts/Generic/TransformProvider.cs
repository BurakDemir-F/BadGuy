using Generic.Providers;
using UnityEngine;

namespace Generic
{
    public class TransformProvider : MonoBehaviour, IObjectProvider<Transform>
    {
        public Transform Get()
        {
            return transform;
        }
    }
}