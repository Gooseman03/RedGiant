using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class AirFilterController : ObjectDirector , IDirt, IInteract, IGrabbable
{
    [SerializeField] private float _Dirt = 0;
    [SerializeField]private float _MaxDirt = 100;
    public float Dirt
    {
        get { return _Dirt; }
        set { _Dirt = value; }
    }
    public float MaxDirt
    {
        get { return _MaxDirt; }
        set { _MaxDirt = value; }
    }
    public void ChangeDirt(float ammount)
    {
        Dirt += ammount;
        if (Dirt < 0)
        {
            Dirt = 0;
        }
    }
    public void SetDirt(float dirt)
    {
        Dirt = dirt;
    }
    public void SetMaxDirt(float dirt)
    {
        MaxDirt = dirt;
    }
    public float GetPercentDirt()
    {
        return Dirt / MaxDirt;
    }

    public void Start()
    {

    }

    public void OnInteract(PlayerController playerController)
    {
        MenuRequester.AddMessageToConsole(this.name + " Has Been Interacted With");
        Dirt = 0;
    }
}
