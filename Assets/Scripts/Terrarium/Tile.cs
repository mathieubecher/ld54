using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private TileData m_data;
    [SerializeField] private Position m_position;
    [SerializeField] private Neighbours m_neighbours;
    private SpriteRenderer m_sprite;
    
    private bool m_isFloor;
    private bool m_isCeil;
    
    public Position position => m_position;

    private void Awake()
    {
        m_sprite = GetComponent<SpriteRenderer>();
    }
    
    public void Init(int _x, int _y)
    {
        Init(new Position(_x, _y));
    }
    public void Init(Position _position)
    {
        m_position = _position;
    }
    
    public void SetData(TileData _data)
    {
        m_data = _data;
        m_sprite.sprite = _data.sprite;
    }

    public void SetNeighbours(Neighbours _neighbours)
    {
        m_neighbours = _neighbours;
        m_isCeil = !m_neighbours.up;
        m_isFloor = !m_neighbours.bottom;
    }
    
    public virtual void UpdateTile()
    {
        
    }
}
