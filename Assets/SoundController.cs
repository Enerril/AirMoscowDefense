using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    // Start is called before the first frame update
    public static SoundController Instance;
    [SerializeField] public GameObject gMusicSource, gSoundSource;


    private AudioSource MusicSource, SoundSource;
    MusicHolder musicHolder;
    SoundsHolder soundsHolder;

    WaitForSeconds soundTrackTime;

    void Awake()
    {

       

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


        
    }

    private void Start()
    {
        MusicSource = gMusicSource.GetComponent<AudioSource>();
        SoundSource = gSoundSource.GetComponent<AudioSource>();
        musicHolder = gMusicSource.GetComponent<MusicHolder>();
        soundsHolder = gSoundSource.GetComponent<SoundsHolder>();
        PlayMusic(0);
    }

    public void PlaySound(int i)
    {
        SoundSource.PlayOneShot(soundsHolder.GetAudioClipAt(i));

    }

    public void PlayMusic(int i)
    {
        MusicSource.PlayOneShot(musicHolder.GetAudioClipAt(i));
        var f = musicHolder.GetAudioClipAt(i).length;
        soundTrackTime = new WaitForSeconds(f);
        StartCoroutine(PlayRandomMusicTrack());
    }

    public void StopMusic()
    {
        StopAllCoroutines();
        MusicSource.Stop();
    }

    public void StopMusic2()
    {
       
        MusicSource.Stop();
    }

    public void StopSound()
    {
        SoundSource.Stop();
    }
    IEnumerator PlayRandomMusicTrack()
    {
        yield return soundTrackTime;
        StopMusic2();
        PlayMusic(Random.Range(1, 3));


    }
}
