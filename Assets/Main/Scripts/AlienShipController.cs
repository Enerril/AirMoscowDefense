using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AlienShipController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject BoomBoom;
    Camera cam;
    FloatingOrigin floatingOrigin;
    [SerializeField] Transform playerTransform;
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;
    UnitData unitData;
    [SerializeField] AnimationCurve curve;
    [SerializeField] float movingSpeed = 1f;
    [SerializeField] float rotateSpeed=1f;
    [SerializeField] float patrolDistance = 20f;
    Rigidbody rb;


    float lerpVCurrent, lerpVTarget;

    bool isAlive;
    bool PatrolPointA;
    bool deathbool;
    Vector3 upPoint;
    Vector3 downPoint;
    [SerializeField]Vector3 TargetPosition;
    public Vector3 velocity;
    WaitForSeconds patrolDelay = new WaitForSeconds(5f);
    WaitForSeconds boomDelay = new WaitForSeconds(.2f);
    float explosionCounter;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        floatingOrigin=cam.GetComponent<FloatingOrigin>();
        floatingOrigin._OnOriginMoved += AdjustPatrolVectors;
        unitData = GetComponent<UnitData>();

        currentHealth = maxHealth;
        isAlive = true;
        lerpVTarget = 1;
        upPoint = transform.position + Vector3.up * patrolDistance;
        downPoint = transform.position + Vector3.down * patrolDistance;
        //  Debug.Log(transform.position);
        // Debug.Log(upPoint+" up");
        //  Debug.Log(downPoint + " down");
       // transform.position = downPoint;
        PatrolPointA = true;
        TargetPosition = upPoint;
        Invoke("PatrolStart", Random.Range(0.5f, 2.5f));
        unitData._OnZeroHealth += DeathProcess;
    }

    void DeathProcess()
    {
        if (!deathbool)
        {
            StartCoroutine(BoomBoom_Die());
            deathbool = true;
        }
        
    }

    IEnumerator BoomBoom_Die()
    {

        while (true)
        {

            yield return boomDelay;
            
            if (explosionCounter > 8)
            {
                var b =Instantiate(BoomBoom, transform.position, Quaternion.identity);
                b.transform.localScale = new Vector3(50,50,50);
                Destroy(gameObject);
            }
            var k = transform.position + Random.onUnitSphere * 75;
            explosionCounter++;
            Instantiate(BoomBoom, k, Quaternion.identity);
        }

    }


    void AdjustPatrolVectors(Vector3 offset)
    {
        upPoint+=offset;
        downPoint+=offset;
    }

    void PatrolStart()
    {
        StartCoroutine(Patrol());
    }

    IEnumerator Patrol()
    {
        //TargetPosition = upPoint;
        while (isAlive)
        {
            yield return patrolDelay;
            if (TargetPosition == upPoint) {

                TargetPosition = downPoint;
            } 
                else
                {
                    TargetPosition = upPoint;
                }
            //Debug.Log(TargetPosition + " TARGET POSITION");

            //Debug.Log("HRERE-5");   
           

           // lerpVTarget = lerpVTarget == 0 ? 1 : 0;
            lerpVCurrent = 0;
          //  yield return patrolDelay;
        }
    }

    

    

    void LateUpdate()
    {
        // lerpVTarget = lerpVTarget == 0 ? 1 : 0;

         lerpVCurrent = Mathf.MoveTowards(lerpVCurrent, lerpVTarget, movingSpeed*Time.deltaTime);
       // Debug.Log(lerpVCurrent);
       // transform.position = Vector3.Lerp(transform.position, TargetPosition, curve.Evaluate(lerpVCurrent));

        //if (lerpVTarget==0) transform.position = Vector3.Lerp(downPoint, upPoint, lerpVCurrent);
        // else transform.position = Vector3.SmoothDamp(upPoint, downPoint,ref velocity, 0.5f, movingSpeed);

        // Debug.Log(lerpVCurrent);
        // Debug.Log(lerpVTarget);

        // transform.position = new Vector3(Mathf.PingPong(Time.time, 3), transform.position.y, transform.position.z);


        
        if(TargetPosition == upPoint)
        {
            //transform.position = transform.position +Vector3.up*movingSpeed*Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, TargetPosition, curve.Evaluate(lerpVCurrent));
        }
        else
        {
            //transform.position = transform.position + Vector3.down * movingSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, TargetPosition, curve.Evaluate(lerpVCurrent));
        }

        transform.Rotate(0,  rotateSpeed* (1 + lerpVCurrent), 0,Space.Self);
        // Debug.Log(TargetPosition + " TARGET POSITION");
        // transform.position = Vector3.MoveTowards(transform.position, TargetPosition, movingSpeed * Time.deltaTime);
        /*
        Debug.Log("HERE1");
        Debug.Log(Vector3.Distance(transform.position, TargetPosition));
        if (Vector3.Distance(transform.position, TargetPosition) <= 0.1f)
        {
            //TargetPosition=TargetPosition==upPoint?downPoint:upPoint;
            Debug.Log("HERE2");
            if (TargetPosition == upPoint) TargetPosition = downPoint;
            else
            {
                TargetPosition = upPoint;
            }


           
        }
        */





    }


    private void OnDestroy()
    {
        floatingOrigin._OnOriginMoved -= AdjustPatrolVectors;
        unitData._OnZeroHealth -= DeathProcess;
    }



}
