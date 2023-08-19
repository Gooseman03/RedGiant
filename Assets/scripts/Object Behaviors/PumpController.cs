using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpController : ObjectDirector , IDurable , IAudio
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
    private ObjectDirector objectDirector;
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
        if (!ErrorCodes.CheckWorking(objectDirector))
        {
            audioHandler.ChangeAudioPlaying(false);
        }
    }

}
