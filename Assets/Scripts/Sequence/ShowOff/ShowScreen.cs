using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper;
using Sequence.SO;
using Sequence.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sequence.ShowOff
{
    public class ShowScreen : SequenceNode
    {
        [SerializeField] private InterfaceReference<IShowScreenDataProvider> _dataProvider;
        [SerializeField] private List<ShowScreenData> _data;
        [SerializeField] private ShowUI _showUI;
        [SerializeField] private int _camLowPriority, _camHighPriority;

        private static int _counter;
        private ShowScreenData CurrentShowOffData => _data[_counter];

        public override void InitializeNode()
        {
            base.InitializeNode();
            if (_data == null || _data.Count == 0)
            {
                Debug.Log("something missing here.");
            }

            Setup();
        }

        private void Setup()
        {
            foreach (var showOffData in _data)
            {
                if (showOffData.NeedCameraMovement)
                {
                    showOffData.Camera.Setup(_camHighPriority, _camLowPriority);
                }
            }

            _showUI.Disable();
        }

        public override void StartSequenceNode()
        {
            base.StartSequenceNode();
            ShowOff();
        }

        private void ShowOff()
        {
            var data = CurrentShowOffData;

            if (data.NeedCameraMovement)
                data.Camera.Activate();

            StartCoroutine(ShowOffCor(2f,() => _counter++));
        }

        private IEnumerator ShowOffCor(float cameraMovementWait, Action completeCallback)
        {
            yield return new WaitForSeconds(cameraMovementWait);
            
            var data = CurrentShowOffData;
            _showUI.ShowText(data.Description);
            
            if (data.NpcAnimation != null)
                data.NpcAnimation.Animate();

            yield return new WaitForSeconds(data.ShowOffDuration);

            if (data.NeedCameraMovement)
                data.Camera.Deactivate();

            _showUI.Disable();
            completeCallback?.Invoke();
            isNodeCompleted = true;
            SequenceNodeCompleted?.Invoke();
        }

        private void Reset()
        {
            _data = null;
        }
        
#if UNITY_EDITOR

        [ContextMenu("Fill Data")]
        private void FillData()
        {
            var data = _dataProvider.Value.Data;
            foreach (var screenData in data) screenData.Camera = null;
            _data?.Clear();
            _data = data.Where((d)=> d.IsActive).ToList();
        }        
        
#endif
    }

    [Serializable]
    public class ShowUI
    {
        public TMP_Text Text;
        public Image Frame;

        public void ShowText(string text)
        {
            Enable();
            Text.text = text;
        }
        
        public void Enable()
        {
            Text.gameObject.SetActive(true);
            Frame.gameObject.SetActive(true);
        }

        public void Disable()
        {
            Text.gameObject.SetActive(false);
            Frame.gameObject.SetActive(false);
        }
    }
}