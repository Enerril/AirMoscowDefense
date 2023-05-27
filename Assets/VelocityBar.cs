using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VelocityBar : MonoBehaviour
{

    [SerializeField] Image positiveVelocityBar;
    [SerializeField] Image negativeVelocityBar;
    // Start is called before the first frame update
    [SerializeField] GameObject player;
    PlaneControllerFinal pc;


    void Start()
    {
       // pc = player.GetComponent<PlaneControllerFinal>();
    }

    // Update is called once per frame
   // void Update()
    //{
   //     
   // }


    public void UpdateVelocityBar(float throttle,float maxThrust)
    {
        if (throttle >= 0)
        {
            negativeVelocityBar.fillAmount = 0;
            positiveVelocityBar.fillAmount = throttle/maxThrust;
        }
        else
        {
            positiveVelocityBar.fillAmount = 0;
            negativeVelocityBar.fillAmount = Mathf.Abs(throttle / maxThrust);
        }
    }
}
