using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinCamSwticher : MonoBehaviour
{


    [SerializeField] CinemachineVirtualCamera Vcam;
    [SerializeField] CinemachineFreeLook Fcam;

    bool camSwitched;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.V))
        {
            if (!camSwitched)
            {
                Vcam.Priority = 0;
                Fcam.Priority = 1;
                camSwitched = true;
            }

            else if (camSwitched)
            {
                Vcam.Priority = 1;
                Fcam.Priority = 0;
                camSwitched = false;
            }
        }

    }
}
