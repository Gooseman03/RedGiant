using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputSystem;
public class Weapons : BaseSystem
{
    [SerializeField] private GameObject Projectile;
    [SerializeField] private float sensitivity = 1f;
    [SerializeField] private string ShipActionMap;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private GameObject CameraLookat;

    [SerializeField] private GameObject Firepoint;

    private bool PlayerRequestedControl;
    private PlayerController playerController;

    private float RotationX = 0.0f;
    private float RotationY = 0.0f;

    [SerializeField, Range(0f, 89f)] private float UpperLimit = 10f;
    [SerializeField, Range(0f, -89f)] private float LowerLimit = -10f;
    private void Update()
    {
        WhenPowered();
    }
    private void WhenPowered()
    {
        if (playerController == null) { return; }
        if (SystemPower && PowerSwitchState)
        {
            if (PlayerInputs.Fire)
            {
                WeaponFire();
                PlayerInputs.Fire = false;
            }
            if (PlayerInputs.WeaponRotate !=  null)
            {
                RotationX -= PlayerInputs.WeaponRotate.y * sensitivity;
                RotationY += PlayerInputs.WeaponRotate.x * sensitivity;
                RotationX = Mathf.Clamp(RotationX, LowerLimit, UpperLimit);
                CameraLookat.transform.eulerAngles = new Vector3(RotationX, RotationY, 0);
            }
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
    private void WeaponFire()
    {
        GameObject projectileObject = new GameObject();
        projectileObject.SetActive(false);
        projectileObject.transform.position = Firepoint.transform.position;
        projectileObject.layer = 7;
        Projectile projectileScript = projectileObject.AddComponent<Laser>();
        projectileScript.Travel = Firepoint.transform.forward;
        projectileObject.SetActive(true);
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
