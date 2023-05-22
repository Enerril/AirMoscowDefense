using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Unity.Collections;
using Unity.Burst;
using UnityEngine.Jobs;




public class FlockJobs : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
[BurstCompile]
public struct FlockBulletJob : IJobParallelForTransform
{
    //[ReadOnly] public NativeArray<Vector3> _targetPosition;
    [ReadOnly] public NativeArray<Unity.Mathematics.Random> _RandomGen;
    [ReadOnly] public float deltaTime;

    public NativeArray<float> _BulletLiveTime;

    public void Execute(int i, TransformAccess transform)
    {
        _BulletLiveTime[i] += deltaTime;
        var forwardDirection = transform.rotation * Vector3.back;   // this is a bit misleading and i don't quite know how it works but it works
        var forward = transform.position - forwardDirection;
        transform.position = Vector3.MoveTowards(transform.position, forward, 120 * deltaTime);

    }

    private Vector3 DeviateMyTargetPosition(int i)
    {
        float randomfloat = _RandomGen[i].NextFloat(0.99f, 1.02f); // inclusive,exclusive
        Vector3 pos = new Vector3();//_targetPosition[i];

        pos.x = pos.x * randomfloat;
        randomfloat = _RandomGen[i].NextFloat(0.99f, 1.02f);
        pos.y = pos.y * randomfloat;
        randomfloat = _RandomGen[i].NextFloat(0.99f, 1.02f);
        pos.z = pos.z * randomfloat;

        return pos;
    }
}

[BurstCompile]
public struct FlockDroneJob : IJobParallelForTransform
{
  // drone searches for target, drone follows target, drone shoots.
    [ReadOnly]public NativeArray<Vector3> _targetPosition;
    //[ReadOnly] public NativeArray<Vector3> _MyForward;
    //[ReadOnly]public NativeArray<Vector3> _targetForward;
    [ReadOnly] public NativeArray<Unity.Mathematics.Random> _RandomGen;
    [ReadOnly] public NativeArray<int> _DroneStatus;
    private Vector3 _standardPrediction, _deviatedPrediction;
    [ReadOnly] public float timeTime;
    [ReadOnly] public float deltaTime;
    Vector3 myPosition;
    /*
      public float _speed = 15;
      [SerializeField] private float _rotateSpeed = 95;

      [SerializeField] private float _maxDistancePredict = 100;
      [SerializeField] private float _minDistancePredict = 5;
      [SerializeField] private float _maxTimePrediction = 5;

      [SerializeField] private float _deviationAmount = 50;
      [SerializeField] private float _deviationSpeed = 2;
    */

    // return array

    public NativeArray<bool> _NeedNewRandomPositions;
    //public NativeArray<float> _ReloadTime;
    //public NativeArray<int> _BulletsShot;

    public void Execute(int i,TransformAccess transform)
    {
        if (_DroneStatus[i] == 1)
        {
            HunterTree(i, transform);
        }
        
        else if (_DroneStatus[i] == 2)
        {
            WandererTree(i, transform);
        }
    }

    private void WandererTree(int i, TransformAccess transform)
    {
        myPosition = transform.position;
        var forwardDirection = transform.rotation * Vector3.back;   // this is a bit misleading and i don't quite know how it works but it works
        var forward = transform.position - forwardDirection;
        var approximateTargetLoc = forward.normalized * 25f;
        var targetPos = _targetPosition[i];
        var heading = targetPos - myPosition;
        var rotation = Quaternion.LookRotation(heading);

        transform.rotation = (Quaternion.RotateTowards(transform.rotation, rotation, 75 * deltaTime));
        
        transform.position = Vector3.MoveTowards(transform.position, forward, 80 * deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 200)
        {
            _NeedNewRandomPositions[i] = true;
        }
    }

