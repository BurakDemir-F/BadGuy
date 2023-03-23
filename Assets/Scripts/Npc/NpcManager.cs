using System.Collections.Generic;
using General;
using Generic.Items;
using UnityEngine;

namespace Npc
{
    public class NpcManager : MonoBehaviour,IItemHolder<BreakableItem>
    {
        [SerializeField] private CrazyOldMan _creazyOldMan;
        [SerializeField] private DogNpc _dog;
        private HashSet<BreakableItem> _items;

        private void Awake()
        {
            _items = new HashSet<BreakableItem>();
            _creazyOldMan.PlayerCatched += OnPlayerCatched;
            _creazyOldMan.PlayerHold += PlayerHold;
            _dog.PlayerCatched += OnPlayerCatched;
        }
        
        private void OnDestroy()
        {
            _creazyOldMan.PlayerCatched -= OnPlayerCatched;
            _dog.PlayerCatched -= OnPlayerCatched;
            _creazyOldMan.PlayerHold -= PlayerHold;
        }

        private void OnPlayerCatched(NpcBehaviour behaviour, Vector3 targetPos)
        {
            if (behaviour is CrazyOldMan)
            {
                _dog.SetDestination(targetPos);
            }
            else
            {
                _dog.DisableAgent();
                _creazyOldMan.DisableAgent();
            }
        }

        public void SetDestination(Vector3 pos)
        {
            _dog.SetDestination(pos);
            _creazyOldMan.SetDestination(pos);
        }

        private void PlayerHold()
        {
            DisableBreakables();
        }

        public void DisableBreakables()
        {
            foreach (var item in _items)
            {
                item.IsDisabled = true;
            }
        }

        public void Add(BreakableItem item)
        {
            if(_items.Contains(item))
                return;
            _items.Add(item);
            item.OnItemBreak += ItemBroke;
        }

        public void Remove(BreakableItem item)
        {
            if (!_items.Contains(item))
                return;
            _items.Remove(item);
            item.OnItemBreak -= ItemBroke;
        }

        private void ItemBroke(Transform breaker)
        {
            _creazyOldMan.LockToTarget(breaker);
            _dog.LockToTarget(breaker);
        }
    }
}