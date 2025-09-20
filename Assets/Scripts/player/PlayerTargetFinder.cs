using UnityEngine;

/// <summary>
/// Finds the interaction target, based on what the player is looking at
/// </summary>
public class PlayerTargetFinder : PlayerSystem
{
    void Update()
    {
        Ray ray = new(playerData.CameraTransform.position,
            playerData.CameraTransform.forward);
        RaycastHit[] hits = new RaycastHit[1];

        // look for something interactable. Keep in mind a raycast won't always return the closest object
        int length = Physics.RaycastNonAlloc(
            ray,
            hits,
            playerData.interactRange,
            LayerMask.GetMask("Interact"));

        // store it as the target
        if (length > 0)
        {
            RaycastHit hit = hits[0];

            playerData.Target = hit.transform.GetComponent<Interactable>();
            playerData.Target.OnHighlight.Invoke();
        }
        // otherwise, we aren't looking at something interactable
        else
        {
            // if we were looking at something, dehighlight it
            playerData.Target?.OnDehighlight.Invoke();

            playerData.Target = null;
        }
    }
}