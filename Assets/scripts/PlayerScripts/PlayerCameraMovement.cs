using UnityEngine;
using static InputSystem;


public class PlayerCameraMovement : CameraSync
{
    [SerializeField, Range(0f, 89f)] private float UpperLimit = 10f;
    [SerializeField, Range(0f, -89f)] private float LowerLimit = -10f;
    private PlayerController controller;
    private float RotationX = 0.0f;
    private float RotationY = 0.0f;
    void Start()
    {
        controller = GetComponent<PlayerController>();
    }
    protected override void OnCameraUpdate()
    {
        LookPlayer();
    }
    private void LookPlayer()
    {
        if (controller.CurrentOxygenArea == null)
        {
            RotationX -= PlayerInputs.LookInput.y;
            RotationY += PlayerInputs.LookInput.x;
            RotationX = Mathf.Clamp(RotationX, LowerLimit, UpperLimit);
            transform.localEulerAngles = new Vector3(RotationX, RotationY, 0);
        }
        else if (PlayerInputs.LookInput != Vector2.zero)
        {
            RotationX -= PlayerInputs.LookInput.y;
            RotationY += PlayerInputs.LookInput.x;
            RotationX = Mathf.Clamp(RotationX, LowerLimit, UpperLimit);
            controller.CameraFollow.transform.eulerAngles = new Vector3(RotationX, RotationY, 0);
            transform.localEulerAngles = new Vector3(0, RotationY, 0);
        }
    }
}
