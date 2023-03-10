using System;
using System.Collections.Generic;
using Generic.Camera;
using UnityEngine;

namespace Sequence.SO
{
    [CreateAssetMenu(fileName = "ShowOff", menuName = "Data/Config/ShowOff", order = 0)]
    public class CD_ShowScreen : ScriptableObject,IShowScreenDataProvider
    {
        [SerializeField] private List<ShowScreenData> _data;
        public List<ShowScreenData> Data => _data;
    }

    [Serializable]
    public class ShowScreenData
    {
        public bool IsActive;
        public string Description;
        public bool NeedCameraMovement;
        public CameraBase Camera;
        public float ShowOffDuration;
    }

    public interface IShowScreenDataProvider
    {
        List<ShowScreenData> Data { get; }
    }
    
}