using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUniqID
{
     int ID { get;}
}
public interface IGridUserSP:IUniqID
{
    GameObject myGameObject{get;}
    // Space Partitioning Grid User
    int CurrentTileID { get; set; }
    void EnteredTile(TriggerColliderSpacePartitionTile tile);
    //void ChangeTileID(int TileID);
}
