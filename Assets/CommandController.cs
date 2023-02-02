using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CommandController : MonoBehaviour
{
    [SerializeField] private BaseSystem baseSystem;
    [SerializeField] private string ShipActionMap;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private List<Material> materials = new List<Material>();

    private bool PlayerRequestedControl;
    private PlayerController playerController;
    

    private void FixedUpdate()
    {
        WhenPowered();
    }
    private void WhenPowered()
    {
        
        if(baseSystem.itemRegister.HasObject(ObjectType.Monitor, out List<ObjectDirector> ListOfMonitors))
        {
            foreach(ObjectDirector monitor in ListOfMonitors)
            {
                monitor.MonitorPlaneEnable();
                monitor.SetMonitorPlaneMaterial(materials[0]);
            }
        }

        if (playerController == null) { return; }

        if (baseSystem.SystemPower && baseSystem.PowerSwitchState)
        {
            if(PlayerRequestedControl)
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