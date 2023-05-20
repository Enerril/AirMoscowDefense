using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.AI;
public static class RandomPoint
{


    public static Vector3 RandomNavmeshLocation(Vector3 tposition, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += tposition;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }



}