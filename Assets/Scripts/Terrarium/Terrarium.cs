using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Terrarium : MonoBehaviour
{
    [SerializeField, TextArea] private string m_shape;
    [SerializeField] private Position m_offset;
    [SerializeField] private GameObject m_defaultTile;
    
    private List<List<Tile>> m_tiles;
    private List<Entity> m_entities;
    // Start is called before the first frame update
    void Awake()
    {
        m_tiles = new List<List<Tile>>();
        m_entities = new List<Entity>();

        InitTiles();
        InitNeighbours();
    }

    public Position WorldToTerrariumPosition(Vector3 _position)
    {
        Vector3 localPosition = transform.InverseTransformPoint(_position + Vector3.one * 0.5f);
        return new Position((int)math.floor(localPosition.x), -(int)math.floor(localPosition.y)) - m_offset;
    }
    
    public bool TryFindEntitiesAtPosition(Position _position, out List<Entity> _entities)
    {
        _entities = m_entities.FindAll(x => x.position == _position);
        return _entities.Count > 0;
    }

    public bool TryFindTileAtPosition(Position _position, out Tile _tile)
    {
        _tile = null;
        if (_position.y >= 0 && _position.y < m_tiles.Count)
        {
            if (_position.x >= 0 && _position.x < m_tiles[_position.y].Count)
            {
                _tile = m_tiles[_position.y][_position.x];
                return _tile;
            }
        }

        return false;
    }
    
    public bool CanSpawnEntityAtPosition(Position _position, Entity _entity, out Tile _tileAtPosition)
    {
        _tileAtPosition = null;
        if (TryFindTileAtPosition(_position, out _tileAtPosition))
        {
            return _entity.CanSpawnOnTile(_tileAtPosition);
        }

        return false;
    }

    
    public bool CanSpawnTileAtPosition(Position _position, out Tile _tileAtPosition)
    {
        return TryFindTileAtPosition(_position, out _tileAtPosition);
    }
    private void InitTiles()
    {
        int y = 0;
        foreach (string line in m_shape.Split('\n'))
        {
            var currentLine = new List<Tile>();
            int x = 0;
            foreach (string box in line.Split(','))
            {
                if (box == "0")
                {
                    currentLine.Add(null);
                    ++x;
                    continue;
                }

                GameObject tileObject = Instantiate(m_defaultTile, transform);
                Tile tile = tileObject.GetComponent<Tile>();
                Position position = new Position(x, y) + m_offset;
                tileObject.transform.localPosition = new Vector3(position.x, -position.y);
                tile.Init(position);
                currentLine.Add(tile);
                ++x;
            }

            m_tiles.Add(currentLine);
            ++y;
        }
    }
    private void InitNeighbours()
    {
        for(int y = 0; y < m_tiles.Count; ++y)
        {
            for (int x = 0; x < m_tiles[y].Count; ++x)
            {
                if (!m_tiles[y][x]) continue;
                
                Neighbours neighbours = new Neighbours();
                neighbours.left = x - 1 >= 0 ? m_tiles[y][x - 1] : null;
                neighbours.right = x + 1 < m_tiles[y].Count ? m_tiles[y][x + 1] : null;
                neighbours.up = y - 1 >= 0 && x < m_tiles[y - 1].Count ? m_tiles[y - 1][x] : null;
                neighbours.bottom = y + 1 < m_tiles.Count && x < m_tiles[y + 1].Count ? m_tiles[y + 1][x] : null;
                
                m_tiles[y][x].SetNeighbours(neighbours);
            }
        }
    }
}
