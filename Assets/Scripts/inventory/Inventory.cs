using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace InventorySystem
{
    /// <summary>
    /// A wrapper class for a list of Items. Invokes Unity Events as needed to call any other functions.
    /// </summary>
    public class Inventory
    {
        public List<Item> Items { get; private set; }

        public UnityEvent<Item[]> ItemsAdded = new();
        public UnityEvent<Item[]> ItemsRemoved = new();

        public Inventory(int capacity = 10)
        {
            Items = new(capacity);
        }

        public void Add(params Item[] items)
        {
            int remaining = Items.Capacity - Items.Count;
            if (items.Length > remaining)
            {
                throw new Exception($"Number of items to add {items.Length} exceeds remaining Inventory capacity {remaining}");
            }
            else
            {
                Items.AddRange(items);
                ItemsAdded.Invoke(items);
            }

        }

        public void Remove(params Item[] items)
        {
            List<Item> successfullyRemoved = new(items.Length);
            foreach (Item item in items)
            {
                if (Items.Contains(item))
                {
                    Items.Remove(item);
                    successfullyRemoved.Add(item);
                }
            }
            ItemsRemoved.Invoke(successfullyRemoved.ToArray());
        }

        public void Remove(int index)
        {
            if (index > Items.Count)
                throw new IndexOutOfRangeException($"No item exists at {index}");
            else
                Items.RemoveAt(index);
        }

        public void Clear()
        {
            Item[] cleared = Items.ToArray();
            Items.Clear();
            ItemsRemoved.Invoke(cleared);
        }
    }
}