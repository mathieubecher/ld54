using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "TileData", order = 1)]
public class TileData : ScriptableObject
{
    public Sprite sprite;
    public float waterDownTransfer = 1.0f;
    public float waterSideTransfer = 0.0f;
}
