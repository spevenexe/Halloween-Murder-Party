using UnityEngine;

public class PlayerControl : PlayerSystem
{
    void Update()
    {
        playerData.MoveDirection = playerData.MovementInput.ReadValue<Vector2>();
        playerData.LookDelta = playerData.LookInput.ReadValue<Vector2>();
    }
}