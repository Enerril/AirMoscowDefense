using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESSTSHIT : MonoBehaviour
{

    [SerializeField] GameObject samolet;
    [SerializeField]Vector3 temp;

    [SerializeField] bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        
        if (isActive)
        {
            samolet.transform.position = temp;
        }

        

    }

}
