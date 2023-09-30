using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "TileData", order = 1)]
public class TileData : ScriptableObject
{
    public enum TileType
    {
        DIRT,
        GRAVEL,
        AIR,
    }
    public TileType type;
    public Sprite sprite;
    public float waterDownTransfer = 1.0f;
    public float waterSideTransfer = 0.0f;
    public Gradient color;
    
    public bool CanSpawnOnTile(Tile _tile)
    {
        return true;
    }
}
