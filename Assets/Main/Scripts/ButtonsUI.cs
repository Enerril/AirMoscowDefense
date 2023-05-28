using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsUI : MonoBehaviour
{
   

    public void GoHangar()
    {
        SceneManager.LoadScene(1);
    }

    public void GoLevel1()
    {
        SoundController.Instance.StopMusic();
        SoundController.Instance.PlayMusic(Random.Range(1,3));
        SceneManager.LoadScene(2);
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
