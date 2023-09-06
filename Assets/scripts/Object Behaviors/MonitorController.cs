using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonitorController : ObjectDirector , IDurable, IDisconnect
{
    [SerializeField] private GameObject MonitorTextObject;
    [SerializeField] private TextMeshPro MonitorText;
    private string NewMonitorText;
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
        if (Durability < 0)
        {
            Durability = 0;
        }
    }
    public void SetDurability(float durability)
    {
        Durability = durability;
    }
    public void SetMaxDurability(float durability)
    {
        MaxDurability = durability;
    }
    public float GetPercentDurability()
    {
        return Durability / MaxDurability;
    }



    private float Timer;
    private void Update()
    {
        if (Timer > 1f)
        {
            MonitorText.text = NewMonitorText;
            Timer = 0;
        }
        Timer += Time.deltaTime;
    }
    public void Disconnect()
    {
        InstantChangeMonitorText("");
    }
    public GameObject GetMonitorTextGameobject()
    {
        return MonitorTextObject;
    }
    public string GetMonitorText()
    {
        MonitorText.enabled = true;
        return MonitorText.text;
    }
    public void InstantChangeMonitorText(string value)
    {
        MonitorText.enabled = true;
        MonitorText.text = value;
        NewMonitorText = value;
    }
    public void ChangeMonitorText(string NewText)
    {
        if (MonitorText == null) { return; }
        MonitorText.enabled = true;
        if (Durability < 60f)
        {
            char[] CharArrayText = NewText.ToCharArray();
            int i = 0;
            
            foreach(char Character in CharArrayText)
            { 
                if (CharArrayText[i] != '\n' && Random.Range(0, 100) < 100 - Durability)
                { 
                    CharArrayText[i] = '\u25A1'; 
                }
                i++;
            }
            NewText = CharArrayText.ArrayToString();
        }
        NewMonitorText = NewText;
    }
    public void ClearMonitorText()
    {
        InstantChangeMonitorText("");
    }
}