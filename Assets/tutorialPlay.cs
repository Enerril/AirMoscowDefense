using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialPlay : MonoBehaviour
{

    AudioSource audioSource;
    SoundsHolder soundsHolder;

    int counter;
    float trackLength;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        soundsHolder = GetComponent<SoundsHolder>();
        /*
        StartCoroutine(SequenceStart());

        IEnumerator SequenceStart()
        {
            yield return StartCoroutine(c1);
            yield return StartCoroutine(c2);
            yield return StartCoroutine(c3);
        }

        */

        StartCoroutine(SequenceStart());

    }

    IEnumerator SequenceStart()
    {

        yield return StartCoroutine(vstuplenie());
        yield return StartCoroutine(uprWS());
        yield return StartCoroutine(mish());
        yield return StartCoroutine(uprAD());
        yield return StartCoroutine(tormoz());
        yield return StartCoroutine(vysota());
        yield return StartCoroutine(lives());
        yield return StartCoroutine(pyloty());
        yield return StartCoroutine(kamikaza());
        yield return StartCoroutine(strelba());
       
    
    }

    IEnumerator vstuplenie()
    {
        audioSource.PlayOneShot(soundsHolder.GetAudioClipAt(counter));
        yield return new WaitForSeconds(soundsHolder.GetAudioClipAt(counter).length+1f);
        counter++;
    }
    IEnumerator uprWS()
    {
        audioSource.PlayOneShot(soundsHolder.GetAudioClipAt(counter));
        yield return new WaitForSeconds(soundsHolder.GetAudioClipAt(counter).length+1f);
        counter++;
    }
    IEnumerator mish()
    {
        audioSource.PlayOneShot(soundsHolder.GetAudioClipAt(counter));
        yield return new WaitForSeconds(soundsHolder.GetAudioClipAt(counter).length + 1f);
        counter++;
    }
    IEnumerator uprAD()
    {
        audioSource.PlayOneShot(soundsHolder.GetAudioClipAt(counter));
        yield return new WaitForSeconds(soundsHolder.GetAudioClipAt(counter).length + 1f);
        counter++;
    }
    IEnumerator tormoz()
    {
        audioSource.PlayOneShot(soundsHolder.GetAudioClipAt(counter));
        yield return new WaitForSeconds(soundsHolder.GetAudioClipAt(counter).length + 1f);
        counter++;
    }
    IEnumerator vysota()
    {
        audioSource.PlayOneShot(soundsHolder.GetAudioClipAt(counter));
        yield return new WaitForSeconds(soundsHolder.GetAudioClipAt(counter).length + 1f);
        counter++;
    }
    
    IEnumerator lives()
    {
        audioSource.PlayOneShot(soundsHolder.GetAudioClipAt(counter));
        yield return new WaitForSeconds(soundsHolder.GetAudioClipAt(counter).length + 1f);
        counter++;
    }
    IEnumerator pyloty()
    {
        audioSource.PlayOneShot(soundsHolder.GetAudioClipAt(counter));
        yield return new WaitForSeconds(soundsHolder.GetAudioClipAt(counter).length + 1f);
        counter++;
    }
    IEnumerator kamikaza()
    {
        audioSource.PlayOneShot(soundsHolder.GetAudioClipAt(counter));
        yield return new WaitForSeconds(soundsHolder.GetAudioClipAt(counter).length + 1f);
        counter++;
    }
    IEnumerator strelba()
    {
        audioSource.PlayOneShot(soundsHolder.GetAudioClipAt(counter));
        yield return new WaitForSeconds(soundsHolder.GetAudioClipAt(counter).length + 1f);
        counter++;
    }

}
