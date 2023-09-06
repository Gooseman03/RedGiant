using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [SerializeField] private float speed = 5f;

    [SerializeField] private float ReachDistance = 1.0f;

    [SerializeField] private string DefaultActionMap;

    [SerializeField] private bool IsBeingShocked = false;
    [SerializeField] private float DeathTimer = 0;
    [SerializeField] private int FightDeathPresses = 20;
    [SerializeField] private int LastFightDeathPresses = 20;

    [SerializeField, Range(0f, 100f)] private float OxygenLevel;
    [SerializeField, Range(0f, 100f)] private float CarbonLevel;
    [SerializeField] public OxygenCube CurrentOxygenArea;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float gravity;

    private CharacterController controller;
    [SerializeField] private GameObject _cameraFollow;
    [SerializeField] private Transform objectLeftGrabPointTransform;
    [SerializeField] private GameObject ItemInLeftHand;
    [SerializeField] private Transform objectRightGrabPointTransform;
    [SerializeField] private GameObject ItemInRightHand;

    [SerializeField] private UiUpdate UiScript;

    [SerializeField] private AudioSource Audio;
    [SerializeField] private AudioSource ElectrocutionAudioSource;

    private Vector3 movement = Vector3.zero;
    public GameObject CameraFollow { get => _cameraFollow; private set => _cameraFollow = value; }
    private void Start()
    {
        PlayerMessaging.PlayerRegister(this);
        controller = GetComponent<CharacterController>();
    }
    public void ShockPlayer()
    {
        IsBeingShocked = true;
    }
    private void Update()
    {
        if (MenuRequester.IsConsoleOpen())
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        { 
            SendMessageUpwards("ShipMove", PlayerInputs.ShipMoveInput);
            SendMessageUpwards("ShipLook", PlayerInputs.ShipLookInput);
            if (PlayerInputs.ShipExit)
            {
                this.transform.parent.BroadcastMessage("ShipExit",playerController,SendMessageOptions.DontRequireReceiver);
                PlayerInputs.ShipExit = false;
            }
            if (!IsBeingShocked)
            {
                MovePlayer();
                Pickup();
                Interact();
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Shocked();
            }
        }
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
            PlayerInputs.PlayerInput.SwitchCurrentActionMap(DefaultActionMap);
            return;
        }
        PlayerInputs.PlayerInput.SwitchCurrentActionMap(inputActionMap);
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
        if (PlayerInputs.InteractInput)
        {
            FightDeathPresses -= 1;
            PlayerInputs.InteractInput = false;
        }
        if (FightDeathPresses <= 0)
        {
            UiScript.EscapeShock();
            IsBeingShocked = false;
            DeathTimer = 0;
            FightDeathPresses = LastFightDeathPresses * 2;
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
            OxygenLevel = Mathf.Lerp(OxygenLevel, 0f, .5f * Time.deltaTime);
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
        if (CarbonLevel >= 99.9f)
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
        if (PlayerInputs.InteractInput)
        {
            RaycastHit hit;
            if (Physics.Raycast(CameraFollow.transform.position, CameraFollow.transform.forward, out hit, ReachDistance))
            {
                hit.transform.gameObject.SendMessage("OnInteract", this, SendMessageOptions.DontRequireReceiver);
            }
        }
        PlayerInputs.InteractInput = false;
    }

    private void OnGUI()
    {
        if (IsBeingShocked)
        {
            UiScript.ChangeShockStates(DeathTimer, FightDeathPresses);
        }
        UiScript.ChangeColor(new Color(0f, 0f, 0f, (CarbonLevel / 100)), new Color(0f, 0f, 0f, ((-OxygenLevel + 3) / 100) + .97f));
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
        if (CurrentOxygenArea == null)
        {
            movement += PlayerInputs.MoveInput.x * transform.right * speed * Time.deltaTime;
            movement += PlayerInputs.MoveInput.y * transform.up * speed * Time.deltaTime;
            movement += PlayerInputs.MoveInput.z * transform.forward * speed * Time.deltaTime;
            controller.Move(movement * Time.deltaTime);
            return;
        }

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
            movement.y = movement.y + (-gravity * Time.deltaTime);
        }
        controller.Move(movement * Time.deltaTime);
    }
    //private void LookPlayer()
    //{
    //    if (CurrentOxygenArea == null)
    //    {
    //        RotationX -= PlayerInputs.LookInput.y * .2f;
    //        RotationY += PlayerInputs.LookInput.x * .2f;
    //        RotationX = Mathf.Clamp(RotationX, LowerLimit, UpperLimit);
    //        transform.localEulerAngles = new Vector3(RotationX, RotationY, 0);
    //    }
    //    else if (PlayerInputs.LookInput != Vector2.zero)
    //    {
    //        RotationX -= PlayerInputs.LookInput.y * .2f;
    //        RotationY += PlayerInputs.LookInput.x * .2f;
    //        RotationX = Mathf.Clamp(RotationX, LowerLimit, UpperLimit);
    //        CameraFollow.transform.eulerAngles = new Vector3(RotationX, RotationY, 0);
    //        transform.localEulerAngles = new Vector3(0, RotationY, 0);
    //    }
    //}
    private void Pickup()
    {
        if (PlayerInputs.PickupInput)
        {
            RaycastHit hit;
            
            if (Physics.Raycast(CameraFollow.transform.position, CameraFollow.transform.forward, out hit, ReachDistance))
            {
                if (
                    hit.collider.transform.TryGetComponent(out ObjectPlace objectPlace)
                    && ItemInLeftHand != null
                    && (objectPlace.objectType == ItemInLeftHand.GetComponent<ObjectDirector>().objectType || objectPlace.objectType == ObjectType.Generic))
                {
                    ItemInLeftHand.GetComponent<IGrabbable>().Place(objectPlace.transform);
                    ItemInLeftHand = null;
                    PlayerInputs.PickupInput = false;
                    return;
                }
                else if (hit.collider.transform.TryGetComponent(out IGrabbable objectGrabbable) && ItemInLeftHand == null)
                {
                    ItemInLeftHand = hit.collider.transform.gameObject;
                    objectGrabbable.Grab(objectLeftGrabPointTransform);
                    PlayerInputs.PickupInput = false;
                    return;
                }
                else if (ItemInLeftHand != null)
                {
                    ItemInLeftHand.GetComponent<IGrabbable>().Drop(transform.parent);
                    ItemInLeftHand = null;
                    PlayerInputs.PickupInput = false;
                    return;
                }
            }
            else if (ItemInLeftHand != null)
            {
                ItemInLeftHand.GetComponent<IGrabbable>().Drop(transform.parent);
                ItemInLeftHand = null;
                PlayerInputs.PickupInput = false;
                return;
            }
        }
        PlayerInputs.PickupInput = false;
    }
    /// <summary>
    /// This is to grab The current Oxygen Cube
    /// and open doors
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<DoorControl>(out DoorControl doorcontrol) == true)
        {
            doorcontrol.Open = true;
            return;
        }
        if (other.TryGetComponent<OxygenCube>(out OxygenCube oxygenCube) == true)
        {
            CurrentOxygenArea = oxygenCube;
            return;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<OxygenCube>(out OxygenCube oxygenCube) == true)
        {
            CurrentOxygenArea = oxygenCube;
            return;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<DoorControl>(out DoorControl doorcontrol) == true)
        {
            doorcontrol.Close = true;
            return;
        }
        if (other.TryGetComponent<OxygenCube>(out OxygenCube oxygenCube) == true && oxygenCube == CurrentOxygenArea)
        {
            CurrentOxygenArea = null;
            return;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(CameraFollow.transform.position, CameraFollow.transform.forward * ReachDistance);
    }
}
