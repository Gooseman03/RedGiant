using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpController : MonoBehaviour
{
    private List<AudioClip> audioClips;
    private AudioHandler _audioHandler;
    private ObjectDirector objectDirector;
    public AudioHandler audioHandler
    {
        get { return _audioHandler; }
        private set { _audioHandler = value; }
    }
    public void StartUp(ObjectReferences objectReferences)
    {
        objectDirector = this.GetComponent<ObjectDirector>();
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
