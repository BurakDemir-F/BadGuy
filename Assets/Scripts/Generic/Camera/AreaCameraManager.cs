using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Utilities;

namespace Generic.Camera
{
    public class AreaCameraManager : MonoBehaviour
    {
        [SerializeField] private TriggerCameraDict _triggerAreas;
        [SerializeField]private TriggerArea _enteredArea;
        [SerializeField] private Transform _lookTarget;

        [SerializeField] private int _lowPriority;
        [SerializeField] private int _highPriority;
        [SerializeField] private string _controlTag;

        private void Start()
        {
            foreach (var areaCamPair in _triggerAreas)
            {
                var area = areaCamPair.Key;
                var cam = areaCamPair.Value;
                
                area.Setup(_controlTag);
                cam.Setup(_highPriority,_lowPriority);
                cam.SetLookTarget(_lookTarget);

                area.TriggerEnter += TriggerEntered;
                area.TriggerExit += TriggerExited;
            }
            
            TriggerEntered(_enteredArea);
        }

        private void OnDestroy()
        {
            foreach (var areaCamPair in _triggerAreas)
            {
                var area = areaCamPair.Key;
                
                area.TriggerEnter -= TriggerEntered;
                area.TriggerExit -= TriggerExited;
            }
        }

        private void TriggerEntered(TriggerArea area)
        {
            if(_enteredArea != null)
                return;
            
            _enteredArea = area;

            foreach (var areaCamPair in _triggerAreas)
            {
                var cam = areaCamPair.Value;
                cam.Deactivate();
            }
            
            _triggerAreas[area].Activate();
            
        }

        private void TriggerExited(TriggerArea area)
        {
            if(_enteredArea == null)
                return;
            
            if(_enteredArea != area)
                return;

            _enteredArea = null;
        }
        
#if UNITY_EDITOR

        [ContextMenu("FillTriggerCamDict")]
        private void FillTriggerCamDict()
        {
            _triggerAreas = new TriggerCameraDict();
            foreach (Transform t in transform)
            {
                var triggerArea = t.GetComponentInChildren<TriggerArea>();
                var camArea = t.GetComponentInChildren<CameraBase>();
                camArea.SetLookTarget(_lookTarget);
                EditorUtility.SetDirty(camArea);
                _triggerAreas.Add(triggerArea,camArea);
            }
            
            EditorUtility.SetDirty(this);
        } 
        
#endif
        
    }

    [System.Serializable]
    public class TriggerCameraDict : SerializableDictionary<TriggerArea,CameraBase>{}
}