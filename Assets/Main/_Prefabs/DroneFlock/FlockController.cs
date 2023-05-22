using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using UnityEngine.Jobs;
using System.Net;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;


public class FlockController : MonoBehaviour
{
    [SerializeField] GameObject[] flock;
    [SerializeField]GameObject[] bullets;
    [SerializeField] Transform playerTransofrm;
    CommonDrone[] commonDrones;

    //[SerializeField] GameObject humanDrone;
    //[SerializeField] GameObject alienDrone;
    [SerializeField] int droneAmount;
    int bulletsAmount;
    [SerializeField] int sphereSizeMult=4;
    int droneCounter;
    int bulletCounter;
    int maxHunters; //should not exceed more than 45% of total amount of drones... Want to have some free wanderers
    Vector3 spawnPos;
    Vector3 randomPointInsideSphere;
 
    [SerializeField] LeanGameObjectPool Reds;
    [SerializeField] LeanGameObjectPool Blues;
    [SerializeField] LeanGameObjectPool HumanBullets;
    [SerializeField] LeanGameObjectPool AlienBullets;
    JobHandle drone_JobHandle;
    JobHandle bullets_JobHandle;

    TransformAccessArray transformAccessArrayDrones;
    TransformAccessArray transformAccessArrayBullets;

    public NativeArray<Vector3> myTargetPos;
    public NativeArray<Vector3> myTargetForwardPos;
    public NativeArray<Vector3> myForwardPos;
    public NativeArray<bool> NeedNewRandomPositions;
    public NativeArray<Unity.Mathematics.Random> randomGens;
    public NativeArray<int> droneStatus;
    float[] reloadTime;
    public NativeArray<int> bulletsShot;    // basically we spawn N bullets after which we increase reload to from ~300ms to 15sec.
    bool[] canShoot;
    public NativeArray<float> bulletLiveTime;
    
    [SerializeField] private float rateOfFire=0.1f;
    [SerializeField] private float longReloadDelay=10f;
    [SerializeField] private int maxBulletsInSpray=10;
    [Range(0.01f,0.5f)]
    [SerializeField] private float droneAccuracyOffset=0.03f;
    // Start is called before the first frame update
    void Start()
    {
        DroneInitialSetup();
        //if (bulletsAmount > 5000) bulletsAmount = 5000;
        bulletsAmount = droneAmount * 20;
        bullets = new GameObject[bulletsAmount * 2];

        for (int i = 0; i < bulletsAmount; i++)
        {
          
            var g = HumanBullets.Spawn(transform.position, Quaternion.identity);
            bullets[bulletCounter] = g;
            bulletCounter++;
                
                    
            var k = AlienBullets.Spawn(transform.position, Quaternion.identity);
            bullets[bulletCounter] = k;
            bulletCounter++;
            
           
        }
        for (int i = 0; i < bullets.Length; i++)
        {
            LeanPool.Despawn(bullets[i]);
        }


        bulletLiveTime =new NativeArray<float> (bulletsAmount * 2, Allocator.Persistent);
        transformAccessArrayBullets = new TransformAccessArray(bulletsAmount * 2);
        for (int i = 0; i < bulletsAmount * 2; i++)
        {
            transformAccessArrayBullets.Add(bullets[i].gameObject.transform);
        }
        canShoot = new bool[droneAmount * 2];
        reloadTime = new float[droneAmount * 2];
    }

