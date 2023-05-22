using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;


public class CommonDrone : MonoBehaviour
{
    // this only fly around targetting drones of another team. no physics no nothing. Just visual
    FlockController flockController;
    [SerializeField] GameObject _MyTarget;
    [SerializeField] GameObject _BulletPrefab;
    [SerializeField] int _ID;
    [SerializeField] Vector3 _RandomPosInsideSphere;
    [SerializeField] float retargetDelay;
    [SerializeField] float shootDelay;

    [SerializeField] int _MyTeamID;

    public int droneStatus; // if 1 - drone hunter. if 2 - drone flee. smth like that no killing/dying. they just fly around and shoot. // fleer is basically wanderer
    public bool HaveAttacker;   // can't have more than 1 drone attacking me
    public bool NeedNewRandomPosition;
    public Unity.Mathematics.Random randomSeed;
    /*
    [Header("MOVEMENT")]
    [SerializeField] private float _speed = 15;
    [SerializeField] private float _rotateSpeed = 95;

    [Header("PREDICTION")]
    [SerializeField] private float _maxDistancePredict = 100;
    [SerializeField] private float _minDistancePredict = 5;
    [SerializeField] private float _maxTimePrediction = 5;
    private Vector3 _standardPrediction, _deviatedPrediction;

    [Header("DEVIATION")]
    [SerializeField] private float _deviationAmount = 50;
    [SerializeField] private float _deviationSpeed = 2;
    */


    public int ID { get { return _ID; } set { _ID = value; } }
    public GameObject MyTarget { get { return _MyTarget; } set { _MyTarget = value; } }
    public GameObject BulletPrefab { get { return _BulletPrefab; } set { _BulletPrefab = value; } }
    public int MyTeamID { get { return _MyTeamID; } set { _MyTeamID = value; } }
    public Vector3 RandomPosInsideSphere
    {
        get { return _RandomPosInsideSphere; }
        set { _RandomPosInsideSphere = value; }
        // Start is called before the first frame update


    }
}
