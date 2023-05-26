using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlaneController : MonoBehaviour
{
    public float throttleIncrement = 0.1f;
    public float maxThrust = 200f;
    public float responsiveness = 10f;
    [SerializeField]CinemachineBrain cinemachineBrain; 

    public float planeMass = 400f;
    [Header("in-game values!")]
    [SerializeField] private float throttle;
    [SerializeField] private float roll;
    [SerializeField] private float pitch; 
    [SerializeField] private float yaw;
    [SerializeField] private float speedMult=1;

    private float moveStep;
    private float forwardForce;

    Rigidbody rb;
    private float responseModifier
    {
        get
        {
            return (planeMass / 10f)*responsiveness;
        }
    }

    #region AnotherController
    public float forwardSpeed = 25f, strafeSpeed = 7.5f, hoverSpeed = 5f;
    [Header("in-game values!")]
    private float activeForwardSpeed, activeStrafeSpeed, activeHoverSpeed;


    private float forwardAcceleration = 2.5f, strafeAcceleration = 2f, hoverAcceleration = 2f;

    public float lookRateSpeed = 90f;
    private Vector2 lookInput, screenCenter, mouseDistance;

    private float rollInput;
    public float rollSpeed = 90f, rollAcceleration = 3.5f;


    #endregion

    bool MouseControl=true;



    private void Awake()
    {
        //   rb = GetComponent<Rigidbody>();
        //cinemachineBrain = GetComponent<CinemachineVirtualCamera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        screenCenter.x = Screen.width * .5f;
        screenCenter.y = Screen.height * .5f;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (MouseControl) MouseControl = false; else MouseControl = true;
        }

        if (MouseControl)
        {

            lookInput.x = Input.mousePosition.x;
            lookInput.y = Input.mousePosition.y;

            mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.y;
            mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;

            mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);

            rollInput = Mathf.Lerp(rollInput, Input.GetAxisRaw("Roll2"), rollAcceleration * Time.deltaTime);

            

            activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, Input.GetAxisRaw("Vertical") * forwardSpeed, forwardAcceleration * Time.deltaTime);
            activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, Input.GetAxisRaw("Horizontal") * strafeSpeed, strafeAcceleration * Time.deltaTime);
            activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, Input.GetAxisRaw("Pitch") * hoverSpeed, hoverAcceleration * Time.deltaTime);

            
          

        }
        else
        {
            HandleInputs();
        }


    }


    private void FixedUpdate()
    {
        
        if(MouseControl)
        {
            transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, mouseDistance.x * lookRateSpeed * Time.deltaTime, rollInput * rollSpeed * Time.deltaTime, Space.Self);



            transform.position += transform.forward * activeForwardSpeed * Time.deltaTime;
            transform.position += (transform.right * activeStrafeSpeed * Time.deltaTime);
        }

        else
        {

            //Debug.Log(throttle);
            forwardForce = Mathf.Clamp(throttle, 0f, maxThrust);
            moveStep = speedMult * Time.deltaTime;
            //Debug.Log(forwardForce);

            transform.position += transform.forward * forwardForce * moveStep;


            //transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, mouseDistance.x * lookRateSpeed * Time.deltaTime, rollInput * rollSpeed * Time.deltaTime, Space.Self);

            //transform.Rotate(transform.forward * roll * responseModifier * Time.deltaTime);
            transform.Rotate(Vector3.forward, -roll * responseModifier * Time.deltaTime);
            transform.Rotate(Vector3.up, yaw * responseModifier / 2 * Time.deltaTime);

            transform.Rotate(Vector3.right, pitch * responseModifier / 2 * Time.deltaTime);
            //cinemachineBrain.ManualUpdate();

        }

    }

    private void HandleInputs()
    {
        roll = Input.GetAxis("Roll");
        pitch = Input.GetAxis("Pitch");
        yaw = Input.GetAxis("Yaw");
        if (Input.GetKey(KeyCode.Space))
        {
            throttle += throttleIncrement;
            //Debug.Log("space button pushed");
        }
        else if (Input.GetKey(KeyCode.LeftControl)) throttle -= throttleIncrement;
        throttle = Mathf.Clamp(throttle, 0f, 100f); 

        
    }


    /*
    private void FixedUpdate()
    {



      
        //transform.position += (transform.right * activeStrafeSpeed * Time.deltaTime);

    

        // rb.AddForce(transform.forward * maxThrust * throttle);
        //  rb.AddTorque(transform.up * yaw * responseModifier);
        //  rb.AddTorque(transform.right * pitch * responseModifier);
        // rb.AddTorque(-transform.forward * roll * responseModifier);





    }
    */

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("DRONE CRASHED");
    }
}
