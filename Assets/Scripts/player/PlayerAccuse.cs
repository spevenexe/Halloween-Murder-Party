using DialogueSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAccuse : PlayerSystem
{
    public void AccuseNPC(InputAction.CallbackContext context)
    {
        // only does something if the player is looking at an NPC, or we are tlaking to one currently
        Interactable target = playerData.Target;
        DialogueSource dialogueSource = DialogueUI.instance.CurrentDialogueSource;
        if (target is NPC npc)
        {
            npc.EvaluateCheck(playerData);
        }
        // could be useful later, but for now, this block below doesn't execute
        else if (dialogueSource != null && dialogueSource is NPC dialogueNPC)
        {
            dialogueNPC.EvaluateCheck(playerData);
        }
    }

    void OnEnable()
    {
        playerData.AccuseInput.performed += AccuseNPC;
    }

    void OnDisable()
    {
        playerData.AccuseInput.performed -= AccuseNPC;
    }
}