using DialogueSystem;
using InventorySystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interaction
{
    public class ItemEntity : Interactable
    {
        [SerializeField] private Item item;

        void OnEnable()
        {
            OnHighlight.AddListener(RevealInteractPrompt);
            OnDehighlight.AddListener(HideInteractPrompt);
        }

        public override void Activate(PlayerData playerData = null)
        {
            Debug.Log("weeee");

            // add to inventory
            PlayerData.instance.Inventory.Add(item);

            // move it object pool (maybe bring it back later?)
            transform.SetPositionAndRotation(new(0, 1000, 0), Quaternion.identity);
        }

        protected override void RevealInteractPrompt()
        {
            string message = $"[{PlayerData.instance.InteractInput.GetBindingDisplayString()}] Collect";

            DialogueUI.instance.ShowInteractionUI(true, message);
        }

        protected override void HideInteractPrompt()
        {
            //Hide interaction UI
            DialogueUI.instance.ShowInteractionUI(false);
        }

#if UNITY_EDITOR

        public Item Item => item;

#endif

    }
}