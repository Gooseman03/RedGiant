using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiUpdate : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private Image image;
    [SerializeField] private Image DeathImage;
    [SerializeField] private Image FightImage;
    [SerializeField] private Image DeathBackGround;
    [SerializeField] private float SecondsForDeath;
    [SerializeField] private float MaxFightPresses;
    private void Start()
    {
        DeathImage.enabled = false;
        FightImage.enabled = false;
        DeathBackGround.enabled = false;
    }
    void Update()
    {
        image.color = color;
    }
    public void StartShocking(float seconds, float FightPresses)
    {
        SecondsForDeath = seconds;
        MaxFightPresses = FightPresses;
        DeathBackGround.enabled = true;
        DeathImage.enabled = true;
        FightImage.enabled = true;
    }
    public void ChangeShockStates(float TimeForDeath, float FightPresses)
    {
        FightImage.rectTransform.sizeDelta = new Vector2(100, ((MaxFightPresses - FightPresses)/MaxFightPresses)*200);
        DeathImage.rectTransform.sizeDelta = new Vector2(100, (TimeForDeath / SecondsForDeath) * 200);
    }
    public void EscapeShock()
    {
        DeathBackGround.enabled = false;
        DeathImage.enabled = false;
        FightImage.enabled = false;
    }
    public void ChangeColor(Color CarbonColor, Color OxygenColor) 
    {
        if (CarbonColor.a > OxygenColor.a)
        {
            color = CarbonColor;
        }
        else
        {
            color = OxygenColor;
        }
    }
}
