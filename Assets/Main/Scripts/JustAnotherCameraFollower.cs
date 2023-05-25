using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustAnotherCameraFollower : MonoBehaviour
{
    [SerializeField]Transform player;
    [SerializeField] Transform turret;
    [SerializeField] Transform turretCameraPosition;


    [SerializeField] Vector3 offset=new Vector3(1,1,1);
    [SerializeField] float distanceFromPlayer=1f;
    [SerializeField] float cameraSpeed=1f;
    [SerializeField] float cameraLookForwardOffset=1f;
    Vector3 curPos;
    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    [SerializeField] float MathfMult = 1f;
    float current, curTarget = 1;

    private Vector2 lookInput, screenCenter, mouseDistance;
    private float rollInput;
    [SerializeField]
    private float lookRateSpeed;
    void Start()
    {
        screenCenter.x = Screen.width * .5f;
        screenCenter.y = Screen.height * .5f;
    }

    private void FixedUpdate()
    {
        /*if (Input.GetButton("Fire1"))
        {

            lookInput.x = Input.mousePosition.x;
            lookInput.y = Input.mousePosition.y;

            mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.y;
            mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;

            mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);

            rollInput = Mathf.Lerp(rollInput, Input.GetAxisRaw("Roll"), lookRateSpeed * Time.deltaTime);

            turret.transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, mouseDistance.x * lookRateSpeed * Time.deltaTime, lookRateSpeed * Time.deltaTime, Space.Self);
            transform.position = curPos;








            /*
            curPos = turretCameraPosition.position;
            current = Mathf.MoveTowards(current, curTarget, cameraSpeed * Time.deltaTime);
            //transform.position = Vector3.SmoothDamp(transform.position, curPos, ref velocity, cameraSpeed,1000f, Time.smoothDeltaTime);
            transform.position = Vector3.Lerp(transform.position, curPos, current);
                //transform.position = curPos;

            transform.LookAt(turret);
            */



       // }

        //else
        //{
            transform.LookAt(player.transform.position + player.transform.forward * cameraLookForwardOffset);


            curPos = offset + player.transform.position - player.transform.forward * distanceFromPlayer + transform.forward * offset.x + transform.up * offset.y + transform.right * offset.z;

            //transform.position += +transform.forward * offset.x;
            //transform.position += +transform.right * offset.z;
            //transform.position += +transform.up * offset.y;
            current = Mathf.MoveTowards(current, curTarget, cameraSpeed * Time.deltaTime);
            //transform.position = Vector3.SmoothDamp(transform.position, curPos, ref velocity, cameraSpeed,1000f, Time.smoothDeltaTime);
            transform.position = Vector3.Lerp(transform.position, curPos, current);
        //}
       
        
    }
    // Update is called once per frame
    void LateUpdate()
    {

        current = 0;

    }
}
