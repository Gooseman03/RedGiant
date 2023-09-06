using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.Examples.TMP_ExampleScript_01;

public class FuseController : ObjectDirector , IDurable
{
    private float _Durability;
    private float _MaxDurability;
    public float Durability
    {
        get { return _Durability; }
        set { _Durability = value; }
    }
    public float MaxDurability
    {
        get { return _MaxDurability; }
        set { _MaxDurability = value; }
    }
    public void ChangeDurability(float ammount)
    {
        Durability += ammount;
        if (Durability <= 0)
        {
            this.GetComponentInChildren<MeshRenderer>().material.color = Color.black;
            Durability = 0;
        }
    }
    public void SetDurability(float durability)
    {
        Durability = durability;
        if (Durability <= 0)
        {
            this.GetComponentInChildren<MeshRenderer>().material.color = Color.black;
            Durability = 0;
        }
    }
    public void SetMaxDurability(float durability)
    {
        MaxDurability = durability;
    }
    public float GetPercentDurability()
    {
        return Durability / MaxDurability;
    }
    public void ShockPlayer()
    {
        PlayerMessaging.ShockPlayer();
    }
}
