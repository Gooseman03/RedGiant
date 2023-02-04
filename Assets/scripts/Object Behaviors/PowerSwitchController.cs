using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerSwitchController : MonoBehaviour
{
    [SerializeField] private bool Activated = false;
    private ObjectReferences objectReferences;

    [SerializeField] private GameObject GreenButton;
    [SerializeField] private GameObject RedButton;

    private List<AudioClip> audioClips;
    private AudioHandler _audioHandler;
    public AudioHandler audioHandler
    {
        get { return _audioHandler; }
        private set { _audioHandler = value; }
    }
    private List<Material> materials;


    public void StartUp(ObjectReferences newReferences,  GameObject _redButton, GameObject _greenButton) 
    {
        GreenButton = _greenButton;
        RedButton = _redButton;


        objectReferences = newReferences;
        objectReferences.GetConstructorItemReferences(ObjectType.PowerSwitch, out List<Mesh> ListOfMeshs, out List<Material> _materials);
        materials = _materials;
        objectReferences.GetConstructorAudioReferences(ObjectType.PowerSwitch, out audioClips);

        audioHandler = this.AddComponent<AudioHandler>();
        audioHandler.Setup(audioClips, false);
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
            GreenButton.GetComponent<MeshRenderer>().material = materials[0];
            RedButton.GetComponent<MeshRenderer>().material = materials[3];
            GreenButton.transform.localPosition = new Vector3(0f, 0f, -.01f);
            RedButton.transform.localPosition = Vector3.zero;
        }
        else
        {
            GreenButton.GetComponent<MeshRenderer>().material = materials[1];
            RedButton.GetComponent<MeshRenderer>().material = materials[2];
            GreenButton.transform.localPosition = Vector3.zero; ;
            RedButton.transform.localPosition = new Vector3(0f, 0f, -.01f);
        }
    }
}