    private void DroneInitialSetup()
    {
        if (droneAmount > 1000) droneAmount = 1000;
        flock = new GameObject[droneAmount * 2];
        commonDrones = new CommonDrone[droneAmount * 2];
        SpawnDrones();
        myTargetPos = new NativeArray<Vector3>(droneAmount * 2, Allocator.Persistent);
        myTargetForwardPos = new NativeArray<Vector3>(droneAmount * 2, Allocator.Persistent);
        myForwardPos = new NativeArray<Vector3>(droneAmount * 2, Allocator.Persistent);
        randomGens = new NativeArray<Unity.Mathematics.Random>(droneAmount * 2, Allocator.Persistent);
        NeedNewRandomPositions = new NativeArray<bool>(droneAmount * 2, Allocator.Persistent);
        droneStatus = new NativeArray<int>(droneAmount * 2, Allocator.Persistent);
        transformAccessArrayDrones = new TransformAccessArray(droneCounter);
        
        bulletsShot= new NativeArray<int>(droneAmount * 2, Allocator.Persistent);

        for (int i = 0; i < droneCounter; i++)
        {
            transformAccessArrayDrones.Add(flock[i].gameObject.transform);
        }

        for (int i = 0; i < droneCounter; i++)
        {
            commonDrones[i] = flock[i].GetComponent<CommonDrone>();
        }

        for (int i = 0; i < droneCounter; i++)
        {
            randomGens[i] = commonDrones[i].randomSeed;
        }

        maxHunters = (droneAmount) - (int)(droneAmount * 0.05f);

        //Debug.Log(maxHunters + " max hunters");
        int huntersCounter = 0;

        for (int i = 0; i < droneCounter; i++)
        {
            if (huntersCounter < maxHunters)
            {
                var s = UnityEngine.Random.Range(1, 3);
                if (s == 1)
                {
                    commonDrones[i].droneStatus = 1;

                    huntersCounter++;
                }
                else
                {
                    commonDrones[i].droneStatus = 2;
                }

            }
            else
            {
                commonDrones[i].droneStatus = 2;
            }
            // Debug.Log(commonDrones[i].droneStatus);

            /*
            for (int j = 0; j < 10; j++)
            {
                var k = UnityEngine.Random.Range(0, droneCounter);
                if (commonDrones[i].MyTeamID != commonDrones[k].MyTeamID)
                {
                    commonDrones[i].MyTarget = flock[k];
                    break;
                }

            }
            */
        }
        //Debug.Log(huntersCounter + " hunter amount");
        for (int i = 0; i < droneCounter; i++)
        {
            if (commonDrones[i].droneStatus == 1)
            {
                // look for available target for hunter drone    
                for (int j = 0; j < droneCounter; j++)
                {

                    if (commonDrones[j].droneStatus == 2 && commonDrones[j].HaveAttacker == false && commonDrones[i].MyTeamID != commonDrones[j].MyTeamID)
                    {
                        commonDrones[i].MyTarget = flock[j];
                        commonDrones[j].HaveAttacker = true;
                        break;
                    }


                }


            }
            else if (commonDrones[i].droneStatus == 2)
            {
                // wanderer drone just periodicaly moves to random point inside sphere;
                commonDrones[i].RandomPosInsideSphere = GetRandomMovePointInSphere();
                //commonDrones[i].NeedNewRandomPosition = false;
            }

        }
        for (int i = 0; i < droneCounter; i++)
        {
            if (commonDrones[i].droneStatus == 1 && commonDrones[i].MyTarget == null)
            {
                commonDrones[i].droneStatus = 2;    //failed to find target. switch to wanderer
                huntersCounter--;
            }

        }
        //Debug.Log(huntersCounter + " active hunters left after searching target");
        for (int i = 0; i < droneCounter; i++)
        {
            droneStatus[i] = commonDrones[i].droneStatus;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PrepareDroneJobData();

        FlockBulletJob flockBulletJob = new FlockBulletJob()
        {
            _BulletLiveTime = bulletLiveTime,
            _RandomGen = randomGens,
            //_targetPosition = myTargetPos,
            deltaTime = Time.deltaTime


        };
        bullets_JobHandle = flockBulletJob.Schedule(transformAccessArrayBullets);

        FlockDroneJob flockDroneJob = new FlockDroneJob()
        {
            //_targetForward = myTargetForwardPos,
            _targetPosition = myTargetPos,
            //_MyForward = myForwardPos,
            _RandomGen = randomGens,
            _DroneStatus = droneStatus,
            _NeedNewRandomPositions = NeedNewRandomPositions,
            //_BulletsShot=bulletsShot,
           // _ReloadTime=reloadTime,
            timeTime = Time.time,
            deltaTime = Time.deltaTime

        };
        drone_JobHandle = flockDroneJob.Schedule(transformAccessArrayDrones);
       
    }

    private void PrepareDroneJobData()
    {
        for (int i = 0; i < droneCounter; i++)
        {
            if (commonDrones[i].droneStatus == 1)
            {
                myTargetPos[i] = commonDrones[i].MyTarget.transform.position;

            }
            else if (commonDrones[i].droneStatus == 2)
            {
                myTargetPos[i] = commonDrones[i].RandomPosInsideSphere;

            }



            //myTargetForwardPos[i] = commonDrones[i].MyTarget.transform.localToWorldMatrix.MultiplyVector(transform.forward);

            //myForwardPos[i] = flock[i].transform.localToWorldMatrix.MultiplyVector(transform.forward);
            // myTargetForwardPos[i] = commonDrones[i].MyTarget.transform.position + commonDrones[i].MyTarget.transform.forward;

            //myForwardPos[i] = flock[i].transform.position + flock[i].transform.forward;




        }

       
        /*
        for (int i = 0; i < droneCounter; i++)
        {
            Debug.Log("------");
            Debug.Log(myTargetPos[i]+ " my target pos");
            Debug.Log(myTargetForwardPos[i]+ "forward of my target");
            Debug.Log(myForwardPos[i] + "my forward. local i hope");
            Debug.Log("------");
        }
        */
    }

    private Vector3 GetRandomMovePointInSphere()
    {
        var pos = transform.position + UnityEngine.Random.insideUnitSphere * sphereSizeMult * droneAmount;

        if (pos.y < 30)
        {
            pos.y = UnityEngine.Random.Range(30,150);
        }

        return pos;
    }

    void InstantJobValuesEvaluate()
    {
        CheckForNewRandomPoint();

        HandleDronesShooting();

        HandleBulletLiveTime();
    }

    private void HandleDronesShooting()
    {
        for (int i = 0; i < flock.Length; i++)
        {
            if (commonDrones[i].droneStatus == 1)
            {
                // Debug.Log("here 2");
                if (commonDrones[i].MyTeamID == 1)
                {

                    if (reloadTime[i] <= 0 && canShoot[i])
                    {
                        // Debug.Log(reloadTime[i] + "my reload time left", flock[i]);
                        // Debug.Log(bulletsShot[i] + "how much I shot", flock[i]);
                        // Debug.Log("here 3-1");
                        if (bulletsShot[i] < maxBulletsInSpray)
                        {
                            // Debug.Log("spawning alien bullet " + flock[i]);
                            var g = AlienBullets.Spawn(flock[i].transform.position, Quaternion.identity);

                            var b = flock[i].transform.rotation;

                            b.x = flock[i].transform.rotation.x * UnityEngine.Random.Range(1- droneAccuracyOffset, 1 + droneAccuracyOffset);
                            b.y = flock[i].transform.rotation.y * UnityEngine.Random.Range(1 - droneAccuracyOffset, 1 + droneAccuracyOffset);
                            b.z = flock[i].transform.rotation.z * UnityEngine.Random.Range(1 - droneAccuracyOffset, 1 + droneAccuracyOffset);

                            g.transform.rotation = b;

                            reloadTime[i] = rateOfFire;
                            bulletsShot[i]++;
                            canShoot[i] = false;
                        }
                        else if (bulletsShot[i] >= maxBulletsInSpray)
                        {
                            // Debug.Log("LONG RELOAD", flock[i]);
                            reloadTime[i] = longReloadDelay + UnityEngine.Random.Range(0f, 2f);    // long reload
                            bulletsShot[i] = 0;
                            canShoot[i] = false;
                        }
                    }
                    else
                    {
                        //Debug.Log("my reload time is "+ reloadTime[i]);
                        reloadTime[i] -= Time.deltaTime;
                        //Debug.Log("reload time left " + reloadTime[i]);
                        if (reloadTime[i] <= 0)
                        {
                            reloadTime[i] = 0;
                            canShoot[i] = true;
                        }
                    }

                }
                else if (commonDrones[i].MyTeamID == 2)
                {
                    if (reloadTime[i] <= 0 && canShoot[i])
                    {
                        // Debug.Log(reloadTime[i] + "my reload time left", flock[i]);
                        // Debug.Log(bulletsShot[i] + "how much I shot", flock[i]);
                        // Debug.Log("here 3-1");
                        if (bulletsShot[i] < maxBulletsInSpray)
                        {
                            // Debug.Log("spawning alien bullet " + flock[i]);
                            var g = HumanBullets.Spawn(flock[i].transform.position, Quaternion.identity);

                            var b = flock[i].transform.rotation;

                            b.x = flock[i].transform.rotation.x * UnityEngine.Random.Range(1 - droneAccuracyOffset, 1 + droneAccuracyOffset);
                            b.y = flock[i].transform.rotation.y * UnityEngine.Random.Range(1 - droneAccuracyOffset, 1 + droneAccuracyOffset);
                            b.z = flock[i].transform.rotation.z * UnityEngine.Random.Range(1 - droneAccuracyOffset, 1 + droneAccuracyOffset);

                            g.transform.rotation = b;

                            reloadTime[i] = rateOfFire;
                            bulletsShot[i]++;
                            canShoot[i] = false;
                        }
                        else if (bulletsShot[i] >= maxBulletsInSpray)
                        {
                            // Debug.Log("LONG RELOAD", flock[i]);
                            reloadTime[i] = longReloadDelay + UnityEngine.Random.Range(0f, 2f); ;    // long reload
                            bulletsShot[i] = 0;
                            canShoot[i] = false;
                        }
                    }
                    else
                    {

                        // Debug.Log("my reload time is " + reloadTime[i]);
                        reloadTime[i] -= Time.deltaTime;
                        // Debug.Log("reload time left " + reloadTime[i]);

                        if (reloadTime[i] <= 0)
                        {
                            reloadTime[i] = 0;
                            canShoot[i] = true;
                        }
                    }
                }




            }



        }
    }

    private void HandleBulletLiveTime()
    {
        for (int i = 0; i < bullets.Length; i++)
        {

            if (bullets[i].activeSelf == true)
            {
                if (bulletLiveTime[i] > 2f)
                {
                    bulletLiveTime[i] = 0;
                    LeanPool.Despawn(bullets[i]);
                }
            }
        }
    }

    private void CheckForNewRandomPoint()
    {
        for (int i = 0; i < droneCounter; i++)
        {

            if (NeedNewRandomPositions[i] == true)
            {
                commonDrones[i].RandomPosInsideSphere = GetRandomMovePointInSphere();
                NeedNewRandomPositions[i] = false;

            }

        }
    }

    public void LateUpdate()
    {/*
        m_DistancesJobHandle.Complete();
        m_NormalizeJobHandle.Complete();
        */
        // float startTime = Time.realtimeSinceStartup;

        //drone_JobHandle.Complete();
        //Debug.Log(((Time.realtimeSinceStartup - startTime) * 1000f) + " ms"+" JOBS COMPLETE ON FRAME");
        drone_JobHandle.Complete();
        bullets_JobHandle.Complete();
        InstantJobValuesEvaluate();
        //transform.position = playerTransofrm.position;
    }

    private void SpawnDrones()
    {

        for (int i = 0; i < droneAmount; i++)
        {
            spawnPos = transform.position + UnityEngine.Random.insideUnitSphere * droneAmount/2* sphereSizeMult;
            var g= Reds.Spawn(spawnPos, Quaternion.identity);
            var gc = g.GetComponent<CommonDrone>(); 
            gc.ID = droneCounter;
            gc.MyTarget = this.gameObject;
            gc.randomSeed = new Unity.Mathematics.Random((uint)UnityEngine.Random.Range(1, 100000)); 
            flock[droneCounter] = g;
            droneCounter++;



            spawnPos = transform.position + UnityEngine.Random.insideUnitSphere * droneAmount/2* sphereSizeMult;
            var k = Blues.Spawn(spawnPos, Quaternion.identity);
            var kc = k.GetComponent<CommonDrone>();
            kc.ID = droneCounter;
            kc.MyTarget = this.gameObject;
            kc.randomSeed = new Unity.Mathematics.Random((uint)UnityEngine.Random.Range(1, 100000));
            flock[droneCounter] = k;
            droneCounter++;
        }




    }

    private void OnDestroy()
    {
        NeedNewRandomPositions.Dispose();
        myForwardPos.Dispose();
        myTargetPos.Dispose();
        myTargetForwardPos.Dispose();
        transformAccessArrayDrones.Dispose();
        transformAccessArrayBullets.Dispose();
        randomGens.Dispose();
        droneStatus.Dispose();
        //reloadTime.Dispose();
        bulletLiveTime.Dispose();
        bulletsShot.Dispose();
    }
}
