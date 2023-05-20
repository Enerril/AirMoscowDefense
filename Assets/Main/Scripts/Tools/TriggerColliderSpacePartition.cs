using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerColliderSpacePartition : MonoBehaviour
{
    // REVIEW MAGIC NUMBERS!!! problem is getting tile size. leave that for later.

    Transform[] transformGrid = new Transform[100];
    [SerializeField] static TriggerColliderSpacePartitionTile[] gridTiles = new TriggerColliderSpacePartitionTile[100];
    [SerializeField] GameObject gridCollider;
    [SerializeField] bool isActive;
    //[SerializeField] Vector3 gridStartPos;
    BoxCollider boxCollider;
    Vector3 transformGridPoint;
    Vector3 gridCoord;
    int counter;
    //Vector3 transformGridPoint;
    // Start is called before the first frame update
    void Start()
    {
        // transformGridPoint = gridStartPos;
        CreateGridM1();

        // Debug.Log(boxCollider.bounds.size);
    }

    private void CreateGridM1()
    {
        // we create grid where IDs go from bottom up
        if (isActive)
        {
            for (int i = -5; i < 5; i++)
            {
                gridCoord.x = i * 100;

                for (int j = -5; j < 5; j++)
                {

                    gridCoord.z = j * 100;
                    var k = Instantiate(gridCollider, gridCoord, Quaternion.identity);
                    var b = k.gameObject.GetComponent<TriggerColliderSpacePartitionTile>();



                    b.gridTileID = counter;
                    gridTiles[counter] = b;
                    b.tileCoord = gridCoord;
                    counter++;
                    k.transform.parent = transform;
                }


            }

            boxCollider = gridCollider.GetComponent<BoxCollider>();
        }
    }

    public static TriggerColliderSpacePartitionTile ReturnTileByCoord(float x, float y)
    {
        // this is bad. iterating through whole array. Need to find another way
        //Debug.Log(x+" coords "+ y);
        for (int i = 0; i < gridTiles.Length; i++)
        {
            if (gridTiles[i].tileCoord.x == x && gridTiles[i].tileCoord.z == y)
            {
                // Debug.Log(gridTiles[i].tileCoord+" - coord. id - " + gridTiles[i].gridID);
                return gridTiles[i];
            }
        }

        return null;
    }

    public static TriggerColliderSpacePartitionTile ReturnTileByID(int ID)
    {
        // this is bad. iterating through whole array. Need to find another way

        for (int i = 0; i < gridTiles.Length; i++)
        {
            if (gridTiles[i].gridTileID == ID)
            {
                //Debug.Log(gridTiles[i].tileCoord + " - coord. id - " + gridTiles[i].gridID);
                return gridTiles[i];
            }
        }

        return null;
    }



}
