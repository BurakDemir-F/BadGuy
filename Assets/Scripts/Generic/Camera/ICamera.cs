using Cinemachine;
using UnityEngine;

namespace Generic.Camera
{
    public interface ICamera
    {
        void Setup(int highPriority, int lowPriority);
        void Activate();
        void Deactivate();
        void SetLookTarget(Transform target);
        int HighPriority { get; }
        int LowPriority { get; }
        CinemachineVirtualCamera VCam { get; }
    }
}