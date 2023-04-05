using DG.Tweening;
using TMPro;
using UnityEngine;

namespace LevelSpecific.MobBakery.Machines.CustomerArea
{
    public class MobBakeryGameResultUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _wrongServiceText;
        [SerializeField] private TextMeshProUGUI _timeOverText;
        [SerializeField] private TextMeshProUGUI _adventureWillContinue;
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

        public void AdventureWillContinue()
        {
            _adventureWillContinue.gameObject.SetActive(true);
            DOVirtual.DelayedCall(5f, () => _adventureWillContinue.gameObject.SetActive(false));
        }
    }
}