using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsHolder : MonoBehaviour
{
    [SerializeField] AudioClip[] soundEffects;
    // Start is called before the first frame update
    public AudioClip GetAudioClipAt(int i)
    {
        return soundEffects[i];
    }
}
