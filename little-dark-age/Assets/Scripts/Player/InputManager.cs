using Photon.Pun;
using UnityEngine;

public class InputManager : MonoBehaviourPunCallbacks
{
    public static InputManager Instance;
    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    private void OnEnable()
    {
        playerControls?.Enable();
    }

    private void OnDisable()
    {
        playerControls?.Disable();
    }

    public bool PlayerIsAttacking()
        => playerControls.Ground.Attack.triggered;

    public bool PlayerIsShield()
        => playerControls.Ground.Shield.IsPressed() || playerControls.Ground.Shield.IsInProgress();

    public Vector2 GetPlayerMovement()
        => playerControls.Ground.Movement.ReadValue<Vector2>().normalized;

    public float OnRotate()
        => playerControls.Ground.Rotate.ReadValue<float>() * 0.15f;

    public bool PlayerJumpedThisFrame()
        => playerControls.Ground.Jump.IsPressed();

    public bool PlayerIsSneaking()
        => playerControls.Ground.Crouch.IsPressed();

    public bool PlayerIsRunning()
        => playerControls.Ground.Run.IsPressed();

    public bool PlayerOpenInventory()
        => playerControls.Ground.OpenInventory.triggered;
    
    public bool PlayerCloseInventory()
        => playerControls.Inventory.CloseInventory.triggered;

    public bool PlayerOpenShop()
        => playerControls.Ground.OpenShop.triggered;

    public bool PlayerCloseShop()
        => playerControls.Shop.CloseShop.triggered;

    public void OpenInventory()
    {
        playerControls.Ground.Disable();
        playerControls.Inventory.Enable();
    }

    public void CloseInventory()
    {
        playerControls.Inventory.Disable();
        playerControls.Ground.Enable();
    }

    public void DisableActionMap()
    {
        playerControls.Ground.Disable();
        playerControls.Shop.Disable();
        playerControls.Inventory.Disable();
        playerControls.PauseMenu.Disable();
    }


    public bool PlayerOpenPauseMenu()
        => playerControls.Ground.OpenPauseMenu.triggered;
    
    public bool PlayerClosePauseMenu()
        => playerControls.PauseMenu.Resume.triggered;
    
    public void OpenPauseMenu()
    {
        playerControls.Ground.Disable();
        playerControls.PauseMenu.Enable();
    }

    public void ClosePauseMenu()
    {
        playerControls.PauseMenu.Disable();
        playerControls.Ground.Enable();
    }

    public bool BossTeleport()
    {
        return playerControls.Ground.BossTP.triggered;
    }

    
    public void OpenShop()
    {
        playerControls.Ground.Disable();
        playerControls.Shop.Enable();
    }

    public void CloseShop()
    {
        playerControls.Shop.Disable();
        playerControls.Ground.Enable();
    }

    public void EnableControls()
        => playerControls.Ground.Enable();
}