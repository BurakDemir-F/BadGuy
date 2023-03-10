using Cinemachine;
using UnityEngine;

namespace Generic.Camera
{
    public class CameraBase : MonoBehaviour, ICamera
    {
        [SerializeField] private CinemachineVirtualCamera _vcam;
        
        private int _highPriority;
        private int _lowPriority;

        public int HighPriority => _highPriority;
        public int LowPriority => _lowPriority;

        public CinemachineVirtualCamera VCam => _vcam;
        
        public virtual void Setup(int highPriority, int lowPriority)
        {
            _highPriority = highPriority;
            _lowPriority = lowPriority;
            _vcam.Priority = _lowPriority;
            _vcam.gameObject.SetActive(false);
        }

        public virtual void Activate()
        {
            _vcam.gameObject.SetActive(true);
            _vcam.Priority = _highPriority;
        }

        public virtual void Deactivate()
        {
            _vcam.gameObject.SetActive(false);
            _vcam.Priority = _lowPriority;
        }
        
        public void SetLookTarget(Transform target)
        {
            _vcam.m_LookAt = target;
        }
    }
}