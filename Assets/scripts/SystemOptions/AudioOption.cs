using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOption : MonoBehaviour
{
    [SerializeField] private BaseSystem baseSystem;
    private void pumpPlayAudio(bool ChangeTo)
    {
        if (baseSystem.itemRegister.HasObject(ObjectType.Pump, out List<ObjectDirector> PumpList))
        {
            foreach (ObjectDirector Pump in PumpList)
            {
                if (ChangeTo == true && Pump.IsAudioPlaying() == false)
                {
                    Pump.playAudio();
                }
                else if (!ChangeTo)
                {
                    Pump.stopAudio();
                }
            }
        }
    }
}
