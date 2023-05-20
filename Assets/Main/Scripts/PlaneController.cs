using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public float throttleIncrement = 0.1f;
    public float maxThrust = 200f;
    public float responsiveness = 10f;

    [Header("in-game values!")]
    [SerializeField] private float throttle;
    [SerializeField] private float roll;
    [SerializeField] private float pitch;
    [SerializeField] private float yaw;

    Rigidbody rb;
    private float responseModifier
    {
        get
        {
            return (rb.mass/10f)*responsiveness;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
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



    private void FixedUpdate()
    {
        //Debug.Log(throttle);
        rb.AddForce(transform.forward * maxThrust * throttle);
        rb.AddTorque(transform.up * yaw * responseModifier);
        rb.AddTorque(transform.right * pitch*responseModifier);
        rb.AddTorque(-transform.forward * roll*responseModifier);
    }
}
