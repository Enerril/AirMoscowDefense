using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class CameraController : MonoBehaviour
{
    [SerializeField] Transform[] povs;
    [SerializeField] float speed;
    [SerializeField] float mult;
    private int index = 1;
    private Vector3 target;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) index = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) index = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) index = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4)) index = 3;

        target = povs[index].position;

        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed * mult);
        transform.forward = povs[index].forward;
    }

    private void FixedUpdate()
    {
     
    }
}
*/

/*
public class SmoothCameraFollow : MonoBehaviour
{
    #region Variables

    private Vector3 _offset;
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime;
    private Vector3 _currentVelocity = Vector3.zero;

    #endregion

    #region Unity callbacks

    private void Awake() => _offset = transform.position - target.position;

    private void LateUpdate()
    {
        Vector3 targetPosition = target.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
    }

    #endregion
}
*/
/*
public class SmoothCameraFollowNew : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] Vector3 offset;

    void Update()
    {
        // Define a target position above and behind the target transform

        var forwardDirection = target.rotation * Vector3.back;   // this is a bit misleading and i don't quite know how it works but it works
        var forward = target.position - forwardDirection;

        Vector3 targetPosition = target.TransformPoint(forward);

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime,1000f);
    }
}
*/
public class SmoothCameraFollowNew : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] Vector3 offset;
    [SerializeField] float distanceToTarget = 2f;
    [SerializeField] float playerSpeed = 2f;
   
    private void Start()
    {
      //  pSpeed = target.gameObject.GetComponent<LookingForForward>();

    }
    void Update()
    {
       // pSpeed.speed = playerSpeed;


    }

    private void LateUpdate()
    {/*
        var forwardDirection = target.rotation * Vector3.back;   // this is a bit misleading and i don't quite know how it works but it works
        var forward = transform.forward+offset;

        // Define a target position above and behind the target transform
        Vector3 targetPosition = target.TransformPoint(forward.normalized * distanceToTarget);

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        transform.LookAt(target);
        */
    }
}
