using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudio
{
    public AudioHandler audioHandler { set; get; }
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
}
