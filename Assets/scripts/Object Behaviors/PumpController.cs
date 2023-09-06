using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpController : ObjectDirector, IDurable, IAudio, IGrabbable
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

    private List<AudioClip> _audioClips;
    private AudioSource _audioSource;
    public List<AudioClip> audioClips
    {
        get { return _audioClips; }
        set { _audioClips = value; }
    }
    public AudioSource audioSource
    {
        get { return _audioSource; }
        set { _audioSource = value; }
    }

    private bool _IsAudioPlaying;
    private bool HoldRequests;
    private bool RequestStart;
    private bool RequestStop;

    public bool IsAudioPlaying
    {
        get { return _IsAudioPlaying; }
        private set { _IsAudioPlaying = value; }
    }
    public void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (!ErrorCodes.CheckWorking(this))
        {
            ChangeAudioPlaying(false);
        }
        if (RequestStart == true)
        {
            IsAudioPlaying = true;
            RequestStart = false;
            audioSource.PlayOneShot(audioClips[0]);
            MenuRequester.AddMessageToConsole(this.name + " Is Playing " + audioClips[0]);
            HoldRequests = true;
            return;
        }
        if (!audioSource.isPlaying && HoldRequests)
        {
            audioSource.loop = true;
            audioSource.clip = audioClips[1];
            MenuRequester.AddMessageToConsole(this.name + " Is Playing " + audioClips[1]);
            audioSource.Play();
            HoldRequests = false;
            return;
        }
        if (RequestStop)
        {
            IsAudioPlaying = false;
            RequestStop = false;
            audioSource.Stop();
            audioSource.PlayOneShot(audioClips[2]);
            MenuRequester.AddMessageToConsole(this.name + " Is Playing " + audioClips[2]);
            return;
        }
    }
    public void ChangeAudioPlaying(bool value)
    {
        if (HoldRequests) { return; }
        if (value == true && !IsAudioPlaying)
        {
            MenuRequester.AddMessageToConsole(this.name + "Was Requested To Start Playing");
            RequestStart = true;
        }
        else
        if (value == false && IsAudioPlaying)
        {
            MenuRequester.AddMessageToConsole(this.name + "Was Requested To Stop Playing");
            RequestStop = true;
        }
    }
    public new void Grab(Transform objectGrabPointTransform)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.layer = 6;
        foreach (Transform child in this.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = 6;
        }
        transform.position = objectGrabPointTransform.position;
        transform.SetParent(objectGrabPointTransform);
        ChangeAudioPlaying(false);
    }
}