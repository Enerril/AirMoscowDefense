using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using System;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] float speed=1f;
    LeanGameObjectPool poolExpl;
    // Start is called before the first frame update
    [SerializeField] float deathTime=2f;
    WaitForSeconds waitSeconds=new WaitForSeconds(5f);
    TrailRenderer trailRenderer;
    public float playerDroneSpeed=0f;

    UnitData unitData;
    private void Awake()
    {
        trailRenderer = this.gameObject.GetComponent<TrailRenderer>();
    }
    void Start()
    {
        poolExpl = GameObject.FindWithTag("PoolExpl").GetComponent<LeanGameObjectPool>();
       
        waitSeconds = new WaitForSeconds(deathTime);
    }

    void OnEnable()
    {
        trailRenderer.Clear();
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        yield return waitSeconds;
        ReturnProjectile();
    }


    // Update is called once per frame
    void Update()
    {
        //var forwardDirection = transform.rotation * Vector3.back;   // this is a bit misleading and i don't quite know how it works but it works
        //var forward = transform.position - forwardDirection;
        //transform.position = Vector3.MoveTowards(transform.position, forward, speed * Time.deltaTime);
        transform.position += transform.forward * Time.deltaTime * (speed+ playerDroneSpeed);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //
        

        if (collision.gameObject.layer == 11)
        {
           // Debug.Log("HIT");
            unitData = collision.gameObject.GetComponent<UnitData>();
            unitData.Health = 5;

            ReturnProjectile();
        }
        else
        {
            ReturnProjectile();
        }
        
        //ReturnProjectile();
    }

    void ReturnProjectile()
    {
        poolExpl.Spawn(this.transform.position);
        LeanPool.Despawn(this.gameObject);
    }

}
