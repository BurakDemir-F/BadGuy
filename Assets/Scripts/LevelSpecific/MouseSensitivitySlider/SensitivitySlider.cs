using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

namespace LevelSpecific.MouseSensitivitySlider
{
    public class SensitivitySlider : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private CinemachineFreeLook _freeLookCam;
        [Range(0,240f)]
        [SerializeField] private float _mouseRotationXMax = 240f;
        [Range(0,3f)]
        [SerializeField] private float _mouseRotationYMax = 3f;
        
        private void Start()
        {
            _slider.onValueChanged.AddListener(ValueChanged);
            _freeLookCam.m_YAxis.m_SpeedMode = AxisState.SpeedMode.MaxSpeed;
            _freeLookCam.m_XAxis.m_SpeedMode = AxisState.SpeedMode.MaxSpeed;
            _slider.value = .5f;
        }

        private void OnDestroy()
        {
            _slider.onValueChanged.RemoveListener(ValueChanged);
        }

        private void ValueChanged(float value)
        {
            var xRotation = _mouseRotationXMax * value;
            var yRotation = _mouseRotationYMax * value;

            _freeLookCam.m_YAxis.m_MaxSpeed = yRotation;
            _freeLookCam.m_XAxis.m_MaxSpeed = xRotation;
        }
    }
}