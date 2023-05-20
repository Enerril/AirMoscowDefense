using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.AI;

//
//  Returns random point within radius of given point on Navmesh. Used for random wander scripts.
//
//
//

public static class RandomPointOnNavmesh
{
    static NavMeshHit hit;
    static Vector3 randomDirection;
    static Vector3 finalPosition;
    static RandomPointOnNavmesh()
    {
        
    }

    public static Vector3 RandomNavmeshLocation(Vector3 tposition, float radius)
    {
        randomDirection = Random.insideUnitSphere * radius;
        randomDirection += tposition;
        finalPosition = Vector3.zero;

        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }

        return finalPosition;
    }



}