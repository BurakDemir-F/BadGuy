using TMPro;
using UnityEngine;

namespace LevelSpecific.MobBakery
{
    public class MachineInteractionUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _pressFToSelect;
        [SerializeField] private TextMeshProUGUI _useArrowCaseForSwitch;

        public void ShowPressFToSelect()
        {
            _pressFToSelect.gameObject.SetActive(true);
        }

        public void HidePressFToSelect()
        {
            _pressFToSelect.gameObject.SetActive(false);
        }

        public void ShowUseArrowCaseForSwitch()
        {
            _useArrowCaseForSwitch.gameObject.SetActive(true);
        }
        
        public void HideUseArrowCaseForSwitch()
        {
            _useArrowCaseForSwitch.gameObject.SetActive(false);
        }
    }
}