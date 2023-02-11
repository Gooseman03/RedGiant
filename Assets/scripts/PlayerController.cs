using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    [SerializeField] private Vector3 ShipMoveInput = Vector3.zero;
    [SerializeField] private Vector2 ShipLookInput = Vector2.zero;
    [SerializeField] private bool ShipExit = false;

    [SerializeField] private Vector2 LookInput = Vector2.zero;
    [SerializeField] private Vector2 MoveInput = Vector2.zero;
    [SerializeField] private bool PickupInput = false;
    [SerializeField] private bool InteractInput = false;

    [SerializeField] private string DefaultActionMap;


    [SerializeField, Range(0f,89f)] private float UpperLimit = 10f;
    [SerializeField, Range(0f, -89f)] private float LowerLimit = -10f;

    [SerializeField] private bool IsBeingShocked = false;
    [SerializeField] private float DeathTimer = 0;
    [SerializeField] private int FightDeathPresses = 20;
    [SerializeField] private int LastFightDeathPresses = 20;

    [SerializeField, Range(0f, 100f)] private float OxygenLevel;
    [SerializeField, Range(0f, 100f)] private float CarbonLevel;
    [SerializeField] private OxygenCube CurrentOxygenArea;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float gravity;

    private CharacterController controller;
    [SerializeField] private GameObject CameraFollow;
    [SerializeField] private Transform objectLeftGrabPointTransform;
    [SerializeField] private GameObject ItemInLeftHand;
    [SerializeField] private Transform objectRightGrabPointTransform;
    [SerializeField] private GameObject ItemInRightHand;

    [SerializeField] private UiUpdate UiScript;

    [SerializeField] private AudioSource Audio;
    [SerializeField] private AudioSource ElectrocutionAudioSource;


    private float RotationX = 0.0f;
    private float RotationY = 0.0f;

    private void Start() 
    {
        controller = GetComponent<CharacterController>();
    }
    private void ShockPlayer()
    {
        IsBeingShocked = true;
    }
    private void Update()
    {
        SendMessageUpwards("ShipMove", ShipMoveInput);
        SendMessageUpwards("ShipLook", ShipLookInput);
        if (ShipExit)
        {
            this.transform.parent.BroadcastMessage("ShipExit");
            ShipExit = false;
        }
        if (!IsBeingShocked)
        {
            MovePlayer();
            LookPlayer();
            Pickup();
            Interact();
            Cursor.lockState = CursorLockMode.Locked;
        } else
        {
            Shocked();
        }
        UpdateUi();
        UpdateOxygen();
        UpdateCarbon();
        Breathing();
        if (OxygenLevel <= 0 || CarbonLevel >= 100)
        {
            Death();
        }
    }

    public void RequestControlChange(string inputActionMap)
    {
        if (inputActionMap == null)
        {
            this.GetComponent<PlayerInput>().SwitchCurrentActionMap(DefaultActionMap);
            return;
        }
        this.GetComponent<PlayerInput>().SwitchCurrentActionMap(inputActionMap);
    }
    private void Shocked()
    {
        if (!ElectrocutionAudioSource.isPlaying)
        {
            UiScript.StartShocking(3, LastFightDeathPresses);
            LastFightDeathPresses = FightDeathPresses;
            ElectrocutionAudioSource.Play();
        }
        DeathTimer += Time.deltaTime;
        Cursor.lockState = CursorLockMode.None;
        if (InteractInput)
        {
            FightDeathPresses -= 1;
            InteractInput = false;
        }
        if (FightDeathPresses <= 0)
        {
            UiScript.EscapeShock();
            IsBeingShocked = false;
            DeathTimer = 0;
            FightDeathPresses = LastFightDeathPresses*2;
            ElectrocutionAudioSource.Stop();
        }


        if (DeathTimer >= 3)
        {
            Death();
        }
    }
    private void UpdateCarbon()
    {
        if (CurrentOxygenArea != null)
        {
            CurrentOxygenArea.ChangeCarbon(1 * Time.deltaTime);
            CarbonLevel = Mathf.Lerp(CarbonLevel, CurrentOxygenArea.Carbon, Time.deltaTime);
        }
        else
        {
            CarbonLevel = Mathf.Lerp(CarbonLevel, 0f, .5f * Time.deltaTime);
        }

    }
    private void UpdateOxygen()
    {
        if (CurrentOxygenArea != null)
        {
            CurrentOxygenArea.ChangeOxygen(-1 * Time.deltaTime);
            OxygenLevel = Mathf.Lerp(OxygenLevel, CurrentOxygenArea.Oxygen, 1f * Time.deltaTime);
        }
        else
        {
            OxygenLevel = Mathf.Lerp(OxygenLevel, 0f, .5f*Time.deltaTime);
        }
        
    }
    private void Breathing()
    {
        if ((OxygenLevel < 50f || CarbonLevel > 50f) && !Audio.isPlaying)
        {
            Audio.Play();
        }
        else if ((OxygenLevel > 50f && CarbonLevel < 50f))
        {
            Audio.Stop();
        }
        if (OxygenLevel <= 0.1f)
        {
            OxygenLevel = 0;
        }
        if ( CarbonLevel >= 99.9f)
        {
            CarbonLevel = 100;
        }
    }
    private bool CheckGrounded()
    {
        if (!isGrounded)
        {
            return false;
        }
        return true;
    }
    private void OnCollisionStay(Collision collision)
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
                hit.transform.gameObject.SendMessage("OnInteract", this,SendMessageOptions.DontRequireReceiver);
            }
        }
        InteractInput = false;
    }
    private void UpdateUi()
    {
        if (IsBeingShocked)
        {
            UiScript.ChangeShockStates(DeathTimer, FightDeathPresses);
        }
        UiScript.ChangeColor(new Color(0f, 0f, 0f, ( CarbonLevel/100)), new Color(0f, 0f, 0f, ((-OxygenLevel + 3) / 100) + .97f));
    }
    private void Death()
    {
        RestartLevel();
    }
    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void MovePlayer()
    {
        Vector3 move = MoveInput.x * transform.right + MoveInput.y * transform.forward;
        if (!CheckGrounded())
        {
            move += -gravity * Time.deltaTime * transform.up;
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
            transform.localEulerAngles = new Vector3(0, RotationY, 0);
        }
    }
    private void Pickup()
    {
        if (PickupInput)
        {
            RaycastHit hit;
            if (Physics.Raycast(CameraFollow.transform.position, CameraFollow.transform.forward, out hit, 10f))
            {
                if (hit.transform.TryGetComponent(out ObjectPlace objectPlace) && ItemInLeftHand != null && (objectPlace.objectType == ItemInLeftHand.GetComponent<ObjectDirector>().objectType || objectPlace.objectType == ObjectType.Generic))
                {   
                    ItemInLeftHand.GetComponent<ObjectDirector>().Place(objectPlace.transform);
                    ItemInLeftHand = null;
                    PickupInput = false;
                    return;
                }
                else if (hit.transform.TryGetComponent(out ObjectDirector objectGrabbable) && ItemInLeftHand == null)
                {
                    objectGrabbable.Grab(objectLeftGrabPointTransform);
                    ItemInLeftHand = hit.transform.gameObject;
                    PickupInput = false;
                    return;
                }
                else if (ItemInLeftHand != null)
                {
                    ItemInLeftHand.GetComponent<ObjectDirector>().Drop(transform.parent);
                    ItemInLeftHand = null;
                    PickupInput = false;
                    return;
                }
            }
            else if (ItemInLeftHand != null)
            {
                ItemInLeftHand.GetComponent<ObjectDirector>().Drop(transform.parent);
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
    public void OnShipMove(InputValue value)
    {
        ShipMoveInput = value.Get<Vector3>();
    }
    public void OnShipLook(InputValue value)
    {
        ShipLookInput = value.Get<Vector2>();
    }
    public void OnExit(InputValue value)
    {
        ShipExit = value.isPressed;
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
    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent.TryGetComponent<DoorControl>(out DoorControl doorcontrol);
        if (doorcontrol != null)
        {
            doorcontrol.Open = true;
            return;
        }
        if (other.GetComponent<OxygenCube>() != null)
        {
            CurrentOxygenArea = other.GetComponent<OxygenCube>();
            return;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        other.transform.parent.TryGetComponent<DoorControl>(out DoorControl doorcontrol);
        if (doorcontrol != null)
        {
            doorcontrol.Close = true;
            return;
        }
        if (other.GetComponent<OxygenCube>() != null && other == CurrentOxygenArea)
        {
            CurrentOxygenArea = null;
            return;
        }
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(CameraFollow.transform.position, CameraFollow.transform.forward * 10f);
    }
}
