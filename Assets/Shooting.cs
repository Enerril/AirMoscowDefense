using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
public class Shooting : MonoBehaviour
{

    [SerializeField] float fireDelay;
    [SerializeField] float offsetAccuracy=1f;
    
    [SerializeField] LeanGameObjectPool explosionsPool;
    [SerializeField] LeanGameObjectPool muzzlePool;
    [SerializeField] LeanGameObjectPool projectilePool;
    float timeSinceLastShot;
    //ParticleSystem muzzlePS;
    GameObject proj;
    GameObject muzzle;

    // Start is called before the first frame update
    void Start()
    {
        //muzzlePS=muzzle.GetComponent<ParticleSystem>();
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
            muzzle = muzzlePool.Spawn(transform.position, Quaternion.identity,this.transform);
           // proj.transform.rotation=transform.rotation; - working

            float randomX = Random.Range(-offsetAccuracy, offsetAccuracy);
            float randomY = Random.Range(-offsetAccuracy, offsetAccuracy);
            float randomZ = Random.Range(-offsetAccuracy, offsetAccuracy);
            float randomW = Random.Range(-offsetAccuracy, offsetAccuracy);

            Quaternion oldRot = transform.rotation;
            Quaternion newRot = new Quaternion(oldRot.x + randomX, oldRot.y + randomY, oldRot.z + randomZ, oldRot.w + randomW);
            proj.transform.rotation = newRot;
            muzzle.transform.rotation = newRot;
            //proj.transform.rotation.x += UnityEngine.Random.Range(-offsetAccuracy, -offsetAccuracy);

            timeSinceLastShot = 0;
           
        }
        else
        {
            //Debug.Log("HERE");
           
        }
        
    }

   

    private void LateUpdate()
    {
      
    }
}
