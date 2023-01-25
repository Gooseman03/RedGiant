using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PumpController : MonoBehaviour
{
    private List<AudioClip> audioClips;
    private AudioHandler _audioHandler;
    private ObjectGrabbable objectGrabbable;
    public AudioHandler audioHandler
    {
        get { return _audioHandler; }
        private set { _audioHandler = value; }
    }
    public void StartUp( ObjectReferences objectReferences)
    {
        objectGrabbable = this.GetComponent<ObjectGrabbable>();
        objectReferences.GetConstructorAudioReferences(ObjectType.Pump, out audioClips);
        audioHandler = this.AddComponent<AudioHandler>();
        audioHandler.Setup(audioClips, true);
    }
    private void Update()
    {
        if (!ErrorCodes.CheckWorking(objectGrabbable))
        {
            audioHandler.ChangeAudioPlaying(false);
        }
    }
}
