using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private Tile m_position;
    public Position position => m_position.position;
    
    public void Init(Tile _position)
    {
        m_position = _position;
        transform.parent = m_position.transform;
        transform.position = Vector3.zero;
    }
    
    public bool CanSpawnOnTile(Tile _tile)
    {
        return true;
    }
    
    public virtual void UpdateEntity()
    {
        
    }
}
