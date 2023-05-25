using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
public class Shooting : MonoBehaviour
{

    [SerializeField] float fireDelay;
    [SerializeField] Vector3 offsetAccuracy;
    
    [SerializeField] LeanGameObjectPool explosionsPool;
    [SerializeField] GameObject muzzle;
    [SerializeField] LeanGameObjectPool projectilePool;
    float timeSinceLastShot;
    ParticleSystem muzzlePS;
    GameObject proj;

    // Start is called before the first frame update
    void Start()
    {
        muzzlePS=muzzle.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (Input.GetButton("Fire1") && timeSinceLastShot> fireDelay)   
        {
            //explosionsPool.Spawn();
            //muzzlePool.Spawn();
            proj=projectilePool.Spawn(transform.position,Quaternion.identity,null);
            proj.transform.rotation=transform.rotation;

            timeSinceLastShot = 0;
            muzzlePS.Play();
        }

        
    }

    private void LateUpdate()
    {
        muzzlePS.Stop();
    }
}
