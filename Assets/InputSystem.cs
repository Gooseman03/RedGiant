using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputSystem
{
    public class InputSet
    {
        private Vector2 LookInput = Vector2.zero;
        private Vector2 MoveInput = Vector2.zero;
        private bool PickupInput = false;
        private bool InteractInput = false;
    }
    public static InputSet PlayerInput;
    public static void OnLook(InputValue value)
    {
        LookInput = value.Get<Vector2>();
    }
}
