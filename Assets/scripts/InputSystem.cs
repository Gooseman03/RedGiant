using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputSystem
{
    public class InputSet
    {
        public Vector3 MoveInput = Vector3.zero;
        public Vector2 LookInput = Vector2.zero;
        public bool PickupInput = false;
        public bool InteractInput = false;
        public Vector3 ShipMoveInput = Vector3.zero;
        public Vector2 ShipLookInput = Vector2.zero;
        public bool ShipExit = false;
        public Vector2 WeaponRotate = Vector2.zero;
        public bool WeaponExit = false;
        public bool Fire = false;
        public PlayerInput PlayerInput = null;
    }

    public static InputSet PlayerInputs = new InputSet();
}