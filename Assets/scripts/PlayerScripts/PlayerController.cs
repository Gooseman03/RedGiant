using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static InputSystem;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float speed = 5f;

    [SerializeField] private float ReachDistance = 1.0f;

    private string DefaultActionMap = "Player";

    [SerializeField, Range(0f, 89f)] private float UpperLimit = 10f;
    [SerializeField, Range(0f, -89f)] private float LowerLimit = -10f;
    private float RotationX = 0.0f;
    private float RotationY = 0.0f;

    private CharacterController controller;
    [SerializeField] private GameObject cameraFollow;
    private Vector3 movement = Vector3.zero;

    private void Awake()
    {
        PlayerMessaging.PlayerRegister(this);
        controller = GetComponent<CharacterController>();
    }

    [ServerRpc]
    private void OnCharactorMoveServerRPC(Vector3 MoveVector)
    {
        controller.Move(MoveVector);
    }

    private void LookPlayer()
    {
        //if (CurrentOxygenArea == null)
        //{
        //    RotationX -= PlayerInputs.LookInput.y;
        //    RotationY += PlayerInputs.LookInput.x;
        //    RotationX = Mathf.Clamp(RotationX, LowerLimit, UpperLimit);
        //    transform.localEulerAngles = new Vector3(RotationX, RotationY, 0);
        //}
        //else if (PlayerInputs.LookInput != Vector2.zero)
        //{
        RotationX -= PlayerInputs.LookInput.y;
        RotationY += PlayerInputs.LookInput.x;
        RotationX = Mathf.Clamp(RotationX, LowerLimit, UpperLimit);
        cameraFollow.transform.eulerAngles = new Vector3(RotationX, RotationY, 0);
        transform.localEulerAngles = new Vector3(0, RotationY, 0);
        //}
    }
    private void Update()
    {
        if (!IsOwner)
        {
            this.GetComponentInChildren<CinemachineVirtualCamera>().enabled = false;
            return;
        }
        if (MenuRequester.IsConsoleOpen())
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
                MovePlayer();
                //Pickup();
                //Interact();
                Cursor.lockState = CursorLockMode.Locked;
            //else
            //{
            //    //Shocked();
            //}
        }
        //UpdateOxygen();
        //UpdateCarbon();
        //Breathing();
    }
    private void MovePlayer()
    {
        
        movement.x = 0;
        movement.z = 0;
        movement += PlayerInputs.MoveInput.x * transform.right * speed;
        movement += PlayerInputs.MoveInput.z * transform.forward * speed;
        if (controller.isGrounded)
        {
            movement.y = 0;
        }
        else
        {
            movement.y = movement.y + (-1 * Time.deltaTime);
        }
        if(!IsServer)
        {
            controller.Move(movement * Time.deltaTime);
        }
        OnCharactorMoveServerRPC(movement * Time.deltaTime);
    }
    public void RequestControlChange(string inputActionMap)
    {
        if (inputActionMap == null)
        {
            PlayerInputs.PlayerInput.SwitchCurrentActionMap(DefaultActionMap);
            return;
        }
        PlayerInputs.PlayerInput.SwitchCurrentActionMap(inputActionMap);
    }

    //private void Interact()
    //{
    //    if (PlayerInputs.InteractInput)
    //    {
    //        RaycastHit hit;
    //        if (Physics.Raycast(CameraFollow.transform.position, CameraFollow.transform.forward, out hit, ReachDistance))
    //        {
    //            hit.transform.gameObject.SendMessage("OnInteract", this, SendMessageOptions.DontRequireReceiver);
    //        }
    //    }
    //    PlayerInputs.InteractInput = false;
    //}

    //private void Pickup()
    //{
    //    if (PlayerInputs.PickupInput)
    //    {
    //        RaycastHit hit;
            
    //        if (Physics.Raycast(CameraFollow.transform.position, CameraFollow.transform.forward, out hit, ReachDistance))
    //        {
    //            if (
    //                hit.collider.transform.TryGetComponent(out ObjectPlace objectPlace)
    //                && ItemInLeftHand != null
    //                && (objectPlace.objectType == ItemInLeftHand.GetComponent<ObjectDirector>().objectType || objectPlace.objectType == ObjectType.Generic))
    //            {
    //                ItemInLeftHand.GetComponent<IGrabbable>().Place(objectPlace.transform);
    //                ItemInLeftHand = null;
    //                PlayerInputs.PickupInput = false;
    //                return;
    //            }
    //            else if (hit.collider.transform.TryGetComponent(out IGrabbable objectGrabbable) && ItemInLeftHand == null)
    //            {
    //                ItemInLeftHand = hit.collider.transform.gameObject;
    //                objectGrabbable.Grab(objectLeftGrabPointTransform);
    //                PlayerInputs.PickupInput = false;
    //                return;
    //            }
    //            else if (ItemInLeftHand != null)
    //            {
    //                ItemInLeftHand.GetComponent<IGrabbable>().Drop(transform.parent);
    //                ItemInLeftHand = null;
    //                PlayerInputs.PickupInput = false;
    //                return;
    //            }
    //        }
    //        else if (ItemInLeftHand != null)
    //        {
    //            ItemInLeftHand.GetComponent<IGrabbable>().Drop(transform.parent);
    //            ItemInLeftHand = null;
    //            PlayerInputs.PickupInput = false;
    //            return;
    //        }
    //    }
    //    PlayerInputs.PickupInput = false;
    //}

   
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(cameraFollow.transform.position, cameraFollow.transform.forward * ReachDistance);
    }
}
