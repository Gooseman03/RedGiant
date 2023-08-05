using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    private AudioSource _audioSource;
    private List<AudioClip> _audioClips;

    private bool _canLoop;
    private bool SetupComplete = false;

    public bool canLoop
    {
        get { return _canLoop; }
        private set { _canLoop = value; }
    }
    public AudioSource audioSource
    {
        get { return _audioSource; }
        set { _audioSource = value; }
    }
    public List<AudioClip> audioClips
    {
        get { return _audioClips; }
        set { _audioClips = value; }
    }
    private AudioClip TempAudioClip;

    private bool _IsAudioPlaying;
    private bool HoldRequests;
    private bool RequestStart;
    private bool RequestStop;
    

    public bool IsAudioPlaying
    {
        get { return _IsAudioPlaying; }
        private set { _IsAudioPlaying = value; }
    }

    public void ChangeAudioPlaying(bool value)
    {
        if(HoldRequests) { return; }
        if (value == true && !IsAudioPlaying)
        {
            Debug.Log(this.name + "Was Requested To Start Playing");
            RequestStart = true;
        }
        else
        if (value == false && IsAudioPlaying)
        {
            Debug.Log(this.name + "Was Requested To Stop Playing");
            RequestStop = true;
        }
    }
    public void ChangeAudioPlaying(bool value, AudioClip _tempAudioClip)
    {
        TempAudioClip = _tempAudioClip;
        if (HoldRequests) { return; }
        if (value == true && !IsAudioPlaying)
        {
            
            RequestStart = true;
        }
        else
        if (value == false && IsAudioPlaying)
        {
            
            RequestStop = true;
        }
    }

    public void Setup(List<AudioClip> audioClipsIn, bool canLoopIn)
    {
        canLoop = canLoopIn;
        audioClips = audioClipsIn;
        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1;
        audioSource.dopplerLevel = 0;
        SetupComplete = true;
    }

    private void Update()
    {
        if (!SetupComplete) { return; }
        if (canLoop == false)
        {
            if (RequestStart == true)
            {
                IsAudioPlaying = true;
                RequestStart = false;
                if (TempAudioClip != null)
                {
                    audioSource.PlayOneShot(TempAudioClip);
                    Debug.Log(this.name + "Is Playing" + TempAudioClip);
                    TempAudioClip = null;
                }
                else
                {
                    audioSource.PlayOneShot(audioClips[0]);
                    Debug.Log(this.name + "Is Playing" + audioClips[0]);
                }
                HoldRequests = true;
                return;
            }
            if (!audioSource.isPlaying && HoldRequests)
            { 
                IsAudioPlaying = false;
                HoldRequests = false;
                return;
            }
        }
        if (canLoop == true)
        {
            if (RequestStart == true)
            {
                IsAudioPlaying = true;
                RequestStart = false;
                audioSource.PlayOneShot(audioClips[0]);
                Debug.Log(this.name + "Is Playing" + audioClips[0]);
                HoldRequests = true;
                return;
            }
            if (!audioSource.isPlaying && HoldRequests)
            {
                audioSource.loop = true;
                audioSource.clip = audioClips[1];
                Debug.Log(this.name + "Is Playing" + audioClips[1]);
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
                Debug.Log(this.name + "Is Playing" + audioClips[2]);
                return;
            }
        }
    }
}
