using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class AlarmController : ObjectDirector , IDurable, IInteract, IAudio
{
    private float _Durability;
    private float _MaxDurability;
    private AudioHandler _audioHandler;
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
    public AudioHandler audioHandler
    {
        get { return _audioHandler; }
        set { _audioHandler = value; }
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
    public void playAudio()
    {
        audioHandler.ChangeAudioPlaying(true);
    }
    public void stopAudio()
    {
        audioHandler.ChangeAudioPlaying(false);
    }
    public bool IsAudioPlaying()
    {
        return audioHandler.IsAudioPlaying;
    }
    private List<AudioClip> audioClips;
    private AudioSource audioSource;

    // bool is acknownlegdement
    public List<ErrorTypes> ErrorActive = new List<ErrorTypes>();
    public Dictionary<ErrorTypes, bool> ErrorsAcknownlegdement = new Dictionary<ErrorTypes, bool>();

    public void Start()
    {
        audioSource = this.gameObject.AddComponent<AudioSource>();
        objectReferences.GetConstructorAudioReferences(ObjectType.Alarm, out audioClips);
        audioSource.loop = true;
        audioSource.clip = audioClips[0];
        audioSource.volume = .5f;
        audioSource.spatialBlend = 1;
    }
    public void UpdateErrorTypes(List<ErrorTypes> ErrorsInput)
    {
        ErrorActive.Clear();
        foreach (ErrorTypes error in ErrorsInput)
        {
            ErrorActive.Add(error);
            if (ErrorsAcknownlegdement.ContainsKey(error) && ErrorsAcknownlegdement[error])
            {
                
            }
            else
            {
                ErrorsAcknownlegdement.TryAdd(error, false);
            }
        }
        for (int i = 0; i < ErrorsAcknownlegdement.Count; i++)
        {
            ErrorTypes errorType = ErrorsAcknownlegdement.ElementAt(i).Key;
            if (!ErrorsInput.Contains(errorType))
            {
                ErrorsAcknownlegdement[errorType] = false;
            }
        }
    }
    public void CancelAlarm()
    {
        for (int i = 0; i < ErrorsAcknownlegdement.Count; i++)
        {
            ErrorsAcknownlegdement[ErrorsAcknownlegdement.ElementAt(i).Key] = true;
        }
    }
    public void AlarmUpdate()
    {
        if (!ErrorsAcknownlegdement.ContainsValue(false) || ErrorActive.Count()==0)
        {
            audioSource.loop = false;
            return;
        }
        if (audioSource.isPlaying)
        {
            return;
        }
        audioSource.loop = true;
        audioSource.Play();
    }
    public void OnInteract(PlayerController playerController)
    {
        MenuRequester.AddMessageToConsole(this.name + " Has Been Interacted With");
        CancelAlarm();
    }
}
