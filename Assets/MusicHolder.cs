using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHolder : MonoBehaviour
{
    [SerializeField] AudioClip[] soundtracks;
    // Start is called before the first frame update
    public AudioClip GetAudioClipAt(int i)
    {
        return soundtracks[i];
    }
}
