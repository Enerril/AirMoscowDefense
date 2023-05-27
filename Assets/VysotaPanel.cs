using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void PlayerTooLow();
public class VysotaPanel : MonoBehaviour
{
    [SerializeField] Image vysotaPanel;
    [SerializeField] Transform player;

    [SerializeField] float tooLowGround = 150f;
    [SerializeField] float tooHighGround = 350f;
    public PlayerTooLow _OnTooLow;
    Color color;


    // Start is called before the first frame update
    void Start()
    {
        vysotaPanel=GetComponent<Image>();
        color = vysotaPanel.color;
    }

    // Update is called once per frame
    void Update()
    {

        


        if (player.transform.position.y<tooLowGround || player.transform.position.y > tooHighGround)
        {
            _OnTooLow?.Invoke();
        }
    }

    public void UpdateVysotaPanel()
    {
        if(player.transform.position.y< tooLowGround+50)
        {
            vysotaPanel.color=Color.red;
        }
        if(player.transform.position.y>= tooLowGround + 51 && player.transform.position.y < tooHighGround - 51)
        {
            vysotaPanel.color = color;
        }
        if (player.transform.position.y > tooHighGround-50)
        {
            vysotaPanel.color = Color.red;
        }

        vysotaPanel.fillAmount = player.transform.position.y / 350f;
    }



}
