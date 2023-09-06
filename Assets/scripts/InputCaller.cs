using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem;

public class InputCaller : MonoBehaviour
{
    public PlayerInput playerInput;

    private void Start()
    {
        PlayerInputs.PlayerInput = playerInput;
    }
    public void OnMove(InputValue value)
    {
        PlayerInputs.MoveInput = value.Get<Vector3>().normalized;
    }
    public void OnLook(InputValue value)
    {
        PlayerInputs.LookInput = value.Get<Vector2>();
    }
    public void OnPickup(InputValue value)
    {
        PlayerInputs.PickupInput = value.isPressed;
    }
    public void OnInteract(InputValue value)
    {
        PlayerInputs.InteractInput = value.isPressed;
    }
    public void OnShipMove(InputValue value)
    {
        PlayerInputs.ShipMoveInput = value.Get<Vector3>();
    }
    public void OnShipLook(InputValue value)
    {
        PlayerInputs.ShipLookInput = value.Get<Vector2>();
    }
    public void OnExit(InputValue value)
    {
        PlayerInputs.ShipExit = value.isPressed;
    }
    public void OnWeaponRotate(InputValue value)
    {
        PlayerInputs.WeaponRotate = value.Get<Vector2>().normalized;
    }
    public void OnFire(InputValue value)
    {
        PlayerInputs.Fire = value.isPressed;
    }
}
