using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{

    [SerializeField] public int droneLives=5;
    [SerializeField] GameObject player;
    [SerializeField] TextMeshProUGUI tmp_lives;
    PlaneControllerFinal planeControllerFinal;

    private void Awake()
    {
        StaticSceneData.InitValues(); Time.timeScale = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        planeControllerFinal = player.GetComponent<PlaneControllerFinal>();
        planeControllerFinal._OnPlayerDeath += UpdateLivesUI;
        tmp_lives.text = droneLives.ToString();
    }

    void UpdateLivesUI()
    {
        droneLives -= 1;

        // update UI. if 0 lives then go to the main menu. change player position i.e. we take control of another drone

        if (droneLives < 0)
        {
            Cursor.visible = true;
            SceneManager.LoadScene(0);
        }

        tmp_lives.text = droneLives.ToString();

        StartCoroutine(RevivePlayer());
    }

    IEnumerator RevivePlayer()
    {
        player.transform.position = this.transform.position + Random.insideUnitSphere * 200;
        player.transform.position = new Vector3(player.transform.position.x,100f,player.transform.position.z);
        yield return new WaitForSeconds(.5f);
        planeControllerFinal.OpenDeathScreen();

    }


    private void OnDestroy()
    {
        planeControllerFinal._OnPlayerDeath -= UpdateLivesUI;
    }

}
