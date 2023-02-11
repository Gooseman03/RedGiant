using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CommandController : MonoBehaviour
{
    [SerializeField] private BaseSystem _baseSystem;
    public BaseSystem baseSystem
    {
        get { return _baseSystem; }
    }
    [SerializeField] private string ShipActionMap;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private bool PlayerRequestedControl;
    private PlayerController playerController;
    

    private void FixedUpdate()
    {
        WhenPowered();
    }
    private void WhenPowered()
    {

        if (playerController == null) { return; }
        if (baseSystem.SystemPower && baseSystem.PowerSwitchState)
        {
            if (playerController == null) { return; }
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
    }

    private void SystemInteract(PlayerController _playerController)
    {
        playerController = _playerController;
        PlayerRequestedControl = true;
    }
}
