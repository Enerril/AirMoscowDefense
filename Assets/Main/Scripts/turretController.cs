using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretController : MonoBehaviour
{
    Camera cam;
    [SerializeField]Transform myTurret;
    // Start is called before the first frame update
    void Start()
    {
        cam=Camera.main;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButton("Fire1"))
        {
            // myTurret.transform.Rotate(Vector3.up, cam.transform.rotation.y, Space.Self);
            //Get the Screen positions of the object
            Vector3 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

            //Get the Screen position of the mouse
            Vector3 mouseOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            //Get the angle between the points
            float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

            //Ta Daaa
            transform.rotation = Quaternion.Euler(new Vector3(0f, angle, 0f));


            //Cursor.lockState = CursorLockMode.Locked;
            


        }
    }
    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    private void LateUpdate()
    {

        //Cursor.lockState = CursorLockMode.None;
    }

}
