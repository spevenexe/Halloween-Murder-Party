using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : PlayerSystem
{
    void Update()
    {
        if (playerData.isPaused)
        {
            playerData.MoveDirection = Vector2.zero;
            playerData.LookDelta = Vector2.zero;
            return;
        }

        playerData.MoveDirection = playerData.MovementInput.ReadValue<Vector2>();
        playerData.LookDelta = playerData.LookInput.ReadValue<Vector2>();
    }
}