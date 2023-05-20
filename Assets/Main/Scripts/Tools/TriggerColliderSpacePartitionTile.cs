using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TriggerColliderSpacePartitionTile : MonoBehaviour
{

    public int gridTileID;
    public int gridUsersSPPositionerCounter;
    public int gridUsersSPCurrentAmount;
    [SerializeField] public Vector3 tileCoord;
    IGridUserSP[] gridUsersSP=new IGridUserSP[100];
    [SerializeField] GameObject[] gridUnits = new GameObject[100];

    //public GameObject[] gridUsersSP = new GameObject[100];
    public int[] gridUnitIDs = new int[100];
    BoxCollider boxCollider;
    IGridUserSP gridUserSP;
    private void Start()
    {
        gridUsersSPPositionerCounter = 0;
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {

        AddUnitToTheGridArray(other.gameObject);

    }

    private void OnTriggerExit(Collider other)
    {
        RemoveUnitFromTheGridArray(other.gameObject);
    }

    public void AddUnitToTheGridArray(GameObject go)
    {

        //Debug.Log(go);
        gridUserSP = go.GetComponent<IGridUserSP>();
        
        //gridUserSP = go.GetComponent(typeof(IGridUserSP)) as IGridUserSP;
        //Debug.Log(gridUserSP);
        //GetComponent(typeof(IEnemy)) as IEnemy;
        gridUserSP.EnteredTile(this);
        gridUsersSP[gridUsersSPPositionerCounter] = gridUserSP;
        gridUnitIDs[gridUsersSPPositionerCounter] = gridUserSP.ID;
        gridUnits[gridUsersSPPositionerCounter] = gridUserSP.myGameObject;
        //Debug.Log($"'{unitClass.UnidID}' Gameobject entered Grid tile '{gridID}'");
        IncreaseCounter();
    }

    public void RemoveUnitFromTheGridArray(GameObject go)
    {
        var a = gridUserSP = go.GetComponent<IGridUserSP>();

        for (int i = 0; i < gridUsersSP.Length; i++)
        {
            if (gridUserSP.ID == gridUnitIDs[i])
            {
                gridUsersSP[i] = null;
                gridUnitIDs[i] = 0;
                gridUnits[i] = null;
                gridUsersSPCurrentAmount--;
                //Debug.Log($"'{unitClass.UnidID}' Gameobject removed Grid tile  '{gridID}' list");
                break;
            }


        }

    }
    

    private void IncreaseCounter()
    {
        // cant have big list for each tiles or too many iterations. Therefore we reset collider when the end of array reached. 
        // reset array, counter. disable/enable collider to re-add units.
        gridUsersSPPositionerCounter++;
        gridUsersSPCurrentAmount++;
        if (gridUsersSPPositionerCounter >= gridUsersSP.Length)
        {
            Debug.Log(this.gridTileID + " tile reseted counters");
            var k = gridUsersSP.Length;
            //gridUsersSP = new GameObject[k];
            Array.Clear(gridUsersSP,0,gridUsersSP.Length);
            gridUnitIDs = new int[k];
            gridUsersSPPositionerCounter = 0;
            boxCollider.enabled = false;
            StartCoroutine(EnableCollider());
        }


    }

    IEnumerator EnableCollider()
    {
        yield return 0;

        boxCollider.enabled = true;
    }

    public IGridUserSP[] UnitsInTile()
    {
        return gridUsersSP;
    }

    public static TriggerColliderSpacePartitionTile[] GetNeighbors(TriggerColliderSpacePartitionTile originGridTile)
    {
        var neighbors = new TriggerColliderSpacePartitionTile[9];//we can only have max of  8 neighbors on the cube grid plus current tile
        /*
        table[i - 1][j - 1]
        table[i - 1][j]
        table[i - 1][j + 1]

        table[i][j - 1]
        table[i][j + 1]

        table[i + 1][j - 1]
        table[i + 1][j]
        table[i + 1][j + 1]
        */

        var x = originGridTile.tileCoord.x;
        var y = originGridTile.tileCoord.z;

        neighbors[8] = TriggerColliderSpacePartition.ReturnTileByCoord(x, y);

        var temp_x = x - 100;
        var temp_y = y - 100;
        neighbors[0] = TriggerColliderSpacePartition.ReturnTileByCoord(temp_x, temp_y);

        temp_x = x - 100;
        temp_y = y;
        neighbors[1] = TriggerColliderSpacePartition.ReturnTileByCoord(temp_x, temp_y);

        temp_x = x - 100;
        temp_y = y + 100;
        neighbors[2] = TriggerColliderSpacePartition.ReturnTileByCoord(temp_x, temp_y);

        temp_x = x;
        temp_y = y - 100;
        neighbors[3] = TriggerColliderSpacePartition.ReturnTileByCoord(temp_x, temp_y);

        temp_x = x;
        temp_y = y + 100;
        neighbors[4] = TriggerColliderSpacePartition.ReturnTileByCoord(temp_x, temp_y);

        temp_x = x + 100;
        temp_y = y - 100;
        neighbors[5] = TriggerColliderSpacePartition.ReturnTileByCoord(temp_x, temp_y);

        temp_x = x + 100;
        temp_y = y;
        neighbors[6] = TriggerColliderSpacePartition.ReturnTileByCoord(temp_x, temp_y);

        temp_x = x + 100;
        temp_y = y + 100;
        neighbors[7] = TriggerColliderSpacePartition.ReturnTileByCoord(temp_x, temp_y);

        return neighbors;
    }

    public static TriggerColliderSpacePartitionTile[] GetNeighbors(int tileID)
    {
        var originGridTile = TriggerColliderSpacePartition.ReturnTileByID(tileID);
        var neighbors = new TriggerColliderSpacePartitionTile[9];//we can only have max of  8 neighbors on the cube grid plus current tile

        var x = originGridTile.tileCoord.x;
        var y = originGridTile.tileCoord.z;

        //Debug.Log(x);
        //Debug.Log(y);

        neighbors[8] = TriggerColliderSpacePartition.ReturnTileByCoord(x, y);

        var temp_x = x - 100;
        var temp_y = y - 100;
        neighbors[0] = TriggerColliderSpacePartition.ReturnTileByCoord(temp_x, temp_y);

        temp_x = x - 100;
        temp_y = y;
        neighbors[1] = TriggerColliderSpacePartition.ReturnTileByCoord(temp_x, temp_y);

        temp_x = x - 100;
        temp_y = y + 100;
        neighbors[2] = TriggerColliderSpacePartition.ReturnTileByCoord(temp_x, temp_y);

        temp_x = x;
        temp_y = y - 100;
        neighbors[3] = TriggerColliderSpacePartition.ReturnTileByCoord(temp_x, temp_y);

        temp_x = x;
        temp_y = y + 100;
        neighbors[4] = TriggerColliderSpacePartition.ReturnTileByCoord(temp_x, temp_y);

        temp_x = x + 100;
        temp_y = y - 100;
        neighbors[5] = TriggerColliderSpacePartition.ReturnTileByCoord(temp_x, temp_y);

        temp_x = x + 100;
        temp_y = y;
        neighbors[6] = TriggerColliderSpacePartition.ReturnTileByCoord(temp_x, temp_y);

        temp_x = x + 100;
        temp_y = y + 100;
        neighbors[7] = TriggerColliderSpacePartition.ReturnTileByCoord(temp_x, temp_y);

        return neighbors;
    }

    public static GameObject[] GetAllNeighborUnits(int tileID)
    {
        // another "super efficient" function. gotta redesign this
        var originGridTile = TriggerColliderSpacePartition.ReturnTileByID(tileID);

        TriggerColliderSpacePartitionTile[] relevantTiles = GetNeighbors(originGridTile);
        int tileUnitCounter = 0;
        int currentAddPos = 0;

        for (int i = 0; i < relevantTiles.Length; i++)
        {
            for (int j = 0; j < relevantTiles[i].gridUnits.Length; j++)
            {
                if (relevantTiles[i].gridUnits[j] != null)
                {
                    tileUnitCounter++;
                }

            }
        }
        //Debug.Log(tileUnitCounter);
        GameObject[] tempArray = new GameObject[tileUnitCounter];

        for (int i = 0; i < relevantTiles.Length; i++)
        {
            for (int j = 0; j < relevantTiles[i].gridUnits.Length; j++)
            {
                if (relevantTiles[i].gridUnits[j] != null)
                {
                    //Debug.Log(i+j);
                    tempArray[currentAddPos] = relevantTiles[i].gridUnits[j];
                    currentAddPos++;
                }

            }
        }
        //Debug.Log(tempArray.Length);
        return tempArray;

    }

    public static GameObject[] GetAllNeighborUnits(TriggerColliderSpacePartitionTile originGridTile)
    {
        // another "super efficient" function. gotta redesign this

        TriggerColliderSpacePartitionTile[] relevantTiles = TriggerColliderSpacePartitionTile.GetNeighbors(originGridTile);
        int tileUnitCounter = 0;


        for (int i = 0; i < relevantTiles.Length; i++)
        {
            for (int j = 0; j < relevantTiles[i].gridUnits.Length; j++)
            {
                if (relevantTiles[i].gridUnits[j] != null)
                {
                    tileUnitCounter++;
                }

            }
        }
        GameObject[] tempArray = new GameObject[tileUnitCounter];

        for (int i = 0; i < relevantTiles.Length; i++)
        {
            for (int j = 0; j < relevantTiles[i].gridUnits.Length; j++)
            {
                if (relevantTiles[i].gridUnits[j] != null)
                {
                    tempArray[i + j] = relevantTiles[i].gridUnits[j];
                }

            }
        }

        return tempArray;

    }
}
