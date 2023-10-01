using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    
    private Terrarium m_terrarium;
    private bool m_isFloor;
    private bool m_isCeil;
    [SerializeField] private float m_water = 0.0f;
    [SerializeField] private float m_waterToEvaporate = 0.0f;
    public float water => m_water;
    public float totalWater => m_water + m_waterToEvaporate;
    public Neighbours neighbours => m_neighbours;
    
    public Position position => m_position;
    public TileData.TileType type => m_data.type;

    private void Awake()
    {
        m_sprite = GetComponent<SpriteRenderer>();
    }
    
    public void Init(Terrarium _terrarium, Position _position)
    {
        m_terrarium = _terrarium;
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

    public void Humidify(float _value)
    {
        m_water = math.clamp(m_water + _value, 0.0f, 1.0f);
        m_sprite.color = m_data.color.Evaluate(m_water);
    }

    public void Evaporate(float _value)
    {
        if (_value == 0.0f) return;
        float waterToEvaporate = math.min(m_water, _value);
        m_water -= waterToEvaporate;
        AddWaterToCeil(waterToEvaporate);
    }

    public float DrainWater(float _waterDrain)
    {
        float waterDrained = math.min(m_water, _waterDrain);
        m_water -= waterDrained;
        return waterDrained;
    }
    public void AddWaterToCeil(float _waterEvaporated)
    {
        if (m_isCeil) m_waterToEvaporate += _waterEvaporated;
        else m_neighbours.up.AddWaterToCeil(_waterEvaporated);
    }

    public virtual void FirstUpdate()
    {
        Evaporate(m_data.waterEvaporation);
    }
    
    public virtual void UpdateWaterDown()
    {
        bool canShareDown = m_neighbours.bottom && m_neighbours.bottom.water < 1.0f;
        
        float waterToShare = 0.0f;
        if (canShareDown)
        {
            float waterToShareDown = math.min(1.0f - m_neighbours.bottom.water, math.min(water, m_data.waterDownTransfer));
            m_neighbours.bottom.Humidify(waterToShareDown);
            m_water -= waterToShareDown;
            waterToShare += m_data.waterDownTransfer - waterToShareDown;
        }
        else waterToShare += m_data.waterDownTransfer;
    }
    
    public virtual void UpdateWaterLeft()
    {
        bool canShareLeft = m_neighbours.left && m_neighbours.left.water < water;

        float waterToShare = 0.0f;
        if (canShareLeft)
        {
            float waterToShareLeft =
                math.min(1.0f - m_neighbours.left.water, math.min(water, m_data.waterSideTransfer));
            m_neighbours.left.Humidify(waterToShareLeft);
            m_water -= waterToShareLeft;
            waterToShare += m_data.waterSideTransfer - waterToShareLeft;
        }
        else waterToShare += m_data.waterSideTransfer;

    }

    public virtual void UpdateWaterRight()
    {
        bool canShareRight = m_neighbours.right && m_neighbours.right.water < water;
        
        float waterToShare = 0.0f;
        if (canShareRight)
        {
            float waterToShareRight = math.min(1.0f - m_neighbours.right.water, math.min(water, m_data.waterSideTransfer));
            m_neighbours.right.Humidify(waterToShareRight);
            m_water -= waterToShareRight;
            waterToShare += m_data.waterSideTransfer - waterToShareRight;
        }
        else waterToShare += m_data.waterSideTransfer;
    }
    
    public virtual void UpdateLateTurn()
    {
        float condensation = math.min(m_waterToEvaporate, 1.0f - m_water);
        m_water += condensation;
        m_waterToEvaporate -= condensation;
        m_sprite.color = m_data.color.Evaluate(m_water);

    }
    
    public virtual void Reset()
    {
        
    }
    
    public List<Entity> GetEntities()
    {
        return GetComponentsInChildren<Entity>().ToList();
    }
}
