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

    private void Awake()
    {
        //   rb = GetComponent<Rigidbody>();
        //cinemachineBrain = GetComponent<CinemachineVirtualCamera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();

        //Debug.Log(throttle);
        forwardForce = Mathf.Clamp(throttle, 0f, maxThrust); 
        moveStep = speedMult * Time.deltaTime;
        //Debug.Log(forwardForce);

        transform.position += transform.forward * forwardForce*moveStep;


        //transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, mouseDistance.x * lookRateSpeed * Time.deltaTime, rollInput * rollSpeed * Time.deltaTime, Space.Self);

        //transform.Rotate(transform.forward * roll * responseModifier * Time.deltaTime);
        transform.Rotate(Vector3.forward, -roll * responseModifier * Time.deltaTime);
        transform.Rotate(Vector3.up, -yaw * responseModifier * Time.deltaTime);

        transform.Rotate(Vector3.right, pitch * responseModifier * Time.deltaTime);
        //cinemachineBrain.ManualUpdate();
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
}
