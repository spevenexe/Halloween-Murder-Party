using Interaction;
using UnityEngine;

namespace InventorySystem
{
    [System.Serializable]
    public class Item
    {
        [SerializeField] private ItemData data;

        public string Name => data.Name;
        public Sprite Sprite => data.Sprite;
        public ItemEntity Prefab => data.Prefab;

        public override bool Equals(object other)
        {
            if (other != null && other is Item item)
                return Equals(item);
            else return false;
        }

        public override int GetHashCode()
        {
            return data.name.GetHashCode();
        }

        public bool Equals(Item other)
        {
            return data.name.ToLowerInvariant().Trim() == other.data.name.ToLowerInvariant().Trim(); // might want this to be more sophisticated
        }

        public static bool operator ==(Item item1, Item item2)
        {
            return item1.Equals(item2);
        }

        public static bool operator !=(Item item1, Item item2)
        {
            return !item1.Equals(item2);
        }
    }
}