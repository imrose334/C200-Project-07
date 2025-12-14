using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicKeep : MonoBehaviour
{
    private static MusicKeep instance; // store the singleton instance

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // prevent duplicates
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // keep this object across scenes
    }
}
