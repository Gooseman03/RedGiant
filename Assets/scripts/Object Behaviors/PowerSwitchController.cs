using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSwitchController : ObjectDirector , IDurable, IInteract
{
    private float _Durability;
    private float _MaxDurability;
    public float Durability
    {
        get { return _Durability; }
        set { _Durability = value; }
    }
    public float MaxDurability
    {
        get { return _MaxDurability; }
        set { _MaxDurability = value; }
    }
    public void ChangeDurability(float ammount)
    {
        Durability += ammount;
        if (Durability < 0)
        {
            Durability = 0;
        }
    }
    public void SetDurability(float durability)
    {
        Durability = durability;
    }
    public void SetMaxDurability(float durability)
    {
        MaxDurability = durability;
    }
    public float GetPercentDurability()
    {
        return Durability / MaxDurability;
    }

    [SerializeField] private bool Activated = false;

    [SerializeField] private GameObject GreenButton;
    [SerializeField] private GameObject RedButton;
    private List<AudioClip> audioClips;
    private AudioHandler _audioHandler;
    public AudioHandler audioHandler
    {
        get { return _audioHandler; }
        private set { _audioHandler = value; }
    }
    private List<Material> materials;
    public bool GetState()
    {
        return Activated;
    }
    public void Start()
    {
        GreenButton = ConstructedGameObjects[1];
        RedButton = ConstructedGameObjects[2];
        objectReferences.GetConstructorItemReferences(ObjectType.PowerSwitch, out List<Mesh> ListOfMeshs, out List<Material> _materials);
        materials = _materials;
        objectReferences.GetConstructorAudioReferences(ObjectType.PowerSwitch, out audioClips);

        audioHandler = this.gameObject.AddComponent<AudioHandler>();
        audioHandler.Setup(audioClips, false);
    }
    public void SetState(bool state)
    {
        //if (state) { audioHandler.ChangeAudioPlaying(true, audioClips[0]); }
        //if (!state) { audioHandler.ChangeAudioPlaying(true, audioClips[1]); }
        if (Durability < 60 && Random.Range(0, 100) > Durability)
        {
            return;
        }
        Activated = state;
        if (Activated)
        {
            GreenButton.GetComponent<MeshRenderer>().material = materials[0];
            RedButton.GetComponent<MeshRenderer>().material = materials[3];
            GreenButton.transform.localPosition = new Vector3(0f, 0f, -.01f);
            RedButton.transform.localPosition = Vector3.zero;
        }
        else
        {
            GreenButton.GetComponent<MeshRenderer>().material = materials[1];
            RedButton.GetComponent<MeshRenderer>().material = materials[2];
            GreenButton.transform.localPosition = Vector3.zero; ;
            RedButton.transform.localPosition = new Vector3(0f, 0f, -.01f);
        }

    }
    public bool GetSwitchState()
    {
        return GetState();
    }
    public void ChangeSwitchState(bool newState)
    {
        SetState(newState);
    }
    public void SwapSwitchState()
    {
        SetState(!GetState());
    }
    public void OnInteract(PlayerController playerController)
    {
        MenuRequester.AddMessageToConsole(this.name + " Has Been Interacted With");
        if (objectType == ObjectType.PowerSwitch)
        {
            SwapSwitchState();
        }
    }
}
