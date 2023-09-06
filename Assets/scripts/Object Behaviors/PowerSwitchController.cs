using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSwitchController : ObjectDirector , IDurable, IInteract, IAudio
{
    private float _Durability;
    private float _MaxDurability;
    private AudioSource audioSource;
    private List<AudioClip> _audioClips;
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
    public List<AudioClip> audioClips
    {
        get { return _audioClips; }
        set { _audioClips = value; }
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
    [SerializeField] private List<Material> materials;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public bool GetState()
    {
        return Activated;
    }
    public void SetState(bool state)
    {
        if(audioSource != null) 
        {
            if (state) { audioSource.PlayOneShot(audioClips[0]); }
            if (!state) { audioSource.PlayOneShot(audioClips[1]); }
        }
        
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
    public void SwapSwitchState()
    {
        SetState(!GetState());
    }
    public void OnInteract(PlayerController playerController)
    {
        MenuRequester.AddMessageToConsole(this.name + " Has Been Interacted With");
        SwapSwitchState();
    }
}