    private void HunterTree(int i, TransformAccess transform)
    {
        myPosition = transform.position;

        //_rb.velocity = transform.forward * _speed;


        //var leadTimePercentage = Mathf.InverseLerp(5, 100, Vector3.Distance(transform.position, _targetPosition[i]));

        //var leadTimePercentage = 5;

        //PredictMovement(leadTimePercentage,i);

        // AddDeviation(leadTimePercentage,i);

        //RotateRocket();
        var forwardDirection = transform.rotation * Vector3.back;   // this is a bit misleading and i don't quite know how it works but it works
        var forward = transform.position - forwardDirection;

        var approximateTargetLoc = forward.normalized * 25f;
        var targetPos = _targetPosition[i];


        var heading = targetPos - myPosition;


        var rotation = Quaternion.LookRotation(heading);

        //Vector3 newDirection = Vector3.RotateTowards(myPosition, _targetForward[i], 95 * deltaTime, 0f);
        // transform.rotation = Quaternion.LookRotation(newDirection);

        transform.rotation = (Quaternion.RotateTowards(transform.rotation, rotation, 75 * deltaTime));

        //var forward= rotation = Quaternion.LookRotation(value);
        //transform.position = Vector3.MoveTowards(transform.position, destinationPoint[index], speed[index] * deltaTime);
        //transform.position = Vector3.MoveTowards(transform.position, transform.rotation* Vector3.forward, 15 * deltaTime);
       /* if (Vector3.Distance(transform.position, targetPos) < 35)
        {
           // transform.position = Vector3.MoveTowards(transform.position, _MyForward[i], 12 * deltaTime);
            _ReloadTime[i] -= deltaTime;
        }*/

        //return rotation * Vector3.forward;
        /*
        A Quaternion that stores the rotation of the Transform in world space.
        public Quaternion rotation
         {
        get
        {
            get_rotation_Injected(out var ret);
            return ret;
        }
        set
        {
            set_rotation_Injected(ref value);
        }
        }*/
        //var quaternion = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        //var forward = transform.position+ quaternion * Vector3.forward;
        // var quaternion = Quaternion.Euler(10, 20, 30);
        //var forward = quaternion * Vector3.forward;
        
               
        

        
        if (Vector3.Distance(transform.position, targetPos) < 20)
        {
            transform.position = Vector3.MoveTowards(transform.position, forward, 10 * deltaTime);
            //_ReloadTime[i] -= deltaTime;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, forward, 80 * deltaTime);
            //_ReloadTime[i] = 0;
        }
        
    }

    /*
private void PredictMovement(float leadTimePercentage, int i)
{
   var predictionTime = Mathf.Lerp(0, 5, leadTimePercentage);

   _standardPrediction = _targetForward[i] * predictionTime;
}

private void AddDeviation(float leadTimePercentage,int i)
{
   var deviation = new Vector3(Mathf.Cos(timeTime * 2), 0, 0);

   //var predictionOffset = transform.TransformDirection(deviation) * _deviationAmount * leadTimePercentage;

   var predictionOffset = (_targetForward[i]+ deviation) * 50 * leadTimePercentage; // Unsure if this correct. specifically forward whether local or world

   _deviatedPrediction = _standardPrediction + predictionOffset;
}
*/
    private void RotateRocket()
    {
        var heading = _deviatedPrediction - myPosition;

        var rotation = Quaternion.LookRotation(heading);
        //_rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _rotateSpeed * Time.deltaTime));
    }

    private Vector3 DeviateMyTargetPosition(int i,Vector3 targetPos)
    {
        float randomfloat = _RandomGen[i].NextFloat(0.99f, 1.02f); // inclusive,exclusive
        Vector3 pos= targetPos;

        pos.x=pos.x *randomfloat;
        randomfloat = _RandomGen[i].NextFloat(0.99f, 1.02f);
        pos.y = pos.y * randomfloat;
        randomfloat = _RandomGen[i].NextFloat(0.99f, 1.02f);
        pos.z = pos.z * randomfloat;

        return pos;
    }


}