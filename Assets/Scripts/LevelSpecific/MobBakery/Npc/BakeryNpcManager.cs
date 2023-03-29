using System.Collections.Generic;
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
        [SerializeField] private List<BakeryNpc> _npcListForEditor;
        private HashList<BakeryNpc> _npcList;
        private int _npcIndex;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _npcList = new HashList<BakeryNpc>();
            _npcListForEditor = new List<BakeryNpc>();
            _targetNpc.Init(_npcData.TargetNpcData);
        }

        private void RemoveTargetNpcFromList()
        {
            _npcList.Remove(_targetNpc);
        }

        public void Add(BakeryNpc item)
        {
            if(item == _targetNpc)
                return;
            
            _npcList.Add(item);
            _npcListForEditor.Add(item);
            var itemCount = _npcList.Count;
            var dataCount = _npcData.NpcData.Count;
            item.Init(_npcData.NpcData[itemCount % dataCount]);
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