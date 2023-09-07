using UnityEngine;
using Cinemachine;
using static InputSystem;

public class CommandController : BaseSystem
{
    [SerializeField] private string ShipActionMap;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private bool PlayerRequestedControl;
    private PlayerController playerController;
    

    private void Update()
    {
        if (InputSystem.PlayerInputs.ShipExit)
        {
            ShipExit();
        }
        
        WhenPowered();
    }
    private void WhenPowered()
    {
        if (playerController == null) { return; }
        if (SystemPower && PowerSwitchState)
        {
            if (PlayerRequestedControl)
            {
                PlayerTakeOver();
            }
        }
        else
        {
            PlayerLoseControl();
        }
        PlayerRequestedControl = false;
    }
    private void ShipExit()
    {
        PlayerLoseControl();
    }
    private void PlayerTakeOver()
    {
        cinemachineVirtualCamera.gameObject.SetActive(true);
        playerController.RequestControlChange(ShipActionMap);
    }
    private void PlayerLoseControl()
    {
        cinemachineVirtualCamera.gameObject.SetActive(false);
        playerController.RequestControlChange(null);
        playerController = null;
    }

    private void SystemInteract(PlayerController _playerController)
    {
        playerController = _playerController;
        PlayerRequestedControl = true;
    }
}
