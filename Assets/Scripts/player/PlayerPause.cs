using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPause : PlayerSystem
{
    [SerializeField] private GameObject PauseMenu;

    protected override void Awake()
    {
        base.Awake();
        PauseMenu.SetActive(false);
    }

    void OnEnable()
    {
        playerData.PauseInput.performed += Pause;
    }

    void OnDisable()
    {
        playerData.PauseInput.performed -= Pause;
        playerData.PauseInput.performed -= Unpause;
    }

    private void Pause(InputAction.CallbackContext context)
    {
        PlayerCamera.instance.LockCamera();
        PauseMenu.SetActive(true);
        playerData.PauseInput.performed -= Pause;
        playerData.PauseInput.performed += Unpause;
        playerData.isPaused = true;
    }

    private void Unpause(InputAction.CallbackContext context) => Unpause();

    public void Unpause()
    {
        PlayerCamera.instance.UnlockCamera();
        PauseMenu.SetActive(false);
        playerData.PauseInput.performed -= Unpause;
        playerData.PauseInput.performed += Pause;
        playerData.isPaused = false;
    }
    
}