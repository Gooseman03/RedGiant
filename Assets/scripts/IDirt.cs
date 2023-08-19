using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDirt
{
    public float Dirt { get; set; }
    public float MaxDirt { get; set; }
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
}
