using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Vector2 LookInput = Vector2.zero;
    [SerializeField] private Vector2 MoveInput = Vector2.zero;
    [SerializeField] private bool PickupInput = false;
    [SerializeField] private bool InteractInput = false;
    [SerializeField, Range(0f,89f)] private float UpperLimit = 10f;
    [SerializeField, Range(0f, -89f)] private float LowerLimit = -10f;

    [SerializeField, Range(0f, 100f)] private float OxygenLevel;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float gravity;

    private CharacterController controller;
    [SerializeField] private GameObject CameraFollow;


    [SerializeField] private Transform objectLeftGrabPointTransform;
    [SerializeField] private GameObject ItemInLeftHand;
    [SerializeField] private Transform objectRightGrabPointTransform;
    [SerializeField] private GameObject ItemInRightHand;

    [SerializeField] private UiUpdate UiScript;


    private float RotationX = 0.0f;
    private float RotationY = 0.0f;

    private void Start() 
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MovePlayer();
        LookPlayer();
        Pickup();
        Interact();
        UpdateUi();
        Cursor.lockState = CursorLockMode.Locked;
    }


    private bool CheckGrounded()
    {
        if (!isGrounded)
        {
            return false;
        }
        return true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            isGrounded = false;
        }
    }
    private void Interact()
    {
        if (InteractInput)
        {
            RaycastHit hit;
            if (Physics.Raycast(CameraFollow.transform.position, CameraFollow.transform.forward, out hit, 10f))
            {
                hit.transform.gameObject.SendMessage("OnInteract", SendMessageOptions.DontRequireReceiver);
            }
        }
        InteractInput = false;
    }

    private void UpdateUi()
    {
        UiScript.ChangeColor(new Color (0, 0, 0, (-OxygenLevel/100) +1));
    }

    private void ChangeOxygen(float amount)
    {
        OxygenLevel += amount;
    }
    private void Death()
    {

    }
    private void MovePlayer()
    {
        Vector3 move = MoveInput.x * transform.right + MoveInput.y * transform.forward;
        if (!CheckGrounded())
        {
            move += -gravity * transform.up;
        }
        controller.Move(move * speed * Time.deltaTime);
    }
    private void LookPlayer()
    {
        if (LookInput != Vector2.zero)
        {
            RotationX -= LookInput.y * .2f;
            RotationY += LookInput.x * .2f;
            RotationX = Mathf.Clamp(RotationX, LowerLimit, UpperLimit);
            CameraFollow.transform.eulerAngles = new Vector3(RotationX, RotationY, 0);
            transform.eulerAngles = new Vector3(0, RotationY, 0);
        }
    }
    private void Pickup()
    {
        if (PickupInput)
        {
            RaycastHit hit;
            if (Physics.Raycast(CameraFollow.transform.position, CameraFollow.transform.forward, out hit, 10f))
            {
                if (hit.transform.TryGetComponent(out ObjectPlace objectPlace) && ItemInLeftHand != null && objectPlace.GetObjectType() == ItemInLeftHand.GetComponent<ObjectGrabbable>().GetObjectType())
                {   
                    ItemInLeftHand.GetComponent<ObjectGrabbable>().Place(objectPlace.transform);
                    ItemInLeftHand = null;
                    PickupInput = false;
                    return;
                }
                else if (hit.transform.TryGetComponent(out ObjectGrabbable objectGrabbable) && ItemInLeftHand == null)
                {
                    objectGrabbable.Grab(objectLeftGrabPointTransform);
                    ItemInLeftHand = hit.transform.gameObject;
                    PickupInput = false;
                    return;
                }
                else if (ItemInLeftHand != null)
                {
                    ItemInLeftHand.GetComponent<ObjectGrabbable>().Drop();
                    ItemInLeftHand = null;
                    PickupInput = false;
                    return;
                }
            }
            else if (ItemInLeftHand != null)
            {
                ItemInLeftHand.GetComponent<ObjectGrabbable>().Drop();
                ItemInLeftHand = null;
                return;
            }
        }
        PickupInput = false;
    }
    public void OnLook(InputValue value)
    {
        LookInput = value.Get<Vector2>();
    }
    public void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();
    }
    public void OnPickup(InputValue value)
    {
        PickupInput = value.isPressed;
    }

    public void OnInteract(InputValue value)
    {
        InteractInput = value.isPressed;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(CameraFollow.transform.position, CameraFollow.transform.forward * 10f);
    }
}
