using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private List<TileData.TileType> m_canMoveInType; 
    private Tile m_position;
    private Brush m_brush;
    public Position position => m_position.position;
    
    public void Init(Tile _position, Brush _brush)
    {
        m_position = _position;
        transform.parent = m_position.transform;
        transform.localPosition = Vector3.zero;
        m_brush = _brush;
        
        m_position.OnTileChange += OnTileChange;
    }

    private void OnDestroy()
    {
        m_position.OnTileChange -= OnTileChange;
    }

    private void OnTileChange(TileData _data)
    {
        if (!CanSpawnOnTile(m_position))
        {
            m_brush.RemoveItemInTerrarium();
            Destroy(gameObject);
        }
    }

    public bool CanSpawnOnTile(Tile _tile)
    {
        return m_canMoveInType.Exists(x => x == _tile.type);
    }
    
    public virtual void UpdateEntity()
    {
        
    }
}
