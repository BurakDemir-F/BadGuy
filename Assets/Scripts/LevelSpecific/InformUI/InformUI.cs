using System.Collections.Generic;
using System.Linq;
using System.Text;
using InputRelated;
using TMPro;
using UnityEngine;
using Utilities;

namespace LevelSpecific.InformUI
{
    public class InformUI : InputReceiver<InGameInputType>
    {
        [SerializeField] private GameObject _screen;
        [SerializeField] private TextMeshProUGUI _objectiveText;
        [SerializeField] private GameObject _frame;
        [SerializeField] private TextMeshProUGUI _informText;
        [SerializeField] private CD_InformUI _informUI;
        [SerializeField] private List<AudioSource> _audioSources;
        
        public void DisableInformUI()
        {
            DisableInput();
            CloseVisual();
        }
        [ContextMenu("Enable Inform UI")]
        public void EnableInformUI()
        {
            OpenVisual();
            SetText();
            EnableInput();
        }

        private void SetText()
        {
            var sb = new StringBuilder();
            var itemColor = _informUI.ItemColor;
            var textColor = _informUI.TextColor;
            foreach (var informUIElement in _informUI.Elements)
            {
                sb.Append(
                    $"{informUIElement.Item.GetColored(itemColor)}: {informUIElement.Text.GetColored(textColor)} ");
            }

            _informText.text = sb.ToString();
        }

        private void OpenVisual()
        {
            _informText.gameObject.SetActive(true);
            _frame.SetActive(true);   
        }

        private void CloseVisual()
        {
            _informText.gameObject.SetActive(false);
            _frame.SetActive(false);
        }
        
        private void OnValidate()
        {
            _audioSources = FindObjectsOfType<AudioSource>().ToList();
        }

        private bool _isScreenOpen;
        private bool _isSoundClosed;

        protected override void OnGameActionPerformed(InGameInputType gameActionType)
        {
            if (gameActionType == InGameInputType.OpenCloseObjectives)
            {
                _isScreenOpen = !_isScreenOpen;
                _screen.SetActive(_isScreenOpen);
                _objectiveText.gameObject.SetActive(_isScreenOpen);
                return;
            }

            if (gameActionType == InGameInputType.OpenCloseSound)
            {
                _isSoundClosed = !_isSoundClosed;
                _audioSources.ForEach((source)=> source.Stop());
            }
        }
    }

    public enum InGameInputType
    {
        None,
        OpenCloseObjectives,
        OpenCloseSound,
    }
}