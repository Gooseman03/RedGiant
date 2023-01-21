using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSwitchController : MonoBehaviour
{
    [SerializeField] private bool Activated;



    [SerializeField] private Color ActivatedColor;
    [SerializeField] private Color DeactivatedColor;

    private void Start()
    {
        ActivatedColor = new Color(0f, 1f, 0f, 1f);
        DeactivatedColor = new Color(1f, 0f, 0f, 1f);
    }

    public bool GetState()
    {
        return Activated;
    }

    public void SetState(bool state)
    {
        Activated = state;
        if (Activated)
        {
            this.GetComponent<Renderer>().material.color = ActivatedColor;
        }
        else
        {
            this.GetComponent<Renderer>().material.color = DeactivatedColor;
        }
    }
}
