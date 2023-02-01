using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerSwitchController : MonoBehaviour
{
    [SerializeField] private bool Activated = false;
    private ObjectReferences objectReferences;

    private List<Color> colorList;

    [SerializeField] private GameObject GreenButton;
    [SerializeField] private GameObject RedButton;

    private List<AudioClip> audioClips;
    private AudioHandler _audioHandler;
    public AudioHandler audioHandler
    {
        get { return _audioHandler; }
        private set { _audioHandler = value; }
    }


    public void StartUp(ObjectReferences newReferences,  GameObject _redButton, GameObject _greenButton) 
    {
        GreenButton = _greenButton;
        RedButton = _redButton;


        objectReferences = newReferences;
        objectReferences.GetConstructorItemReferences(ObjectType.PowerSwitch, out List<Mesh> ListOfMeshs, out Material material, out List<Color> ListOfColors);
        objectReferences.GetConstructorAudioReferences(ObjectType.PowerSwitch, out audioClips);

        audioHandler = this.AddComponent<AudioHandler>();
        audioHandler.Setup(audioClips, false);

        colorList = ListOfColors;
    }

    public bool GetState()
    {
        return Activated;
    }

    public void SetState(bool state)
    {
        if (state) { audioHandler.ChangeAudioPlaying(true, audioClips[0]); }
        if (!state) { audioHandler.ChangeAudioPlaying(true, audioClips[1]); }
        float durability = (float) this.GetComponent<ObjectDirector>().Durability;
        if (durability < 60 && Random.Range(0, 100) > durability)
        {
            return;
        }
        Activated = state;
        if (Activated)
        {
            GreenButton.GetComponent<MeshRenderer>().material.color = colorList[0];
            RedButton.GetComponent<MeshRenderer>().material.color = colorList[3];
            GreenButton.transform.localPosition = new Vector3(0f, 0f, -.01f);
            RedButton.transform.localPosition = Vector3.zero;
        }
        else
        {
            GreenButton.GetComponent<MeshRenderer>().material.color = colorList[1];
            RedButton.GetComponent<MeshRenderer>().material.color = colorList[2];
            GreenButton.transform.localPosition = Vector3.zero; ;
            RedButton.transform.localPosition = new Vector3(0f, 0f, -.01f);
        }
    }
}
