using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    private static Singleton instance;
    public static Singleton GetInstance()
    {
        if (!instance)
        {
            instance = (Singleton)FindObjectOfType(typeof(Singleton));
            if (!instance)
            {

                GameObject obj = new GameObject("GameManagers");
                instance = obj.AddComponent(typeof(Singleton)) as Singleton;
            }
        }

        return instance;
    }


}
