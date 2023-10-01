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
    [SerializeField] private float m_water = 0.0f;
    [SerializeField] private float m_waterAdded = 0.0f;
    public float water => m_water + m_waterAdded;
    
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

    public void Humidify(float _value, bool _applyDirectly = false)
    {
        if(_applyDirectly)
        {
            m_water = math.clamp(m_water + _value, 0.0f, 1.0f);
            m_sprite.color = m_data.color.Evaluate(m_water);
        }
        else m_waterAdded += _value;
    }

    public void Evaporate(float _value)
    {
        float waterToEvaporate = math.min(m_water, _value);
        m_water -= waterToEvaporate;
        AddWaterToCeil(waterToEvaporate);
    }

    private void AddWaterToCeil(float _waterEvaporated)
    {
        if (m_isCeil) m_waterAdded += _waterEvaporated;
        else m_neighbours.up.AddWaterToCeil(_waterEvaporated);
    }

    public virtual void UpdateTurnLeft()
    {
        Evaporate(m_data.waterEvaporation);

        bool canShareLeft = m_neighbours.left && m_neighbours.left.water < water;

        float waterToShare = 0.0f;
        if (canShareLeft)
        {
            float waterToShareLeft =
                math.min(1.0f - m_neighbours.left.water, math.min(m_water, m_data.waterSideTransfer));
            m_neighbours.left.Humidify(waterToShareLeft);
            m_water -= waterToShareLeft;
            waterToShare += m_data.waterSideTransfer - waterToShareLeft;
        }
        else waterToShare += m_data.waterSideTransfer;

    }

    public virtual void UpdateTurnRight()
    {
        Evaporate(m_data.waterEvaporation);

        bool canShareRight = m_neighbours.right && m_neighbours.right.water < water;
        
        float waterToShare = 0.0f;
        if (canShareRight)
        {
            float waterToShareRight = math.min(1.0f - m_neighbours.right.water, math.min(m_water, m_data.waterSideTransfer));
            m_neighbours.right.Humidify(waterToShareRight);
            m_water -= waterToShareRight;
            waterToShare += m_data.waterSideTransfer - waterToShareRight;
        }
        else waterToShare += m_data.waterSideTransfer;
    }
    
    public virtual void UpdateTurnDown()
    {
        bool canShareDown = m_neighbours.bottom && m_neighbours.bottom.water < 1.0f;
        
        float waterToShare = 0.0f;
        if (canShareDown)
        {
            float waterToShareDown = math.min(1.0f - m_neighbours.bottom.water, math.min(m_water, m_data.waterDownTransfer));
            m_neighbours.bottom.Humidify(waterToShareDown);
            m_water -= waterToShareDown;
            waterToShare += m_data.waterDownTransfer - waterToShareDown;
        }
        else waterToShare += m_data.waterDownTransfer;
    }
    public virtual void UpdateLateTurn()
    {
        m_water = math.clamp(m_water + m_waterAdded, 0.0f, 1.0f);
        m_sprite.color = m_data.color.Evaluate(m_water);
        m_waterAdded = 0.0f;
    }
    
    public virtual void Reset()
    {
        
    }
}
