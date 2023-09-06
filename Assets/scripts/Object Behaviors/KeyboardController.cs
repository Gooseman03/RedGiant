using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : ObjectDirector , IInteract
{
    public void OnInteract(PlayerController playerController)
    {
        SendMessageUpwards("SystemInteract", playerController);
    }
}
