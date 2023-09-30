using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public delegate void TileChangeEvent(TileData _data);
    public TileChangeEvent OnTileChange;
    
    [SerializeField] private TileData m_data;
    [SerializeField] private Position m_position;
    [SerializeField] private Neighbours m_neighbours;
    private SpriteRenderer m_sprite;
    private Brush m_brush;
    
    private bool m_isFloor;
    private bool m_isCeil;
    private float m_water = 0.0f;
    public float water => m_water;
    
    public Position position => m_position;
    public TileData.TileType type => m_data.type;

    private void Awake()
    {
        m_sprite = GetComponent<SpriteRenderer>();
    }
    
    public void Init(Position _position)
    {
        m_position = _position;
    }
    
    public void SetData(TileData _data, Brush _brush)
    {
        bool needToReset = false;
        if (m_data) needToReset = true;
        if (m_data != _data)
        {
            m_data = _data;
            m_sprite.sprite = _data.sprite;
            m_water = 0.0f;
        }

        if (needToReset)
        {
            if(m_brush) m_brush.RemoveItemInTerrarium();
            OnTileChange?.Invoke(m_data);
        }

        m_brush = _brush;
        m_sprite.color = m_data.color.Evaluate(m_water);
    }

    public void SetNeighbours(Neighbours _neighbours)
    {
        m_neighbours = _neighbours;
        m_isCeil = !m_neighbours.up;
        m_isFloor = !m_neighbours.bottom;
    }

    public void Humidify(float value)
    {
        m_water = math.clamp(m_water + value, 0.0f, 1.0f);
        m_sprite.color = m_data.color.Evaluate(m_water);
    }
    
    public virtual void UpdateTile()
    {
        
    }
}
