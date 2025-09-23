using UnityEngine;
using Interaction;
using UnityEngine.UI;



#if UNITY_EDITOR
using UnityEditor;
#endif

namespace InventorySystem
{
    public class HotbarSlot : MonoBehaviour
    {
        [SerializeField] private Image image;

        public void Set(Item item)
        {
            image.enabled = true;
            image.sprite = item.Sprite;
        }

        public void Unset()
        {
            image.enabled = false;
        }
    }

#if UNITY_EDITOR
[CustomEditor(typeof(HotbarSlot))]
public class HotbarSlotEditor : Editor
{
    private ItemEntity entity;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        HotbarSlot slot = target as HotbarSlot;

        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Item", GUILayout.Width(45f));
        entity = EditorGUILayout.ObjectField(entity, typeof(ItemEntity),true) as ItemEntity;

        if (GUILayout.Button("Set", GUILayout.Width(120f)))
        {
            slot.Set(entity.Item);
        }

        GUILayout.EndHorizontal();

        if (GUILayout.Button("Unset", GUILayout.Width(120f)))
        {
            slot.Unset();
        }
    }
}

#endif
}
