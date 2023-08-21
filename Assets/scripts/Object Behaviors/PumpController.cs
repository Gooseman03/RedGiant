using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpController : ObjectDirector , IDurable , IAudio , IGrabbable
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
    private List<AudioClip> audioClips;
    private AudioHandler _audioHandler;
    public AudioHandler audioHandler
    {
        get { return _audioHandler; }
        set { _audioHandler = value; }
    }
    public void playAudio()
    {
        if (audioHandler == null) { return; }
        audioHandler.ChangeAudioPlaying(true);
    }
    public void stopAudio()
    {
        if (audioHandler == null) { return; }
        audioHandler.ChangeAudioPlaying(false);
    }
    public bool IsAudioPlaying()
    {
        return audioHandler.IsAudioPlaying;
    }

    public void Start()
    {
        objectReferences.GetConstructorAudioReferences(ObjectType.Pump, out audioClips);
        audioHandler = this.gameObject.AddComponent<AudioHandler>();
        audioHandler.Setup(audioClips, true);
    }
    private void Update()
    {
        if (!ErrorCodes.CheckWorking(this))
        {
            audioHandler.ChangeAudioPlaying(false);
        }
    }
    public bool Grab(Transform objectGrabPointTransform)
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
        return true;
    }
    public void Place(Transform objectPlacePointTransform)
    {
        transform.SetParent(objectPlacePointTransform);
        GetComponent<Rigidbody>().isKinematic = true;
        gameObject.layer = 0;
        foreach (Transform child in this.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = 0;
        }
        transform.position = objectPlacePointTransform.position;
        transform.rotation = objectPlacePointTransform.rotation;
        gameObject.GetComponent<Collider>().enabled = true;
    }
    public void Drop(Transform NewParent)
    {
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.gameObject.layer = 0;
        foreach (Transform child in this.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = 0;
        }
        this.transform.SetParent(NewParent);
        this.gameObject.GetComponent<Collider>().enabled = true;
    }
}
