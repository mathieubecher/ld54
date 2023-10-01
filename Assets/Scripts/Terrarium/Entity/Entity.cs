using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum EntityType { NONE, VEGETABLE, SEED, ANIMAL, WASTE }
    
    public delegate void RemoveByToolEvent(Entity _entity);
    public RemoveByToolEvent OnRemoveByTool;
    
    [SerializeField] private List<TileData.TileType> m_canMoveInType; 
    [SerializeField] protected Tile m_position;
    [SerializeField] private EntityType m_type;
    public EntityType type => m_type;
    public Position position => m_position.position;

    private bool m_isDead = false;
    public bool dead => m_isDead;
    
    public virtual void Init(Tile _position)
    {
        m_position = _position;
        transform.parent = m_position.transform;
        transform.localPosition = Vector3.zero;
        
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
            OnRemoveByTool?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public virtual bool CanSpawnOnTile(Tile _tile)
    {
        if(_tile.GetEntities().Exists(x => x.m_type == m_type)) return false;
        return m_canMoveInType.Exists(x => x == _tile.type);
    }
    
    public virtual void UpdateTurn()
    {
        
    }

    public virtual void Reset()
    {
        
    }
    public virtual void Die()
    {
        m_isDead = true;
    }

    public virtual void RequestDestroy()
    {
        if(this && m_isDead) Destroy(gameObject);
    }

    public static Entity Spawn(Terrarium _terrarium, Tile _tile, Entity _entity, bool _addToTerrariumList = true)
    {
        var entity = Instantiate(_entity, _terrarium.transform);
        entity.Init(_tile);
        if (_addToTerrariumList) _terrarium.AddEntity(entity);
        return entity;
    }
}
