using Interaction;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "New Item Data",menuName = "InventorySystem/Item Data")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private new string name;
        public string Name => name;

        // image
        [SerializeField] private Sprite sprite;
        public Sprite Sprite => sprite;

        // in-game entity
        [SerializeField] private ItemEntity prefab;
        public ItemEntity Prefab => prefab;
    }
}