using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOption : MonoBehaviour
{
    private BaseSystem baseSystem;
    private void Start()
    {
        baseSystem = GetComponent<BaseSystem>();
    }
    private void pumpPlayAudio(bool ChangeTo)
    {
        if (baseSystem.itemRegister.HasObject<PumpController>(out List<PumpController> PumpList))
        {
            foreach (PumpController Pump in PumpList)
            {
                if (Pump == null)
                {
                    return;
                }
                if (ChangeTo == true && Pump.IsAudioPlaying == false)
                {
                    Pump.ChangeAudioPlaying(true);
                }
                else if (!ChangeTo)
                {
                    Pump.ChangeAudioPlaying(false);
                }
            }
        }
    }
}
