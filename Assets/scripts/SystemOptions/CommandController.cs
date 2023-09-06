using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CommandController : BaseSystem
{
    [SerializeField] private string ShipActionMap;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private bool PlayerRequestedControl;
    private PlayerController playerController;
    

    private void Update()
    {
        WhenPowered();
    }
    private void WhenPowered()
    {
        if (playerController == null) { return; }
        if (SystemPower && PowerSwitchState)
        {
            if (playerController == null) { return; }
            if (PlayerRequestedControl)
            {
                PlayerTakeOver(playerController);
            }
        }
        else
        {
            PlayerLoseControl(playerController);
        }
        PlayerRequestedControl = false;
    }
    private void ShipExit(PlayerController playerController)
    {
        PlayerLoseControl(playerController);
    }
    private void PlayerTakeOver(PlayerController playerController)
    {
        cinemachineVirtualCamera.gameObject.SetActive(true);
        playerController.RequestControlChange(ShipActionMap);
    }
    private void PlayerLoseControl(PlayerController playerController)
    {
        cinemachineVirtualCamera.gameObject.SetActive(false);
        playerController.RequestControlChange(null);
    }

    private void SystemInteract(PlayerController _playerController)
    {
        playerController = _playerController;
        PlayerRequestedControl = true;
    }
}
