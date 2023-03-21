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
            _dog.PlayerCatched += OnPlayerCatched;
        }
        
        private void OnDestroy()
        {
            _creazyOldMan.PlayerCatched -= OnPlayerCatched;
            _dog.PlayerCatched -= OnPlayerCatched;
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