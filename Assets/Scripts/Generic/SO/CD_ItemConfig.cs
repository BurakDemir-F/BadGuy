using System.Collections.Generic;
using UnityEngine;

namespace Generic.SO
{
    [CreateAssetMenu(menuName = "Data/Config/ItemConfig", fileName = "CD_ItemConfig", order = 0)]
    public class CD_ItemConfig : ScriptableObject , IItemCreationVOProvider
    {
        [SerializeField] private List<ItemCreationVO> _items;
        public List<ItemCreationVO> Items
        {
            get => _items;
            set => _items = value;
        }
    }
    
    [System.Serializable]
    public class ItemCreationVO
    {
        public string NewPrefabName;
        public GameObject MeshPrefab;
    }

    public interface IItemCreationVOProvider
    {
        List<ItemCreationVO> Items { get; set; }
    }
}