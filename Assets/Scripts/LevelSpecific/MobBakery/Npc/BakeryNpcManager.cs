using General;
using LevelSpecific.MobBakery.Npc.SO;
using UnityEngine;
using Utilities.DataStructures;

namespace LevelSpecific.MobBakery.Npc
{
    public class BakeryNpcManager : MonoBehaviour, IItemHolder<BakeryNpc>
    {
        [SerializeField] private CD_BakeryNpc _npcData;
        [SerializeField] private BakeryNpc _targetNpc;
        private HashList<BakeryNpc> _npcList;
        private int _npcIndex;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _npcList = new HashList<BakeryNpc>();
            var dataCount = _npcData.NpcData.Count;
            for (var i = 0; i < _npcList.Count; i++)
            {
                var npc = _npcList[i];
                npc.Init(_npcData.NpcData[i % dataCount]);
            }
            RemoveTargetNpcFromList();
            _targetNpc.Init(_npcData.TargetNpcData);
        }

        private void RemoveTargetNpcFromList()
        {
            _npcList.Remove(_targetNpc);
        }

        public void Add(BakeryNpc item)
        {
            _npcList.Add(item);
        }

        public void Remove(BakeryNpc item)
        {
            _npcList.Remove(item);
        }

        public BakeryNpc GetNpc()
        {
            if (_npcList.Count == 0)
            {
                Debug.Log("npc list is empty");
                return null;
            }
            return _npcList[_npcIndex++ % _npcList.Count];
        }
    }
}