using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudio
{
    public List<AudioClip> audioClips { get; set; }
    //public void playAudio()
    //{
    //    audioHandler.ChangeAudioPlaying(true);
    //}
    //public void stopAudio()
    //{
    //    audioHandler.ChangeAudioPlaying(false);
    //}
    //public bool IsAudioPlaying()
    //{
    //    return audioHandler.IsAudioPlaying;
    //}
}
