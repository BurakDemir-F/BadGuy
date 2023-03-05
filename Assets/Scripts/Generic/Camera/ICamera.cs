using Cinemachine;

namespace Generic.Camera
{
    public interface ICamera
    {
        void Setup(int highPriority, int lowPriority);
        void Activate();
        void Deactivate();
        int HighPriority { get; }
        int LowPriority { get; }
        CinemachineVirtualCamera VCam { get; }
    }
}