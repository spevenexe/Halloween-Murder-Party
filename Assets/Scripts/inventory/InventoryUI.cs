using System.Collections.Generic;
using UnityEngine;
using InventorySystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace InventorySystem
{
    public class InventoryUI : DontDestroySingleton<InventoryUI>
    {
        [SerializeField] private GameObject hotbarRoot;
        private HotbarSlot[] hotbar;

        protected override void Awake()
        {
            base.Awake();

            hotbar = hotbarRoot.GetComponentsInChildren<HotbarSlot>();
        }

        void OnEnable()
        {
            PlayerData.instance.Inventory.ItemsAdded.AddListener(UpdateHotbar);
            PlayerData.instance.Inventory.ItemsRemoved.AddListener(UpdateHotbar);
        }

        void OnDisable()
        {
            PlayerData.instance.Inventory.ItemsAdded.RemoveListener(UpdateHotbar);
            PlayerData.instance.Inventory.ItemsRemoved.RemoveListener(UpdateHotbar);
        }

        public void UpdateHotbar(Item[] _)
        {
            List<Item> items = PlayerData.instance.Inventory.Items;
            int i = 0;
            for (; i < items.Count && i < hotbar.Length; i++)
            {
                HotbarSlot _slot = hotbar[i];
                Item _item = items[i];

                _slot.Set(_item);
            }

            for (; i < hotbar.Length; i++)
            {
                HotbarSlot _slot = hotbar[i];

                _slot.Unset();
            }
        }

        public void Show()
        {
            hotbarRoot.SetActive(true);
        }

        public void Hide()
        {
            hotbarRoot.SetActive(false);
        }

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(InventoryUI))]
public class InventoryUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        InventoryUI inventoryUI = target as InventoryUI;
        if (GUILayout.Button("Show", GUILayout.Width(120f)))
        {
            inventoryUI.Show();
        }

        if (GUILayout.Button("Hide", GUILayout.Width(120f)))
        {
            inventoryUI.Hide();
        }
    }
}
#endif