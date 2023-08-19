using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerMessaging
{
    private static PlayerController player;
    public static void ShockPlayer()
    {
        player.ShockPlayer();
    }

    public static void PlayerRegister(PlayerController playerController)
    {
        player = playerController;
    }
}
