using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustAnotherCameraFollower : MonoBehaviour
{
    [SerializeField]Transform player;
    [SerializeField] Vector3 offset=new Vector3(1,1,1);
    [SerializeField] float distanceFromPlayer=1f;
    [SerializeField] float cameraSpeed=1f;
    [SerializeField] float cameraLookForwardOffset=1f;
    Vector3 curPos;
    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    [SerializeField] float MathfMult = 1f;
    float current, curTarget = 1;
    void Start()
    {
        
    }

    private void FixedUpdate()
    {

        transform.LookAt(player.transform.position+ player.transform.forward*cameraLookForwardOffset);


        curPos = offset + player.transform.position - player.transform.forward * distanceFromPlayer + transform.forward * offset.x + transform.up * offset.y + transform.right * offset.z;

        //transform.position += +transform.forward * offset.x;
        //transform.position += +transform.right * offset.z;
        //transform.position += +transform.up * offset.y;
        current=Mathf.MoveTowards(current, curTarget, cameraSpeed * Time.deltaTime);
        //transform.position = Vector3.SmoothDamp(transform.position, curPos, ref velocity, cameraSpeed,1000f, Time.smoothDeltaTime);
        transform.position = Vector3.Lerp(transform.position, curPos, current);
        
    }
    // Update is called once per frame
    void LateUpdate()
    {

        current = 0;

    }
}
