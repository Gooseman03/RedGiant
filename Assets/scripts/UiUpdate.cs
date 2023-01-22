using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class UiUpdate : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private Image image;
    void Update()
    {
        image.color = color;
    }
    public void ChangeColor(Color newColor) 
    {
        color = newColor;
    }
}
