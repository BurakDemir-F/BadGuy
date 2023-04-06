using System.Collections;
using DG.Tweening;
using Generic;
using Player;
using UnityEngine;

namespace Npc
{
    public class DogNpc : NpcBehaviour
    {
        [SerializeField] private Transform _mouthTransform;
        [SerializeField] private Transform _animationRoot;

        protected override void HarmPlayer(Collider other)
        {
            base.HarmPlayer(other);
            other.transform.DOMove(_mouthTransform.position, .1f);
            other.transform.DORotate(_mouthTransform.rotation.eulerAngles, .1f).OnComplete(() =>
            {
                other.GetComponent<PlayerEffects>().PlayDieEffect();
                other.transform.SetParent(_animationRoot);
                _animator.Animate(NpcAnimType.Win);
            });
            StartCoroutine(GameLooseCor());
        }

        private IEnumerator GameLooseCor()
        {
            yield return new WaitForSeconds(3f);
            GameManager.Instance.GameLoose();
        }
    }
}