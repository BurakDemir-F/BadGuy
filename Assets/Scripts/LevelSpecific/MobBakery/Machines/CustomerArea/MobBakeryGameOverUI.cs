using DG.Tweening;
using TMPro;
using UnityEngine;

namespace LevelSpecific.MobBakery.Machines.CustomerArea
{
    public class MobBakeryGameOverUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _wrongServiceText;
        [SerializeField] private TextMeshProUGUI _timeOverText;
        public void WrongService()
        {
            _wrongServiceText.gameObject.SetActive(true);
            DOVirtual.DelayedCall(5f, () => _wrongServiceText.gameObject.SetActive(false));
        }

        public void TimeOver()
        {
            _timeOverText.gameObject.SetActive(true);
            DOVirtual.DelayedCall(5f, () => _timeOverText.gameObject.SetActive(false));
        }
    }
}