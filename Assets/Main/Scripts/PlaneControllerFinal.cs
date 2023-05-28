using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MeshDecimator;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public delegate void PlayerDeath();
public delegate void ToggleShooting(int i);
public class PlaneControllerFinal : MonoBehaviour
{
    public float throttleIncrement = 0.1f;
    public float maxThrust = 200f;
    public float responsiveness = 10f;
    [SerializeField] CinemachineBrain cinemachineBrain;
    [SerializeField] VelocityBar velocityBar;
    [SerializeField] VysotaPanel vysotaPanel;
    [SerializeField] Image playerDeathImage;
    [SerializeField] GameObject Explosion;
    [SerializeField] GameObject PauseUI;
    public float planeMass = 400f;
    [Header("in-game values!")]
    [SerializeField] private float throttle;
    [SerializeField] private float roll;
    [SerializeField] private float pitch;
    [SerializeField] private float yaw;
    [SerializeField] private float speedMult = 1;
    public event PlayerDeath _OnPlayerDeath;
    public event ToggleShooting _OnToggleShooting;
    private float moveStep;
    private float forwardForce;
    public float actualSpeed;
    Rigidbody rb;
    float UI_button_delay;
    MeshRenderer myMesh;

    private float responseModifier
    {
        get
        {
            return (planeMass / 10f) * responsiveness;
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

    bool MouseControl = true;
    public bool playerDead;
    public bool screenOpened;
    public bool pauseMenuActive;

    public void PlayerDeath()
    {
        if (!playerDead)
        {


            playerDead = true;
            myMesh.enabled = false;
            Instantiate(Explosion, transform.position, Quaternion.identity);
            _OnToggleShooting?.Invoke(0);
            StartCoroutine(CloseScreenDelay());
        }
       

    }

    IEnumerator CloseScreenDelay()
    {
        yield return new WaitForSeconds(.5f);
        StartCoroutine(CloseScreen());

    }

    IEnumerator PlayerRebirth()
    {

        yield return new WaitForSeconds(2f);
        DeathScreenHandle();

    }


    public void DeathScreenHandle()
    {

        if (!playerDead && !screenOpened)
        {
            // open screen
            StartCoroutine(OpenScreen());

            
        }
        else if (playerDead && screenOpened)
        {
            // close screen

            StartCoroutine(CloseScreen());
            
        }



    }

    public void OpenDeathScreen()
    {
        myMesh.enabled = true;
        StartCoroutine(OpenScreen());
    }
    IEnumerator OpenScreen()
    {
        bool work = true;
        float current = 1f;
        
        while (work)
        {


            yield return null;

            current = Mathf.MoveTowards(current, 0, 1f * Time.deltaTime);

            playerDeathImage.fillAmount = current / 1;

            if (current == 0) {
                Debug.Log(" OPEN SCREEN COROT STOPPED");
                _OnToggleShooting?.Invoke(1);
                playerDead = false;
                screenOpened = true;
                yield break;
            }

        }
      
        
    }

    IEnumerator CloseScreen()
    {
        bool work = true;
        float current = 0f;

        while (work)
        {


            yield return null;

            current = Mathf.MoveTowards(current, 1, 1f * Time.deltaTime);

            playerDeathImage.fillAmount = current / 1;

            if (current == 1)
            {
                Debug.Log(" CLOSE SCREEN COROT STOPPED");
                _OnPlayerDeath?.Invoke();
                _OnToggleShooting?.Invoke(0);
                playerDead = true;
                screenOpened = false;
                yield break;
            }

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
        screenCenter.x = Screen.width * .5f;
        screenCenter.y = Screen.height * .5f;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        vysotaPanel._OnTooLow += PlayerBeyondBorder;
        myMesh = GetComponent<MeshRenderer>();
        DeathScreenHandle();
    }

    // Update is called once per frame

    void PlayerBeyondBorder()
    {
        if (!playerDead)
        {
            PlayerDeath();
        }
        
    }
    void Update()
    {

        

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (MouseControl) MouseControl = true;// else MouseControl = true;
        }

        if (MouseControl && !playerDead)
        {
            HandleInputs();

            lookInput.x = Input.mousePosition.x;
            lookInput.y = Input.mousePosition.y;

            mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.y;
            mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;

            mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);

            rollInput = Mathf.Lerp(rollInput, Input.GetAxisRaw("Roll2"), rollAcceleration * Time.deltaTime);



            activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, Input.GetAxisRaw("Vertical") * forwardSpeed, forwardAcceleration * Time.deltaTime);
            activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, Input.GetAxisRaw("Horizontal") * strafeSpeed, strafeAcceleration * Time.deltaTime);
            activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, Input.GetAxisRaw("Hover") * hoverSpeed, hoverAcceleration * Time.deltaTime);




        }
        else
        {
           // HandleInputs();
        }


    }


    private void FixedUpdate()
    {

        if (MouseControl && !playerDead)
        {
            
            transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, mouseDistance.x * lookRateSpeed * Time.deltaTime, -rollInput * rollSpeed * Time.deltaTime, Space.Self);
            forwardForce = Mathf.Clamp(throttle, -maxThrust, maxThrust);


            moveStep = speedMult * Time.deltaTime;
            velocityBar.UpdateVelocityBar(forwardForce, maxThrust);
            vysotaPanel.UpdateVysotaPanel();
            actualSpeed = forwardForce * moveStep * Time.deltaTime;
            transform.position += transform.forward * actualSpeed;
            transform.position += (transform.right * activeStrafeSpeed * Time.deltaTime);
            transform.position += (transform.up * activeHoverSpeed * Time.deltaTime);


        }

        else
        {
            /*
            //Debug.Log(throttle);
            forwardForce = Mathf.Clamp(throttle, 0f, maxThrust);
            moveStep = speedMult * Time.deltaTime;
            //Debug.Log(forwardForce);

            transform.position += transform.forward * moveStep;


            //transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, mouseDistance.x * lookRateSpeed * Time.deltaTime, rollInput * rollSpeed * Time.deltaTime, Space.Self);

            //transform.Rotate(transform.forward * roll * responseModifier * Time.deltaTime);
            transform.Rotate(Vector3.forward, -roll * responseModifier * Time.deltaTime);
            transform.Rotate(Vector3.up, yaw * responseModifier / 2 * Time.deltaTime);

            transform.Rotate(Vector3.right, pitch * responseModifier / 2 * Time.deltaTime);
            //cinemachineBrain.ManualUpdate();
            */
        }

      
    }

    private void HandleInputs()
    {
        roll = Input.GetAxis("Roll");
        //pitch = Input.GetAxis("Pitch");
        //yaw = Input.GetAxis("Yaw");
        if (Input.GetKey(KeyCode.W))
        {
            throttle += throttleIncrement;
            //Debug.Log("space button pushed");
        }
        else if (Input.GetKey(KeyCode.S)) throttle -= throttleIncrement;
        throttle = Mathf.Clamp(throttle, -maxThrust, maxThrust);
        if (Input.GetKey(KeyCode.C))
        {
            throttle = Mathf.MoveTowards(throttle, 0f, throttleIncrement*1.25f);
            //Debug.Log("space button pushed");
        }

        if (Input.GetKeyUp(KeyCode.Escape) )
        {
            if (!pauseMenuActive )
            {
                UI_EnablePauseMenu();
            }
            else
            {
                UI_DisablePauseMenu();
            }
            //UI_button_delay = 0;
        }


    }

    void UI_EnablePauseMenu()
    {
        Cursor.visible = true;
        Cursor.lockState= CursorLockMode.None;
        Time.timeScale = 0;
        PauseUI.SetActive(true);
        pauseMenuActive=true;

    }
    public void UI_DisablePauseMenu()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 1;
        PauseUI.SetActive(false);
        pauseMenuActive = false;

    }
    public void UI_GO_HANGAR()
    {
        SoundController.Instance.StopMusic();
        SoundController.Instance.PlayMusic(0);
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
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
        PlayerDeath();
    }

    private void OnDestroy()
    {
        vysotaPanel._OnTooLow -= PlayerBeyondBorder;
    }
}
