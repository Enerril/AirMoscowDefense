using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticSceneData
{
    public static GameObject player;

    public static void InitValues()
    {
        player = GameObject.FindWithTag("MainPlayer");
    }
   



}
